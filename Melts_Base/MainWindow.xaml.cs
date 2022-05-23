using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Melts_Base.SQLiteViewModel;
using Melts_Base.SQLiteModels;

namespace Melts_Base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        private readonly MeltContext meltsContext = new MeltContext();
        private readonly epasportContext meltsPostgresContext = new epasportContext();
        private readonly ModelPlantContext meltsPlantOracleContext = new ModelPlantContext();
        private CollectionViewSource meltsPlantOracleViewSource;
        private ObservableMeltsViewModel observableMeltsViewModel;

        public MainWindow()
        {
            InitializeComponent();
            meltsContext.Database.EnsureCreated();
            meltsContext.Melts.Load();
            observableMeltsViewModel = new ObservableMeltsViewModel(meltsContext.Melts.Local.ToObservableCollection());
            this.DataContext = observableMeltsViewModel;
        }

        private void RibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MeltsStartDate.DataContext = observableMeltsViewModel;
            MeltsEndDate.DataContext = observableMeltsViewModel;
            MeltNumberSought.DataContext = observableMeltsViewModel;

            if (meltsPlantOracleContext.Database.CanConnect()) 
            {
                MessageBox.Show("Сonnection with Plant's Oracle granted!");
                meltsPlantOracleContext.Melt31s.Load();
                meltsPlantOracleViewSource = (CollectionViewSource)FindResource(nameof(meltsPlantOracleViewSource));
                meltsPlantOracleViewSource.Source = new ObservableCollection<V_NC24_PLAV31>(meltsPlantOracleContext.Melt31s.ToList<V_NC24_PLAV31>());
            }
            else MessageBox.Show("No connection with Plant's Oracle!");
        }
        
                
        private void DateFilter_doFiltering()
        {
            CollectionViewSource.GetDefaultView(dataGrid1.ItemsSource).Refresh();
            //throw new NotImplementedException();
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            meltsContext.SaveChanges();
            meltsContext.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (meltsPlantOracleContext.Database.CanConnect())
            {
                var sqlightAllMelts = meltsContext.Melts.ToList();
                var plantOracleAllMelts = meltsPlantOracleContext.Melt31s.ToList();
                PumpPlantOracleData(plantOracleAllMelts, sqlightAllMelts);
                MessageBox.Show("Подкачка выполнена!");
            }
            else MessageBox.Show("Соединение с цеховой базой Oracle отсутствует!");
        }
        private void PumpPlantOracleData(List<V_NC24_PLAV31> listOracleMelts, List<Melt> listSqLiteMelts) 
        {
            //var listNewMelts = new List<PostgresFiles.MeltPostgres>();
            //var listChangedMelts = new List<PostgresFiles.MeltPostgres>();
            var MeltFound = false;
            foreach (var oracleMelt in listOracleMelts) 
            {
                //проверяем, есть ли запись в базе SqLite, соответствующая плавке в базе Postgres
                //сравнение ведём по номеру плавки
                //если записи такой нет, добавляем
                foreach(var melt in listSqLiteMelts) 
                { 
                    if(oracleMelt.Nplav == melt.MeltNumber) 
                    {
                        MeltFound = true;
                    }
                }
                if (!MeltFound) 
                {
                    //конструируется новая запись по плавке для SqLite
                    var newMelt = new Melt()
                    {
                        //MeltNumber = postgresMelt.Nplav,
                        //MeltDate = postgresMelt.Dpl,
                        //AlloyName = postgresMelt.Spl,
                        //AlloyIndex = postgresMelt.Ind,
                        //MouldSet = postgresMelt.Nkompl,
                        //ElectrodeDiameter = postgresMelt.Del,
                        //MelterNumber = postgresMelt.TabNPl,
                        //TEKNumber = postgresMelt.Ntek,
                        //IL_UiS_SHN = postgresMelt.NomInsp,
                        //Contract = postgresMelt.Kont,
                        //Supplement = postgresMelt.Pril,
                        //Purpose = postgresMelt.Nazn,
                        //IngotDiameter = postgresMelt.Diam

                    };
                    meltsContext.Add<Melt>(newMelt);
                    meltsContext.SaveChanges();
                }
            }
        }

        private void CollectionViewSource_Filter(object sender, FilterEventArgs e)
        {
            var melt = e.Item as Melt;
            if (melt != null)
            {
                DateOnly startdate;
                DateOnly enddate;
                bool startdateFilterSet = DateOnly.TryParse(this.MeltsStartDate.Text,out startdate);
                bool enddateFilterSet   = DateOnly.TryParse(this.MeltsEndDate.Text, out enddate);
                e.Accepted = 
                    (!startdateFilterSet || (startdate <= melt.MeltDate)) && (!enddateFilterSet || (melt.MeltDate <=  enddate));

            }
            
        }
            private bool ListCollectionView_Filter(object Item)
            {
                var melt = Item as Melt;
                if (melt != null)
                {
                    DateOnly startdate;
                    DateOnly enddate;
                    bool startdateFilterSet = DateOnly.TryParse(this.MeltsStartDate.Text, out startdate);
                    bool enddateFilterSet = DateOnly.TryParse(this.MeltsEndDate.Text, out enddate);
                    return
                        (!startdateFilterSet || (startdate <= melt.MeltDate)) && (!enddateFilterSet || (melt.MeltDate <= enddate));

                }return false;

            }
    }
}
