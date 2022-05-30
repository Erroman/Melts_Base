using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Collections.ObjectModel;

using Melts_Base.SQLiteModels;

namespace Melts_Base.OracleViewModel
{
    internal class ObservableMappedmeltsViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableMappedmeltsViewModel(ObservableCollection<Mappedmelt> melts)
        {
            Melts = melts;

            _view = new ListCollectionView(melts);
            _view.Filter = ListCollectionView_Filter;
        }
        private ObservableCollection<Mappedmelt> _melts;
        public ObservableCollection<Mappedmelt> Melts
        {
            get => _melts;
            set { _melts = value; OnPropertyChanged(); }

        }
        private ListCollectionView _view;
        public ICollectionView View => _view;



        string startDate;
        public string StartDate
        {
            get => startDate;
            set
            {
                if (startDate == value) return;
                startDate = value;
                OnPropertyChanged();
                View.Refresh();
            }
        }
        string endDate;
        public string EndDate
        {
            get => endDate;
            set
            {
                if (endDate == value) return;
                endDate = value;
                OnPropertyChanged();
                View.Refresh();

            }
        }

        string startCloseDate;
        public string StartCloseDate
        {
            get => startCloseDate;
            set
            {
                if (startCloseDate == value) return;
                startCloseDate = value;
                OnPropertyChanged();
                View.Refresh();
            }
        }
        string endCloseDate;
        public string EndCloseDate
        {
            get => endCloseDate;
            set
            {
                if (endCloseDate == value) return;
                endCloseDate = value;
                OnPropertyChanged();
                View.Refresh();

            }
        }
        string meltNumberSought;
        public string MeltNumberSought
        {
            get => meltNumberSought;
            set
            {
                if (meltNumberSought == value) return;
                meltNumberSought = value;
                OnPropertyChanged();
                View.Refresh();
            }
        }
        private bool ListCollectionView_Filter(object Item)
        {

            return meltDateFilter(Item) && meltNumberFilter(Item) && meltCloseDateFilter(Item);

        }
        private bool meltDateFilter(object Item)
        {

            var melt = Item as Mappedmelt;
            if (melt != null)
            {
                DateTime startdate;
                DateTime enddate;
                bool startdateFilterSet = DateTime.TryParse(StartDate, out startdate);
                bool enddateFilterSet = DateTime.TryParse(EndDate, out enddate);
                return
                    (!startdateFilterSet || (startdate <= melt.DateZap)) && (!enddateFilterSet || (melt.DateZap <= enddate));

            }
            return false;
        }
        private bool meltCloseDateFilter(object Item)
        {

            var melt = Item as Mappedmelt;
            if (melt != null)
            {
                DateTime startdate;
                DateTime enddate;
                bool startdateFilterSet = DateTime.TryParse(StartCloseDate, out startdate);
                bool enddateFilterSet = DateTime.TryParse(EndCloseDate, out enddate);
                return
                    (!startdateFilterSet || (startdate <= melt.DateClose)) && (!enddateFilterSet || (melt.DateClose <= enddate));

            }
            return false;
        }
        private bool meltNumberFilter(object Item)
        {
            var melt = Item as Mappedmelt;
            if (melt != null)
            {
                //int meltnumbersought;
                //int current_meltnumber;
                //bool meltNumberGoodFormatted = Int32.TryParse(melt.MeltNumber, out current_meltnumber);
                //bool meltnumberFilterSet = Int32.TryParse(MeltNumberSought, out meltnumbersought);
                //return (!meltnumberFilterSet || meltNumberGoodFormatted && current_meltnumber == meltnumbersought);
                if (String.IsNullOrEmpty(MeltNumberSought))
                    return true;
                else
                    if (melt.Nplav == null)
                    return false;
                else
                    return (melt.Nplav.IndexOf(MeltNumberSought, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }
    }
}
