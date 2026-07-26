using System;
using System.IO;
using System.Threading;

namespace Melts_Base.BackgroundSync
{
    internal sealed class ApplicationPollingRuntimeOptions : IPollingRuntimeOptions
    {
        private int _testMode;
        private int _pollingEnabled;

        public ApplicationPollingRuntimeOptions()
        {
            _testMode = Properties.Settings.Default.TestMode ? 1 : 0;
            _pollingEnabled = Properties.Settings.Default.PollingEnabled ? 1 : 0;
            Current = this;
        }

        public static ApplicationPollingRuntimeOptions? Current { get; private set; }
        public bool TestMode => Volatile.Read(ref _testMode) == 1;
        public bool PollingEnabled => Volatile.Read(ref _pollingEnabled) == 1;
        public TimeSpan PollingInterval => TimeSpan.FromSeconds(3);
        public string LocalDatabasePath => TestMode ? TestDatabasePaths.JoinedDatabase : Path.GetFullPath("melts.db");

        public void SetTestMode(bool value)
        {
            Volatile.Write(ref _testMode, value ? 1 : 0);
            Properties.Settings.Default.TestMode = value;
        }

        public void SetPollingEnabled(bool value)
        {
            Volatile.Write(ref _pollingEnabled, value ? 1 : 0);
            Properties.Settings.Default.PollingEnabled = value;
        }
    }
}
