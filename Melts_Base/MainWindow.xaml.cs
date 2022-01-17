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

            //Поставить проверку соединения
            if (meltPostgresContext.Database.CanConnect()) { 

            //meltPostgresContext.Database.EnsureCreated();
            meltPostgresContext.Melts.Load();
            //meltsPostgresViewSource.Source = meltPostgresContext.Melts.ToList();
            meltsPostgresViewSource.Source = meltPostgresContext.Melts.Local.ToObservableCollection();
            }
            //if()
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            meltContext.SaveChanges();
            meltContext.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (meltPostgresContext.Database.CanConnect())
            {
                var sqlightAllMelts = meltContext.Melts.ToList();
                var postgresAllMelts = meltPostgresContext.Melts.ToList();
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
                    meltContext.Add<Melt>(newMelt);
                    meltContext.SaveChanges();
                }
            }
        }
    }
}
