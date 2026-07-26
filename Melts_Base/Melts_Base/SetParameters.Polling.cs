using Melts_Base.BackgroundSync;
using System;
using System.Windows;
using System.Windows.Controls;

namespace Melts_Base
{
    public partial class SetParameters
    {
        private bool _pollingControlsAdded;

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            if (_pollingControlsAdded || Content is not Grid root)
            {
                return;
            }

            _pollingControlsAdded = true;
            var runtime = ApplicationPollingRuntimeOptions.Current;
            var testMode = new CheckBox
            {
                Content = "Тестовый режим (локальные базы Sybase и Oracle)",
                IsChecked = runtime?.TestMode ?? Properties.Settings.Default.TestMode,
                Margin = new Thickness(8)
            };
            var polling = new CheckBox
            {
                Content = "Фоновый опрос баз данных каждые 3 секунды",
                IsChecked = runtime?.PollingEnabled ?? Properties.Settings.Default.PollingEnabled,
                Margin = new Thickness(8)
            };

            testMode.Checked += TestModeChanged;
            testMode.Unchecked += TestModeChanged;
            polling.Checked += PollingChanged;
            polling.Unchecked += PollingChanged;

            var panel = new StackPanel { VerticalAlignment = VerticalAlignment.Center };
            panel.Children.Add(testMode);
            panel.Children.Add(polling);
            Grid.SetRow(panel, 2);
            Grid.SetRowSpan(panel, 2);
            Grid.SetColumn(panel, 1);
            Grid.SetColumnSpan(panel, 2);
            root.Children.Add(panel);
        }

        private async void TestModeChanged(object sender, RoutedEventArgs e)
        {
            var enabled = ((CheckBox)sender).IsChecked == true;
            Properties.Settings.Default.TestMode = enabled;
            ApplicationPollingRuntimeOptions.Current?.SetTestMode(enabled);

            var service = MeltPollingBackgroundService.Current;
            if (service is not null)
            {
                try
                {
                    await service.PreviewSourcesAsync();
                }
                catch
                {
                    // The main window reports source connection errors in its status area.
                }
            }
        }

        private void PollingChanged(object sender, RoutedEventArgs e)
        {
            var enabled = ((CheckBox)sender).IsChecked == true;
            Properties.Settings.Default.PollingEnabled = enabled;
            ApplicationPollingRuntimeOptions.Current?.SetPollingEnabled(enabled);
        }
    }
}
