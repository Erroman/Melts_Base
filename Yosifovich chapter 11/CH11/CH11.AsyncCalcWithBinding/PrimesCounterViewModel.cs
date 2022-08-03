using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CH11.AsyncCalcWithBinding {
	class PrimesCounterViewModel : INotifyPropertyChanged {
		public event PropertyChangedEventHandler PropertyChanged;

		void OnPropertyChanged(string propertyName) {
			var pc = PropertyChanged;
			if(pc != null)
				pc(this, new PropertyChangedEventArgs(propertyName));
		}

		int _first, _last;

		public int Last {
			get { return _last; }
			set { 
				_last = value;
				OnPropertyChanged("Last");
			}
		}

		public int First {
			get { return _first; }
			set { 
				_first = value;
				OnPropertyChanged("First");
			}
		}

		bool _isBusy;

		public bool IsBusy {
			get { return _isBusy; }
			set { 
				_isBusy = value;
				OnPropertyChanged("IsBusy");
				OnPropertyChanged("IsNotBusy");
			}
		}

		public bool IsNotBusy {
			get { return !_isBusy; }
		}

		int _count;

		public int Count {
			get { return _count; }
			set { 
				_count = value;
				OnPropertyChanged("Count");
			}
		}
	}
}
