using Melts_Base.OracleModels;
using Melts_Base.SQLiteModels;
using Melts_Base.SybaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melts_Base.BackgroundSync
{
    internal sealed class MeltPollingBackgroundService : BackgroundService
    {
        private readonly ISybaseMeltSource _sybaseSource;
        private readonly IOracleMeltSource _oracleSource;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IOptions<PollingSettings> _settings;
        private readonly ILogger<MeltPollingBackgroundService> _logger;

        public MeltPollingBackgroundService(
            ISybaseMeltSource sybaseSource,
            IOracleMeltSource oracleSource,
            IServiceScopeFactory scopeFactory,
            IOptions<PollingSettings> settings,
            ILogger<MeltPollingBackgroundService> logger)
        {
            _sybaseSource = sybaseSource;
            _oracleSource = oracleSource;
            _scopeFactory = scopeFactory;
            _settings = settings;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            if (!_settings.Value.Enabled)
            {
                _logger.LogInformation("Background polling is disabled in config.");
                return;
            }

            var interval = TimeSpan.FromSeconds(Math.Max(5, _settings.Value.IntervalSeconds));
            _logger.LogInformation("Background polling started. Interval={interval}s Fake={fake}", interval.TotalSeconds, _settings.Value.UseFakeSources);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var sybaseMelts = await _sybaseSource.ReadAsync(stoppingToken);
                    var oracleMelts = await _oracleSource.ReadAsync(stoppingToken);

                    using var scope = _scopeFactory.CreateScope();
                    var context = scope.ServiceProvider.GetRequiredService<MeltContext>();
                    await context.Melts.LoadAsync(stoppingToken);

                    var added = SyncLocalDatabase(context, sybaseMelts, oracleMelts);
                    await context.SaveChangesAsync(stoppingToken);
                    MeltPollingMonitor.Publish(sybaseMelts, oracleMelts, added);

                    _logger.LogInformation(
                        "Polling cycle done. Sybase={sybaseCount} Oracle={oracleCount} AddedOrUpdated={added}",
                        sybaseMelts.Count,
                        oracleMelts.Count,
                        added);
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Polling cycle failed.");
                }

                await Task.Delay(interval, stoppingToken);
            }
        }

        private static int SyncLocalDatabase(MeltContext context, IReadOnlyList<SybaseMelt> sybaseMelts, IReadOnlyList<OracleMelt> oracleMelts)
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
                    Eq_id = sybaseMelt.Eq_id,
                    Me_num = sybaseMelt.Me_num,
                    Me_beg = sybaseMelt.Me_beg,
                    Me_end = sybaseMelt.Me_end,
                    Me_splav = sybaseMelt.Me_splav,
                    Sp_name = sybaseMelt.Sp_name,
                    Me_mould = sybaseMelt.Me_mould,
                    Me_del = sybaseMelt.Me_del,
                    Me_ukaz = sybaseMelt.Me_ukaz,
                    Me_kont = sybaseMelt.Me_kont,
                    Me_pril = sybaseMelt.Me_pril,
                    Me_nazn = sybaseMelt.Me_nazn,
                    Me_diam = sybaseMelt.Me_diam,
                    Me_weight = sybaseMelt.Me_weight,
                    Me_zakaz = sybaseMelt.Me_zakaz,
                    Me_pos = sybaseMelt.Me_pos,
                    Me_kat = sybaseMelt.Me_kat,
                    Sp_id = sybaseMelt.Sp_id,
                    Me_energy = sybaseMelt.Me_energy,
                    Oracle_Ins = sybaseMelt.Oracle_Ins,
                    Oracle_Tek = sybaseMelt.Oracle_Tek,
                    Oracle_Poz = sybaseMelt.Oracle_Poz,
                    Oracle_PozNaim = sybaseMelt.Oracle_PozNaim,
                    Oracle_Pereplav = sybaseMelt.Oracle_Pereplav,
                    Oracle_OkonchPereplav = sybaseMelt.Oracle_OkonchPereplav
                });
                addedOrUpdated++;
            }

            return addedOrUpdated;
        }
    }
}
