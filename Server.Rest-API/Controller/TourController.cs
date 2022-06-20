using log4net;
using Server.Rest_API.API;
using Server.Rest_API.Common;
using Server.Rest_API.SqlServer;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Tour_Planner.Models;

namespace Server.Rest_API.Controller
{
    public class TourController : IController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly TourSqlDAO _tourSqlDao = new();
        private readonly IMapQuest _mapQuest = DALFactory.GetMapQuestApi();

        private IEnumerable<Tour> GetAllTours()
        {
            return _tourSqlDao.GetTours();
        }

        private Tour AddTour(Tour tour)
        {
            return _tourSqlDao.AddNewTour(tour);
        }

        private void DeleteATour(int id)
        {
            _tourSqlDao.DeleteTour(id);
        }
        private void UpdateTour(Tour tour)
        {
            _tourSqlDao.UpdateTour(tour);
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
            try
            {
                Tour tour = JsonSerializer.Deserialize<Tour>(body.ToString()!);
                MapQuestResponse response = await _mapQuest.GetRoute(tour);
                if (response == null) return null;
                if (tour != null)
                {
                    tour.Distance = response.Distance;
                    tour.Duration = response.Time;
                    string result = await _mapQuest.GetRouteImagePath(response.BoundingBox, response.SessionId);
                    if (result == null) return null;
                    tour.ImagePath = result;
                    return JsonSerializer.Serialize(AddTour(tour));
                }
                Log.Warn("Response does not contain a tour.");
                return null;
            }
            catch (Exception e)
            {
                Log.Error("Cannot add tour: " + e.Message);
                return null;
            }
        }


        public string Get(object id)
        {
            throw new NotImplementedException();
        }

        public void Patch(object body)
        {
            try
            {
                Tour tour = JsonSerializer.Deserialize<Tour>(body.ToString()!);
                UpdateTour(tour);
            }
            catch (Exception e)
            {
                Log.Error("Cannot update tour: " + e.Message);
            }

        }

        public void Delete(object id)
        {
            DeleteATour((int)id);
        }
    }
}
