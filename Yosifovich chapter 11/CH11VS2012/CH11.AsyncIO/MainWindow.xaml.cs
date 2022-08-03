using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
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

namespace CH11.AsyncIO {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		public MainWindow() {
			InitializeComponent();
		}

		private void OnGetDataSync(object sender, RoutedEventArgs e) {
			var wc = new WebClient();
			_result.Text = "Please wait...";
			try {
				_result.Text = wc.DownloadString("http://msdn.microsoft.com");
			}
			catch(WebException ex) {
				_result.Text = "Error: " + ex.Message;
			}

		}

		private void OnGetDataAsyncOld(object sender, RoutedEventArgs e) {
			var wc = new WebClient();
			_result.Text = "Please wait...";
			wc.DownloadStringCompleted += (s, args) => {
				if(args.Error != null)
					_result.Text = "Error: " + args.Error.Message;
				else
					_result.Text = args.Result;
			};
			wc.DownloadStringAsync(new Uri("http://msdn.microsoft.com"));
		}

		private async void OnGetDataAsyncNew(object sender, RoutedEventArgs e) {
			var wc = new WebClient();
			_result.Text = "Please wait...";
			try {
				_result.Text = await wc.DownloadStringTaskAsync("http://msdn.microsoft.com");
			}
			catch(WebException ex) {
				_result.Text = "Error: " + ex.Message;
			}
		}

		private async void OnGetWithTimeout(object sender, RoutedEventArgs e) {
			var wc = new WebClient();
			var t1 = wc.DownloadStringTaskAsync("http://msdn.microsoft.com");
			var t2 = Task.Delay(3000);

			var tresult = await Task.WhenAny(t1, t2);
			if(tresult == t1)
				_result.Text = t1.Result;
			else
				_result.Text = "Timeout!";
		}

	}
}
