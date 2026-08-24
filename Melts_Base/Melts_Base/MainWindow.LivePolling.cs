using Melts_Base.BackgroundSync;
using Melts_Base.OracleModels;
using Melts_Base.OracleViewModel;
using Melts_Base.SQLiteModels;
using Melts_Base.SQLiteViewModel;
using Melts_Base.SybaseModels;
using Melts_Base.SybaseViewModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Melts_Base
{
    public partial class MainWindow
    {
        private DispatcherTimer? _livePollingTimer;
        private long _lastDisplayedPollingVersion;
        private bool _isApplyingPollingSnapshot;

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            // Manual refresh and startup now use the same pipeline as background polling.
            //Loaded -= Window_Loaded;
            //Loaded += RuntimeWindow_Loaded;
            refreshButton.Click -= refreshDataClick;
            refreshButton.Click += RuntimeRefreshButton_Click;

            _livePollingTimer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _livePollingTimer.Tick += LivePollingTimer_Tick;
            _livePollingTimer.Start();
        }

        private void RuntimeWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetTabsVisibility();
            localdateZap.Width = new DataGridLength(120);
            localnPlav.Width = new DataGridLength(90);
            dateZap.Width = new DataGridLength(120);
            dateClose.Width = new DataGridLength(120);
            nPlav.Width = new DataGridLength(90);
            refreshButton.IsEnabled = true;
            ShowConnectionCheckInProgress();

            var service = MeltPollingBackgroundService.Current;
            if (service is null)
            {
                ShowPollingError("Сервис опроса не запущен");
                return;
            }

            // The hosted service performs the startup connection check and, when enabled,
            // the initial poll. The UI timer displays its result without opening a second
            // legacy ODBC connection on the UI thread.
        }

        private async void RuntimeRefreshButton_Click(object sender, RoutedEventArgs e)
        {
            var service = MeltPollingBackgroundService.Current;
            if (service is null)
            {
                return;
            }

            refreshButton.IsEnabled = false;
            ShowConnectionCheckInProgress();
            try
            {
                var resetTestDatabase = ApplicationPollingRuntimeOptions.Current?.TestMode == true;
                await service.PollOnceAsync(resetTestDatabase);
                await DisplayLatestSnapshotAsync();
            }
            catch (Exception ex)
            {
                ShowPollingError($"Обновление не выполнено: {ex.Message}");
            }
            finally
            {
                loadingProgress.IsIndeterminate = false;
                refreshButton.IsEnabled = true;
            }
        }

        private void ShowConnectionCheckInProgress()
        {
            var checkingBrush = new SolidColorBrush(Colors.Gray);
            oracleConnection.Fill = checkingBrush;
            sybaseConnection.Fill = checkingBrush;
            loadingProgress.IsIndeterminate = true;
            textOfProgress.Foreground = new SolidColorBrush(Colors.Black);
            textOfProgress.Text = "Проверка связи с базами...";
        }

        protected override void OnClosed(EventArgs e)
        {
            if (_livePollingTimer is not null)
            {
                _livePollingTimer.Stop();
                _livePollingTimer.Tick -= LivePollingTimer_Tick;
            }

            base.OnClosed(e);
        }

        private async void LivePollingTimer_Tick(object? sender, EventArgs e)
        {
            var snapshot = MeltPollingMonitor.Latest;
            if (snapshot is null || snapshot.Version == _lastDisplayedPollingVersion || _isApplyingPollingSnapshot)
            {
                return;
            }

            _isApplyingPollingSnapshot = true;
            try
            {
                await DisplayPollingSnapshotAsync(snapshot);
                _lastDisplayedPollingVersion = snapshot.Version;
            }
            catch (Exception ex)
            {
                ShowPollingError($"Ошибка отображения обновления: {ex.Message}");
            }
            finally
            {
                _isApplyingPollingSnapshot = false;
            }
        }

        private async System.Threading.Tasks.Task DisplayLatestSnapshotAsync()
        {
            var snapshot = MeltPollingMonitor.Latest;
            if (snapshot is null || snapshot.Version == _lastDisplayedPollingVersion)
            {
                return;
            }

            await DisplayPollingSnapshotAsync(snapshot);
            _lastDisplayedPollingVersion = snapshot.Version;
        }

        private async System.Threading.Tasks.Task DisplayPollingSnapshotAsync(MeltPollingSnapshot snapshot)
        {
            UpdateConnectionIndicators(snapshot);
            if (!snapshot.ConnectionsAvailable)
            {
                loadingProgress.IsIndeterminate = false;
                textOfProgress.Foreground = new SolidColorBrush(Colors.Red);
                textOfProgress.Text = "Загрузка данных невозможна!";
                return;
            }

            if (!snapshot.DataLoaded)
            {
                loadingProgress.IsIndeterminate = false;
                textOfProgress.Foreground = new SolidColorBrush(Colors.Green);
                textOfProgress.Text = "Связь с базами установлена";
                return;
            }

            loadingProgress.IsIndeterminate = false;

            var oracleFilter = observableOracleMeltsViewModel;
            observableOracleMeltsViewModel = new ObservableOracleMeltsViewModel(
                new ObservableCollection<OracleMelt>(snapshot.OracleMelts));
            CopyOracleFilters(oracleFilter, observableOracleMeltsViewModel);
            BindOracleViewModel(observableOracleMeltsViewModel);

            var sybaseFilter = observableSybaseMeltsViewModel;
            observableSybaseMeltsViewModel = new ObservableSybaseMeltsViewModel(
                new ObservableCollection<SybaseMelt>(snapshot.SybaseMelts));
            CopySybaseFilters(sybaseFilter, observableSybaseMeltsViewModel);
            BindSybaseViewModel(observableSybaseMeltsViewModel);

            var localFilter = observableMeltsViewModel;
            await using var displayContext = new PollingMeltContext(snapshot.LocalDatabasePath);
            await displayContext.Database.EnsureCreatedAsync();
            localSQLLiteMelts = new ObservableCollection<Melt>(
                await displayContext.Melts.AsNoTracking().OrderByDescending(melt => melt.Me_beg).ToListAsync());
            observableMeltsViewModel = new ObservableMeltsViewModel(localSQLLiteMelts);
            CopyLocalFilters(localFilter, observableMeltsViewModel);
            BindLocalViewModel(observableMeltsViewModel);

            textOfProgress.Foreground = new SolidColorBrush(Colors.Green);
            var mode = snapshot.TestMode ? "тест" : "рабочий режим";
            var action = snapshot.Joined ? "Обновление" : "Источники загружены";
            textOfProgress.Text =
                $"{action} {snapshot.CompletedAt:HH:mm:ss} ({mode}): " +
                $"Oracle {snapshot.OracleMelts.Count}, Sybase {snapshot.SybaseMelts.Count}, " +
                $"изменено {snapshot.AddedOrUpdated}";
        }

        private void UpdateConnectionIndicators(MeltPollingSnapshot snapshot)
        {
            sybaseConnection.Fill = new SolidColorBrush(
                snapshot.SybaseConnected ? Colors.Green : Colors.Red);
            oracleConnection.Fill = new SolidColorBrush(
                snapshot.OracleConnected ? Colors.Green : Colors.Red);
        }

        private void ShowPollingError(string message)
        {
            oracleConnection.Fill = new SolidColorBrush(Colors.Red);
            sybaseConnection.Fill = new SolidColorBrush(Colors.Red);
            textOfProgress.Foreground = new SolidColorBrush(Colors.Red);
            textOfProgress.Text = message;
        }

        private void BindOracleViewModel(ObservableOracleMeltsViewModel viewModel)
        {
            oracleGrid.DataContext = viewModel;
            ZapuskStartDate.DataContext = viewModel;
            ZapuskEndDate.DataContext = viewModel;
            CloseStartDate.DataContext = viewModel;
            CloseEndDate.DataContext = viewModel;
            PlantMeltNumberSought.DataContext = viewModel;
        }

        private void BindSybaseViewModel(ObservableSybaseMeltsViewModel viewModel)
        {
            shop31Grid.DataContext = viewModel;
            shop31PlantMeltNumberSought.DataContext = viewModel;
            shop31ZapuskStartDate.DataContext = viewModel;
            shop31ZapuskEndDate.DataContext = viewModel;
        }

        private void BindLocalViewModel(ObservableMeltsViewModel viewModel)
        {
            localcopyGrid.DataContext = viewModel;
            localZapuskStartDate.DataContext = viewModel;
            localZapuskEndDate.DataContext = viewModel;
            localPlantMeltNumberSought.DataContext = viewModel;
        }

        private static void CopyOracleFilters(ObservableOracleMeltsViewModel? source, ObservableOracleMeltsViewModel target)
        {
            if (source is null) return;
            target.MeltNumberSought = source.MeltNumberSought;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.StartCloseDate = source.StartCloseDate;
            target.EndCloseDate = source.EndCloseDate;
        }

        private static void CopySybaseFilters(ObservableSybaseMeltsViewModel? source, ObservableSybaseMeltsViewModel target)
        {
            if (source is null) return;
            target.MeltNumberSought = source.MeltNumberSought;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
        }

        private static void CopyLocalFilters(ObservableMeltsViewModel? source, ObservableMeltsViewModel target)
        {
            if (source is null) return;
            target.MeltNumberSought = source.MeltNumberSought;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.StartCloseDate = source.StartCloseDate;
            target.EndCloseDate = source.EndCloseDate;
        }
    }
}
