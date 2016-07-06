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
    /// Interaction logic for DeleteStockItem.xaml
    /// </summary>
    public partial class DeleteStockItem : Window
    {
        public DeleteStockItem()
        {
            InitializeComponent();
        }

        private void SearchSKU_Click(object sender, RoutedEventArgs e)
        {
            if (!(string.IsNullOrEmpty(SKUNumberToFind.Text)))
            {
                string SKUToFind = SKUNumberToFind.Text;

                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "select * from ShopStock";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();

                bool SKUDNE = true;
                while (reader.Read())
                {
                    string currentSKU = Convert.ToString(reader["SKUNumber"]);
                    if (currentSKU == SKUToFind)
                    {
                        LblSKUNumberToDelete.Content = reader["SKUNumber"];
                        LBLItemNameToDelete.Content = reader["ItemName"];
                        SKUDNE = false;
                    }
                }

                if (SKUDNE == true)
                {
                    MessageBox.Show("SKU Number does not exist.");
                }
                else
                {
                    LblSKUNumberToDelete.Visibility = Visibility.Visible;
                    LBLItemNameToDelete.Visibility = Visibility.Visible;
                    DeleteStock.Visibility = Visibility.Visible;
                    Dash.Visibility = Visibility.Visible;
                    LblWishToDelete.Visibility = Visibility.Visible;
                    Cancel.Visibility = Visibility.Visible;
                    Cancel.IsEnabled = true;
                    DeleteStock.IsEnabled = true;
                }
            }
            else
            {
                MessageBox.Show("Please input a valid SKU.");
            }
        }

        private void DeleteStock_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "DELETE FROM ShopStock WHERE IDNumber = " + SKUNumberToFind.Text;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            MessageBox.Show("Stock item has been deleted.");
            DeleteStock.IsEnabled = false;
            Cancel.IsEnabled = false;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            LblSKUNumberToDelete.Visibility = Visibility.Hidden;
            LBLItemNameToDelete.Visibility = Visibility.Hidden;
            DeleteStock.Visibility = Visibility.Hidden;
            Dash.Visibility = Visibility.Hidden;
            LblWishToDelete.Visibility = Visibility.Hidden;
            Cancel.Visibility = Visibility.Hidden;
            Cancel.IsEnabled = true;
            DeleteStock.IsEnabled = true;

            MessageBox.Show("Deletion has been cancelled.");
        }
    }
}
