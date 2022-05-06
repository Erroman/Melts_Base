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
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public ObservableMeltsViewModel(ObservableCollection<Melt> melts) 
        { 
            View = new ListCollectionView(melts);
            View.Filter = ListCollectionView_Filter;
        }
        public ListCollectionView View { get; set; }
        private bool ListCollectionView_Filter(object Item)
        {
            var melt = Item as Melt;
            if (melt != null)
            {
                DateOnly startdate;
                DateOnly enddate;
                bool startdateFilterSet = DateOnly.TryParse(MeltsStartDate, out startdate);
                bool enddateFilterSet = DateOnly.TryParse(MeltsEndDate, out enddate);
                return
                    (!startdateFilterSet || (startdate <= melt.MeltDate)) && (!enddateFilterSet || (melt.MeltDate <= enddate));

            }
            return false;

        }
    }
}
