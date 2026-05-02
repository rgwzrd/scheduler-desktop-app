using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal static class ValidationService
    {
        private static readonly Regex PhoneRegex =
            new Regex(@"^[0-9\-\+\(\)\s\.]{7,20}$");

        public static List<string> ValidateCustomer(Customer customer)
        {
            var errors = new List<string>();

            if (customer == null)
            {
                errors.Add("Customer is required.");
                return errors;
            }

            customer.CustomerName = (customer.CustomerName ?? string.Empty).Trim();
            customer.Address = (customer.Address ?? string.Empty).Trim();
            customer.Phone = (customer.Phone ?? string.Empty).Trim();

            if (string.IsNullOrWhiteSpace(customer.CustomerName))
            {
                errors.Add("Customer name is required.");
            }

            if (string.IsNullOrWhiteSpace(customer.Address))
            {
                errors.Add("Address is required.");
            }

            if (string.IsNullOrWhiteSpace(customer.Phone))
            {
                errors.Add("Phone number is required.");
            }
            else if (!PhoneRegex.IsMatch(customer.Phone))
            {
                errors.Add("Phone number must contain only digits, spaces, dashes, periods, parentheses, or a leading plus sign.");
            }

            return errors;
        }
    }
}
