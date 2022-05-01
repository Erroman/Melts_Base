﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Oracle.EntityFrameworkCore;
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
using Melts_Base.PostgresFiles;
using Melts_Base.OracleModels;
using Melts_Base.ViewModel;

namespace Melts_Base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        private readonly MeltContext meltsContext = new MeltContext();
        private readonly epasportContext meltsPostgresContext = new epasportContext();
        private readonly ModelContext meltsOracleContext = new ModelContext();
        private CollectionViewSource meltsViewSource;
        private CollectionViewSource meltsPostgresViewSource;
        private CollectionViewSource meltsOracleViewSource;
        private DateFilter dateFilter;
        public MainWindow()
        {
            InitializeComponent();
            meltsViewSource = (CollectionViewSource)FindResource(nameof(meltsViewSource));
            meltsPostgresViewSource = (CollectionViewSource)FindResource(nameof(meltsPostgresViewSource));
            meltsOracleViewSource = (CollectionViewSource)FindResource(nameof(meltsOracleViewSource));
            //dateFilter = (DateFilter)FindResource(nameof(datefilter));
            dateFilter = (DateFilter)Resources[nameof(dateFilter)];
        }

        private void RibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            meltsContext.Database.EnsureCreated();
            meltsContext.Melts.Load();
            meltsViewSource.Source = meltsContext.Melts.Local.ToObservableCollection();

            //Поставить проверку соединения
            if (meltsPostgresContext.Database.CanConnect()) { 

            //meltsPostgresContext.Database.EnsureCreated();
            meltsPostgresContext.Melts.Load();
            //meltsPostgresViewSource.Source = meltsPostgresContext.Melts.ToList();
            meltsPostgresViewSource.Source = meltsPostgresContext.Melts.Local.ToObservableCollection();
            }
            if (meltsOracleContext.Database.CanConnect())
            {
                MessageBox.Show("Сonnection with Oracle granted!");
                meltsOracleContext.Melts31s.Load();
                meltsOracleViewSource.Source = meltsOracleContext.Melts31s.Local.ToObservableCollection();




            }
            else MessageBox.Show("No connection with Oracle!");
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            meltsContext.SaveChanges();
            meltsContext.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (meltsPostgresContext.Database.CanConnect())
            {
                var sqlightAllMelts = meltsContext.Melts.ToList();
                var postgresAllMelts = meltsPostgresContext.Melts.ToList();
                PumpPostgresData(postgresAllMelts, sqlightAllMelts);
                MessageBox.Show("Подкачка выполнена!");
            }
            else MessageBox.Show("Соединение с цеховой базой Postgres нарушено!");
        }
        private void PumpPostgresData(List<PostgresFiles.Melt> listPostgresMelts, List<Melt> listSqLiteMelts) 
        {
            //var listNewMelts = new List<PostgresFiles.Melt>();
            //var listChangedMelts = new List<PostgresFiles.Melt>();
            var MeltFound = false;
            foreach (var postgresMelt in listPostgresMelts) 
            {
                //проверяем, есть ли запись в базе SqLite, соответствующая плавке в базе Postgres
                //сравнение ведём по номеру плавки
                //если записи такой нет, добавляем
                foreach(var melt in listSqLiteMelts) 
                { 
                    if(postgresMelt.Nplav == melt.MeltNumber) 
                    {
                        MeltFound = true;
                    }
                }
                if (!MeltFound) 
                {
                    //конструируется новая запись по плавке для SqLite
                    var newMelt = new Melt()
                    {
                        MeltNumber = postgresMelt.Nplav,
                        MeltDate = postgresMelt.Dpl,
                        AlloyName = postgresMelt.Spl,
                        AlloyIndex = postgresMelt.Ind,
                        MouldSet = postgresMelt.Nkompl,
                        ElectrodeDiameter = postgresMelt.Del,
                        MelterNumber = postgresMelt.TabNPl,
                        TEKNumber = postgresMelt.Ntek,
                        IL_UiS_SHN = postgresMelt.NomInsp,
                        Contract = postgresMelt.Kont,
                        Supplement = postgresMelt.Pril,
                        Purpose = postgresMelt.Nazn,
                        IngotDiameter = postgresMelt.Diam

                    };
                    meltsContext.Add<Melt>(newMelt);
                    meltsContext.SaveChanges();
                }
            }
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var melt = e.Item as Melts31;
            if (melt != null)
            {
                DateTime startdate;
                DateTime enddate;
                bool startdateFilterSet = DateTime.TryParse(this.MeltsStartDate.Text,out startdate);
                bool enddateFilterSet   = DateTime.TryParse(this.MeltsEndDate.Text, out enddate);
                e.Accepted = 
                    (!startdateFilterSet || (startdate <= melt.Meltdate)) && (!enddateFilterSet || (melt.Meltdate <=  enddate));

            }
            
        }
    }
}
