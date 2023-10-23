using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BizIntegrator.Security
{
    public class JwtDates
    {
        private static DateTime Epoch { get; } = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

        public int ExpiresAt { get; }

        public int IssuedAt { get; }

        public JwtDates(DateTime issueTime, int validForMinutes)
        {
            issueTime = JwtDates.EnsureUtc(issueTime);
            this.IssuedAt = JwtDates.TotalSecondsFromEpoch(issueTime);
            this.ExpiresAt = JwtDates.TotalSecondsFromEpoch(issueTime.AddMinutes((double)validForMinutes));
        }

        public static double MinutesToExpiry(DateTime now, int expiresAt)
        {
            int num = JwtDates.TotalSecondsFromEpoch(JwtDates.EnsureUtc(now));
            return TimeSpan.FromSeconds((double)(expiresAt - num)).TotalMinutes;
        }

        private static DateTime EnsureUtc(DateTime dateTime) => dateTime.Kind == DateTimeKind.Utc ? dateTime : dateTime.ToUniversalTime();

        private static int TotalSecondsFromEpoch(DateTime dateTime) => (int)dateTime.Subtract(JwtDates.Epoch).TotalSeconds;
    }
}
