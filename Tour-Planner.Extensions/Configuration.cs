using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Tour_Planner.Extensions
{
    public class Configuration
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public NameValueCollection PathsCollection { get; }
        public Configuration()
        {
            try
            {
                PathsCollection = ConfigurationManager.GetSection("path") as NameValueCollection ?? throw new KeyNotFoundException("Missing section 'path'");
            }
            catch (Exception e)
            {
                Log.Error("Cannot read Config File: " + e.Message);
                throw;
            }
        }
    }
}
