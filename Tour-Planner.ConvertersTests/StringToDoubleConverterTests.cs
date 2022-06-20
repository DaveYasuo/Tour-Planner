using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Tour_Planner.Converters;

namespace Tour_Planner.ConvertersTests
{
    [TestClass()]
    public class StringToDoubleConverterTests
    {
        [DataRow("12341234")]
        [DataRow("123412.34")]
        [DataRow("12.341234")]
        [DataRow("123412.34")]
        [DataRow("12341.12e")]
        [DataTestMethod]
        public void ConvertBackTestAlsoValidIfLastIndexIsInvalid(string test)
        {
            try
            {
                StringToDoubleConverter stringToDoubleConverter = new StringToDoubleConverter();
                var result = stringToDoubleConverter.ConvertBack(test, null!, null!, null!);
                Assert.IsInstanceOfType(result, typeof(double));
            }
            catch (Exception)
            {
                Assert.Fail("Given input is not a double string");
            }

        }

        [DataRow("1234sd1234")]
        [DataRow("123.412.34")]
        [DataRow("12.341w234")]
        [DataRow(".123412.34")]
        [DataRow("12.341.12e")]
        [DataTestMethod]
        public void ConvertBackTestWithInvalidStringsToReturnZero(string test)
        {
            try
            {
                StringToDoubleConverter stringToDoubleConverter = new StringToDoubleConverter();
                var result = stringToDoubleConverter.ConvertBack(test, null!, null!, null!);
                Assert.IsInstanceOfType(result, typeof(int));
            }
            catch (Exception)
            {
                Assert.Fail("Given input is a valid double string");
            }

        }
    }
}