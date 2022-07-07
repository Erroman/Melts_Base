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
using Melts_Base.OracleViewModel;
using Melts_Base.OracleModels;
using Melts_Base.SybaseViewModel;
using Melts_Base.SybaseModels;
using Melts_Base.SQLiteViewModel;
using Melts_Base.SQLiteModels;
using System.Data.Odbc;
using System.Data;
using Excel = Microsoft.Office.Interop.Excel;       //Add Microsoft Excel 16.0 Object Library to the project
                                                    //by Dependencies/Add COM Reference...


//using Microsoft.Office.Interop.Excel;

namespace Melts_Base
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
        
    {
        private readonly MeltContext meltsContext = new MeltContext();
        private readonly ModelPlantContext meltsPlantOracleContext = new ModelPlantContext();
        private ObservableMeltsViewModel observableMeltsViewModel;
        private ObservableOracleMeltsViewModel observableOracleMeltsViewModel;
        private ObservableSybaseMeltsViewModel  observableSybaseMeltsViewModel;
        OdbcConnectionStringBuilder constr = new OdbcConnectionStringBuilder()
        {
            ["Dsn"] = "sybase",
            ["uid"] = "romanovskii",
            ["pwd"] = "12345"
        };
        ObservableCollection<Melt> localSQLLiteMelts = null;
        List<SybaseMelt> sybaseMelts = new List<SybaseMelt>();
        List<OracleMelt> oracleMelts = null;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void RibbonApplicationMenuItem_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            localdateZap.Width = new DataGridLength(120);
            localdateClose.Width = new DataGridLength(120);
            localnPlav.Width = new DataGridLength(90);
            dateZap.Width = new DataGridLength(120);
            dateClose.Width = new DataGridLength(120);
            nPlav.Width = new DataGridLength(90);

            readFromSQLiteLocal();
            readFromSybase();
            readFromOracle();
        }
        private void readFromSQLiteLocal() 
        {
            meltsContext.Database.EnsureCreated();
            meltsContext.Melts.Load();
            localSQLLiteMelts = meltsContext.Melts.Local.ToObservableCollection();
            observableMeltsViewModel = new ObservableMeltsViewModel(localSQLLiteMelts);
            localcopyGrid.DataContext = observableMeltsViewModel;
            localZapuskStartDate.DataContext = observableMeltsViewModel;
            localZapuskEndDate.DataContext = observableMeltsViewModel;
            localCloseStartDate.DataContext = observableMeltsViewModel;
            localCloseEndDate.DataContext = observableMeltsViewModel;
            localPlantMeltNumberSought.DataContext = observableMeltsViewModel;
        }
        private bool readFromSybase() 
        {
            using (OdbcConnection connection = new OdbcConnection(constr.ConnectionString))
            {
                try
                {
                    connection.Open();


                    if (connection.State == ConnectionState.Open)
                    {
                        string queryString = @"SELECT * FROM ""DBA"".""rmelts""";
                        OdbcCommand command = new OdbcCommand(queryString);
                        command.Connection = connection;
                        OdbcDataReader odbcDataReader = command.ExecuteReader();
                        while (odbcDataReader.Read())
                        {
                            DateTime melt_end;
                            DateTime.TryParse(odbcDataReader["me_end"].ToString(), out melt_end);
                            var melt_sybase = new SybaseMelt
                            {
                                me_id = odbcDataReader["me_id"].ToString(),
                                me_num = odbcDataReader["me_num"].ToString(),
                                eq_id = odbcDataReader["eq_id"].ToString(),
                                me_beg = DateTime.Parse(odbcDataReader["me_beg"].ToString()),
                                me_end = melt_end == DateTime.Parse("01.01.0001") ? null : melt_end,
                                me_splav = odbcDataReader["me_splav"].ToString(),
                                sp_name = odbcDataReader["sp_name"].ToString(),
                                me_mould = odbcDataReader["me_mould"].ToString(),
                                me_del = odbcDataReader["me_del"].ToString(),
                                me_weigth = odbcDataReader["me_weigth"].ToString(),
                                me_ukaz = odbcDataReader["me_ukaz"].ToString(),
                                me_kont = odbcDataReader["me_kont"].ToString(),
                                me_pril = odbcDataReader["me_pril"].ToString(),
                                me_nazn = odbcDataReader["me_nazn"].ToString(),
                                me_diam = odbcDataReader["me_diam"].ToString(),
                                me_pos = odbcDataReader["me_pos"].ToString(),
                                me_kat = odbcDataReader["me_kat"].ToString(),
                                sp_id = odbcDataReader["sp_id"].ToString(),
                                me_energy = odbcDataReader["me_energy"].ToString(),
                            };
                            sybaseMelts.Add(melt_sybase); ;
                        }
                        observableSybaseMeltsViewModel = new ObservableSybaseMeltsViewModel(new ObservableCollection<SybaseMelt>(sybaseMelts));
                        shop31Grid.DataContext = observableSybaseMeltsViewModel;
                        shop31PlantMeltNumberSought.DataContext = observableSybaseMeltsViewModel;
                        shop31ZapuskStartDate.DataContext = observableSybaseMeltsViewModel;
                        shop31ZapuskEndDate.DataContext = observableSybaseMeltsViewModel;
                        return true;
                    }
                    else { MessageBox.Show("The connection to Sybase is " + connection.State.ToString()); return true; }
                    // The connection is automatically closed at
                    // the end of the Using block.
                }
                catch { MessageBox.Show("No connection with Sybase."); return false; }
            }
        }
        private bool readFromOracle() 
        {
            if (meltsPlantOracleContext.Database.CanConnect())
            {
                //MessageBox.Show("Сonnection with Plant's Oracle granted!");

                meltsPlantOracleContext.Melt31s.Load();
                oracleMelts = meltsPlantOracleContext.Melt31s.ToList<OracleMelt>();
                observableOracleMeltsViewModel = new ObservableOracleMeltsViewModel(new ObservableCollection<OracleMelt>(oracleMelts));
                oracleGrid.DataContext = observableOracleMeltsViewModel;
                ZapuskStartDate.DataContext = observableOracleMeltsViewModel;
                ZapuskEndDate.DataContext = observableOracleMeltsViewModel;
                CloseStartDate.DataContext = observableOracleMeltsViewModel;
                CloseEndDate.DataContext = observableOracleMeltsViewModel;
                PlantMeltNumberSought.DataContext = observableOracleMeltsViewModel;
                return true;
            }
            else { MessageBox.Show("No connection with Plant's Oracle!"); return false; }
        }
        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            meltsContext.SaveChanges();
            meltsContext.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if(readFromSybase() && readFromOracle()) {
                PumpPlantData(sybaseMelts,oracleMelts,localSQLLiteMelts.ToList());
                MessageBox.Show("Подкачка выполнена!");
            }
            else MessageBox.Show("Подкачка невозможна!");
        }
        private void PumpPlantData(List<SybaseMelt> listSybaseMelts,List<OracleMelt> listOracleMelts, List<Melt> listSqLiteMelts)
        {
            //var listNewMelts = new List<PostgresFiles.MeltPostgres>();
            //var listChangedMelts = new List<PostgresFiles.MeltPostgres>();
            int fullPlantOracleCount = listOracleMelts.Count();
            int fullPlantSybaseCount = listSybaseMelts.Count();
            int fullLocalCount = listSqLiteMelts.Count();
            //return; 

            foreach (var sybaseMelt in listSybaseMelts)
            {
                var MeltFound = false;
                //находим информацию по плавке с данным номером в listOracleMelts,если нашли,составляем полную плавку,
                //добавляя поля из найденной записи.
                var oracleMelt = listOracleMelts.Where<OracleMelt>(p => p.Nplav == sybaseMelt.me_num).FirstOrDefault<OracleMelt>();
                if (oracleMelt != null) 
                {
                    sybaseMelt.oracle_Ins = oracleMelt.Ins;
                    sybaseMelt.oracle_Tek = oracleMelt.Tek;
                }
                //проверяем, есть ли запись в локальной базе SqLite, соответствующая плавке в базе Sybase
                //сравнение ведём по номеру плавки и hash-коду.
                //если записи такой нет, добавляем запись с данным номером плавки, содержащую
                //информацию из Sybase м Oracle
                foreach (var melt in listSqLiteMelts)
                {

                    if (sybaseMelt.me_num == melt.me_num && sybaseMelt.MyHashCode() == melt.MyHashCode())
                    {
                        MeltFound = true;
                    }
                }
                if (!MeltFound)
                {
                    //конструируется новая запись по плавке для SqLite,
                    //дополнительные поля берутся из Oracle,основной список полей берём из Sybase
                    var newMelt = new Melt()
                    {
                        eq_id = sybaseMelt.eq_id,
                        me_num = sybaseMelt.me_num,
                        me_beg = sybaseMelt.me_beg,
                        me_end = sybaseMelt.me_end,
                        me_splav=sybaseMelt.me_splav,
                        sp_name = sybaseMelt.sp_name,
                        me_mould=sybaseMelt.me_mould,
                        me_del=sybaseMelt.me_del,
                        me_ukaz=sybaseMelt.me_ukaz,
                        me_kont=sybaseMelt.me_kont,
                        me_pril=sybaseMelt.me_pril,
                        me_nazn=sybaseMelt.me_nazn,
                        me_diam=sybaseMelt.me_diam,
                        me_weigth=sybaseMelt.me_weigth,
                        me_zakaz=sybaseMelt.me_zakaz,
                        me_pos=sybaseMelt.me_pos,
                        me_kat=sybaseMelt.me_kat,
                        sp_id=sybaseMelt.sp_id,
                        me_energy=sybaseMelt.me_energy,
                        oracle_Ins=sybaseMelt.oracle_Ins,
                        oracle_Tek=sybaseMelt.oracle_Tek,
                    };
                    meltsContext.Add<Melt>(newMelt);
                }

            }
            meltsContext.SaveChanges();
        }

        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add();
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
            var numberOfColumns =  localcopyGrid.Columns.Count;
            var numberOfRows = localcopyGrid.Items.Count;
            for(int j = 0; j < numberOfColumns; j++)
            {
                //Range myRange = (Range)sheet1.Cells[1, j];
                sheet1.Cells[1, j+1].Font.Bold = true;
                sheet1.Columns[j+1].ColumnWidth = 15;
                sheet1.Cells[1,j+1] = localcopyGrid.Columns[j].Header;
            }

            for(int j = 0;j< numberOfColumns;j++)
                {
                for (int i = 0; i < numberOfRows; i++)
                    {

                    sheet1.Cells[i + 2, j + 1] = (localcopyGrid.Columns[j].GetCellContent(localcopyGrid.Items[i]) as TextBlock)?.Text;
                    //sheet1.Cells[i + 2, j + 1] = localcopyGrid.Columns[j].GetCellContent(localcopyGrid.Items[i]);
                }

        }

    }
}
}
