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

namespace MyAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void  Button_Click(object sender, RoutedEventArgs e)
        {
            progressBar.Value = 50;
            await longTask();
            progressBar.Value += 50;
        }
        async Task<int>  longTask() 
        { 
            var task = Task.Run(()=> 
            { 
                Thread.Sleep(1000);
                Thread.Sleep(1000);
                Thread.Sleep(1000);
                Thread.Sleep(1000);
                Thread.Sleep(1000);
                return 0; 
            }
            );
            return await task;
        }
    }
}
