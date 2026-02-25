using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal class ValidationService
    {
        private static readonly Regex PhoneRegex = new Regex(@"^[0-9-]{7,15}$");
        public static List<string> ValidateCustomer(Customer customer)
        {
            var errors = new List<string>();

            customer.CustomerName = (customer.CustomerName ?? "").Trim();
            customer.Address = (customer.Address ?? "").Trim();
            customer.Phone = (customer.Phone ?? "").Trim();

            if (string.IsNullOrWhiteSpace(customer.CustomerName))
                errors.Add("Customer name is required.");

            if (string.IsNullOrWhiteSpace(customer.Address))
                errors.Add("Address is required.");

            if (string.IsNullOrWhiteSpace(customer.Phone))
                errors.Add("Phone number is required.");

            if (!string.IsNullOrWhiteSpace(customer.Phone) && !PhoneRegex.IsMatch(customer.Phone))
                errors.Add("Phone number must contain only digits and dashes.");

            return errors;
        }
    }
}
