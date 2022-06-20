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
            await service.DeleteTour(result.Id);
            List<Tour> tourLists = await service.GetTours();
            Assert.AreNotEqual(tourLists.Last().Id, tour.Id);
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
            await service.DeleteTour(result.Id);
            foreach (Tour to in tourList)
            {
                if(to.Id == result.Id)
                {
                    Assert.AreEqual(to.Description, result.Description);
                    Assert.AreEqual(to.Title, result.Title);
                    return;
                }
            }
            Assert.Fail();
        }

        [Test]
        //[TestCase("TestCase1", "Wien", "Linz", "ASDF", RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task GetTours(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();
            List<Tour> oldList =await service.GetTours();
            Tour result = await service.AddTour(tour);
            List<Tour> newList = await service.GetTours();
            await service.DeleteTour(result.Id);
            Assert.AreNotEqual(oldList, newList);    
        }

        [Test]
        //[TestCase("TestCase1","Wien","Linz","ASDF",RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task AddTourLogToDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            TourLog tourLog = new TourLog(result.Id,DateTime.Now,time,Rating.medium,Difficulty.medium,123456,"Comment");
            tourLog = await service.AddTourLog(tourLog);
            List<TourLog> tourLogs = await service.GetAllTourLogsFromTour(result);
            await service.DeleteTour(result.Id);

            Assert.AreEqual(tourLogs[0].Comment, tourLog.Comment);
            Assert.AreEqual(tourLogs[0].Difficulty, tourLog.Difficulty);
            Assert.AreEqual(tourLogs[0].Rating, tourLog.Rating);
            Assert.AreEqual(tourLogs[0].TourId, tourLog.TourId);
            Assert.AreEqual(tourLogs[0].Distance, tourLog.Distance);
            Assert.AreEqual(tourLogs[0].Id, tourLog.Id);
            
        }

        [Test]
        //[TestCase("TestCase1","Wien","Linz","ASDF",RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task DeleteTourLogFromDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            TourLog tourLog = new TourLog(result.Id, DateTime.Now, time, Rating.medium, Difficulty.medium, 123456, "Comment");
            await service.AddTourLog(tourLog);
            await service.DeleteTourLog(tourLog.Id);

            List<TourLog> newTourLogs = await service.GetAllTourLogsFromTour(result);
            await service.DeleteTour(result.Id);
            if (newTourLogs.Last().Id == tourLog.Id) Assert.Fail();
            if(newTourLogs.Count == 0)
            Assert.Pass();
        }

        [Test]
        //[TestCase("TestCase1","Wien","Linz","ASDF",RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task UpdateTourLogFromDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            TourLog tourLog = new TourLog(result.Id, DateTime.Now, time, Rating.medium, Difficulty.medium, 123456, "Comment");
            tourLog = await service.AddTourLog(tourLog);

            tourLog.Comment = "Comment2";
            await service.UpdateTourLog(tourLog);
            List<TourLog> tourLogs = await service.GetAllTourLogsFromTour(result);
            await service.DeleteTour(result.Id);
            if (tourLogs.Last().Id == tourLog.Id)
            {
                Assert.AreEqual(tourLogs.Last().Comment ,tourLog.Comment);return;
                
            }
            Assert.Fail();

        }

        [Test]
        //[TestCase("TestCase1","Wien","Linz","ASDF",RouteType.fastest)]
        [TestCase("TestCase1", "Graz", "Salzburg", "ASDF", RouteType.shortest)]
        public async Task GetTourLogsFromDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            IRestService service = new RestService();
            Tour result = await service.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            TourLog tourLog = new TourLog(result.Id, DateTime.Now, time, Rating.medium, Difficulty.medium, 123456, "Comment");
            tourLog = await service.AddTourLog(tourLog);

            List<TourLog> oldList = await service.GetAllTourLogs();
            await service.DeleteTourLog(tourLog.Id);
            List<TourLog> newList = await service.GetAllTourLogs();
            await service.DeleteTour(result.Id);
            Assert.AreNotEqual(oldList.Count, newList.Count);
        }
    }
}