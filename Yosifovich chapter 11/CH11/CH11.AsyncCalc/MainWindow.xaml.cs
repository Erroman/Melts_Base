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
using System.Windows.Threading;
namespace CH11.AsyncCalc {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		CancellationTokenSource _cts;

		public MainWindow() {
			InitializeComponent();
			MessageBox.Show($"I'll show something:{SynchronizationContext.Current.GetType()}");
		}

		static int CountPrimes(int from, int to, CancellationToken ct) {
			int total = 0;
			for(int i = from; i <= to; i++) {
				if(ct.IsCancellationRequested) 
					return -1;
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

		//private void OnCalculate(object sender, RoutedEventArgs e)
		//{
		//	try
		//	{
		//		int first = int.Parse(_from.Text), last = int.Parse(_to.Text);
		//		_calcButton.IsEnabled = false;
		//		_cancelButton.IsEnabled = true;
		//		_cts = new CancellationTokenSource();
		//		ThreadPool.QueueUserWorkItem(_ =>
		//		{
		//			int total = CountPrimes(first, last, _cts.Token);
		//			Dispatcher.BeginInvoke(new Action(() =>
		//			{
		//				_result.Text = "Total Primes: " + total.ToString();
		//				_cancelButton.IsEnabled = false;
		//				_calcButton.IsEnabled = true;
		//			}));
		//		});
		//	}
		//	catch (FormatException ex)
		//	{
		//		MessageBox.Show(ex.Message);
		//	}

		//}

		private void OnCalculate2(object sender, RoutedEventArgs e) {
			try {
				int first = int.Parse(_from.Text), last = int.Parse(_to.Text);
				_calcButton.IsEnabled = false;
				_cancelButton.IsEnabled = true;

				_cts = new CancellationTokenSource();
				var sc = SynchronizationContext.Current;
				ThreadPool.QueueUserWorkItem(_ => {
					int total = CountPrimes(first, last, _cts.Token);
					sc.Post(delegate {
						_result.Text = total < 0 ? "Cancelled!" : "Total Primes: " + total.ToString();
						_calcButton.IsEnabled = true;
						_cancelButton.IsEnabled = false;
					}, null);
				});
			}
			catch(FormatException ex) {
				MessageBox.Show(ex.Message);
			}

		}

		private void OnCancel(object sender, RoutedEventArgs e) {
			if(_cts != null) {
				_cts.Cancel();
				_cts = null;
			}
		}
	}
}
