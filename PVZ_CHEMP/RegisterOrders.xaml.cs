using System;
using System.Windows;

namespace PVZ_CHEMP
{
    public partial class RegisterOrders : Window
    {
        public RegisterOrders()
        {
            InitializeComponent();

            // Получение последнего номера заказа и отображение его в текстовом поле
            int lastOrderId = DBConnector.GetLastOrderId();
            txtIdOrder.Text = (lastOrderId + 1).ToString();
            int lastCellId = DBConnector.GetLastCellId();
            txtCell.Text = (lastCellId + 1).ToString();
            txtStatus.Text = "Поступил";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string date = txtData.Text;
            string status = txtStatus.Text = "Поступил";

            // Создание объекта Order
            Order order = new Order
            {
                Date = DateTime.Parse(date),
                Status = status,
            };

            // Добавление заказа в базу данных
            DBConnector.AddOrder(order);

            // Обновление номера заказа на следующий
            txtIdOrder.Text = (Convert.ToInt32(txtIdOrder.Text) + 1).ToString();


            txtCell.Text = (Convert.ToInt32(txtCell.Text) + 1).ToString();
        }
    }
}