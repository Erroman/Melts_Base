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

            _livePollingTimer = new DispatcherTimer(DispatcherPriority.Background)
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _livePollingTimer.Tick += LivePollingTimer_Tick;
            _livePollingTimer.Start();
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
            if (snapshot is null ||
                snapshot.Version == _lastDisplayedPollingVersion ||
                _isApplyingPollingSnapshot)
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
                textOfProgress.Foreground = new SolidColorBrush(Colors.Red);
                textOfProgress.Text = $"Ошибка отображения обновления: {ex.Message}";
            }
            finally
            {
                _isApplyingPollingSnapshot = false;
            }
        }

        private async System.Threading.Tasks.Task DisplayPollingSnapshotAsync(MeltPollingSnapshot snapshot)
        {
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
            meltsContext.ChangeTracker.Clear();
            await meltsContext.Melts.LoadAsync();
            localSQLLiteMelts = new ObservableCollection<Melt>(
                meltsContext.Melts.Local.OrderByDescending(melt => melt.Me_beg));
            observableMeltsViewModel = new ObservableMeltsViewModel(localSQLLiteMelts);
            CopyLocalFilters(localFilter, observableMeltsViewModel);
            BindLocalViewModel(observableMeltsViewModel);

            oracleConnection.Fill = new SolidColorBrush(Colors.Green);
            sybaseConnection.Fill = new SolidColorBrush(Colors.Green);
            textOfProgress.Foreground = new SolidColorBrush(Colors.Green);
            textOfProgress.Text =
                $"Автообновление {snapshot.CompletedAt:HH:mm:ss}: " +
                $"Oracle {snapshot.OracleMelts.Count}, " +
                $"Sybase {snapshot.SybaseMelts.Count}, " +
                $"изменено {snapshot.AddedOrUpdated}";
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

        private static void CopyOracleFilters(
            ObservableOracleMeltsViewModel? source,
            ObservableOracleMeltsViewModel target)
        {
            if (source is null)
            {
                return;
            }

            target.MeltNumberSought = source.MeltNumberSought;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.StartCloseDate = source.StartCloseDate;
            target.EndCloseDate = source.EndCloseDate;
        }

        private static void CopySybaseFilters(
            ObservableSybaseMeltsViewModel? source,
            ObservableSybaseMeltsViewModel target)
        {
            if (source is null)
            {
                return;
            }

            target.MeltNumberSought = source.MeltNumberSought;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
        }

        private static void CopyLocalFilters(
            ObservableMeltsViewModel? source,
            ObservableMeltsViewModel target)
        {
            if (source is null)
            {
                return;
            }

            target.MeltNumberSought = source.MeltNumberSought;
            target.StartDate = source.StartDate;
            target.EndDate = source.EndDate;
            target.StartCloseDate = source.StartCloseDate;
            target.EndCloseDate = source.EndCloseDate;
        }
    }
}
