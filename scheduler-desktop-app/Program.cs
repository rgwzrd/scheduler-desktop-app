using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using scheduler_desktop_app.Data;
using scheduler_desktop_app.Models;
using scheduler_desktop_app.Database;

namespace scheduler_desktop_app
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                DBConnection.StartConnection();
                AppState.CustomerRepo = new MySqlCustomerRepository();
                AppState.AppointmentRepo = new MySqlAppointmentRepository();
                Application.Run(new SchedulerApplicationContext());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                DBConnection.CloseConnection();
            }
        }
    }
}
