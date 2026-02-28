using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal static class AppointmentValidationService
    {
        public static List<string> Validate(
            DateTime startUtc,
            DateTime endUtc,
            string type,
            Func<bool> overlapsExisting
            )
        {
            var errors = new List<string>();

            type = (type ?? "").Trim();
            if (string.IsNullOrEmpty(type))
            {
                errors.Add("Appointment type is required.");
            }

            if (endUtc <= startUtc)
            {
                errors.Add("End time must be after start time.");
            }

            if (!TimeService.IsWithinBusinessHoursEastern(startUtc, endUtc))
            {
                errors.Add("ASppointment must be scheduled Mon-Fri between 9:00 and 17:00 Eastern Time.");
            }

            if (overlapsExisting())
            {
                errors.Add("Appointment overlaps an existing appointment.");
            }

            return errors;
        }
    }
}
