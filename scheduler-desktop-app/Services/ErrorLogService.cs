using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace scheduler_desktop_app.Services
{
    internal static class ErrorLogService
    {
        private const string LogFileName = "error-log.txt";

        public static string LogFilePath => AppFileService.GetFilePath(LogFileName);

        public static void Log(Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException(nameof(ex));
            }

            string entry =
                $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}]{Environment.NewLine}" +
                $"{ex}{Environment.NewLine}{Environment.NewLine}";

            File.AppendAllText(LogFilePath, entry);
        }
    }
}
