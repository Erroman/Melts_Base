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
        string SortMemberPath = "Me_beg";
        ListSortDirection? SortDirection = null;
        string MeltNumberSought = "";//melt number for filtering melts
        string StartDate = "";//Start date for filtering melts
        string EndDate = ""; //End date for filtering melts
        

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
            meltsContext.Database.EnsureCreated();
            readFromSQLiteLocal();
           // localcopyGrid.Items.SortDescriptions.Add(new SortDescription("Kuku", ListSortDirection.Descending));
            readFromSybase();
            readFromOracle();
        }
        private void readFromSQLiteLocal() 
        {
            MeltNumberSought = observableMeltsViewModel?.MeltNumberSought;
            StartDate = observableMeltsViewModel?.StartDate;
            EndDate = observableMeltsViewModel?.EndDate;
            meltsContext.Melts.Load();
            localSQLLiteMelts = new ObservableCollection<Melt>(meltsContext.Melts.Local.ToObservableCollection().OrderByDescending(melt=>melt.Me_beg));
            observableMeltsViewModel = new ObservableMeltsViewModel(localSQLLiteMelts);
            localcopyGrid.DataContext = observableMeltsViewModel;
            localZapuskStartDate.DataContext = observableMeltsViewModel;
            localZapuskEndDate.DataContext = observableMeltsViewModel;
            localPlantMeltNumberSought.DataContext = observableMeltsViewModel;
            observableMeltsViewModel.MeltNumberSought = MeltNumberSought;
            observableMeltsViewModel.StartDate = StartDate;
            observableMeltsViewModel.EndDate = EndDate;
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
            readFromSQLiteLocal();
        }

        private void ExportToExcel(object sender, RoutedEventArgs e)
        {
            Excel.Application excel = new Excel.Application();
            excel.Visible = true;
            Excel.Workbook workbook = excel.Workbooks.Add();
            Excel.Worksheet sheet1 = (Excel.Worksheet)workbook.Sheets[1];
 
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

            Func<Melt, DateTime?> func_for_ordering_dates = melt => melt.Me_beg;
            Func<Melt, string> func_for_ordering_strings;
            Func<Melt, int> func_for_ordering_numbers;

            IEnumerable<Melt> listMelt = localSQLLiteMelts.Where<Melt>(melt => observableMeltsViewModel.
                    ListCollectionView_Filter(melt));
            switch (SortMemberPath)
            {
                case "MeltId":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.MeltId);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.MeltId);
                    break;
                case "Eq_id":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Eq_id);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Eq_id);
                    break;
                case "Me_num":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_num);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_num);
                    break;
                case "Me_beg":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_beg);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_beg);
                    break;
                case "Me_end":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_end);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_end);
                    break;
                case "Me_splav":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_splav);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_splav);
                    break;
                case "Sp_name":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Sp_name);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Sp_name);
                    break;
                case "Me_mould":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_mould);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_mould);
                    break;
                case "Me_del":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_del);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_del);
                    break;
                case "Me_ukaz":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_ukaz);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_ukaz);
                    break;
                case "Me_kont":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_kont);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_kont);
                    break;
                case "Me_pril":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_pril);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_pril);
                    break;
                case "Me_nazn":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_nazn);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_nazn);
                    break;
                case "Me_diam":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_diam);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_diam);
                    break;
                case "Me_weight":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_weight);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_weight);
                    break;
                case "Me_zakaz":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_zakaz);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_zakaz);
                    break;
                case "Me_pos":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_pos);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_pos);
                    break;
                case "Me_kat":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_kat);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_kat);
                    break;
                case "Sp_id":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Sp_id);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Sp_id);
                    break;
                case "Me_energy":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Me_energy);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Me_energy);
                    break;
                case "Oracle_Ins":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Oracle_Ins);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Oracle_Ins);
                    break;
                case "Oracle_Tek":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Oracle_Tek);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Oracle_Tek);
                    break;
                case "Oracle_Poz":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Oracle_Poz);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Oracle_Poz);
                    break;
                case "Oracle_PozNaim":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Oracle_PozNaim);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Oracle_PozNaim);
                    break;
                case "Oracle_Pereplav":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Oracle_Pereplav);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Oracle_Pereplav);
                    break;
                case "Oracle_OkonchPereplav":
                    if (SortDirection == ListSortDirection.Ascending)
                        listMelt = listMelt.OrderBy(melt => melt.Oracle_OkonchPereplav);
                    else if (SortDirection == ListSortDirection.Descending)
                        listMelt = listMelt.OrderByDescending(melt => melt.Oracle_OkonchPereplav);
                    break;
                default:
                    break;

            }
          
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
        }

        private void localcopyGrid_Sorting(object sender, DataGridSortingEventArgs e)
        {
            SortMemberPath =  e.Column.SortMemberPath;
            if(e.Column.SortDirection == null) SortDirection = ListSortDirection.Ascending;
            else
            SortDirection = e.Column.SortDirection==ListSortDirection.Descending?
                            ListSortDirection.Ascending:ListSortDirection.Descending;
            MessageBox.Show(string.Format("sorting grid by '{0}' column in {1} order", e.Column.SortMemberPath, e.Column.SortDirection));
        }
    }
}
