using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Media.Media3D;
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

                string query = "INSERT INTO Orders (Date, Status) " +
                               "VALUES (@Date, @Status)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Date", order.Date);
                    command.Parameters.AddWithValue("@Status", order.Status);
                    command.ExecuteNonQuery();
                }
            }
        }

        public static int GetLastOrderId()
        {
            int lastOrderId = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 ID_order FROM Orders ORDER BY ID_order DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        lastOrderId = Convert.ToInt32(result);
                    }
                }
            }

            return lastOrderId;
        }

        public static int GetLastCellId()
        {
            int lastCellId = 0;

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                connection.Open();

                string query = "SELECT TOP 1 Cell FROM Orders ORDER BY Cell DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    object result = command.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        lastCellId = Convert.ToInt32(result);
                    }
                }
            }

            return lastCellId;
        }
    }
}
