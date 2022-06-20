using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Server.Rest_API.SqlServer;
using Tour_Planner.Models;

namespace Server.Rest_API.Controller
{
    public class TourLogController : IController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly TourLogSqlDAO _tourLogSqlDao = new();

        private TourLog AddTourLog(TourLog tourLog)
        {
            return _tourLogSqlDao.AddNewTourLog(tourLog);
        }
        private IEnumerable<TourLog> GetAllTourLogs()
        {
            return _tourLogSqlDao.GetAllTourLogs();
        }

        public void Delete(object id)
        {
            DeleteATourLog((int)id);
        }

        private void DeleteATourLog(int id)
        {
            _tourLogSqlDao.DeleteTourLog(id);
        }

        public string Get()
        {
            try
            {
                string json = JsonSerializer.Serialize(GetAllTourLogs());
                Log.Info("Serialize all tour logs");
                return json;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot serialize all tour logs: " + ex.Message);
                return null;
            }
        }

        public string Get(object id)
        {
            try
            {
                int tourId = int.Parse(id.ToString()!);
                string json = JsonSerializer.Serialize(GetAllTourLogsFromTour(tourId));
                Log.Info("Serialize all tour logs");
                return json;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot serialize all tour logs: " + ex.Message);
                return null;
            }
        }

        private IEnumerable<TourLog> GetAllTourLogsFromTour(int tourId)
        {
            return _tourLogSqlDao.GetAllTourLogsFromTour(tourId);
        }

        public Task<string> Post(object body)
        {
            TourLog tourLog = JsonSerializer.Deserialize<TourLog>(body.ToString()!);
            return Task.Run(() => JsonSerializer.Serialize(AddTourLog(tourLog)));
        }

        public void Patch(object body)
        {
            TourLog tourLog = JsonSerializer.Deserialize<TourLog>(body.ToString()!);
            UpdateTourLog(tourLog);
        }

        private void UpdateTourLog(TourLog tourLog)
        {
            _tourLogSqlDao.UpdateTourLog(tourLog);
        }
    }
}
