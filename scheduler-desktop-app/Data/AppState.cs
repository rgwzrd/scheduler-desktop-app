using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Data
{
    internal static class AppState
    {
        public static int CurrentUserId = 0;

        public static ICustomerRepository CustomerRepo = new MySqlCustomerRepository();
        public static IAppointmentRepository AppointmentRepo = new MySqlAppointmentRepository();
    }
}
