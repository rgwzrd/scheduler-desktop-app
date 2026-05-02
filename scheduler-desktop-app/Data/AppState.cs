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

        public static bool IsDemoMode { get; set; }

        public static IUserRepository UserRepo { get; set; }
        public static ICustomerRepository CustomerRepo { get; set; }
        public static IAppointmentRepository AppointmentRepo { get; set; }
    }
}
