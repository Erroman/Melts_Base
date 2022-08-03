using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;

namespace CH11.AsyncCalcWithBinding {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		PrimesCounterViewModel _vm = new PrimesCounterViewModel();

		public MainWindow() {
			InitializeComponent();

			DataContext = _vm;
		}

		private void OnCalculate(object sender, RoutedEventArgs e) {
			_vm.IsBusy = true;
			ThreadPool.QueueUserWorkItem(_ => {
				int total = 0;
				for(int i = _vm.First; i <= _vm.Last; i++) {
					bool isPrime = true;
					int limit = (int)Math.Sqrt(i);
					for(int j = 2; j <= limit; j++)
						if(i % j == 0) {
							isPrime = false;
							break;
						}
					if(isPrime)
						total++;
				}
				_vm.Count = total;
				_vm.IsBusy = false;
			});
		}

		private void OnCancel(object sender, RoutedEventArgs e) {

		}
	}
}
