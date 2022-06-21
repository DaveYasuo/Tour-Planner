using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;

namespace Test.Tour_Planner.Services
{
    public class TestRestServiceTour
    {

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "ASDF", RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task AddTourToDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type); ;
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            if (result != null)
            {
                Assert.IsNotNull(result.Distance);
                Assert.IsNotNull(result.ImagePath);
                Assert.AreEqual(result.Title, title);
                Assert.AreEqual(result.Origin, origin);
                Assert.AreEqual(result.Destination, destination);
                Assert.AreEqual(result.Description, description);
                Assert.AreEqual(result.RouteType, type);
                await service.DeleteTour(result.Id);
                List<Tour> tours = await service.GetTours();
                if (tours != null)
                {
                    foreach (var tour1 in tours.Where(tour1 => tour1.Id == result.Id))
                    {
                        Assert.Fail("Tour still here after deletion");
                    }
                    Assert.Pass("add tour success");
                }
            }
        }

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "ASDF", RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task DeleteTourData(string title, string origin, string destination, string comment, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, comment, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            if (result != null)
            {
                await service.DeleteTour(result.Id);
                List<Tour> tourLists = await service.GetTours();
                if (tourLists != null)
                {
                    if (tourLists.Any(tLog => tLog.Id == result.Id))
                    {
                        Assert.Fail();
                        return;
                    }
                }
                Assert.Pass("Tour deleted");
            }
        }

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "ASDF", RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task UpdateTour(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            if (result != null)
            {
                result.Description = "Hiii";
                result.Title = "Aloha";
                await service.UpdateTour(result);
                List<Tour> tourList = await service.GetTours();
                await service.DeleteTour(result.Id);
                if (tourList != null)
                {

                    foreach (Tour to in tourList)
                    {
                        if (to.Id == result.Id)
                        {
                            Assert.AreEqual(to.Description, result.Description);
                            Assert.AreEqual(to.Title, result.Title);
                            return;
                        }
                    }
                    Assert.Fail("Tour did not update");
                }
            }
        }

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "ASDF", RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task GetTours(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();

            Tour result = await service.AddTour(tour);
            List<Tour> list = await service.GetTours();
            if (result != null)
            {
                if (list != null)
                {
                    foreach (Tour to in list)
                    {
                        if (to.Id == result.Id)
                        {
                            await service.DeleteTour(result.Id);
                            Assert.Pass("Get Tour success");
                            return;
                        }
                    }
                    Assert.Fail("Get Tour failed");
                }
            }
        }
    }
}