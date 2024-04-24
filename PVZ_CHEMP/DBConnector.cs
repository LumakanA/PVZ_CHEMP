using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

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

        public static int AddClient()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Получаем максимальный ClientID из таблицы Clients
                string queryMaxID = "SELECT COALESCE(MAX(ClientID), 0) FROM Clients";

                using (SqlCommand commandMaxID = new SqlCommand(queryMaxID, connection))
                {
                    int maxID = Convert.ToInt32(commandMaxID.ExecuteScalar());

                    // Увеличиваем максимальный ClientID на 1
                    int newID = maxID + 1;

                    // Вставляем новую запись в таблицу Clients
                    string queryInsert = "INSERT INTO Clients (ClientID) VALUES (@ClientID)";

                    using (SqlCommand commandInsert = new SqlCommand(queryInsert, connection))
                    {
                        commandInsert.Parameters.AddWithValue("@ClientID", newID);
                        commandInsert.ExecuteNonQuery();

                        return newID;
                    }
                }
            }
        }

        public static void AddOrder(Order order, int orderId, int cellNumber)
        {
            // Создаем новый экземпляр DBConnector
            DBConnector dbConnector = new DBConnector();

            // Создаем нового клиента и получаем его ClientID
            int clientID = AddClient();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Проверяем доступность ячейки
                if (dbConnector.IsCellAvailable(cellNumber))
                {
                    // Добавляем заказ с полученным ClientID, OrderID и номером ячейки
                    string query = "INSERT INTO Orders (OrderID, ArrivedDate, Status, CellNumber, ClientID) " +
                                   "VALUES (@OrderID, @ArrivedDate, @Status, @CellNumber, @ClientID)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@OrderID", orderId); // Передаем OrderID
                        command.Parameters.AddWithValue("@ArrivedDate", order.ArrivedDate);
                        command.Parameters.AddWithValue("@Status", order.Status);
                        command.Parameters.AddWithValue("@CellNumber", cellNumber); // Передаем номер ячейки
                        command.Parameters.AddWithValue("@ClientID", clientID); // Используем полученный ClientID
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    // Отобразить окно ошибки, если ячейка занята
                    MessageBox.Show("Данная ячейка уже занята", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        public int GetNextCellNumber()
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Получаем минимальный доступный номер ячейки
                string query = "SELECT COALESCE(MIN(CellNumber + 1), 1) FROM Orders AS o1 WHERE NOT EXISTS (SELECT * FROM Orders AS o2 WHERE o2.CellNumber = o1.CellNumber + 1)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public bool IsCellAvailable(int cellNumber)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                // Проверяем, существует ли указанная ячейка
                string query = "SELECT COUNT(*) FROM Orders WHERE CellNumber = @CellNumber";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@CellNumber", cellNumber);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0; // Возвращаем true, если ячейка свободна
                }
            }
        }

        public int GetLastOrderId()
        {
            using (SqlConnection connection = new SqlConnection(DBConnector.ConnectionString))
            {
                connection.Open();

                string query = "SELECT ISNULL(MAX(OrderID), 0) FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public List<InventoryItem> GetInventoryData()
        {
            List<InventoryItem> inventoryData = new List<InventoryItem>();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT Orders.OrderID, Orders.ArrivedDate, Orders.Status, Orders.CellNumber FROM Orders";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            InventoryItem item = new InventoryItem
                            {
                                OrderID = reader.GetInt32(0),
                                ArrivedDate = reader.GetDateTime(1),
                                Status = reader.GetString(2),
                                CellNumber = reader.GetInt32(3)
                            };

                            inventoryData.Add(item);
                        }
                    }
                }
            }

            return inventoryData;
        }
        public bool DeliverOrder(int orderId)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Проверяем, существует ли заказ с указанным номером
                    string checkOrderQuery = "SELECT COUNT(*) FROM Orders WHERE OrderID = @OrderId";
                    using (SqlCommand checkOrderCommand = new SqlCommand(checkOrderQuery, connection))
                    {
                        checkOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
                        int orderCount = Convert.ToInt32(checkOrderCommand.ExecuteScalar());

                        // Если заказ с указанным номером не найден, возвращаем false
                        if (orderCount == 0)
                        {
                            return false;
                        }
                    }

                    // Удаляем заказ из базы данных
                    string deleteOrderQuery = "DELETE FROM Orders WHERE OrderID = @OrderId";
                    using (SqlCommand deleteOrderCommand = new SqlCommand(deleteOrderQuery, connection))
                    {
                        deleteOrderCommand.Parameters.AddWithValue("@OrderId", orderId);
                        deleteOrderCommand.ExecuteNonQuery();
                    }

                    return true;
                }
            }
            catch (Exception ex)
            {
                // Логирование ошибок, если необходимо
                Console.WriteLine("Ошибка при выдаче заказа: " + ex.Message);
                return false;
            }
        }

        public List<InventoryItem> GetInventoryDataReports()
        {
            List<InventoryItem> inventoryItems = new List<InventoryItem>();

            try
            {
                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    connection.Open();

                    string query = "SELECT OrderID, ArrivedDate, Status, CellNumber FROM Orders";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                InventoryItem item = new InventoryItem
                                {
                                    OrderID = reader.GetInt32(0),
                                    ArrivedDate = reader.GetDateTime(1),
                                    Status = reader.GetString(2),
                                    CellNumber = reader.GetInt32(3)
                                };
                                inventoryItems.Add(item);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ошибка при получении данных инвентаря: " + ex.Message);
            }

            return inventoryItems;
        }
    }
}
