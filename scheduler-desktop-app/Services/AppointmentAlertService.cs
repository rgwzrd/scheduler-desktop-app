using scheduler_desktop_app.Data;
using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal class AppointmentAlertService
    {
        public static Appointment GetNextAppointmentWithinMinutes(int userId, int minutes)
        {
            var nowUtc = DateTime.UtcNow;
            var windowEndUtc = nowUtc.AddMinutes(minutes);

            return AppState.AppointmentRepo
                .GetByUser(userId)
                .Where(a => a.StartUtc >= nowUtc && a.StartUtc <= windowEndUtc)
                .OrderBy(a => a.StartUtc)
                .FirstOrDefault();
        }

        public static string BuildAlertMessage(Appointment appt)
        {
            if (appt == null) return null;

            return
                "Upcoming appointment within 15 minutes:\n\n" +
                $"ID: {appt.AppointmentId}\n" +
                $"Customer: {appt.CustomerName}\n" +
                $"Type: {appt.Type}\n" +
                $"Start (local): {appt.StartLocal:yyyy-MM-dd HH:mm}\n" +
                $"End (local): {appt.EndLocal:yyyy-MM-dd HH:mm}";
        }
    }
}
