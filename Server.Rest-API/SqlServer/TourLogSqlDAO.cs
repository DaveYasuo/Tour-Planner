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
    public class TourLogSqlDAO :ITourLogDAO
    {
        private readonly IDatabase _db;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public TourLogSqlDAO()
        {
            _db = DALFactory.GetDatabase();
        }
        private NpgsqlConnection Connection()
        {
            return _db.Connection();
        }
        public TourLog AddNewTourLog(TourLog tourLog)
        {
            using var conn = Connection();
            using var transaction = conn.BeginTransaction();
            try
            {
                using (var cmd = new NpgsqlCommand("INSERT INTO public.tourlog (id, tour, date_time, total_time, rating, difficulty, comment) VALUES (DEFAULT, @tour, @date_time, @total_time, @rating, @difficulty, @comment);", conn))
                {
                    cmd.Parameters.AddWithValue("tour", tourLog.TourId);
                    cmd.Parameters.AddWithValue("date_time", tourLog.DateTime);
                    cmd.Parameters.AddWithValue("total_time", tourLog.TotalTime);
                    cmd.Parameters.AddWithValue("rating", tourLog.Rating);
                    cmd.Parameters.AddWithValue("difficulty", tourLog.Difficulty);
                    cmd.Parameters.AddWithValue("comment", tourLog.Comment);
                    cmd.Prepare();
                    cmd.ExecuteNonQuery();
                    transaction.Commit();
                    Log.Info($"Inserted tourLog ");
                    return tourLog;
                };
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                Log.Error($"Cannot insert tourLog: " + ex.Message);
                Console.WriteLine($"Cannot insert tourLog: " + ex.Message);
                return null;
            }
        }

        public IEnumerable<TourLog> GetAllTourLogs()
        {
            try
            {
                var tourLogs = new List<TourLog>();
                using var conn = Connection();
                using var cmd = new NpgsqlCommand("SELECT id, tour, date_time, total_time, rating, difficulty, comment from public.tourlog;", conn);
                cmd.Prepare();
                using var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    tourLogs.Add(new TourLog(reader.SafeGet<int>("id"),
                        reader.SafeGet<int>("tour"),
                        reader.SafeGet<DateTime>("date_time"),
                        reader.SafeGet<TimeSpan>("total_time"),
                        reader.SafeGet<Rating>("rating"),
                        reader.SafeGet<Difficulty>("difficulty"),
                        reader.SafeGet<string>("comment")));
                }
                conn.Close();
                Log.Info("Get all tours");
                return tourLogs;
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot get all tourlogs: " + ex.Message);
                return null;
            }
        }
    }
}
