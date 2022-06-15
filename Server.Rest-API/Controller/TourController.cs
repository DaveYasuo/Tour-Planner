using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using log4net;
using Npgsql;
using Server.Rest_API.Common;
using Server.Rest_API.SqlServer;
using Tour_Planner.Models;

namespace Server.Rest_API.Controller
{
    public class TourController : IController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly TourSqlDAO tourSqlDao = new TourSqlDAO();
        private readonly IMapQuest mapQuest = DALFactory.GetMapQuestAPI();
        public TourController()
        {
        }

        private IEnumerable<Tour> GetAllTours()
        {
            return tourSqlDao.GetTours();
        }

        private Tour AddTour(Tour tour)
        {
            return tourSqlDao.AddNewTour(tour);
        }

        public string Get()
        {
            try
            {
                string json = JsonSerializer.Serialize(GetAllTours());
                Log.Info("Serialize all tours");
                return json;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot serialize all tours: " + ex.Message);
                return null;
            }
        }

        public string Post(string body)
        {
            Tour tour = JsonSerializer.Deserialize<Tour>(body);
            mapQuest.GetRoute(tour);
            return JsonSerializer.Serialize(AddTour(tour));
        }


        public string Get(object id)
        {
            throw new NotImplementedException();
        }

        public void Put(object id)
        {
            throw new NotImplementedException();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }
    }
}
