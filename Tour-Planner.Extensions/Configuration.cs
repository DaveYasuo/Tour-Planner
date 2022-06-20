using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using log4net;

namespace Tour_Planner.Extensions
{
    public class Configuration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        static Configuration()
        {
            try
            {
                NameValueCollection pathsCollection = ConfigurationManager.GetSection("path") as NameValueCollection ?? throw new KeyNotFoundException("Missing section 'path'");
                RouteImagePath = pathsCollection.Get("RouteImagePath") ?? throw new KeyNotFoundException(nameof(RouteImagePath));
                AppImagePath = pathsCollection.Get("AppImagePath") ?? throw new KeyNotFoundException(nameof(AppImagePath));
            }
            catch (Exception e)
            {
                Log.Error("Cannot read Config File: " + e.Message);
                throw;
            }
        }
        public static string RouteImagePath { get; }
        public static string AppImagePath { get; }
    }
}
