using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using scheduler_desktop_app.Data;
using scheduler_desktop_app.Models;
using scheduler_desktop_app.Models.Reports;
using scheduler_desktop_app.Services;


namespace scheduler_desktop_app.Services
{
    internal class ReportService
    {
        // report 1: number of apt types by month
        public static List<TypeCountByMonthRow> AppointmentTypeCountsByMonth()
        {
            var appts = AppState.AppointmentRepo.GetAll();

            return appts
                .Where(a => !string.IsNullOrWhiteSpace(a.Type))
                .GroupBy(a => new { a.StartLocal.Year, a.StartLocal.Month, Type = a.Type.Trim() })
                .Select(g => new TypeCountByMonthRow
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    MonthName = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(g.Key.Month),
                    Type = g.Key.Type,
                    Count = g.Count()
                })
                .OrderBy(r => r.Year)
                .ThenBy(r => r.Month)
                .ThenBy(r => r.Type)
                .ToList();
        }

        // report 2: schedule for each user
        public static List<UserScheduleRow> ScheduleForEachUser()
        {
            var appts = AppState.AppointmentRepo.GetAll();

            return appts
                .GroupBy(a => a.UserId)
                .SelectMany(g => g
                    .OrderBy(a => a.StartUtc)
                    .Select(a => new UserScheduleRow
                    {
                        UserId = g.Key,
                        AppointmentId = a.AppointmentId,
                        CustomerName = a.CustomerName,
                        Type = a.Type,
                        StartLocal = a.StartLocal,
                        EndLocal = a.EndLocal
                    }))
                .OrderBy(r => r.UserId)
                .ThenBy(r => r.StartLocal)
                .ToList();
        }

        // report 3: apts per customer + next apt date
        public static List<CustomerAppointmentCountRow> AppointmentCountsByCustomer()
        {
            var customers = AppState.CustomerRepo.GetAll();
            var appts = AppState.AppointmentRepo.GetAll();

            var grouped = appts
                .GroupBy(a => a.CustomerId)
                .ToDictionary(
                    g => g.Key,
                    g => new
                    {
                        Count = g.Count(),
                        NextLocal = g
                            .Where(a => a.StartUtc >= DateTime.UtcNow)
                            .OrderBy(a => a.StartUtc)
                            .Select(a => (DateTime?)a.StartLocal)
                            .FirstOrDefault()
                    });

            return customers
                .Select(c =>
                {
                    bool has = grouped.TryGetValue(c.CustomerId, out var info);
                    return new CustomerAppointmentCountRow
                    {
                        CustomerId = c.CustomerId,
                        CustomerName = c.CustomerName,
                        AppointmentCount = has ? info.Count : 0,
                        NextAppointmentLocal = has ? info.NextLocal : null
                    };
                })
                .OrderByDescending(r => r.AppointmentCount)
                .ThenBy(r => r.CustomerName)
                .ToList();
        }
    }
}
