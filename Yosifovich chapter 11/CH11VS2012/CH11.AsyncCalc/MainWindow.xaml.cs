using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CH11.AsyncCalc {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}
		static int CountPrimes(int from, int to) {
			int total = 0;
			for(int i = from; i <= to; i++) {
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
			return total;
		}

		private void OnCalculate(object sender, RoutedEventArgs e) {
			try {
				int first = int.Parse(_from.Text), last = int.Parse(_to.Text);
				var button = (Button)sender;
				button.IsEnabled = false;
				ThreadPool.QueueUserWorkItem(_ => {
					int total = CountPrimes(first, last);
					Dispatcher.BeginInvoke(new Action(() => {
						_result.Text = "Total Primes: " + total.ToString();
						button.IsEnabled = true;
					}));
				});
			}
			catch(FormatException ex) {
				MessageBox.Show(ex.Message);
			}

		}
	}
}
