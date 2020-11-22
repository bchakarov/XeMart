namespace XeMart.Services
{
    using System;

    public interface ITimeSpanService
    {
        public string GetTimeSince(DateTime objDateTime);
    }
}
