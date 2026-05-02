using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler_desktop_app.Models;
using scheduler_desktop_app.Services;

namespace scheduler_desktop_app.Data
{
    internal static class DemoDataSeeder
    {
        public static void Seed()
        {
            if (AppState.CustomerRepo.GetAll().Any() ||
                AppState.AppointmentRepo.GetAll().Any())
            {
                return;
            }

            Customer customer1 = new Customer
            {
                CustomerName = "Acme Manufacturing",
                Address = "100 Industrial Way",
                Phone = "555-0101",
                Active = true
            };

            Customer customer2 = new Customer
            {
                CustomerName = "Northwind Traders",
                Address = "200 Market Street",
                Phone = "555-0102",
                Active = true
            };

            Customer customer3 = new Customer
            {
                CustomerName = "Contoso Health",
                Address = "300 Wellness Ave",
                Phone = "555-0103",
                Active = true
            };

            AppState.CustomerRepo.Add(customer1);
            AppState.CustomerRepo.Add(customer2);
            AppState.CustomerRepo.Add(customer3);

            var customers = AppState.CustomerRepo.GetAll();

            AppState.AppointmentRepo.Add(new Appointment
            {
                CustomerId = customers[0].CustomerId,
                CustomerName = customers[0].CustomerName,
                UserId = 1,
                Type = "Consultation",
                StartUtc = EasternToUtc(DateTime.Today.AddDays(1).AddHours(9)),
                EndUtc = EasternToUtc(DateTime.Today.AddDays(1).AddHours(10))
            });

            AppState.AppointmentRepo.Add(new Appointment
            {
                CustomerId = customers[1].CustomerId,
                CustomerName = customers[1].CustomerName,
                UserId = 1,
                Type = "Planning",
                StartUtc = EasternToUtc(DateTime.Today.AddDays(2).AddHours(13)),
                EndUtc = EasternToUtc(DateTime.Today.AddDays(2).AddHours(14))
            });

            AppState.AppointmentRepo.Add(new Appointment
            {
                CustomerId = customers[2].CustomerId,
                CustomerName = customers[2].CustomerName,
                UserId = 1,
                Type = "Follow-up",
                StartUtc = EasternToUtc(DateTime.Today.AddDays(3).AddHours(15)),
                EndUtc = EasternToUtc(DateTime.Today.AddDays(3).AddHours(16))
            });
        }

        private static DateTime EasternToUtc(DateTime easternTime)
        {
            TimeZoneInfo easternTimeZone = TimeService.GetEasternTimeZone();

            DateTime unspecifiedEasternTime = DateTime.SpecifyKind(
                easternTime,
                DateTimeKind.Unspecified);

            return TimeZoneInfo.ConvertTimeToUtc(
                unspecifiedEasternTime,
                easternTimeZone);
        }
    }
}
