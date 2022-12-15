
using CLI.Extensions;

namespace CLI.Tests
{
    public class AppExtensionsTests
    {
        private App _app = new App();

        [Theory]
        [InlineData("5", "7", "12")]
        [InlineData("-3", "12", "9")]
        [InlineData("50", "-50", "0")]
        public void AddArguments_AddInts_ResultIsAnIntSumm(string arg1, string arg2, string expectedResult)
        {
            string actualResult = _app.AddArguments(arg1, arg2);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("14.3", "33.6", "47.90")]
        [InlineData("-16.3", "7.7", "-8.60")]
        [InlineData("5", "43.2", "48.20")]
        [InlineData("5.3", "43", "48.30")]
        public void AddArguments_AddFloats_ResultIsAFloatSumm(string arg1, string arg2, string expectedResult)
        {
            string actualResult = _app.AddArguments(arg1, arg2);

            Assert.Equal(expectedResult, actualResult);
        }

        [Theory]
        [InlineData("14.3", "secondArg")]
        [InlineData("-16.3test", "7.7")]
        [InlineData("firstArg", "secondArg")]
        public void AddArguments_AtLeastOneArgIsAString_ResultIsAStringConcat(string arg1, string arg2)
        {
            string actualResult = _app.AddArguments(arg1.ToString(), arg2.ToString());

            Assert.Equal(arg1 + arg2, actualResult);
        }
    }
}