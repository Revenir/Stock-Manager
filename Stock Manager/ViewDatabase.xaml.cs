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
using System.Windows.Shapes;
using System.IO;
using System.Data.SQLite;
using System.Data;

namespace Stock_Manager
{
    /// <summary>
    /// Interaction logic for ViewDatabase.xaml
    /// </summary>
    public partial class ViewDatabase : Window
    {
        public ViewDatabase()
        {
            InitializeComponent();
            FillData();
        }

        void FillData()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            using (SQLiteDataAdapter myAdapter = new SQLiteDataAdapter("select * from ShopStock", m_dbConnection))
            {
                DataTable myData = new DataTable();
                myAdapter.Fill(myData);
                Stock.ItemsSource = myData.AsDataView();
            }
        }

        private void PrintDB_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDlg = new PrintDialog();
            printDlg.PrintVisual(Stock, "Stock Database");
        }
    }
}
