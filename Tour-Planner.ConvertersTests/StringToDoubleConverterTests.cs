using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tour_Planner.Converters;

namespace Tour_Planner.ConvertersTests
{
    [TestClass()]
    public class StringToDoubleConverterTests
    {
        [TestMethod()]
        public void ConvertTest()
        {
            //Assert.Fail();
        }

        [DataRow("12341234")]
        [DataRow("123412.34")]
        [DataRow("12.341234")]
        [DataRow("1234.1234.")]
        [DataRow("123412.34")]
        [DataRow("12341234.")]
        [DataRow("12341.12e")]
        [DataRow(".12341234")]
        [DataTestMethod]
        public void ConvertBackTest(string test)
        {
            try
            {
                StringToDoubleConverter stringToDoubleConverter = new StringToDoubleConverter();
                var result = stringToDoubleConverter.ConvertBack(test, null!, null!, null!);
                var parsed = double.Parse(result.ToString()!);
                Assert.IsTrue(parsed.GetType() != result.GetType());
            }
            catch (Exception)
            {
                Assert.Fail("Cannot convert string to double");
            }

        }
    }
}