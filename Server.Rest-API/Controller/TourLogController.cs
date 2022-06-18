using System;
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
        private readonly IMapQuest mapQuest = DALFactory.GetMapQuestAPI();
        public TourLogController()
        {
        }
        private TourLog AddTourLog(TourLog tourLog)
        {
            return tourLogSqlDao.AddNewTourLog(tourLog);
        }

        public void Delete(object id)
        {
            throw new NotImplementedException();
        }

        public string Get()
        {
            throw new NotImplementedException();
        }

        public string Get(object id)
        {
            throw new NotImplementedException();
        }

        public Task<string> Post(string body)
        {
            TourLog tourLog = JsonSerializer.Deserialize<TourLog>(body);
            //mapQuest.GetRoute(tourLog);
            return Task.Run(() => JsonSerializer.Serialize(AddTourLog(tourLog)));
        }

        public void Put(object id)
        {
            throw new NotImplementedException();
        }
    }
}
