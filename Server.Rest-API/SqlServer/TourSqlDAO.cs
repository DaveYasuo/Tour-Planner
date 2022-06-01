using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using Server.Rest_API.Common;
using Server.Rest_API.DAO;
using Tour_Planner.Models.DataControllers;

namespace Server.Rest_API.SqlServer
{
    public class TourSqlDAO : ITourDAO
    {
        private readonly IDatabase _db;

        public TourSqlDAO()
        {
            _db = DALFactory.GetDatabase();
        }
        private NpgsqlConnection Connection()
        {
            return _db.Connection();
        }

        public Tour FindById(int tourId)
        {
            throw new NotImplementedException();
        }

        public Tour AddNewTour(Tour newTour)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tour> GetTours()
        {
            try
            {
                var tours = new List<Tour>();
                using var conn = Connection();
                using var cmd = new NpgsqlCommand("SELECT * from public.tour;", conn);
                cmd.Prepare();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tours.Add(new Tour(reader.SafeGet<int>("id"),
                        reader.SafeGet<string>("source"),
                        reader.SafeGet<string>("destination"),
                        reader.SafeGet<string>("name"),
                        reader.SafeGet<double>("distance"),
                        reader.SafeGet<string>("description")));
                }
                conn.Close();
                return tours;
            }
            catch (Exception ex)
            {
                //logger.Log(LogLevel.Error, ex.StackTrace);
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Please check your database configuration & health status");
            }
        }
    }
}
