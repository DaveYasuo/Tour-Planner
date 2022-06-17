using System;
using System.Collections.Generic;
using System.Data;
using log4net;
using System.Reflection;
using Npgsql;
using Server.Rest_API.Common;
using Server.Rest_API.DAO;
using Tour_Planner.Models;
using Tour_Planner.DataModels.Enums;

namespace Server.Rest_API.SqlServer
{
    public class TourSqlDAO : ITourDAO
    {
        private readonly IDatabase _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

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

        public Tour AddNewTour(Tour tour)
        {
            using var conn = Connection();
            using var transaction = conn.BeginTransaction();
            try
            {
                using (var cmd = new NpgsqlCommand("INSERT INTO public.tour (id, title, origin, destination, distance, description, duration, imagepath, type) VALUES (DEFAULT, @title, @origin, @destination, @distance, @description, @duration, @imagepath, @type);", conn))
                {
                    cmd.Parameters.AddWithValue("title", tour.Title);
                    cmd.Parameters.AddWithValue("origin", tour.Origin);
                    cmd.Parameters.AddWithValue("destination", tour.Destination);
                    cmd.Parameters.AddWithValue("distance", tour.Distance);
                    cmd.Parameters.AddWithValue("description", tour.Description);
                    cmd.Parameters.AddWithValue("duration", tour.Duration);
                    cmd.Parameters.AddWithValue("imagepath", tour.ImagePath);
                    cmd.Parameters.AddWithValue("type", tour.RouteType);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    Log.Info($"Inserted tour {tour.Title}");
                    return tour;
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error($"Cannot insert tour {tour.Title}: " + ex.Message);
                Console.WriteLine($"Cannot insert tour {tour.Title}: " + ex.Message);
                return null;
            }
        }

        public IEnumerable<Tour> GetTours()
        {
            try
            {
                var tours = new List<Tour>();
                using var conn = Connection();
                using var cmd = new NpgsqlCommand("SELECT id, title, origin, destination, distance, description, duration, imagepath, type from public.tour;", conn);
                cmd.Prepare();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tours.Add(new Tour(reader.SafeGet<int>("id"),
                        reader.SafeGet<string>("title"),
                        reader.SafeGet<string>("origin"),
                        reader.SafeGet<string>("destination"),
                        reader.SafeGet<double>("distance"),
                        reader.SafeGet<string>("description"),
                        reader.SafeGet<TimeSpan>("duration"),
                        reader.SafeGet<string>("imagepath"),
                        reader.SafeGet<RouteType>("type")));
                }
                conn.Close();
                Log.Info("Get all tours");
                return tours;
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot get all tours: " + ex.Message);
                return null;
            }
        }

        public List<string> getTourNames()
        {
            try
            {
                List<String> tourNames = new List<String>();
                using var conn = Connection();
                using var cmd = new NpgsqlCommand("SELECT title from public.tour;", conn);
                cmd.Prepare();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tourNames.Add(reader.GetString(0));
                }
                conn.Close();
                Log.Info("Get all tour names");
                return tourNames;
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot get all tour names: " + ex.Message);
                return null;
            }

        }
    }
}
