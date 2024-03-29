﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Melts_Base
{
    /// <summary>
    /// Interaction logic for SetParameters.xaml
    /// </summary>
    public partial class SetParameters : Window
    {
        public SetParameters()
        {
            InitializeComponent();
            TabsVisibility.IsChecked = Properties.Settings.Default.TabsVisibility; 
        }
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Properties.Settings.Default.TabsVisibility = (bool)TabsVisibility.IsChecked;

        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            this.DialogResult = true;
        }
    }
}
