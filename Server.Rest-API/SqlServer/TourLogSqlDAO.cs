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
                using (var cmd = new NpgsqlCommand("INSERT INTO public.tour (id, tourid, date_and_time, total_time, rating, difficulty, comment) VALUES (DEFAULT, @id, @tourid, @date_and_time, @total_time, @rating, @difficulty, @comment);", conn))
                {
                    cmd.Parameters.AddWithValue("tourid", tourLog.TourId);
                    cmd.Parameters.AddWithValue("date_and_time", tourLog.DateAndTime);
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
    }
}
