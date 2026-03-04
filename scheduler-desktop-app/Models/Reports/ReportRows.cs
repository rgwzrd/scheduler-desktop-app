using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Models.Reports
{
    public class TypeCountByMonthRow
    {
        public int Year { get; set; }
        public int Month { get; set; }      
        public string MonthName { get; set; } 
        public string Type { get; set; }
        public int Count { get; set; }
    }

    public class UserScheduleRow
    {
        public int UserId { get; set; }
        public int AppointmentId { get; set; }
        public string CustomerName { get; set; }
        public string Type { get; set; }
        public DateTime StartLocal { get; set; }
        public DateTime EndLocal { get; set; }
    }

    public class CustomerAppointmentCountRow
    {
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public int AppointmentCount { get; set; }
        public DateTime? NextAppointmentLocal { get; set; }
    }
}
