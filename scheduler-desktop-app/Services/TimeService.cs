using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal static class TimeService
    {
        public static DateTime PickerToUtc(DateTime pickerValue)
        {
            if (pickerValue.Kind == DateTimeKind.Utc)
                return pickerValue;

            if (pickerValue.Kind == DateTimeKind.Local)
                return pickerValue.ToUniversalTime();

            return TimeZoneInfo.ConvertTimeToUtc(pickerValue, TimeZoneInfo.Local);
        }

        public static DateTime LocalPickerValueToUtc(DateTime pickerValue)
        {
            return PickerToUtc(pickerValue);
        }

        public static DateTime UtcToLocal(DateTime utc)
        {
            var u = (utc.Kind == DateTimeKind.Utc) ? utc : DateTime.SpecifyKind(utc, DateTimeKind.Utc);
            return u.ToLocalTime();
        }

        public static DateTime LocalToUtc(DateTime local)
        {
            if (local.Kind == DateTimeKind.Utc)
                return local;

            if (local.Kind == DateTimeKind.Local)
                return local.ToUniversalTime();

            return TimeZoneInfo.ConvertTimeToUtc(local, TimeZoneInfo.Local);
        }

        public static TimeZoneInfo GetEasternTimeZone()
        {
            try { return TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time"); }
            catch { }

            return TimeZoneInfo.FindSystemTimeZoneById("America/New_York");
        }

        public static DateTime UtcToEastern(DateTime utc)
        {
            var tz = GetEasternTimeZone();
            var u = (utc.Kind == DateTimeKind.Utc) ? utc : DateTime.SpecifyKind(utc, DateTimeKind.Utc);
            return TimeZoneInfo.ConvertTimeFromUtc(u, tz);
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
