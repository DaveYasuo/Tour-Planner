using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tour_Planner.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_Planner.Converters.Tests
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
                double.Parse(result.ToString()!);
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail("Cannot convert string to double");
            }

        }
    }
}