namespace Melts_Base.BackgroundSync
{
    internal sealed class PollingSettings
    {
        public bool Enabled { get; set; } = true;
        public bool UseFakeSources { get; set; } = true;
        public int IntervalSeconds { get; set; } = 30;
        public string SybaseDsn { get; set; } = "ssybase";
        public string SybaseUser { get; set; } = "romanovskii";
        public string SybasePassword { get; set; } = "12345";
    }
}