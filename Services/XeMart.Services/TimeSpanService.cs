namespace XeMart.Services
{
    using System;

    public class TimeSpanService : ITimeSpanService
    {
        public string GetTimeSince(DateTime objDateTime)
        {
            TimeSpan ts = DateTime.UtcNow.Subtract(objDateTime);
            int intDays = ts.Days;
            int intHours = ts.Hours;
            int intMinutes = ts.Minutes;
            int intSeconds = ts.Seconds;

            if (intDays > 0)
            {
                return string.Format("{0} days", intDays);
            }

            if (intHours > 0)
            {
                return string.Format("{0} hours", intHours);
            }

            if (intMinutes > 0)
            {
                return string.Format("{0} minutes", intMinutes);
            }

            if (intSeconds > 0)
            {
                return string.Format("{0} seconds", intSeconds);
            }

            return "a bit";
        }
    }
}
