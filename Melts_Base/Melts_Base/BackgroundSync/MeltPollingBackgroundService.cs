using Melts_Base.OracleModels;
using Melts_Base.SQLiteModels;
using Melts_Base.SybaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melts_Base.BackgroundSync
{
    internal sealed class MeltPollingBackgroundService : BackgroundService
    {
        private readonly ISybaseMeltSource _sybaseSource;
        private readonly IOracleMeltSource _oracleSource;
        private readonly IPollingRuntimeOptions _runtime;
        private readonly ILogger<MeltPollingBackgroundService> _logger;
        private readonly SemaphoreSlim _cycleLock = new SemaphoreSlim(1, 1);

        public MeltPollingBackgroundService(
            ISybaseMeltSource sybaseSource,
            IOracleMeltSource oracleSource,
            IPollingRuntimeOptions runtime,
            ILogger<MeltPollingBackgroundService> logger)
        {
            _sybaseSource = sybaseSource;
            _oracleSource = oracleSource;
            _runtime = runtime;
            _logger = logger;
            Current = this;
        }

        public static MeltPollingBackgroundService? Current { get; private set; }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Background polling service started. Runtime interval={interval}s", _runtime.PollingInterval.TotalSeconds);

            var initialConnectionCheckPending = true;
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_runtime.PollingEnabled)
                {
                    try
                    {
                        await PollOnceAsync(false, stoppingToken);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "Polling cycle failed.");
                    }
                }
                else if (initialConnectionCheckPending)
                {
                    await CheckConnectionsAndPublishAsync(stoppingToken);
                }

                initialConnectionCheckPending = false;

                await Task.Delay(_runtime.PollingInterval, stoppingToken);
            }
        }

        public async Task PreviewSourcesAsync(CancellationToken cancellationToken = default)
        {
            await _cycleLock.WaitAsync(cancellationToken);
            try
            {
                var connections = await CheckConnectionsAsync(cancellationToken);
                if (!connections.AllAvailable)
                {
                    PublishConnectionStatus(connections);
                    return;
                }

                var sybaseMelts = await _sybaseSource.ReadAsync(cancellationToken);
                var oracleMelts = await _oracleSource.ReadAsync(cancellationToken);
                var targetPath = _runtime.LocalDatabasePath;
                EnsureParentDirectory(targetPath);
                await using var context = new PollingMeltContext(targetPath);
                await context.Database.EnsureCreatedAsync(cancellationToken);
                MeltPollingMonitor.Publish(sybaseMelts, oracleMelts, 0, targetPath, _runtime.TestMode, false);
            }
            finally
            {
                _cycleLock.Release();
            }
        }

        public async Task PollOnceAsync(bool resetTestDatabase, CancellationToken cancellationToken = default)
        {
            await _cycleLock.WaitAsync(cancellationToken);
            try
            {
                var connections = await CheckConnectionsAsync(cancellationToken);
                if (!connections.AllAvailable)
                {
                    PublishConnectionStatus(connections);
                    return;
                }

                var sybaseMelts = await _sybaseSource.ReadAsync(cancellationToken);
                var oracleMelts = await _oracleSource.ReadAsync(cancellationToken);
                var targetPath = _runtime.LocalDatabasePath;
                EnsureParentDirectory(targetPath);

                await using var context = new PollingMeltContext(targetPath);
                if (resetTestDatabase && _runtime.TestMode)
                {
                    await context.Database.EnsureDeletedAsync(cancellationToken);
                }

                await context.Database.EnsureCreatedAsync(cancellationToken);
                await context.Melts.LoadAsync(cancellationToken);
                var added = SyncLocalDatabase(context, sybaseMelts, oracleMelts);
                await context.SaveChangesAsync(cancellationToken);

                MeltPollingMonitor.Publish(sybaseMelts, oracleMelts, added, targetPath, _runtime.TestMode, true);
                _logger.LogInformation(
                    "Polling cycle done. TestMode={testMode} Sybase={sybaseCount} Oracle={oracleCount} AddedOrUpdated={added}",
                    _runtime.TestMode, sybaseMelts.Count, oracleMelts.Count, added);
            }
            finally
            {
                _cycleLock.Release();
            }
        }

        private async Task CheckConnectionsAndPublishAsync(CancellationToken cancellationToken)
        {
            await _cycleLock.WaitAsync(cancellationToken);
            try
            {
                var connections = await CheckConnectionsAsync(cancellationToken);
                MeltPollingMonitor.PublishConnectionStatus(
                    connections.SybaseConnected,
                    connections.OracleConnected,
                    _runtime.LocalDatabasePath,
                    _runtime.TestMode);
            }
            finally
            {
                _cycleLock.Release();
            }
        }

        private async Task<DatabaseConnectionStatus> CheckConnectionsAsync(CancellationToken cancellationToken)
        {
            // Test mode deliberately avoids touching either real database.
            if (_runtime.TestMode)
            {
                return new DatabaseConnectionStatus(true, true);
            }

            var sybaseCheck = _sybaseSource.CanConnectAsync(cancellationToken);
            var oracleCheck = _oracleSource.CanConnectAsync(cancellationToken);
            await Task.WhenAll(sybaseCheck, oracleCheck);
            return new DatabaseConnectionStatus(await sybaseCheck, await oracleCheck);
        }

        private void PublishConnectionStatus(DatabaseConnectionStatus connections)
        {
            MeltPollingMonitor.PublishConnectionStatus(
                connections.SybaseConnected,
                connections.OracleConnected,
                _runtime.LocalDatabasePath,
                _runtime.TestMode);
            _logger.LogWarning(
                "Data load skipped because a database connection is unavailable. Sybase={sybaseConnected} Oracle={oracleConnected}",
                connections.SybaseConnected,
                connections.OracleConnected);
        }

        private readonly struct DatabaseConnectionStatus
        {
            public DatabaseConnectionStatus(bool sybaseConnected, bool oracleConnected)
            {
                SybaseConnected = sybaseConnected;
                OracleConnected = oracleConnected;
            }

            public bool SybaseConnected { get; }
            public bool OracleConnected { get; }
            public bool AllAvailable => SybaseConnected && OracleConnected;
        }

        private static void EnsureParentDirectory(string databasePath)
        {
            var directory = Path.GetDirectoryName(databasePath);
            if (!string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
        }

        private static int SyncLocalDatabase(
            PollingMeltContext context,
            IReadOnlyList<SybaseMelt> sybaseMelts,
            IReadOnlyList<OracleMelt> oracleMelts)
        {
            var addedOrUpdated = 0;
            var localMelts = context.Melts.ToList();

            foreach (var sybaseMelt in sybaseMelts)
            {
                if (string.IsNullOrWhiteSpace(sybaseMelt.Me_num))
                {
                    continue;
                }

                var oracleMatch = oracleMelts.Where(m => m.Nplav == sybaseMelt.Me_num).ToArray();
                if (oracleMatch.Length > 0)
                {
                    sybaseMelt.Oracle_Ins = oracleMatch[0].Ins;
                    sybaseMelt.Oracle_Tek = oracleMatch[0].Tek;
                    sybaseMelt.Oracle_Poz = string.Join(Environment.NewLine, oracleMatch.Select(m => m.Poz));
                    sybaseMelt.Oracle_PozNaim = oracleMatch[0].PozNaim;
                    sybaseMelt.Oracle_Pereplav = oracleMatch[0].Pereplav;
                    sybaseMelt.Oracle_OkonchPereplav = oracleMatch[0].OkonchPereplav;
                }

                var matchingLocal = localMelts.Where(m => m.Me_num == sybaseMelt.Me_num).ToList();
                var foundSame = false;
                foreach (var local in matchingLocal)
                {
                    if (local.MyHashCode() == sybaseMelt.MyHashCode())
                    {
                        foundSame = true;
                        break;
                    }

                    if (sybaseMelt.Me_beg >= local.Me_beg)
                    {
                        context.Remove(local);
                    }
                    else
                    {
                        foundSame = true;
                        break;
                    }
                }

                if (foundSame)
                {
                    continue;
                }

                context.Add(new Melt
                {
                    Eq_id = sybaseMelt.Eq_id, Me_num = sybaseMelt.Me_num, Me_beg = sybaseMelt.Me_beg,
                    Me_end = sybaseMelt.Me_end, Me_splav = sybaseMelt.Me_splav, Sp_name = sybaseMelt.Sp_name,
                    Me_mould = sybaseMelt.Me_mould, Me_del = sybaseMelt.Me_del, Me_ukaz = sybaseMelt.Me_ukaz,
                    Me_kont = sybaseMelt.Me_kont, Me_pril = sybaseMelt.Me_pril, Me_nazn = sybaseMelt.Me_nazn,
                    Me_diam = sybaseMelt.Me_diam, Me_weight = sybaseMelt.Me_weight, Me_zakaz = sybaseMelt.Me_zakaz,
                    Me_pos = sybaseMelt.Me_pos, Me_kat = sybaseMelt.Me_kat, Sp_id = sybaseMelt.Sp_id,
                    Me_energy = sybaseMelt.Me_energy, Oracle_Ins = sybaseMelt.Oracle_Ins,
                    Oracle_Tek = sybaseMelt.Oracle_Tek, Oracle_Poz = sybaseMelt.Oracle_Poz,
                    Oracle_PozNaim = sybaseMelt.Oracle_PozNaim, Oracle_Pereplav = sybaseMelt.Oracle_Pereplav,
                    Oracle_OkonchPereplav = sybaseMelt.Oracle_OkonchPereplav
                });
                addedOrUpdated++;
            }

            return addedOrUpdated;
        }
    }
}
