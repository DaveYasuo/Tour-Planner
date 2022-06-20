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
    public class TestRestServiceTourLog
    {
        private IRestService _restService;

        [SetUp]
        public void Setup()
        {
            _restService = new RestService();
        }

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "Hello", RouteType.fastest)]
        [TestCase("TestCase2", "Graz", "Salzburg", "HelloHello", RouteType.shortest)]
        public async Task AddTourLogToDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            Tour result = await _restService.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            if (result != null)
            {
                TourLog tourLog = new TourLog(result.Id, DateTime.Now, time, Rating.medium, Difficulty.medium, 123456, "Comment");
                tourLog = await _restService.AddTourLog(tourLog);
                List<TourLog> tourLogs = await _restService.GetAllTourLogsFromTour(result);
                await _restService.DeleteTour(result.Id);

                if (tourLogs != null)
                {
                    Assert.AreEqual(tourLogs[0].Comment, tourLog.Comment);
                    Assert.AreEqual(tourLogs[0].Difficulty, tourLog.Difficulty);
                    Assert.AreEqual(tourLogs[0].Rating, tourLog.Rating);
                    Assert.AreEqual(tourLogs[0].TourId, tourLog.TourId);
                    Assert.AreEqual(tourLogs[0].Distance, tourLog.Distance);
                    Assert.AreEqual(tourLogs[0].Id, tourLog.Id);
                }
            }
        }

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "Hello", RouteType.fastest)]
        [TestCase("TestCase2", "Graz", "Salzburg", "HelloHello", RouteType.shortest)]
        public async Task DeleteTourLogFromDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            Tour result = await _restService.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            if (result != null)
            {
                TourLog tourLog = new TourLog(result.Id, DateTime.Now, time, Rating.medium, Difficulty.medium, 123456, "Comment");
                await _restService.AddTourLog(tourLog);
                await _restService.DeleteTourLog(tourLog.Id);

                List<TourLog> newTourLogs = await _restService.GetAllTourLogsFromTour(result);
                await _restService.DeleteTour(result.Id);
                if (newTourLogs != null)
                {
                    if (newTourLogs.Any(tLog => tLog.Id == result.Id))
                    {
                        Assert.Fail();
                        return;
                    }
                }
                Assert.Pass("TourLog deleted");
            }
        }

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "Hello", RouteType.fastest)]
        [TestCase("TestCase2", "Graz", "Salzburg", "HelloHello", RouteType.shortest)]
        public async Task UpdateTourLogFromDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            Tour result = await _restService.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            if (result != null)
            {
                TourLog tourLog = new TourLog(result.Id, DateTime.Now, time, Rating.medium, Difficulty.medium, 123456, "Comment");
                TourLog resultTourLog = await _restService.AddTourLog(tourLog);
                if (resultTourLog != null)
                {
                    Assert.AreEqual(resultTourLog.Comment, tourLog.Comment);
                    resultTourLog.Comment = "Comment2";
                    await _restService.UpdateTourLog(resultTourLog);
                    List<TourLog> tourLogs = await _restService.GetAllTourLogsFromTour(result);
                    if (tourLogs != null)
                    {
                        foreach (var log in tourLogs)
                        {
                            if (log.Id == resultTourLog.Id)
                            {
                                Assert.AreEqual("Comment2", log.Comment);
                                await _restService.DeleteTour(result.Id);
                                return;
                            }
                        }

                        Assert.Fail("TourLog doesn't get updated.");
                    }
                }
            }
        }

        [Test]
        [TestCase("TestCase1", "Wien", "Linz", "Hello", RouteType.fastest)]
        [TestCase("TestCase2", "Graz", "Salzburg", "HelloHello", RouteType.shortest)]
        public async Task GetTourLogsFromDatabase(string title, string origin, string destination, string description, RouteType type)
        {
            Tour tour = new Tour(title, origin, destination, description, type);
            Tour resultTour = await _restService.AddTour(tour);
            TimeSpan time = new TimeSpan(10000000);
            if (resultTour != null)
            {
                TourLog tourLog = new TourLog(resultTour.Id, DateTime.Now, time, Rating.medium, Difficulty.medium, 123456, "Comment");
                TourLog resultTourLog = await _restService.AddTourLog(tourLog);
                Assert.IsTrue(resultTourLog.Id != 0);
                await _restService.DeleteTourLog(resultTourLog.Id);

                List<TourLog> oldList = await _restService.GetAllTourLogs();
                if (oldList != null)
                {
                    foreach (var log in oldList)
                    {
                        if (log.Id == resultTourLog.Id)
                        {
                            await _restService.DeleteTour(resultTour.Id);
                            Assert.Fail("Tour log still in the db by deleting it");
                            return;
                        }
                    }
                    await _restService.DeleteTour(resultTour.Id);
                    Assert.Pass("TourLog deleted");
                }
            }
        }
    }
}