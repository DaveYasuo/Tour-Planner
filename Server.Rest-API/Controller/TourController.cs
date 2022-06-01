using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using Npgsql;
using Server.Rest_API.SqlServer;
using Tour_Planner.Models;

namespace Server.Rest_API.Controller
{
    public class TourController
    {
        private readonly TourSqlDAO tourSqlDao = new TourSqlDAO();
        public TourController()
        {
        }



        public string GetAllTours()
        {
            IEnumerable<Tour> tours = tourSqlDao.GetTours();
                return tours == null ? "" : JsonSerializer.Serialize(tours);
        }
    }
}
