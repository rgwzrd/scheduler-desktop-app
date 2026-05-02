using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler_desktop_app.Models;
using scheduler_desktop_app.Services;
using Xunit;

namespace scheduler_desktop_app.Tests.Services
{
    public class ValidationServiceTests
    {
        [Fact]
        public void ValidateCustomer_ReturnsErrors_ForMissingFields()
        {
            var customer = new Customer();

            var errors = ValidationService.ValidateCustomer(customer);

            Assert.Contains("Customer name is required.", errors);
            Assert.Contains("Address is required.", errors);
            Assert.Contains("Phone number is required.", errors);
        }

        [Fact]
        public void ValidateCustomer_ReturnsError_ForInvalidPhone()
        {
            var customer = new Customer
            {
                CustomerName = "Acme",
                Address = "100 Main Street",
                Phone = "phone"
            };

            var errors = ValidationService.ValidateCustomer(customer);

            Assert.Contains(
                "Phone number must contain only digits, spaces, dashes, periods, parentheses, or a leading plus sign.",
                errors);
        }

        [Fact]
        public void ValidateCustomer_ReturnsNoErrors_ForValidCustomer()
        {
            var customer = new Customer
            {
                CustomerName = "Acme",
                Address = "100 Main Street",
                Phone = "(555) 123-4567"
            };

            var errors = ValidationService.ValidateCustomer(customer);

            Assert.Empty(errors);
        }
    }
}