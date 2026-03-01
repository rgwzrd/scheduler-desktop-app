using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Data
{
    public interface IAppointmentRepository
    {
        List<Appointment> GetAll();
        List<Appointment> GetByUser(int userId);
        List<Appointment> GetByDayUtc(DateTime dayUtc, int userId);

        void Add(Appointment appt);
        void Update(Appointment appt);
        void Delete(int appointmentId);

        bool Overlaps(int userId, DateTime startUtc, DateTime endUtc, int ignoreAppointmentId);
    }
}
}
