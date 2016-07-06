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

namespace Stock_Manager
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class CreateStockItem : Window
    {
        public CreateStockItem()
        {
            InitializeComponent();
        }

        private void CreateNewStock_Click(object sender, RoutedEventArgs e)
        {
            string SKUNum = SKUNumber.Text;
            string itemName = ItemName.Text;
            string costBeforeMarkup = CostBeforeMarkup.Text;
            string costAfterMarkup = CostAfterMarkup.Text;
            string numberOfStock = NumberOfStock.Text;

            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();

            string sql = "insert into ShopStock (SKUNumber, ItemName, CostBeforeMarkup, CostAfterMarkup, NumberOfStock) values ('" + SKUNum + "','" + itemName + "'," + costBeforeMarkup + ",'" + costAfterMarkup + "','" + numberOfStock + "')";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            MessageBox.Show("New Stock has been created.");
            SKUNumber.Clear();
            ItemName.Clear();
            CostBeforeMarkup.Clear();
            CostAfterMarkup.Clear();
            NumberOfStock.Clear();
        }
    }
}
