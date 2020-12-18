namespace XeMart.Services.Tests
{
    using System;

    using Xunit;

    [Collection("Sequential")]
    public class TimeSpanServiceTests
    {
        [Fact]
        public void TruncateAtWordShouldWorkCorrectlyWithDays()
        {
            var service = new TimeSpanService();
            Assert.Equal("1 days", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromDays(1))));
            Assert.Equal("2 days", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromDays(2))));
        }

        [Fact]
        public void TruncateAtWordShouldWorkCorrectlyWithHours()
        {
            var service = new TimeSpanService();
            Assert.Equal("1 hours", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromHours(1))));
            Assert.Equal("2 hours", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromHours(2))));
        }

        [Fact]
        public void TruncateAtWordShouldWorkCorrectlyWithMinutes()
        {
            var service = new TimeSpanService();
            Assert.Equal("1 minutes", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(1))));
            Assert.Equal("2 minutes", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(2))));
        }

        [Fact]
        public void TruncateAtWordShouldWorkCorrectlyWithSeconds()
        {
            var service = new TimeSpanService();
            Assert.Equal("1 seconds", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(1))));
            Assert.Equal("2 seconds", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromSeconds(2))));
        }

        [Fact]
        public void TruncateAtWordShouldWorkCorrectlyWithMilliseconds()
        {
            var service = new TimeSpanService();
            Assert.Equal("a bit", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(10))));
            Assert.Equal("a bit", service.GetTimeSince(DateTime.UtcNow.Subtract(TimeSpan.FromMilliseconds(20))));
        }
    }
}
