using System.Configuration;

namespace Melts_Base.Properties
{
    internal sealed partial class Settings
    {
        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public bool TestMode
        {
            get => (bool)this[nameof(TestMode)];
            set => this[nameof(TestMode)] = value;
        }

        [UserScopedSetting]
        [DefaultSettingValue("False")]
        public bool PollingEnabled
        {
            get => (bool)this[nameof(PollingEnabled)];
            set => this[nameof(PollingEnabled)] = value;
        }
    }
}
