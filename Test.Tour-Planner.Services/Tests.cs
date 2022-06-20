using NUnit.Framework;
using Tour_Planner.Models;
using Tour_Planner.Services;

namespace Test.Tour_Planner.Services
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
            RestService service = new RestService();
            Tour tour = new Tour("TestCase",Wien,Linz,);
        }

        [Test]
        public void AddTourToDatabase()
        {
            Assert.Pass();
        }
    }
}