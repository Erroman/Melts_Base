using System;
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

namespace Melts_Base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        private readonly MeltContext meltsContext = new MeltContext();
        private readonly epasportContext meltsPostgresContext = new epasportContext();
        private readonly OracleContext meltsOracleContext = new OracleContext();
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
    }
}
