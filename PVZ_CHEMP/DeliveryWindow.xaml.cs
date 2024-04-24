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
    /// Логика взаимодействия для DeliveryWindow.xaml
    /// </summary>
    public partial class DeliveryWindow : Window
    {
        public DeliveryWindow()
        {
            InitializeComponent();
        }

        private void Button_DeliverOrder_Click(object sender, RoutedEventArgs e)
        {
            // Создание экземпляра DBConnector
            DBConnector dbConnector = new DBConnector();

            // Получение номера заказа из текстового поля
            int orderId;
            if (int.TryParse(txtOrderId.Text, out orderId))
            {
                // Выполнение логики выдачи заказа с освобождением ячейки склада
                if (dbConnector.DeliverOrder(orderId))
                {
                    MessageBox.Show("Заказ успешно выдан.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Закрытие окна после успешной выдачи заказа
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Не удалось выдать заказ. Проверьте номер заказа и повторите попытку.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Введите корректный номер заказа.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
