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
        List<V_NC24_PLAV31> oracleMelts = null;
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
        private void readFromSybase() 
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
                    }
                    else MessageBox.Show("The connection to Sybase is " + connection.State.ToString());
                    // The connection is automatically closed at
                    // the end of the Using block.
                }
                catch { MessageBox.Show("No connection with Sybase."); }
            }
        }
        private void readFromOracle() 
        {
            if (meltsPlantOracleContext.Database.CanConnect())
            {
                //MessageBox.Show("Сonnection with Plant's Oracle granted!");

                meltsPlantOracleContext.Melt31s.Load();
                oracleMelts = meltsPlantOracleContext.Melt31s.ToList<V_NC24_PLAV31>();
                observableOracleMeltsViewModel = new ObservableOracleMeltsViewModel(new ObservableCollection<V_NC24_PLAV31>(oracleMelts));
                oracleGrid.DataContext = observableOracleMeltsViewModel;
                ZapuskStartDate.DataContext = observableOracleMeltsViewModel;
                ZapuskEndDate.DataContext = observableOracleMeltsViewModel;
                CloseStartDate.DataContext = observableOracleMeltsViewModel;
                CloseEndDate.DataContext = observableOracleMeltsViewModel;
                PlantMeltNumberSought.DataContext = observableOracleMeltsViewModel;
            }
            else MessageBox.Show("No connection with Plant's Oracle!");
        }
        
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            meltsContext.SaveChanges();
            meltsContext.Dispose();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //using (OdbcConnection connection = new OdbcConnection(constr.ConnectionString)) { }
            //    if (meltsPlantOracleContext.Database.CanConnect())
            //{
                //var sqlightAllMelts = meltsContext.Melts.ToList();
                //var plantOracleAllMelts = meltsPlantOracleContext.Melt31s.ToList();
                //var sqlightAllMelts = localSQLLiteMelts.ToList();
                //var plantOracleAllMelts = oracleMelts;
                //PumpPlantOracleData(plantOracleAllMelts, sqlightAllMelts);
                PumpPlantData(sybaseMelts,oracleMelts,localSQLLiteMelts.ToList());
                MessageBox.Show("Подкачка выполнена!");
            //}
            //else MessageBox.Show("Соединение с цеховой базой Oracle отсутствует!");
        }
        private void PumpPlantOracleData(List<V_NC24_PLAV31> listOracleMelts, List<Melt> listSqLiteMelts) 
        {
            //var listNewMelts = new List<PostgresFiles.MeltPostgres>();
            //var listChangedMelts = new List<PostgresFiles.MeltPostgres>();
            int fullPlantCount = listOracleMelts.Count();               
            int fullLocalCount = listSqLiteMelts.Count();
            int meltPlantCount = 0;
            int meltLocalCount = 0;

            foreach (var oracleMelt in listOracleMelts) 
            {
                var MeltFound = false;
                meltPlantCount++;
                //проверяем, есть ли запись в локальной базе SqLite, соответствующая плавке в базе Oracle
                //сравнение ведём по номеру плавки и hash-коду
                //если записи такой нет, добавляем
                foreach (var melt in listSqLiteMelts) 
                {
                    meltLocalCount++;
                    if(oracleMelt.Nplav == melt.Nplav && oracleMelt.MyHashCode()==melt.MyHashCode()) 
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
                        Npech = oracleMelt.Npech,
                        Nplav = oracleMelt.Nplav,
                        Npart = oracleMelt.Npart,
                        RazmPasp = oracleMelt.RazmPasp,
                        Splav = oracleMelt.Splav,
                        Ins = oracleMelt.Ins,
                        Tek = oracleMelt.Tek,
                        Pereplav = oracleMelt.Pereplav,
                        OkonchPereplav = oracleMelt.OkonchPereplav,
                        DateZap = oracleMelt.DateZap,
                        DateClose = oracleMelt.DateClose,
                        SumVesZapusk = oracleMelt.SumVesZapusk,
                        Zapusk31 = oracleMelt.Zapusk31,
                        ZapuskNakl = oracleMelt.ZapuskNakl,
                        ZapuskPpf = oracleMelt.ZapuskPpf,
                        Dsd = oracleMelt.Dsd,
                        Ncp = oracleMelt.Ncp,
                        VesSdch = oracleMelt.VesSdch,
                        RazmSdch = oracleMelt.RazmSdch,
                        MfgOrderId = oracleMelt.MfgOrderId,
                        DemandOrderId = oracleMelt.DemandOrderId,
                        Poz = oracleMelt.Poz,
                        PozNaim = oracleMelt.PozNaim,
                        PozRazm = oracleMelt.PozRazm,
                        PozIl = oracleMelt.PozIl,
                    };
                    meltsContext.Add<Melt>(newMelt);
                }                    
            }
            meltsContext.SaveChanges();
        }
        private void PumpPlantData(List<SybaseMelt> listSybaseMelts,List<V_NC24_PLAV31> listOracleMelts, List<Melt> listSqLiteMelts)
        {
            //var listNewMelts = new List<PostgresFiles.MeltPostgres>();
            //var listChangedMelts = new List<PostgresFiles.MeltPostgres>();
            int fullPlantOracleCount = listOracleMelts.Count();
            int fullPlantSybaseCount = listSybaseMelts.Count();
            int fullLocalCount = listSqLiteMelts.Count();
            return; 
            int meltPlantCount = 0;
            int meltLocalCount = 0;

            foreach (var oracleMelt in listOracleMelts)
            {
                var MeltFound = false;
                meltPlantCount++;
                //проверяем, есть ли запись в локальной базе SqLite, соответствующая плавке в базе Oracle
                //сравнение ведём по номеру плавки и hash-коду
                //если записи такой нет, добавляем
                foreach (var melt in listSqLiteMelts)
                {
                    meltLocalCount++;
                    if (oracleMelt.Nplav == melt.Nplav && oracleMelt.MyHashCode() == melt.MyHashCode())
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
                        Npech = oracleMelt.Npech,
                        Nplav = oracleMelt.Nplav,
                        Npart = oracleMelt.Npart,
                        RazmPasp = oracleMelt.RazmPasp,
                        Splav = oracleMelt.Splav,
                        Ins = oracleMelt.Ins,
                        Tek = oracleMelt.Tek,
                        Pereplav = oracleMelt.Pereplav,
                        OkonchPereplav = oracleMelt.OkonchPereplav,
                        DateZap = oracleMelt.DateZap,
                        DateClose = oracleMelt.DateClose,
                        SumVesZapusk = oracleMelt.SumVesZapusk,
                        Zapusk31 = oracleMelt.Zapusk31,
                        ZapuskNakl = oracleMelt.ZapuskNakl,
                        ZapuskPpf = oracleMelt.ZapuskPpf,
                        Dsd = oracleMelt.Dsd,
                        Ncp = oracleMelt.Ncp,
                        VesSdch = oracleMelt.VesSdch,
                        RazmSdch = oracleMelt.RazmSdch,
                        MfgOrderId = oracleMelt.MfgOrderId,
                        DemandOrderId = oracleMelt.DemandOrderId,
                        Poz = oracleMelt.Poz,
                        PozNaim = oracleMelt.PozNaim,
                        PozRazm = oracleMelt.PozRazm,
                        PozIl = oracleMelt.PozIl,
                    };
                    meltsContext.Add<Melt>(newMelt);
                }
            }
            meltsContext.SaveChanges();
        }

        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            
                //Excel.Application excel = new Excel.Application();
                //excel.Visible = true;
                //Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
                //Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

                //for (int j = 0; j < DataGridParam.Columns.Count; j++)
                //{
                //    Range myRange = (Range)sheet1.Cells[1, j + 1];
                //    sheet1.Cells[1, j + 1].Font.Bold = true;
                //    sheet1.Columns[j + 1].ColumnWidth = 15;
                //    myRange.Value2 = DataGridParam.Columns[j].Header;
                //}
                //for (int i = 0; i < DataGridParam.Columns.Count; i++)
                //{
                //    for (int j = 0; j < DataGridParam.Items.Count; j++)
                //    {
                //        TextBlock b = DataGridParam.Columns[i].GetCellContent(DataGridParam.Items[j]) as TextBlock;
                //        Microsoft.Office.Interop.Excel.Range myRange = (Microsoft.Office.Interop.Excel.Range)sheet1.Cells[j + 2, i + 1];
                //        myRange.Value2 = b.Text;
                //    }
            
                //}

        }
    }
}
