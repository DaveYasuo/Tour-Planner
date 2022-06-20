using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;

namespace Test.Tour_Planner.Services
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
           
            
        }

        [Test]
        //[TestCase("TestCase1","Wien","Linz","ASDF",RouteType.fastest)]
        [TestCase("TestCase1","Graz","Salzburg","ASDF",RouteType.shortest)]
        public async Task AddTourToDatabase(string title, string origin,string destination,string description , RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type); ;
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            Assert.IsNotNull(result.Distance);
            Assert.IsNotNull(result.ImagePath);
            Assert.AreEqual(result.Title, title);
            Assert.AreEqual(result.Origin, origin);
            Assert.AreEqual(result.Destination, destination);
            Assert.AreEqual(result.Description, description);
            Assert.AreEqual(result.RouteType, type);
            await service.DeleteTour(result.Id);
        }

        [Test]
        //[TestCase("TestCase1", "Wien", "Linz", "ASDF", RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task DeleteTourData(string title, string origin, string destination, string comment, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, comment, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            bool success = await service.DeleteTour(result.Id);
            List<Tour> tourLists = await service.GetTours();
            foreach (Tour to in tourLists)
            {
                if (to.Id == result.Id) Assert.Fail();
            }
            Assert.Pass();
        }

        [Test]
        //[TestCase("TestCase1", "Wien", "Linz", "ASDF", RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task UpdateTour(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            result.Description = "Hiii";
            result.Title = "Aloha";
            await service.UpdateTour(result);
            List <Tour> tourList = await service.GetTours();
            foreach(Tour to in tourList)
            {
                if(to.Id == result.Id)
                {
                    Assert.AreNotEqual(to.Description, description);
                    Assert.AreNotEqual(to.Title, title);
                    return;
                }
            }
            Assert.Fail();
        }
    }
}