using Melts_Base.OracleModels;
using Melts_Base.SybaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Melts_Base.BackgroundSync
{
    /// <summary>
    /// Holds the most recent successful remote poll for presentation by the WPF UI.
    /// A snapshot is published only after the joined SQLite transaction is saved.
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
            int addedOrUpdated)
        {
            var snapshot = new MeltPollingSnapshot(
                Interlocked.Increment(ref _version),
                DateTimeOffset.Now,
                sybaseMelts.ToArray(),
                oracleMelts.ToArray(),
                addedOrUpdated);

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
            int addedOrUpdated)
        {
            Version = version;
            CompletedAt = completedAt;
            SybaseMelts = sybaseMelts;
            OracleMelts = oracleMelts;
            AddedOrUpdated = addedOrUpdated;
        }

        public long Version { get; }
        public DateTimeOffset CompletedAt { get; }
        public IReadOnlyList<SybaseMelt> SybaseMelts { get; }
        public IReadOnlyList<OracleMelt> OracleMelts { get; }
        public int AddedOrUpdated { get; }
    }
}
