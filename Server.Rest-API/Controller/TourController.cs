using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using Tour_Planner.Models.DataControllers;

namespace Server.Rest_API.Controller
{
    public class TourController
    {
        private Database _db;

        public TourController(Database connection)
        {
            _db = connection;
        }

        private NpgsqlConnection Connection()
        {
            return _db.Connection();
        }

        private void GetTours()
        {
            try
            {
                var tours = new List<Tour>();
                var conn = Connection();
                using var cmd = new NpgsqlCommand("SELECT * from public.tour;'", conn);
                cmd.Prepare();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    //Tour tour = new Tour(reader..);
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                //logger.Log(LogLevel.Error, "Could not setup database:");
                //logger.Log(LogLevel.Error, ex.StackTrace);
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Please check your database configuration & health status");
            }
        }
    }
}
