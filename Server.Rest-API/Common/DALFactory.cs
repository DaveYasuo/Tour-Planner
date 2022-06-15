using System;
using System.Reflection;
using System.Configuration;
using System.IO;
using Microsoft.Extensions.Configuration;
using Server.Rest_API.SqlServer;
using log4net;

namespace Server.Rest_API.Common
{
    internal class DALFactory
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private static IDatabase _database;
        private static readonly IConfigurationRoot Configuration;
        private static IMapQuest _mapQuest;

        // load DAL assembly
        static DALFactory()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddUserSecrets<Program>()
                .Build();
            Log.Debug("Build configuration from appsettings.json and secret file");
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
                throw new InvalidOperationException("DB class not found");
            }

            return Activator.CreateInstance(dbClass, connectionString) as IDatabase;
        }

        public static IMapQuest GetMapQuestAPI()
        {
            return _mapQuest ??= CreateMapQuestAPI();
        }

        private static IMapQuest CreateMapQuestAPI()
        {
            IConfigurationSection mapQuest = Configuration.GetSection("MapQuest");
            string key = mapQuest.GetSection("Key").Value;
            string mapQuestClassName = mapQuest.GetSection("DALMapQuestName").Value;
            Type mapQuestClass = Type.GetType(mapQuestClassName);
            return Activator.CreateInstance(mapQuestClass, key) as IMapQuest;
        }
    }
}
