using System;
using System.Reflection;
using log4net;
using Npgsql;
using Server.Rest_API.Common;

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
            CreateTablesIfNotExists();
            Console.WriteLine(_connString);
        }

        private void CreateTablesIfNotExists()
        {
            try
            {
                var conn = Connection();
                // Create tables
                using (var cmd = new NpgsqlCommand(@"
                    Create TABLE IF NOT EXISTS tour(
                        id SERIAL,
                        title VARCHAR(256) NOT NULL,
                        origin VARCHAR(256) NOT NULL,
                        destination VARCHAR(256) NOT NULL,
                        distance DOUBLE PRECISION NOT NULL,
                        description TEXT NOT NULL,
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
                        date DATE NOT NULL,
                        type VARCHAR(256) NOT NULL,
                        duration INTERVAL NOT NULL,
                        distance DOUBLE PRECISION NOT NULL,
                        rating VARCHAR(256) NOT NULL,
                        report TEXT NOT NULL,
                        avgspeed DOUBLE PRECISION NOT NULL,
                        maxspeed DOUBLE PRECISION NOT NULL,
                        heightdifference DOUBLE PRECISION NOT NULL,
                        stops INTEGER NOT NULL,
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
