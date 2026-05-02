using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler_desktop_app.Services;
using Xunit;

namespace scheduler_desktop_app.Tests.Services
{
    public class TimeServiceTests
    {
        [Fact]
        public void IsWithinBusinessHoursEastern_ReturnsTrue_ForWeekdayBusinessHours()
        {
            TimeZoneInfo eastern = TimeService.GetEasternTimeZone();

            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 9, 0, 0),
                eastern);

            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 10, 0, 0),
                eastern);

            bool result = TimeService.IsWithinBusinessHoursEastern(startUtc, endUtc);

            Assert.True(result);
        }

        [Fact]
        public void IsWithinBusinessHoursEastern_ReturnsFalse_ForWeekend()
        {
            TimeZoneInfo eastern = TimeService.GetEasternTimeZone();

            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 2, 9, 0, 0),
                eastern);

            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 2, 10, 0, 0),
                eastern);

            bool result = TimeService.IsWithinBusinessHoursEastern(startUtc, endUtc);

            Assert.False(result);
        }

        [Fact]
        public void IsWithinBusinessHoursEastern_ReturnsFalse_WhenEndingAfterBusinessHours()
        {
            TimeZoneInfo eastern = TimeService.GetEasternTimeZone();

            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 16, 30, 0),
                eastern);

            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 17, 30, 0),
                eastern);

            bool result = TimeService.IsWithinBusinessHoursEastern(startUtc, endUtc);

            Assert.False(result);
        }
    }
}
