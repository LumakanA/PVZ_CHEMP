using System;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace PVZ_CHEMP
{
    internal class DBConnector
    {
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=NFURY\\SQLEXPRESS;Integrated Security=True");
        private const string ConnectionString = "Data Source=NFURY\\SQLEXPRESS;Initial Catalog=PVZ_CHEMP;Integrated Security=True";

        public void openConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void closeConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getSqlConnection()
        {
            return sqlConnection;
        }

        public static void AddOrder(Order order)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "INSERT INTO Orders (ArrivedDate, Status, CellNumber) " +
                               "VALUES (@ArrivedDate, @Status, @CellNumber)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ArrivedDate", order.ArrivedDate);
                    command.Parameters.AddWithValue("@Status", order.Status);
                    command.Parameters.AddWithValue("@CellNumber", order.CellNumber);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetLastOrderID(Order order)
        {
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                string query = "SELECT MAX(OrderID) FROM Orders";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    // Выполняем запрос и получаем результат
                    object result = command.ExecuteScalar();

                    // Проверяем, что результат не является DBNull и конвертируем его в int
                    if (result != DBNull.Value)
                    {
                        return Convert.ToInt32(result);
                    }
                    else
                    {
                        // Если результат равен DBNull, возвращаем -1 или другое значение по умолчанию
                        return 0;
                    }
                }
            }
        }

        public static void AddClientIDFromOrder(int orderId, Order order)
        {
            // Переменная для хранения ID клиента
            int clientId;

            // Получение ID клиента по ID заказа
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                // Запрос для получения ID клиента по ID заказа
                string query = "SELECT ClientID FROM Orders WHERE OrderID = @OrderID";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@OrderId", orderId);
                    clientId = (int)command.ExecuteScalar();
                }

                string query2 = "INSERT INTO Orders (ClientID) " +
               "VALUES (@ClientID)";

                using (SqlCommand command = new SqlCommand(query2, sqlConnection))
                {
                    command.Parameters.AddWithValue("@ClientID", order.ClientID);
                    command.ExecuteNonQuery();
                }
            }

            // Вставка ID клиента в таблицу Clients
            using (SqlConnection sqlConnection = new SqlConnection(ConnectionString))
            {
                sqlConnection.Open();

                // Запрос для вставки ID клиента в таблицу Clients
                string query = "INSERT INTO Clients (ClientID) VALUES (@ClientID)";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@ClientID", clientId);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
