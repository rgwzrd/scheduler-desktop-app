using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scheduler_desktop_app.Services
{
    internal static class LocationService
    {
        public static string GetUserLocationSummary()
        {
            string region = RegionInfo.CurrentRegion.DisplayName;

            string tz = System.TimeZoneInfo.Local.DisplayName;

            return $"{region} | {tz}";
        }
    }
}
