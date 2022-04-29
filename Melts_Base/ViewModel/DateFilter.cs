using System.Windows;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Melts_Base.ViewModel
{
    internal class DateFilter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        { 
            PropertyChanged(this,new PropertyChangedEventArgs(propertyName));
        }
            

        string startDate;
        public string StartDate
        {
            get => startDate;
            set
            {
                if (startDate == value) return;
                startDate = value;
                OnPropertyChanged();
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

            }
        }
    }
}
