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
    /// Interaction logic for ModifyStockItem.xaml
    /// </summary>
    public partial class ModifyStockItem : Window
    {
        public ModifyStockItem()
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
                        SKUNumber.Text = SKUToFind;
                        ItemName.Text = Convert.ToString(reader["ItemName"]);
                        NumberOfStock.Text = Convert.ToString(reader["NumberOfStock"]);
                        CostBeforeMarkup.Text = Convert.ToString(reader["CostBeforeMarkup"]);
                        CostAfterMarkup.Text = Convert.ToString(reader["CostAfterMarkup"]);
                        SKUDNE = false;
                    }
                }

                if (SKUDNE == true)
                {
                    MessageBox.Show("Item does not exist.");
                }
                else
                {
                    LblSKUNumber.Visibility = Visibility.Visible;
                    LblItemName.Visibility = Visibility.Visible;
                    LblNumberOfStock.Visibility = Visibility.Visible;
                    LblCostBeforeMarkup.Visibility = Visibility.Visible;
                    LblCostAfterMarkup.Visibility = Visibility.Visible;
                    SKUNumber.Visibility = Visibility.Visible;
                    ItemName.Visibility = Visibility.Visible;
                    NumberOfStock.Visibility = Visibility.Visible;
                    CostBeforeMarkup.Visibility = Visibility.Visible;
                    CostAfterMarkup.Visibility = Visibility.Visible;
                    ModifyStock.Visibility = Visibility.Visible;
                    StartingStockNumber.Content = NumberOfStock.Text;
                }
            }
            else
            {
                MessageBox.Show("Please Input an SKU Number.");
            }
        }

        private void ModifyStock_Click(object sender, RoutedEventArgs e)
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "UPDATE SHOPSTOCK SET ItemName = '" + ItemName.Text + "', CostBeforeMarkup = '" + CostBeforeMarkup.Text + "',"
                + "CostAfterMarkup = '" + CostAfterMarkup.Text + "', NumberOfStock = '" + NumberOfStock.Text +
                "' WHERE SKUNumber = " + SKUNumberToFind.Text;
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            SQLiteDataReader reader = command.ExecuteReader();

            double currentStock = Convert.ToDouble(NumberOfStock.Text);
            double previousStock = Convert.ToDouble(StartingStockNumber.Content);
            if (currentStock != previousStock)
            {
                double quantityChange = currentStock - previousStock;
                string amountAdded = Convert.ToString(quantityChange);
                DateTime thisDay = DateTime.Today;
                string date = thisDay.ToString("d");
                string sqlCommand = "insert into StockAdditions (SKUNumber, ItemName, AmountAdded, DateAdded) values ('" + SKUNumberToFind.Text + "','" + ItemName.Text + "','" + amountAdded + "','" + date + "')";
                SQLiteCommand commandSql = new SQLiteCommand(sqlCommand, m_dbConnection);
                commandSql.ExecuteNonQuery();
            }

            LblSKUNumber.Visibility = Visibility.Hidden;
            LblItemName.Visibility = Visibility.Hidden;
            LblNumberOfStock.Visibility = Visibility.Hidden;
            LblCostBeforeMarkup.Visibility = Visibility.Hidden;
            LblCostAfterMarkup.Visibility = Visibility.Hidden;
            SKUNumber.Visibility = Visibility.Hidden;
            ItemName.Visibility = Visibility.Hidden;
            NumberOfStock.Visibility = Visibility.Hidden;
            CostBeforeMarkup.Visibility = Visibility.Hidden;
            CostAfterMarkup.Visibility = Visibility.Hidden;
            ModifyStock.Visibility = Visibility.Hidden;

            MessageBox.Show("Stock has been modified.");
        }
    }
}
