using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Database
{
    internal static class DBConnection
    {
        private const string ConnectionStringEnvironmentVariable = "CLIENT_SCHEDULER_CONNECTION";

        public static MySqlConnection Conn { get; private set; }

        public static void StartConnection()
        {
            if (Conn != null && Conn.State == ConnectionState.Open)
            {
                return;
            }

            string connectionString = GetConnectionString();

            Conn = new MySqlConnection(connectionString);
            Conn.Open();
        }

        public static void CloseConnection()
        {
            if (Conn == null)
            {
                return;
            }

            Conn.Close();
            Conn.Dispose();
            Conn = null;
        }

        private static string GetConnectionString()
        {
            string environmentConnectionString =
                Environment.GetEnvironmentVariable(ConnectionStringEnvironmentVariable);

            if (!string.IsNullOrWhiteSpace(environmentConnectionString))
            {
                return environmentConnectionString;
            }

            ConnectionStringSettings configuredConnection =
                ConfigurationManager.ConnectionStrings["localdb"];

            if (configuredConnection == null ||
                string.IsNullOrWhiteSpace(configuredConnection.ConnectionString))
            {
                throw new InvalidOperationException(
                    "Database connection string is missing. Set CLIENT_SCHEDULER_CONNECTION or configure localdb in App.config.");
            }

            return configuredConnection.ConnectionString;
        }
    }
}
