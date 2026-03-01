using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal static class TimeService
    {
        public static TimeZoneInfo GetEasternTimeZone()
        {
            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            }
            catch
            { }

            return TimeZoneInfo.FindSystemTimeZoneById("America/New York");
        }

        public static DateTime LocalToUtc(DateTime local)
        {
            var localKind = DateTime.SpecifyKind(local, DateTimeKind.Local);
            return localKind.ToUniversalTime();
        }

        public static DateTime UtcToLocal(DateTime utc)
        {
            var utcKind = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
            return utcKind.ToLocalTime();
        }

        public static DateTime UtcToEastern(DateTime utc)
        {
            var tz = GetEasternTimeZone();
            var utcKind = DateTime.SpecifyKind(utc, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeToUtc(utcKind, tz);
        }

        public static bool IsWithinBusinessHoursEastern(DateTime startUtc, DateTime endUtc)
        {
            var startEt = UtcToEastern(startUtc);
            var endEt = UtcToEastern(endUtc);

            if (startEt.Date != endEt.Date) return false;

            var day = startEt.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday) return false;

            var businessStart = startEt.Date.AddHours(9);
            var businessEnd = startEt.Date.AddHours(17);

            return startEt >= businessStart && endEt <= businessEnd;
        }
    }
}
