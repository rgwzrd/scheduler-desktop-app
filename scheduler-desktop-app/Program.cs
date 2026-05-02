using System;
using System.Configuration;
using System.Windows.Forms;
using scheduler_desktop_app.Data;
using scheduler_desktop_app.Database;
using scheduler_desktop_app.Services;

namespace scheduler_desktop_app
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool useDemoData = IsDemoModeEnabled();

            try
            {
                ConfigureRepositories(useDemoData);
                Application.Run(new SchedulerApplicationContext());
            }
            catch (Exception ex)
            {
                ShowStartupError(ex);
            }
            finally
            {
                if (!useDemoData)
                {
                    DBConnection.CloseConnection();
                }
            }
        }

        private static bool IsDemoModeEnabled()
        {
            string value = ConfigurationManager.AppSettings["UseDemoData"];

            return string.Equals(
                value,
                "true",
                StringComparison.OrdinalIgnoreCase);
        }

        private static void ConfigureRepositories(bool useDemoData)
        {
            AppState.IsDemoMode = useDemoData;

            if (useDemoData)
            {
                AppState.UserRepo = new InMemoryUserRepository();
                AppState.CustomerRepo = new InMemoryCustomerRepository();
                AppState.AppointmentRepo = new InMemoryAppointmentRepository();

                DemoDataSeeder.Seed();
                return;
            }

            DBConnection.StartConnection();

            AppState.UserRepo = new MySqlUserRepository();
            AppState.CustomerRepo = new MySqlCustomerRepository();
            AppState.AppointmentRepo = new MySqlAppointmentRepository();
        }

        private static void ShowStartupError(Exception ex)
        {
            string message = "The application could not start.";

            try
            {
                ErrorLogService.Log(ex);
                message += Environment.NewLine + "Details were written to:";
                message += Environment.NewLine + ErrorLogService.LogFilePath;
            }
            catch (Exception logException)
            {
                message += Environment.NewLine + ex.Message;
                message += Environment.NewLine + "The error log could not be written:";
                message += Environment.NewLine + logException.Message;
            }

            MessageBox.Show(
                message,
                "Startup Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }
}