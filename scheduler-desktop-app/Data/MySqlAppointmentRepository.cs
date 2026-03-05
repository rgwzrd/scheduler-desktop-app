using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using scheduler_desktop_app.Database;
using scheduler_desktop_app.Exceptions;
using scheduler_desktop_app.Models;

namespace scheduler_desktop_app.Data
{
    internal class MySqlAppointmentRepository : IAppointmentRepository
    {
        private const string AuditUser = "test";
        private const string NotUsedText = "N/A";

        public List<Appointment> GetAll()
        {
            try
            {
                EnsureConn();
                var appts = new List<Appointment>();

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT a.appointmentId,
                               a.customerId,
                               c.customerName,
                               a.userId,
                               a.type,
                               a.`start`,
                               a.`end`
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId
                        ORDER BY a.`start`;";

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            appts.Add(MapAppointment(reader));
                    }
                }

                return appts;
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("GetAll", "Unable to load appointments.", ex);
            }
        }

        public List<Appointment> GetByUser(int userId)
        {
            try
            {
                EnsureConn();
                var appts = new List<Appointment>();

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT a.appointmentId,
                               a.customerId,
                               c.customerName,
                               a.userId,
                               a.type,
                               a.`start`,
                               a.`end`
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId
                        WHERE a.userId = @userId
                        ORDER BY a.`start`;";

                    cmd.Parameters.AddWithValue("@userId", userId);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            appts.Add(MapAppointment(reader));
                    }
                }

                return appts;
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("GetByUser", "Unable to load user appointments.", ex);
            }
        }

        public List<Appointment> GetByDayUtc(DateTime dayUtc, int userId)
        {
            try
            {
                EnsureConn();

                var dayStart = DateTime.SpecifyKind(dayUtc.Date, DateTimeKind.Utc);
                var dayEnd = dayStart.AddDays(1);

                var appts = new List<Appointment>();

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT a.appointmentId,
                               a.customerId,
                               c.customerName,
                               a.userId,
                               a.type,
                               a.`start`,
                               a.`end`
                        FROM appointment a
                        JOIN customer c ON a.customerId = c.customerId
                        WHERE a.userId = @userId
                          AND a.`start` >= @dayStart
                          AND a.`start` < @dayEnd
                        ORDER BY a.`start`;";

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@dayStart", dayStart);
                    cmd.Parameters.AddWithValue("@dayEnd", dayEnd);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                            appts.Add(MapAppointment(reader));
                    }
                }

                return appts;
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("GetByDayUtc", "Unable to load daily appointments.", ex);
            }
        }

        public void Add(Appointment appt)
        {
            try
            {
                EnsureConn();
                if (appt == null) throw new ArgumentNullException(nameof(appt));

                var now = DateTime.UtcNow;

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO appointment
                            (customerId, userId, title, description, location, contact, type, url,
                             `start`, `end`, createDate, createdBy, lastUpdate, lastUpdateBy)
                        VALUES
                            (@customerId, @userId, @title, @description, @location, @contact, @type, @url,
                             @startUtc, @endUtc, @createDate, @createdBy, @lastUpdate, @lastUpdateBy);";

                    cmd.Parameters.AddWithValue("@customerId", appt.CustomerId);
                    cmd.Parameters.AddWithValue("@userId", appt.UserId);

                    cmd.Parameters.AddWithValue("@title", NotUsedText);
                    cmd.Parameters.AddWithValue("@description", NotUsedText);
                    cmd.Parameters.AddWithValue("@location", NotUsedText);
                    cmd.Parameters.AddWithValue("@contact", NotUsedText);
                    cmd.Parameters.AddWithValue("@type", (appt.Type ?? NotUsedText).Trim());
                    cmd.Parameters.AddWithValue("@url", NotUsedText);

                    cmd.Parameters.AddWithValue("@startUtc", EnsureUtc(appt.StartUtc));
                    cmd.Parameters.AddWithValue("@endUtc", EnsureUtc(appt.EndUtc));

                    cmd.Parameters.AddWithValue("@createDate", now);
                    cmd.Parameters.AddWithValue("@createdBy", AuditUser);
                    cmd.Parameters.AddWithValue("@lastUpdate", now);
                    cmd.Parameters.AddWithValue("@lastUpdateBy", AuditUser);

                    cmd.ExecuteNonQuery();
                }
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
                EnsureConn();
                if (appt == null) throw new ArgumentNullException(nameof(appt));

                var now = DateTime.UtcNow;

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        UPDATE appointment
                        SET customerId = @customerId,
                            userId = @userId,
                            title = @title,
                            description = @description,
                            location = @location,
                            contact = @contact,
                            type = @type,
                            url = @url,
                            `start` = @startUtc,
                            `end` = @endUtc,
                            lastUpdate = @lastUpdate,
                            lastUpdateBy = @lastUpdateBy
                        WHERE appointmentId = @appointmentId;";

                    cmd.Parameters.AddWithValue("@appointmentId", appt.AppointmentId);
                    cmd.Parameters.AddWithValue("@customerId", appt.CustomerId);
                    cmd.Parameters.AddWithValue("@userId", appt.UserId);

                    cmd.Parameters.AddWithValue("@title", "not needed");
                    cmd.Parameters.AddWithValue("@description", "not needed");
                    cmd.Parameters.AddWithValue("@location", "not needed");
                    cmd.Parameters.AddWithValue("@contact", "not needed");
                    cmd.Parameters.AddWithValue("@type", (appt.Type ?? "not needed").Trim());
                    cmd.Parameters.AddWithValue("@url", "not needed");

                    cmd.Parameters.AddWithValue("@startUtc", EnsureUtc(appt.StartUtc));
                    cmd.Parameters.AddWithValue("@endUtc", EnsureUtc(appt.EndUtc));

                    cmd.Parameters.AddWithValue("@lastUpdate", now);
                    cmd.Parameters.AddWithValue("@lastUpdateBy", AuditUser);

                    cmd.ExecuteNonQuery();
                }
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
                EnsureConn();

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = "DELETE FROM appointment WHERE appointmentId = @appointmentId;";
                    cmd.Parameters.AddWithValue("@appointmentId", appointmentId);
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("Delete", "Unable to delete appointment.", ex);
            }
        }

        public bool Overlaps(int userId, DateTime startUtc, DateTime endUtc, int ignoreAppointmentId)
        {
            try
            {
                EnsureConn();

                using (var cmd = DBConnection.Conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT COUNT(*)
                        FROM appointment
                        WHERE userId = @userId
                          AND appointmentId <> @ignoreId
                          AND @startUtc < `end`
                          AND @endUtc > `start`;";

                    cmd.Parameters.AddWithValue("@userId", userId);
                    cmd.Parameters.AddWithValue("@ignoreId", ignoreAppointmentId);
                    cmd.Parameters.AddWithValue("@startUtc", EnsureUtc(startUtc));
                    cmd.Parameters.AddWithValue("@endUtc", EnsureUtc(endUtc));

                    var count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
            catch (Exception ex)
            {
                throw new AppointmentOperationException("Overlaps", "Unable to check appointment overlap.", ex);
            }
        }

        private static Appointment MapAppointment(MySqlDataReader reader)
        {
            var start = (DateTime)reader["start"];
            var end = (DateTime)reader["end"];

            return new Appointment
            {
                AppointmentId = Convert.ToInt32(reader["appointmentId"]),
                CustomerId = Convert.ToInt32(reader["customerId"]),
                CustomerName = reader["customerName"].ToString(),
                UserId = Convert.ToInt32(reader["userId"]),
                Type = reader["type"]?.ToString(),

                StartUtc = DateTime.SpecifyKind(start, DateTimeKind.Utc),
                EndUtc = DateTime.SpecifyKind(end, DateTimeKind.Utc)
            };
        }

        private static DateTime EnsureUtc(DateTime dt)
        {
            if (dt.Kind == DateTimeKind.Utc) return dt;
            if (dt.Kind == DateTimeKind.Local) return dt.ToUniversalTime();
            return DateTime.SpecifyKind(dt, DateTimeKind.Utc);
        }

        private static void EnsureConn()
        {
            if (DBConnection.Conn == null || DBConnection.Conn.State != System.Data.ConnectionState.Open)
                throw new InvalidOperationException("Database connection is not open. Call DBConnection.StartConnection() at startup.");
        }
    }
}