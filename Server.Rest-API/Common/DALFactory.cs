using log4net;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Reflection;

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
                .AddJsonFile("properties\\appsettings.json", optional: false, reloadOnChange: true)
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

            if (dbClass != null) return Activator.CreateInstance(dbClass, connectionString) as IDatabase;
            Log.Error("Could not setup database: DB class not found");
            throw new InvalidOperationException("DB class not found");

        }

        public static IMapQuest GetMapQuestApi()
        {
            return _mapQuest ??= CreateMapQuestApi();
        }

        private static IMapQuest CreateMapQuestApi()
        {
            IConfigurationSection mapQuest = Configuration.GetSection("MapQuest");
            string imagePath = Configuration.GetSection("ImagePath").Value;
            string key = mapQuest.GetSection("Key").Value;
            string mapQuestClassName = mapQuest.GetSection("DALMapQuestName").Value;
            Type mapQuestClass = Type.GetType(mapQuestClassName);
            if (mapQuestClass != null) return Activator.CreateInstance(mapQuestClass, key, imagePath) as IMapQuest;
            Log.Error("Could not setup MapAPI: MapAPI class not found");
            throw new InvalidOperationException("MapAPI class not found");

        }
    }
}
