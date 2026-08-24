using Melts_Base.OracleModels;
using Melts_Base.SybaseModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Melts_Base.BackgroundSync
{
    internal sealed class RealSybaseMeltSource : ISybaseMeltSource
    {
        private readonly PollingSettings _settings;

        public RealSybaseMeltSource(IOptions<PollingSettings> settings)
        {
            _settings = settings.Value;
        }

        public Task<IReadOnlyList<SybaseMelt>> ReadAsync(CancellationToken cancellationToken)
        {
            // SQL Anywhere 5 predates the asynchronous ODBC APIs. Its OpenAsync path
            // blocks the caller and can fault inside dbl50t.dll; use the same synchronous
            // ODBC pattern as the working application, isolated on a worker thread.
            return Task.Run(() => Read(cancellationToken), cancellationToken);
        }

        public Task<bool> CanConnectAsync(CancellationToken cancellationToken)
        {
            return Task.Run(() =>
            {
                cancellationToken.ThrowIfCancellationRequested();
                try
                {
                    using var connection = CreateConnection();
                    connection.Open();
                    return connection.State == ConnectionState.Open;
                }
                catch (Exception) when (!cancellationToken.IsCancellationRequested)
                {
                    return false;
                }
            }, cancellationToken);
        }

        private IReadOnlyList<SybaseMelt> Read(CancellationToken cancellationToken)
        {
            using var connection = CreateConnection();
            connection.Open();
            if (connection.State != ConnectionState.Open)
            {
                return Array.Empty<SybaseMelt>();
            }

            var melts = new List<SybaseMelt>();
            using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM \"DBA\".\"rmelts\"";
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                cancellationToken.ThrowIfCancellationRequested();
                DateTime meltEnd;
                DateTime.TryParse(reader["me_end"].ToString(), out meltEnd);
                melts.Add(new SybaseMelt
                {
                    Me_id = reader["me_id"].ToString(),
                    Me_num = reader["me_num"].ToString(),
                    Eq_id = reader["eq_id"].ToString(),
                    Me_beg = DateTime.Parse(reader["me_beg"].ToString() ?? string.Empty),
                    Me_end = meltEnd == DateTime.Parse("01.01.0001") ? null : meltEnd,
                    Me_splav = reader["me_splav"].ToString(),
                    Sp_name = reader["sp_name"].ToString(),
                    Me_mould = reader["me_mould"].ToString(),
                    Me_del = reader["me_del"].ToString(),
                    Me_weight = reader["me_weigth"].ToString(),
                    Me_ukaz = reader["me_ukaz"].ToString(),
                    Me_kont = reader["me_kont"].ToString(),
                    Me_pril = reader["me_pril"].ToString(),
                    Me_nazn = reader["me_nazn"].ToString(),
                    Me_diam = reader["me_diam"].ToString(),
                    Me_pos = reader["me_pos"].ToString(),
                    Me_kat = reader["me_kat"].ToString(),
                    Sp_id = reader["sp_id"].ToString(),
                    Me_energy = reader["me_energy"].ToString()
                });
            }

            return melts;
        }

        private OdbcConnection CreateConnection()
        {
            var constr = new OdbcConnectionStringBuilder
            {
                ["Dsn"] = _settings.SybaseDsn,
                ["uid"] = _settings.SybaseUser,
                ["pwd"] = _settings.SybasePassword
            };

            return new OdbcConnection(constr.ConnectionString);
        }
    }

    internal sealed class RealOracleMeltSource : IOracleMeltSource
    {
        public async Task<bool> CanConnectAsync(CancellationToken cancellationToken)
        {
            try
            {
                await using var context = new ModelPlantContext();
                return await context.Database.CanConnectAsync(cancellationToken);
            }
            catch (Exception) when (!cancellationToken.IsCancellationRequested)
            {
                return false;
            }
        }

        public async Task<IReadOnlyList<OracleMelt>> ReadAsync(CancellationToken cancellationToken)
        {
            await using var context = new ModelPlantContext();
            if (!await context.Database.CanConnectAsync(cancellationToken))
            {
                return Array.Empty<OracleMelt>();
            }

            await context.Melt31s.LoadAsync(cancellationToken);
            return context.Melt31s.ToList();
        }
    }
}
