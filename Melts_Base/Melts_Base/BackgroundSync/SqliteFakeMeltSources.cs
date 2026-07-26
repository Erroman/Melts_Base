using Melts_Base.OracleModels;
using Melts_Base.SybaseModels;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Melts_Base.BackgroundSync
{
    internal static class TestDatabasePaths
    {
        private static readonly string DirectoryPath = Path.Combine(AppContext.BaseDirectory, "TestData");
        public static string SybaseDatabase => Path.Combine(DirectoryPath, "fake-sybase.db");
        public static string OracleDatabase => Path.Combine(DirectoryPath, "fake-oracle.db");
        public static string JoinedDatabase => Path.Combine(DirectoryPath, "test-melts.db");
        public static void EnsureDirectory() => Directory.CreateDirectory(DirectoryPath);
    }

    internal sealed class SqliteFakeSybaseMeltSource : ISybaseMeltSource
    {
        public async Task<IReadOnlyList<SybaseMelt>> ReadAsync(CancellationToken cancellationToken)
        {
            TestDatabasePaths.EnsureDirectory();
            await using var connection = new SqliteConnection($"Data Source={TestDatabasePaths.SybaseDatabase}");
            await connection.OpenAsync(cancellationToken);
            await SeedAsync(connection, cancellationToken);

            await using (var advance = connection.CreateCommand())
            {
                advance.CommandText = @"
                    UPDATE poll_state SET poll_number = poll_number + 1;
                    UPDATE sybase_melts
                    SET me_weight = CAST(980 + (SELECT poll_number FROM poll_state) AS TEXT)
                    WHERE me_num = 'A-1001';
                    INSERT OR IGNORE INTO sybase_melts
                    SELECT 'fake-3', '31', 'A-1003', datetime('now'), NULL,
                           'VT1', 'VT1 Pure', 'K-103', '900', 'P3', 'VT1'
                    WHERE (SELECT poll_number FROM poll_state) >= 3;
                    ";
                await advance.ExecuteNonQueryAsync(cancellationToken);
            }

            var result = new List<SybaseMelt>();
            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT me_id, eq_id, me_num, me_beg, me_end, me_splav, sp_name,
                       me_mould, me_weight, me_pos, sp_id
                FROM sybase_melts ORDER BY me_num
                ";
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                result.Add(new SybaseMelt
                {
                    Me_id = reader.GetString(0), Eq_id = reader.GetString(1), Me_num = reader.GetString(2),
                    Me_beg = reader.GetDateTime(3), Me_end = reader.IsDBNull(4) ? null : reader.GetDateTime(4),
                    Me_splav = reader.GetString(5), Sp_name = reader.GetString(6), Me_mould = reader.GetString(7),
                    Me_weight = reader.GetString(8), Me_pos = reader.GetString(9), Sp_id = reader.GetString(10)
                });
            }

            return result;
        }

        private static async Task SeedAsync(SqliteConnection connection, CancellationToken cancellationToken)
        {
            await using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS poll_state (poll_number INTEGER NOT NULL);
                INSERT INTO poll_state SELECT 0 WHERE NOT EXISTS (SELECT 1 FROM poll_state);
                CREATE TABLE IF NOT EXISTS sybase_melts (
                    me_id TEXT PRIMARY KEY, eq_id TEXT NOT NULL, me_num TEXT NOT NULL,
                    me_beg TEXT NOT NULL, me_end TEXT, me_splav TEXT NOT NULL,
                    sp_name TEXT NOT NULL, me_mould TEXT NOT NULL, me_weight TEXT NOT NULL,
                    me_pos TEXT NOT NULL, sp_id TEXT NOT NULL);
                INSERT OR IGNORE INTO sybase_melts VALUES
                    ('fake-1', '31', 'A-1001', '2026-07-13 07:00:00', '2026-07-13 10:00:00', 'VT6', 'VT6 Titanium', 'K-101', '980', 'P1', 'VT6'),
                    ('fake-2', '31', 'A-1002', '2026-07-13 11:00:00', NULL, 'VT14', 'VT14 Titanium', 'K-102', '1020', 'P2', 'VT14');
                ";
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }

    internal sealed class SqliteFakeOracleMeltSource : IOracleMeltSource
    {
        public async Task<IReadOnlyList<OracleMelt>> ReadAsync(CancellationToken cancellationToken)
        {
            TestDatabasePaths.EnsureDirectory();
            await using var connection = new SqliteConnection($"Data Source={TestDatabasePaths.OracleDatabase}");
            await connection.OpenAsync(cancellationToken);
            await SeedAsync(connection, cancellationToken);

            var result = new List<OracleMelt>();
            await using var command = connection.CreateCommand();
            command.CommandText = @"
                SELECT pasport_id, npech, nplav, ins, tek, pereplav,
                       okonch_pereplav, poz, poz_naim, date_zap, date_close
                FROM oracle_melts ORDER BY pasport_id
                ";
            await using var reader = await command.ExecuteReaderAsync(cancellationToken);
            while (await reader.ReadAsync(cancellationToken))
            {
                result.Add(new OracleMelt
                {
                    PasportId = reader.GetInt32(0), Npech = reader.GetString(1), Nplav = reader.GetString(2),
                    Ins = reader.GetString(3), Tek = reader.GetString(4), Pereplav = reader.GetString(5),
                    OkonchPereplav = reader.GetString(6), Poz = reader.GetString(7), PozNaim = reader.GetString(8),
                    DateZap = reader.GetDateTime(9), DateClose = reader.IsDBNull(10) ? null : reader.GetDateTime(10)
                });
            }

            return result;
        }

        private static async Task SeedAsync(SqliteConnection connection, CancellationToken cancellationToken)
        {
            await using var command = connection.CreateCommand();
            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS oracle_melts (
                    pasport_id INTEGER PRIMARY KEY, npech TEXT NOT NULL, nplav TEXT NOT NULL,
                    ins TEXT NOT NULL, tek TEXT NOT NULL, pereplav TEXT NOT NULL,
                    okonch_pereplav TEXT NOT NULL, poz TEXT NOT NULL, poz_naim TEXT NOT NULL,
                    date_zap TEXT NOT NULL, date_close TEXT);
                INSERT OR IGNORE INTO oracle_melts VALUES
                    (1, '31', 'A-1001', 'INS-001', 'TEK-001', '1', 'Y', 'POZ-1', 'Rotor', '2026-07-13 07:00:00', '2026-07-13 10:00:00'),
                    (2, '31', 'A-1001', 'INS-001', 'TEK-001', '1', 'Y', 'POZ-1B', 'Rotor', '2026-07-13 07:00:00', '2026-07-13 10:00:00'),
                    (3, '31', 'A-1002', 'INS-002', 'TEK-002', '0', 'N', 'POZ-2', 'Compressor', '2026-07-13 11:00:00', NULL),
                    (4, '31', 'A-1003', 'INS-003', 'TEK-003', '0', 'N', 'POZ-3', 'Test item', '2026-07-13 12:00:00', NULL);
                ";
            await command.ExecuteNonQueryAsync(cancellationToken);
        }
    }
}
