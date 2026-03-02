using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Data
{
    internal static class AppState
    {
        public static int CurrentUserId = 1;
        public static ICustomerRepository CustomerRepo = new InMemoryCustomerRepository();
        public static IAppointmentRepository AppointmentRepo = new InMemoryAppointmentRepository();
    }
}
