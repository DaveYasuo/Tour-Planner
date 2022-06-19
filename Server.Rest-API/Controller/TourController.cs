using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Npgsql;
using Server.Rest_API.API;
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

        private void DeleteATour(int id)
        {
           tourSqlDao.DeleteTour(id);
        }
        private void UpdateTour(Tour tour)
        {
            tourSqlDao.UpdateTour(tour);
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

        public async Task<string> Post(object body)
        {
            Tour tour = JsonSerializer.Deserialize<Tour>(body.ToString());
            MapQuestResponse response = await mapQuest.GetRoute(tour);
            if (response == null) return null;
            tour.Distance = response.Distance;
            tour.Duration = response.Time;
            string result = await mapQuest.GetRouteImagePath(response.BoundingBox, response.SessionId);
            if (result == null) return null;
            tour.ImagePath = result;
            return JsonSerializer.Serialize(AddTour(tour));
        }


        public string Get(object id)
        {
            throw new NotImplementedException();
        }

        public void Patch(object body)
        {
            Tour tour = JsonSerializer.Deserialize<Tour>(body.ToString());
            UpdateTour(tour);
        }

        public void Delete(object id)
        {
            DeleteATour((int)id);
        }
    }
}
