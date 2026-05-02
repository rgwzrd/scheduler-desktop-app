using System;
using MySql.Data.MySqlClient;
using scheduler_desktop_app.Database;

namespace scheduler_desktop_app.Data
{
    internal class MySqlUserRepository : IUserRepository
    {
        public int? GetUserIdByCredentials(string username, string password)
        {
            EnsureConn();

            username = (username ?? "").Trim();
            password = (password ?? "").Trim();

            using (var cmd = DBConnection.Conn.CreateCommand())
            {
                cmd.CommandText = @"
                    SELECT userId
                    FROM user
                    WHERE userName = @u AND password = @p
                    LIMIT 1;";

                cmd.Parameters.AddWithValue("@u", username);
                cmd.Parameters.AddWithValue("@p", password);

                var result = cmd.ExecuteScalar();
                if (result == null || result == DBNull.Value) return null;

                return Convert.ToInt32(result);
            }
        }

        private static void EnsureConn()
        {
            if (DBConnection.Conn == null || DBConnection.Conn.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("Database connection is not open. Call DBConnection.StartConnection() at startup.");
        }
    }
}