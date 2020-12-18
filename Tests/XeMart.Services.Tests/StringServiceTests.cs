namespace XeMart.Services.Tests
{
    using Xunit;

    [Collection("Sequential")]
    public class StringServiceTests
    {
        [Fact]
        public void TruncateAtWordShouldReturnNullWithNullInput()
        {
            var service = new StringService();
            Assert.Null(service.TruncateAtWord(null, 30));
        }

        [Fact]
        public void TruncateAtWordShouldReturnTheSameInputWhenItsLengthIsShorterThanTheTruncateLength()
        {
            var service = new StringService();
            var input = "Test Input";
            Assert.Equal(input, service.TruncateAtWord(input, input.Length + 1));
        }

        [Theory]
        [InlineData("Test Input", 7, "Test…")]
        [InlineData("Test Input", 25, "Test Input")]
        [InlineData("Test Input Test", 14, "Test Input…")]
        [InlineData("Test ", 5, "Test…")]
        public void TruncateAtWordShouldWorkCorrectly(string input, int length, string expected)
        {
            var service = new StringService();
            Assert.Equal(expected, service.TruncateAtWord(input, length));
        }
    }
}
