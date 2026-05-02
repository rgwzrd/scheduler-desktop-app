using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal static class LoginHistoryService
    {
        private const string LogFileName = "Login_History.txt";

        public static void Append(string username)
        {
            string safeUsername = string.IsNullOrWhiteSpace(username)
                ? "Unknown"
                : username.Trim();

            string line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | {safeUsername}";
            string path = AppFileService.GetFilePath(LogFileName);

            File.AppendAllText(path, line + Environment.NewLine);
        }
    }
}
