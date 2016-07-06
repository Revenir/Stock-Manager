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
using System.IO;
using System.Data.SQLite;

namespace Stock_Manager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        public void CreateTable()
        {
            SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
            m_dbConnection.Open();
            string sql = "create table ShopStock (SKUNumber varchar(20), ItemName  varchar(20), CostBeforeMarkup  varchar(20), CostAfterMarkup varchar(20), NumberOfStock  varchar(20))";
            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            string sqlCommand = "create table StockAdditions (SKUNumber varchar(20), ItemName varchar(20), AmountAdded varchar(20), DateAdded varchar(20))";
            SQLiteCommand commandSql = new SQLiteCommand(sqlCommand, m_dbConnection);
            commandSql.ExecuteNonQuery();
        }

        public bool CheckFirstTime()
        {
            if (Properties.Settings.Default.FirstUse)
            {
                Properties.Settings.Default.FirstUse = false;
                Properties.Settings.Default.Save();

                string pathString = @"C:\IDDBShared";
                if (!(Directory.Exists(pathString)))
                {
                    System.IO.Directory.CreateDirectory(pathString);
                    SQLiteConnection.CreateFile("C:\\IDDBShared\\IDDatabase.sqlite");
                }

                CreateTable();
                return true;
            }
            else
            {
                return false;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            CheckFirstTime();
        }

        private void AddStock_Click(object sender, RoutedEventArgs e)
        {
            CreateStockItem newStock = new CreateStockItem();
            newStock.Show();
        }

        private void ModifyStock_Click(object sender, RoutedEventArgs e)
        {
            ModifyStockItem modifyStock = new ModifyStockItem();
            modifyStock.Show();
        }

        private void DeleteStock_Click(object sender, RoutedEventArgs e)
        {
            DeleteStockItem deleteStock = new DeleteStockItem();
            deleteStock.Show();
        }

        private void ViewStock_Click(object sender, RoutedEventArgs e)
        {
            ViewStockItem viewStock = new ViewStockItem();
            viewStock.Show();
        }

        private void ViewDB_Click(object sender, RoutedEventArgs e)
        {
            ViewDatabase viewDB = new ViewDatabase();
            viewDB.Show();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Add_Stock addStock = new Add_Stock();
            addStock.ShowDialog();
            string SKUNumber;
            string amount;
            int stockAmount;

//            if (addStock.DialogResult.Equals(true))
//            {
//                SKUNumber = addStock.txtSKU.Text;
//                amount = addStock.txtAmount.Text;
//            }

            SKUNumber = addStock.txtSKU.Text;
            amount = addStock.txtAmount.Text;
            stockAmount = Convert.ToInt16(amount);

            addStock.Dispose();

            if (stockAmount > 0)
            {
                SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=C:\\IDDBShared\\IDDatabase.sqlite;Version=3;");
                m_dbConnection.Open();
                string sql = "select * from ShopStock";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                SQLiteDataReader reader = command.ExecuteReader();
                bool SKUDNE = true;
                string itemName = "";

                while (reader.Read())
                {
                    string currentSKU = Convert.ToString(reader["SKUNumber"]);
                    if (currentSKU == SKUNumber)
                    {
                        SKUDNE = false;
                        itemName = Convert.ToString(reader["ItemName"]);
                    }
                }

                if (SKUDNE == true)
                {
                    MessageBox.Show("Item does not exist.");
                }
                else
                {
                    string modifyStock = "UPDATE SHOPSTOCK SET NumberOfStock = '" + amount + "' WHERE SKUNumber =" + SKUNumber;
                    SQLiteCommand modifyCommand = new SQLiteCommand(modifyStock, m_dbConnection);
                    SQLiteDataReader modifyReader = modifyCommand.ExecuteReader();

                    DateTime thisDay = DateTime.Today;
                    string date = thisDay.ToString("d");
                    string sqlCommand = "insert into StockAdditions (SKUNumber, ItemName, AmountAdded, DateAdded) values ('" + SKUNumber + "','" + itemName + "','" + amount + "','" + date + "')";
                    SQLiteCommand commandSql = new SQLiteCommand(sqlCommand, m_dbConnection);
                    commandSql.ExecuteNonQuery();
                }
            }
            else
            {
                MessageBox.Show("Please input a valid number");
            } 
        }
    }
}
