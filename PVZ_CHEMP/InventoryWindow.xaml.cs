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

namespace PVZ_CHEMP
{
    /// <summary>
    /// Логика взаимодействия для InventoryWindow.xaml
    /// </summary>
    public partial class InventoryWindow : Window
    {
        private DBConnector dbConnector;

        public InventoryWindow()
        {
            InitializeComponent();
            dbConnector = new DBConnector();
            LoadInventoryData();
        }

        private void LoadInventoryData()
        {
            // Получить данные о заказах и их расположении на складе
            var inventoryData = dbConnector.GetInventoryData();

            // Привязать данные к DataGrid
            dgInventory.ItemsSource = inventoryData;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра главного окна
            MainWindow mainWindow = new MainWindow();

            // Отображение главного окна
            mainWindow.Show();

            // Закрытие текущего окна
            this.Close();
        }
    }
}
