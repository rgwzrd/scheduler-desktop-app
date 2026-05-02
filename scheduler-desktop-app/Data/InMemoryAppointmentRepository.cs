using scheduler_desktop_app.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler_desktop_app.Exceptions;

namespace scheduler_desktop_app.Data
{
    internal class InMemoryAppointmentRepository : IAppointmentRepository
    {
        private readonly List<Appointment> _appts = new List<Appointment>();
        private int _nextId = 1;

        public List<Appointment> GetAll()
            => _appts.OrderBy(a => a.StartUtc).ToList();

        public List<Appointment> GetByUser(int userId)
            => _appts.Where(a => a.UserId == userId).OrderBy(a => a.StartUtc).ToList();

        public List<Appointment> GetByDayUtc(DateTime dayUtc, int userId)
        {
            var date = dayUtc.Date;
            return _appts
                .Where(a => a.UserId == userId && a.StartUtc.Date == date)
                .OrderBy(a => a.StartUtc)
                .ToList();
        }

        public void Add(Appointment appt)
        {
            try
            {
                if (appt == null) throw new ArgumentNullException(nameof(appt));

                appt.AppointmentId = _nextId++;
                _appts.Add(Clone(appt));
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("Add", "Unable to add appointment.", ex);
            }
        }

        public void Update(Appointment appt)
        {
            try
            {
                if (appt == null) throw new ArgumentNullException(nameof(appt));

                var existing = _appts.FirstOrDefault(a => a.AppointmentId == appt.AppointmentId);
                if (existing == null) throw new InvalidOperationException("Appointment not found.");

                existing.CustomerId = appt.CustomerId;
                existing.CustomerName = appt.CustomerName;
                existing.UserId = appt.UserId;
                existing.Type = appt.Type;
                existing.StartUtc = appt.StartUtc;
                existing.EndUtc = appt.EndUtc;
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("Update", "Unable to update appointment.", ex);
            }
        }

        public void Delete(int appointmentId)
        {
            try
            {
                var existing = _appts.FirstOrDefault(a => a.AppointmentId == appointmentId);
                if (existing == null) throw new InvalidOperationException("Appointment not found.");

                _appts.Remove(existing);
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("Delete", "Unable to delete appointment.", ex);
            }
        }

        public bool Overlaps(int userId, DateTime startUtc, DateTime endUtc, int ignoreAppointmentId)
        {
            return _appts.Any(a =>
                a.UserId == userId &&
                a.AppointmentId != ignoreAppointmentId &&
                startUtc < a.EndUtc &&
                endUtc > a.StartUtc
            );
        }

        public int CountByCustomer(int customerId)
        {
            return _appts.Count(a => a.CustomerId == customerId);
        }

        private static Appointment Clone(Appointment a) => new Appointment
        {
            AppointmentId = a.AppointmentId,
            CustomerId = a.CustomerId,
            CustomerName = a.CustomerName,
            UserId = a.UserId,
            Type = a.Type,
            StartUtc = a.StartUtc,
            EndUtc = a.EndUtc
        };
    }
}
