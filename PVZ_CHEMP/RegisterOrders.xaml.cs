using System;
using System.Windows;

namespace PVZ_CHEMP
{
    public partial class RegisterOrders : Window
    {
        public RegisterOrders()
        {
            InitializeComponent();

            Order order = new Order
            {
            };
            int lastOrderId = DBConnector.GetLastOrderID(order);
            txtIdOrder.Text = (lastOrderId + 1).ToString();
            txtStatus.Text = "Поступил";
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string date = txtData.Text;
            string status = txtStatus.Text = "Поступил";

            // Создание объекта Order
            Order order = new Order
            {
                ArrivedDate = DateTime.Parse(date),
                Status = status,
            };
            int lastOrderId = DBConnector.GetLastOrderID(order);

            DBConnector.AddClientIDFromOrder(lastOrderId, order);
            // Добавление заказа в базу данных
            DBConnector.AddOrder(order);


            // Обновление номера заказа на следующий
            txtIdOrder.Text = (Convert.ToInt32(txtIdOrder.Text) + 1).ToString();
        }
    }
}