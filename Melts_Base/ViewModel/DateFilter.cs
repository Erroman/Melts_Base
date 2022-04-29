using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace Melts_Base.ViewModel
{
    internal class DateFilter : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
}
