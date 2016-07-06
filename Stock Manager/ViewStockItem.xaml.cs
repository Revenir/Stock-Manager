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
    /// Interaction logic for ViewStockItem.xaml
    /// </summary>
    public partial class ViewStockItem : Window
    {
        public ViewStockItem()
        {
            InitializeComponent();
        }

        private void SearchID_Click(object sender, RoutedEventArgs e)
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
                        SKUNumber.Content = reader["SKUNumber"];
                        ItemName.Content = reader["ItemName"];
                        CostBeforeMarkup.Content = "$" + reader["CostBeforeMarkup"];
                        CostAfterMarkup.Content = "$" + reader["CostAfterMarkup"];
                        QuantityAvailable.Content = reader["NumberOfStock"];
                        SKUDNE = false;
                    }
                }

                if (SKUDNE == true)
                {
                    MessageBox.Show("SKU Number does not exist.");
                }
                else
                {
                    LblItemName.Visibility = Visibility.Visible;
                    LblSKUNumber.Visibility = Visibility.Visible;
                    LblCostBeforeMarkup.Visibility = Visibility.Visible;
                    LblCostAfterMarkup.Visibility = Visibility.Visible;
                    LblQuantityAvailable.Visibility = Visibility.Visible;
                    ItemName.Visibility = Visibility.Visible;
                    SKUNumber.Visibility = Visibility.Visible;
                    CostBeforeMarkup.Visibility = Visibility.Visible;
                    CostAfterMarkup.Visibility = Visibility.Visible;
                    QuantityAvailable.Visibility = Visibility.Visible;
                    ViewReport.Visibility = Visibility.Visible;
                   
                }
            }
            else
            {
                MessageBox.Show("Please input a valid SKU.");
            }
        }

        private void ViewReport_Click(object sender, RoutedEventArgs e)
        {
            ViewReport viewRp = new ViewReport(SKUNumberToFind.Text);
            viewRp.Show();
        }
    }
}
