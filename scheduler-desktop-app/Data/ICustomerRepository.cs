using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Data
{
    internal class ICustomerRepository
    {
        List<Customer> GetAll();
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(int customerID);
    }
}
