using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using scheduler_desktop_app.Services;
using Xunit;

namespace scheduler_desktop_app.Tests.Services
{
    public class AppointmentValidationServiceTests
    {
        [Fact]
        public void Validate_ReturnsError_WhenTypeIsMissing()
        {
            TimeZoneInfo eastern = TimeService.GetEasternTimeZone();

            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 9, 0, 0),
                eastern);

            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 10, 0, 0),
                eastern);

            var errors = AppointmentValidationService.Validate(
                startUtc,
                endUtc,
                string.Empty,
                () => false);

            Assert.Contains("Appointment type is required.", errors);
        }

        [Fact]
        public void Validate_ReturnsError_WhenEndIsBeforeStart()
        {
            TimeZoneInfo eastern = TimeService.GetEasternTimeZone();

            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 10, 0, 0),
                eastern);

            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 9, 0, 0),
                eastern);

            var errors = AppointmentValidationService.Validate(
                startUtc,
                endUtc,
                "Planning",
                () => false);

            Assert.Contains("End time must be after start time.", errors);
        }

        [Fact]
        public void Validate_ReturnsError_WhenAppointmentOverlaps()
        {
            TimeZoneInfo eastern = TimeService.GetEasternTimeZone();

            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 9, 0, 0),
                eastern);

            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 10, 0, 0),
                eastern);

            var errors = AppointmentValidationService.Validate(
                startUtc,
                endUtc,
                "Planning",
                () => true);

            Assert.Contains("Appointment overlaps an existing appointment.", errors);
        }

        [Fact]
        public void Validate_ReturnsNoErrors_ForValidAppointment()
        {
            TimeZoneInfo eastern = TimeService.GetEasternTimeZone();

            DateTime startUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 9, 0, 0),
                eastern);

            DateTime endUtc = TimeZoneInfo.ConvertTimeToUtc(
                new DateTime(2026, 5, 4, 10, 0, 0),
                eastern);

            var errors = AppointmentValidationService.Validate(
                startUtc,
                endUtc,
                "Planning",
                () => false);

            Assert.Empty(errors);
        }
    }
}
