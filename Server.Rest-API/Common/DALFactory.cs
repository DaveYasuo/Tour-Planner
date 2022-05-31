using System;
using System.Reflection;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Server.Rest_API.SqlServer;

namespace Server.Rest_API.Common
{
    internal class DALFactory
    {
        private static IDatabase _database;
        private static readonly IConfigurationRoot Configuration;

        // load DAL assembly
        static DALFactory()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();
        }

        // create database object with connection string from config
        public static IDatabase GetDatabase()
        {
            return _database ??= CreateDatabase();
        }

        private static IDatabase CreateDatabase()
        {
            string connectionString = Configuration.GetConnectionString("DefaultDB");
            return CreateDatabase(connectionString);
        }

        // create database object with specific connection string
        private static IDatabase CreateDatabase(string connectionString)
        {
            string databaseClassName = Configuration.GetSection("DALSqlName").Value;
            Type dbClass = Type.GetType(databaseClassName);

            if (dbClass == null)
            {
                //logger.Log(LogLevel.Error, "Could not setup database:");
                throw new InvalidOperationException("DB assembly not found");
            }

            return Activator.CreateInstance(dbClass, connectionString) as IDatabase;
        }
    }
}
