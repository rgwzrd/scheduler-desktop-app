using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal class LoginHistoryService
    {
        private static readonly string LogFileName = "Login_History.txt";

        public static void Append(string username)
        {
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string line = $"{timestamp} | {username}";

            File.AppendAllText(LogFileName, line + Environment.NewLine);
        }
    }
}
