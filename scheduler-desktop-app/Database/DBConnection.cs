using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Database
{
    internal static class DBConnection
    {
        public static MySqlConnection Conn { get; private set; }

        public static void StartConnection()
        {
            if (Conn != null) return;

            string connStr = ConfigurationManager.ConnectionStrings["localdb"].ConnectionString;
            Conn = new MySqlConnection(connStr);
            Conn.Open();
        }

        public static void CloseConnection()
        {
            try { Conn?.Close(); }
            finally { Conn = null; }
        }
    }
}
