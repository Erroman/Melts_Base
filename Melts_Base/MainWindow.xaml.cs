﻿using System;
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

namespace Melts_Base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        private readonly MeltContext meltContext = new MeltContext();
        private CollectionViewSource meltsViewSource;
        public MainWindow()
        {
            InitializeComponent();
            meltsViewSource = (CollectionViewSource)FindResource(nameof(meltsViewSource));
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
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            meltContext.SaveChanges();
            meltContext.Dispose();
        }
    }
}
