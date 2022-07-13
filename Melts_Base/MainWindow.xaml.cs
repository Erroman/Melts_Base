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
        List<SybaseMelt> sybaseMelts = null;
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
                        sybaseMelts = new List<SybaseMelt>();
                        while (odbcDataReader.Read())
                        {
                            DateTime melt_end;
                            DateTime.TryParse(odbcDataReader["me_end"].ToString(), out melt_end);
                            var melt_sybase = new SybaseMelt
                            {
                                Me_id = odbcDataReader["me_id"].ToString(),
                                Me_num = odbcDataReader["me_num"].ToString(),
                                Eq_id = odbcDataReader["eq_id"].ToString(),
                                Me_beg = DateTime.Parse(odbcDataReader["me_beg"].ToString()),
                                Me_end = melt_end == DateTime.Parse("01.01.0001") ? null : melt_end,
                                Me_splav = odbcDataReader["me_splav"].ToString(),
                                Sp_name = odbcDataReader["sp_name"].ToString(),
                                Me_mould = odbcDataReader["me_mould"].ToString(),
                                Me_del = odbcDataReader["me_del"].ToString(),
                                Me_weight = odbcDataReader["me_weigth"].ToString(),
                                Me_ukaz = odbcDataReader["me_ukaz"].ToString(),
                                Me_kont = odbcDataReader["me_kont"].ToString(),
                                Me_pril = odbcDataReader["me_pril"].ToString(),
                                Me_nazn = odbcDataReader["me_nazn"].ToString(),
                                Me_diam = odbcDataReader["me_diam"].ToString(),
                                Me_pos = odbcDataReader["me_pos"].ToString(),
                                Me_kat = odbcDataReader["me_kat"].ToString(),
                                Sp_id = odbcDataReader["sp_id"].ToString(),
                                Me_energy = odbcDataReader["me_energy"].ToString(),
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
            if(readFromSybase() && readFromOracle()) 
            {
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
                if (string.IsNullOrEmpty(sybaseMelt.Me_num)) continue;
                var MeltFound = false;
                //находим информацию по плавке с данным номером в listOracleMelts,если нашли,составляем полную плавку,
                //добавляя поля из найденной записи.
                var oracleMelt = listOracleMelts.Where<OracleMelt>(p => p.Nplav == sybaseMelt.Me_num).FirstOrDefault<OracleMelt>();
                if (oracleMelt != null) 
                {
                    sybaseMelt.Oracle_Ins = oracleMelt.Ins;
                    sybaseMelt.Oracle_Tek = oracleMelt.Tek;
                    sybaseMelt.Oracle_Poz = oracleMelt.Poz;
                    sybaseMelt.Oracle_PozNaim = oracleMelt.PozNaim;
                    sybaseMelt.Oracle_Pereplav = oracleMelt.Pereplav;
                    sybaseMelt.Oracle_OkonchPereplav = oracleMelt.OkonchPereplav;
                }
                //проверяем, есть ли запись в локальной базе SqLite, соответствующая плавке в базе Sybase
                //сравнение ведём по номеру плавки и hash-коду.
                //если записи такой нет, добавляем запись с данным номером плавки, содержащую
                //информацию из Sybase м Oracle
                foreach (var melt in listSqLiteMelts)
                {

                    if (sybaseMelt.Me_num == melt.Me_num && sybaseMelt.MyHashCode() == melt.MyHashCode())
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
                        Eq_id = sybaseMelt.Eq_id,
                        Me_num = sybaseMelt.Me_num,
                        Me_beg = sybaseMelt.Me_beg,
                        Me_end = sybaseMelt.Me_end,
                        Me_splav = sybaseMelt.Me_splav,
                        Sp_name = sybaseMelt.Sp_name,
                        Me_mould = sybaseMelt.Me_mould,
                        Me_del = sybaseMelt.Me_del,
                        Me_ukaz = sybaseMelt.Me_ukaz,
                        Me_kont = sybaseMelt.Me_kont,
                        Me_pril = sybaseMelt.Me_pril,
                        Me_nazn = sybaseMelt.Me_nazn,
                        Me_diam = sybaseMelt.Me_diam,
                        Me_weight = sybaseMelt.Me_weight,
                        Me_zakaz = sybaseMelt.Me_zakaz,
                        Me_pos = sybaseMelt.Me_pos,
                        Me_kat = sybaseMelt.Me_kat,
                        Sp_id = sybaseMelt.Sp_id,
                        Me_energy = sybaseMelt.Me_energy,
                        Oracle_Ins = sybaseMelt.Oracle_Ins,
                        Oracle_Tek = sybaseMelt.Oracle_Tek,
                        Oracle_Poz = sybaseMelt.Oracle_Poz,
                        Oracle_PozNaim = sybaseMelt.Oracle_PozNaim,
                        Oracle_Pereplav=sybaseMelt.Oracle_Pereplav,
                        Oracle_OkonchPereplav = sybaseMelt.Oracle_OkonchPereplav,
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
            //var numberOfColumns =  localcopyGrid.Columns.Count;
            //Int64 numberOfRows = 0;
            //foreach(var  vv in observableMeltsViewModel.View)numberOfRows++;
            //for (int j = 0; j < numberOfColumns; j++)
            //{
            //    //Range myRange = (Range)sheet1.Cells[1, j];
            //    sheet1.Cells[1, j+1].Font.Bold = true;
            //    sheet1.Columns[j+1].ColumnWidth = 15;
            //    sheet1.Cells[1,j+1] = localcopyGrid.Columns[j].Header;
            //}

            sheet1.Cells[1, 1] = "Номер записи";
            sheet1.Cells[1, 2] = "Номер печи";
            sheet1.Cells[1, 3] = "Номер плавки";
            sheet1.Cells[1, 4] = "Дата плавки";
            sheet1.Cells[1, 5] = "Сплав";
            sheet1.Cells[1, 6] = "Индекс";
            sheet1.Cells[1, 7] = "Номер переплава";
            sheet1.Cells[1, 8] = "Признак окончательного переплава";
            sheet1.Cells[1, 9] = "Номер комплекта";
            sheet1.Cells[1, 10] = "Диаметр расходуемого электрода";
            sheet1.Cells[1, 11] = "№ ТЭК";
            sheet1.Cells[1, 12] = "ИЛ/УиС/ШН";
            sheet1.Cells[1, 13] = "Контракт";
            sheet1.Cells[1, 14] = "Приложение";
            sheet1.Cells[1, 15] = "Назначение";
            sheet1.Cells[1, 16] = "Диаметр слитка";

            IEnumerable<Melt> listMelt = from melt in localSQLLiteMelts 
                       where observableMeltsViewModel.ListCollectionView_Filter(melt) orderby melt.Me_beg descending
                       select melt;
            int i = 2;
            foreach (var melt in listMelt)
            {
                sheet1.Cells[i, 1] = melt.MeltId;
                sheet1.Cells[i, 2] = melt.Eq_id;
                sheet1.Cells[i, 3] = melt.Me_num;
                sheet1.Cells[i, 4] = melt.Me_beg?.ToString("dd.MM.yyyy");
                sheet1.Cells[i, 5] = melt.Sp_name;
                sheet1.Cells[i, 6] = melt.Oracle_Ins;
                sheet1.Cells[i, 7] = melt.Oracle_Pereplav;
                sheet1.Cells[i, 8] = melt.Oracle_OkonchPereplav;
                sheet1.Cells[i, 9] = melt.Me_mould;
                sheet1.Cells[i, 10] = melt.Me_del;
                sheet1.Cells[i, 11] = melt.Oracle_Tek;
                sheet1.Cells[i, 12] = melt.Me_ukaz;
                sheet1.Cells[i, 13] = melt.Me_kont;
                sheet1.Cells[i, 14] = melt.Oracle_Poz;
                sheet1.Cells[i, 15] = melt.Oracle_PozNaim;
                sheet1.Cells[i, 16] = melt.Me_diam;
                i++;
            }



            //sheet1.Cells[1, 1] = "Номер записи";
            //int i = 2;
            //foreach(var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 1] = melt.MeltId;
            //    i++;
            //}

            //sheet1.Cells[1, 2] = "Номер печи";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 2] = melt.Eq_id;
            //    i++;
            //}

            //sheet1.Cells[1, 3] = "Номер плавки";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 3] = melt.Me_num;
            //    i++;
            //}

            //sheet1.Cells[1, 4] = "Дата плавки";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 4] = melt.Me_beg?.ToString("dd.MM.yyyy");
            //    i++;
            //}

            //sheet1.Cells[1, 5] = "Сплав";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    i++;
            //}

            //sheet1.Cells[1, 6] = "Индекс";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 6] = melt.Oracle_Ins;
            //    i++;
            //}

            //sheet1.Cells[1, 7] = "Номер переплава";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 7] = melt.Oracle_Pereplav;
            //    i++;
            //}

            //sheet1.Cells[1, 8] = "Признак окончательного переплава";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 8] = melt.Oracle_OkonchPereplav;
            //    i++;
            //}

            //sheet1.Cells[1, 9] = "Номер комплекта";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 9] = melt.Me_mould;
            //    i++;
            //}

            //sheet1.Cells[1, 10] = "Диаметр расходуемого электрода";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 10] = melt.Me_del;
            //    i++;
            //}

            //sheet1.Cells[1, 11] = "№ ТЭК";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 11] = melt.Oracle_Tek;
            //    i++;
            //}

            //sheet1.Cells[1, 12] = "ИЛ/УиС/ШН";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 12] = melt.Me_ukaz;
            //    i++;
            //}

            //sheet1.Cells[1, 13] = "Контракт";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 13] = melt.Me_kont;
            //    i++;
            //}

            //sheet1.Cells[1, 14] = "Приложение";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 14] = melt.Oracle_Poz;
            //    i++;
            //}

            //sheet1.Cells[1, 15] = "Назначение";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 15] = melt.Oracle_PozNaim;
            //    i++;
            //}

            //sheet1.Cells[1, 16] = "Диаметр слитка";
            //i = 2;
            //foreach (var melt in localSQLLiteMelts)
            //{
            //    sheet1.Cells[i, 16] = melt.Me_diam;
            //    i++;
            //}

        }
    }
}
