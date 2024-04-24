using System;
using System.Windows;

namespace PVZ_CHEMP
{
    public partial class RegisterOrders : Window
    {
        private DBConnector dbConnector;

        public RegisterOrders()
        {
            InitializeComponent();

            // Инициализация экземпляра DBConnector
            dbConnector = new DBConnector();

            // Отображение всех данных при инициализации
            ShowAllData();
        }

        private void ShowAllData()
        {
            // Установка начального значения txtStatus
            txtStatus.Text = "Поступил";

            // Получение и отображение последнего OrderID
            int lastOrderId = dbConnector.GetLastOrderId();
            txtIdOrder.Text = lastOrderId.ToString();

            // Получение номера ячейки
            int cellNumber = GetNextCellNumber();

            // Отображение номера ячейки в поле txtCell
            txtCell.Text = cellNumber.ToString();

            // Отображение текущей даты
            txtData.Text = DateTime.Today.ToShortDateString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            string date = txtData.Text;
            string status = txtStatus.Text;

            // Получение текущего OrderID
            int currentOrderId = int.Parse(txtIdOrder.Text);

            // Получение номера ячейки
            int cellNumber;
            if (!string.IsNullOrWhiteSpace(txtCustomCell.Text))
            {
                // Используем значение из поля txtCustomCell, если оно не пустое
                cellNumber = int.Parse(txtCustomCell.Text);
            }
            else
            {
                // Используем значение из поля txtCell
                cellNumber = GetNextCellNumber();
            }

            // Проверка доступности ячейки
            if (IsCellAvailable(cellNumber))
            {
                // Создание объекта Order
                Order order = new Order
                {
                    ArrivedDate = DateTime.Parse(date),
                    Status = status,
                };

                // Увеличение текущего OrderID на 1
                int nextOrderId = currentOrderId + 1;

                // Добавление заказа в базу данных с увеличенным OrderID и номером ячейки
                DBConnector.AddOrder(order, nextOrderId, cellNumber);

                // Обновление txtIdOrder
                txtIdOrder.Text = nextOrderId.ToString();

                // Обновление всех данных
                ShowAllData();
                // Сброс пользовательского номера ячейки
                txtCustomCell.Clear();
            }
            else
            {
                // Отображение окна ошибки, если ячейка занята
                MessageBox.Show("Данная ячейка уже занята", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }


        private int GetNextCellNumber()
        {
            // Получение следующего доступного номера ячейки
            return dbConnector.GetNextCellNumber();
        }

        private bool IsCellAvailable(int cellNumber)
        {
            // Проверка, свободна ли ячейка
            return dbConnector.IsCellAvailable(cellNumber);
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
