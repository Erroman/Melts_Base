using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Melts_Base.PostgreFiles;

namespace Melts_Base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        private readonly MeltContext meltContext = new MeltContext();
        private readonly epasportContext meltPostgresContext = new epasportContext();
        private CollectionViewSource meltsViewSource;
        private CollectionViewSource meltsPostgresViewSource;
        public MainWindow()
        {
            InitializeComponent();
            meltsViewSource = (CollectionViewSource)FindResource(nameof(meltsViewSource));
            meltsPostgresViewSource = (CollectionViewSource)FindResource(nameof(meltsPostgresViewSource));
        }

        private void RibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            meltContext.Database.EnsureCreated();
            meltContext.Melts.Load();
            meltsViewSource.Source = meltContext.Melts.Local.ToObservableCollection();
            meltPostgresContext.Database.EnsureCreated();
            meltPostgresContext.Melts.Load();
            //meltsPostgresViewSource.Source = meltPostgresContext.Melts.ToList();
            meltsPostgresViewSource.Source = meltPostgresContext.Melts.Local.ToObservableCollection();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            meltContext.SaveChanges();
            meltContext.Dispose();
        }
    }
}
