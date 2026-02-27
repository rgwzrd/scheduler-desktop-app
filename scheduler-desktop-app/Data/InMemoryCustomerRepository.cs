using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler_desktop_app.Exceptions;

namespace scheduler_desktop_app.Data
{
    internal class InMemoryCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new List<Customer>();
        private int _nextId = 1;

        public List<Customer> GetAll()
        {
            return _customers
                .Select(c => new Customer
                {
                    CustomerId = c.CustomerId,
                    CustomerName = c.CustomerName,
                    Address = c.Address,
                    Phone = c.Phone,
                    Active = c.Active
                })
                .OrderBy(c => c.CustomerId)
                .ToList();
        }

        public void Add(Customer customer)
        {
            try
            {
                if (customer == null)
                    throw new ArgumentNullException(nameof(customer));

                customer.CustomerId = ++_nextId;

                _customers.Add(new Customer
                {
                    CustomerId = customer.CustomerId,
                    CustomerName = customer.CustomerName,
                    Address = customer.Address,
                    Phone = customer.Phone,
                    Active = customer.Active
                });
            }

            catch (Exception ex)
            {
                throw new CustomerOperationException("Add", "Unable to add customer record.", ex);
            }
        }

        public void Update(Customer customer)
        {
            try
            {
                var existing = _customers.FirstOrDefault(c => c.CustomerId == customer.CustomerId);
                if (existing == null) return;

                existing.CustomerName = customer.CustomerName;
                existing.Address = customer.Address;
                existing.Phone = customer.Phone;
                existing.Active = customer.Active;
            }

            catch (Exception ex)
            {
                throw new CustomerOperationException("Update", "Unable to update customer record.", ex);
            }
        }

        public void Delete(int customerId)
        {
            try
            {
                var existing = _customers.FirstOrDefault(c => c.CustomerId == customerId);
                if (existing == null)
                    throw new InvalidOperationException("Customer record not found.");
                    
                _customers.Remove(existing);
            }

            catch (Exception ex)
            {
                throw new CustomerOperationException("Delete", "Unable to delete customer record.", ex);

            }
        }
    }
}
