﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Npgsql;
using Server.Rest_API.Common;
using Server.Rest_API.SqlServer;
using Tour_Planner.Models;

namespace Server.Rest_API.Controller
{
    public class TourLogController : IController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private readonly TourLogSqlDAO tourLogSqlDao = new TourLogSqlDAO();
        public TourLogController()
        {
        }
        private TourLog AddTourLog(TourLog tourLog)
        {
            return tourLogSqlDao.AddNewTourLog(tourLog);
        }
        private IEnumerable<TourLog> GetAllTourLogs()
        {
            return tourLogSqlDao.GetAllTourLogs();
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public string Get()
        {
            try
            {
                string json = JsonSerializer.Serialize(GetAllTourLogs());
                Log.Info("Serialize all tourlogs");
                return json;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot serialize all tourlogs: " + ex.Message);
                return null;
            }
        }

        public string Get(object id)
        {
            try
            {
                int tourId = int.Parse(id.ToString());
                string json = JsonSerializer.Serialize(GetAllTourLogsFromTour(tourId));
                Log.Info("Serialize all tourlogs");
                return json;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot serialize all tourlogs: " + ex.Message);
                return null;
            }
        }

        private IEnumerable<TourLog> GetAllTourLogsFromTour(int tourId)
        {
            return tourLogSqlDao.GetAllTourLogsFromTour(tourId);
        }

        public Task<string> Post(string body)
        {
            TourLog tourLog = JsonSerializer.Deserialize<TourLog>(body);
            //mapQuest.GetRoute(tourLog);
            return Task.Run(() => JsonSerializer.Serialize(AddTourLog(tourLog)));
        }

        public void Patch(string body)
        {
            throw new NotImplementedException();
        }
    }
}
