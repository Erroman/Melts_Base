using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Collections.ObjectModel;

using Melts_Base.SybaseModels;

namespace Melts_Base.SybaseViewModel
{
    internal class ObservableSybaseMeltsViewModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableSybaseMeltsViewModel(ObservableCollection<SybaseMelt> melts)
        {
            Melts = melts;

            _view = new ListCollectionView(melts);
            _view.Filter = ListCollectionView_Filter;
        }
        private ObservableCollection<SybaseMelt> _melts;
        public ObservableCollection<SybaseMelt> Melts
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

            return meltDateFilter(Item) && meltNumberFilter(Item);

        }
        private bool meltDateFilter(object Item)
        {

            var melt = Item as SybaseMelt;
            if (melt != null)
            {
                DateTime startdate;
                DateTime enddate;
                bool startdateFilterSet = DateTime.TryParse(StartDate, out startdate);
                bool enddateFilterSet = DateTime.TryParse(EndDate, out enddate);
                return 
                    (!startdateFilterSet || (startdate <= melt.me_beg)) && (!enddateFilterSet || (melt.me_beg <= enddate));

            }
            return false;
        }
        
        private bool meltNumberFilter(object Item)
        {
            var melt = Item as SybaseMelt;
            if (melt != null)
            {
                if (String.IsNullOrEmpty(MeltNumberSought))
                    return true;
                else
                    if (melt.me_num == null)
                    return false;
                else
                    return (melt.me_num.IndexOf(MeltNumberSought, StringComparison.OrdinalIgnoreCase) >= 0);
            }
            return false;
        }
    }
}
