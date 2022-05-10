using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;

namespace Server.Rest_API
{
    internal class DatabaseConnection
    {
        public DatabaseConnection()
        {
            CreateDatabaseIfNotExists();
        }

        private string connString = "Host=host.docker.internal; Port=5432; Username=postgres; Password=tourplanner;";

        private NpgsqlConnection Connection()
        {
            var conn = new NpgsqlConnection(connString);
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
                    connString += "Database=tourplanner;";
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
                connString += "Database=tourplanner;";
                conn = Connection();
                // Create tables
                using (var cmd = new NpgsqlCommand(@"
                    Create TABLE IF NOT EXISTS tour(
                        id SERIAL,
                        source VARCHAR(256) NOT NULL,
                        destination VARCHAR(256) NOT NULL,
                        name VARCHAR(256) NOT NULL,
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
                //logger.Log(LogLevel.Error, "Could not setup database:");
                //logger.Log(LogLevel.Error, ex.StackTrace);
                Console.WriteLine(ex.Message);
                throw new ApplicationException("Please check your database configuration & health status");
            }
        }
    }
}
