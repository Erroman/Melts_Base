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
using System.Windows.Threading;
using System.Reflection;

namespace CH11.PeriodicUpdates {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window {
		DispatcherTimer _timer = new DispatcherTimer();
		int _counter;
		Random _rnd = new Random();

		public MainWindow() {
			InitializeComponent();

			_timer.Interval = TimeSpan.FromSeconds(.5);
			_timer.Tick += delegate {
				_time.Text = DateTime.Now.ToLongTimeString();
				if(++_counter == 3) {
					var brushes = typeof(Brushes).GetProperties(BindingFlags.Public | BindingFlags.Static);
					_e1.Fill = (Brush)brushes[_rnd.Next(brushes.Length)].GetValue(null, null);
					_counter = 0;
				}
			};
			_timer.Start();
		}
	}
}
