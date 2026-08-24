using Melts_Base.OracleModels;
using Melts_Base.SybaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Melts_Base.BackgroundSync
{
    /// <summary>
    /// Holds the most recent connection check or completed poll for presentation by the WPF UI.
    /// </summary>
    internal static class MeltPollingMonitor
    {
        private static readonly object SyncRoot = new object();
        private static long _version;
        private static MeltPollingSnapshot? _latest;

        public static MeltPollingSnapshot? Latest
        {
            get
            {
                lock (SyncRoot)
                {
                    return _latest;
                }
            }
        }

        public static void Publish(
            IReadOnlyList<SybaseMelt> sybaseMelts,
            IReadOnlyList<OracleMelt> oracleMelts,
            int addedOrUpdated,
            string localDatabasePath,
            bool testMode,
            bool joined)
        {
            var snapshot = new MeltPollingSnapshot(
                Interlocked.Increment(ref _version),
                DateTimeOffset.Now,
                sybaseMelts.ToArray(),
                oracleMelts.ToArray(),
                addedOrUpdated,
                localDatabasePath,
                testMode,
                joined,
                true,
                true,
                true);

            SetLatest(snapshot);
        }

        public static void PublishConnectionStatus(
            bool sybaseConnected,
            bool oracleConnected,
            string localDatabasePath,
            bool testMode)
        {
            var snapshot = new MeltPollingSnapshot(
                Interlocked.Increment(ref _version),
                DateTimeOffset.Now,
                Array.Empty<SybaseMelt>(),
                Array.Empty<OracleMelt>(),
                0,
                localDatabasePath,
                testMode,
                false,
                sybaseConnected,
                oracleConnected,
                false);

            SetLatest(snapshot);
        }

        private static void SetLatest(MeltPollingSnapshot snapshot)
        {
            lock (SyncRoot)
            {
                _latest = snapshot;
            }
        }
    }

    internal sealed class MeltPollingSnapshot
    {
        public MeltPollingSnapshot(
            long version,
            DateTimeOffset completedAt,
            IReadOnlyList<SybaseMelt> sybaseMelts,
            IReadOnlyList<OracleMelt> oracleMelts,
            int addedOrUpdated,
            string localDatabasePath,
            bool testMode,
            bool joined,
            bool sybaseConnected,
            bool oracleConnected,
            bool dataLoaded)
        {
            Version = version;
            CompletedAt = completedAt;
            SybaseMelts = sybaseMelts;
            OracleMelts = oracleMelts;
            AddedOrUpdated = addedOrUpdated;
            LocalDatabasePath = localDatabasePath;
            TestMode = testMode;
            Joined = joined;
            SybaseConnected = sybaseConnected;
            OracleConnected = oracleConnected;
            DataLoaded = dataLoaded;
        }

        public long Version { get; }
        public DateTimeOffset CompletedAt { get; }
        public IReadOnlyList<SybaseMelt> SybaseMelts { get; }
        public IReadOnlyList<OracleMelt> OracleMelts { get; }
        public int AddedOrUpdated { get; }
        public string LocalDatabasePath { get; }
        public bool TestMode { get; }
        public bool Joined { get; }
        public bool SybaseConnected { get; }
        public bool OracleConnected { get; }
        public bool DataLoaded { get; }
        public bool ConnectionsAvailable => SybaseConnected && OracleConnected;
    }
}
