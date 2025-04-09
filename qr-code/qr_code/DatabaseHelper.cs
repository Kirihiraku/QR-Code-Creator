using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace qr_code
{
    public class DatabaseHelper
    {
        private readonly string connectionString = "Server=localhost;Database=user48;Integrated Security=True;";

        public void SaveQRCode(string username, string qrtext)
        {
            int userId = GetUserId(username);
            if (userId == -1)
            {
                userId = AddNewUser(username);
            }

            using (var connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO QRCodes (UserId, QRCodeText, CreatedAt) VALUES (@UserId, @QRCodeText, @CreatedAt)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserId", userId);
                    command.Parameters.AddWithValue("@QRCodeText", qrtext);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private int AddNewUser(string username)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO Users (Username) OUTPUT INSERTED.UserId VALUES (@Username)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);

                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
        }

        private int GetUserId(string username)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                string query = "SELECT UserId FROM Users WHERE Username = @Username";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    var result = command.ExecuteScalar();
                    return result != null ? (int)result : -1;
                }
            }
        }
    }
}