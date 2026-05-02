using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace scheduler_desktop_app.Services
{
    internal static class AppFileService
    {
        private const string AppFolderName = "ClientScheduler";

        public static string GetAppDataDirectory()
        {
            string appDataPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            string directoryPath = Path.Combine(appDataPath, AppFolderName);

            Directory.CreateDirectory(directoryPath);

            return directoryPath;
        }

        public static string GetFilePath(string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name is required.", nameof(fileName));
            }

            return Path.Combine(GetAppDataDirectory(), fileName);
        }
    }
}
