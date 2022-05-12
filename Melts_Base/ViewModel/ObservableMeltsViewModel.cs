using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Collections.ObjectModel;

namespace Melts_Base.ViewModel
{
    internal class ObservableMeltsViewModel : INotifyPropertyChanged
    {
        public string MeltsStartDate;
        public string MeltsEndDate;   
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableMeltsViewModel(ObservableCollection<Melt> melts) 
        {
            Melts = melts;

            _view = new ListCollectionView(melts);
            _view.Filter = ListCollectionView_Filter;
        }
        private ObservableCollection<Melt> _melts;
        public ObservableCollection<Melt> Melts 
        { 
            get =>_melts;
            set  {_melts = value;OnPropertyChanged(); }
            
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
        private bool ListCollectionView_Filter(object Item)
        {
            //var melt = Item as Melt;
            //if (melt != null)
            //{
            //    DateOnly startdate;
            //    DateOnly enddate;
            //    bool startdateFilterSet = DateOnly.TryParse(StartDate, out startdate);
            //    bool enddateFilterSet = DateOnly.TryParse(EndDate, out enddate);
            //    return
            //        (!startdateFilterSet || (startdate <= melt.MeltDate)) && (!enddateFilterSet || (melt.MeltDate <= enddate));

            //}
            //return false;
            return meltDateFilter(Item)&& meltNumberFilter(Item) ;

        }
        private bool meltDateFilter(object Item) 
        {

            var melt = Item as Melt;
            if (melt != null)
            {
                DateOnly startdate;
                DateOnly enddate;
                bool startdateFilterSet = DateOnly.TryParse(StartDate, out startdate);
                bool enddateFilterSet = DateOnly.TryParse(EndDate, out enddate);
                return
                    (!startdateFilterSet || (startdate <= melt.MeltDate)) && (!enddateFilterSet || (melt.MeltDate <= enddate));

            }
            return false;
        }
        private bool meltNumberFilter(object Item) 
        {
            var melt = Item as Melt;
            if (melt != null) 
            {
                int meltnumber;
            }
            return true; 
        }
    }
}
