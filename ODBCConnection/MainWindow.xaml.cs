using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Data.Odbc;

namespace ODBCConnection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            String connectionString = "DSN = sybase";
            ReadRow(connectionString);

        }
        static private void ReadRow(string connectionString)
        {
            //string queryString =
            //    "INSERT INTO Customers (CustomerID, CompanyName) Values('NWIND', 'Northwind Traders')";
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                //OdbcCommand command = new OdbcCommand(queryString);
                //command.Connection = connection;
                connection.Open();
                MessageBox.Show(connection.State.ToString());
                //command.ExecuteNonQuery();

                // The connection is automatically closed at
                // the end of the Using block.
            }
        }
    }
}
