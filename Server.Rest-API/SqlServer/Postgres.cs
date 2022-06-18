using System;
using System.Reflection;
using log4net;
using Npgsql;
using Server.Rest_API.Common;
using Tour_Planner.DataModels.Enums;

namespace Server.Rest_API.SqlServer
{
    public class Postgres : IDatabase
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private string _connString;
        public Postgres(string conString)
        {
            _connString = conString;
            CreateDatabaseIfNotExists();
            NpgsqlConnection.GlobalTypeMapper.MapEnum<RouteType>("routetype");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Rating>("ratingtype");
            NpgsqlConnection.GlobalTypeMapper.MapEnum<Difficulty>("difficultytype");
            Console.WriteLine(_connString);
        }

        private void CreateTablesIfNotExists()
        {
            try
            {
                var conn = Connection();
                // Create Enum if not exists (drop it if exists and recreate)
                using (var cmd = new NpgsqlCommand(@"
                    DROP TYPE IF EXISTS routetype;
                    CREATE TYPE routetype AS  ENUM(
                        'fastest',
                        'shortest',
                        'pedestrian',
                        'bicycle'
                    )
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new NpgsqlCommand(@"
                    DROP TYPE IF EXISTS ratingtype;
                    CREATE TYPE ratingtype AS  ENUM(
                        'very_good',
                        'good',
                        'medium',
                        'bad',
                        'very_bad'
                    )
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }
                using (var cmd = new NpgsqlCommand(@"
                    DROP TYPE IF EXISTS difficultytype;
                    CREATE TYPE difficultytype AS  ENUM(
                        'easy',
                        'medium',
                        'hard'
                    )
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Create tables
                using (var cmd = new NpgsqlCommand(@"
                    Create TABLE IF NOT EXISTS tour(
                        id SERIAL,
                        title VARCHAR(256) NOT NULL,
                        origin VARCHAR(256) NOT NULL,
                        destination VARCHAR(256) NOT NULL,
                        distance DOUBLE PRECISION NOT NULL,
                        description TEXT NOT NULL,
                        duration INTERVAL NOT NULL,
                        imagepath VARCHAR(256) NOT NULL,
                        type routetype NOT NULL,
                        PRIMARY KEY(id)
                    )
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (var cmd = new NpgsqlCommand(@"
                    Create TABLE IF NOT EXISTS tourlog(
                        id SERIAL,
                        tour INTEGER NOT NULL,
                        date_time DATE NOT NULL,
                        total_time INTERVAL NOT NULL,
                        rating ratingtype NOT NULL,
                        difficulty difficultytype NOT NULL,
                        comment TEXT NOT NULL,
                        PRIMARY KEY(id),
                        CONSTRAINT fk_tour
                            FOREIGN KEY(tour) 
                                REFERENCES tour(id)
                                ON DELETE CASCADE 
                    )
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                Log.Fatal("Cannot create tables:", ex);
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Please check your database configuration & health status");
            }
        }

        public NpgsqlConnection Connection()
        {
            var conn = new NpgsqlConnection(_connString);
            conn.Open();
            return conn;
        }

        private void CreateDatabaseIfNotExists()
        {
            try
            {
                var conn = Connection();
                using var cmdChek = new NpgsqlCommand("SELECT 1 FROM pg_database WHERE datname='tourplanner'", conn);
                var dbExists = cmdChek.ExecuteScalar() != null;
                if (dbExists)
                {
                    // Add databases to connString
                    _connString += "Database=tourplanner;";
                    return;
                }

                // Create databases
                using (var cmd = new NpgsqlCommand(@"
                    CREATE DATABASE tourplanner
                        WITH OWNER = postgres
                        ENCODING = 'UTF8'
                ", conn))
                {
                    cmd.ExecuteNonQuery();
                }

                // Add databases to connString
                conn.Close();
                _connString += "Database=tourplanner;";
                // Create tables
                CreateTablesIfNotExists();
            }
            catch (Exception ex)
            {
                Log.Fatal("Cannot create database:", ex);
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Please check your database configuration & health status");
            }
        }
    }
}
