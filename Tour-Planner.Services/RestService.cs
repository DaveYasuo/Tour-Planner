using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.Services
{
    public class RestService : IRestService
    {
        
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public readonly string Url = "http://localhost:8888/";

        private static readonly HttpClient Client = new();
        public RestService()
        {
            Test();
        }

        public async void Test()
        {
            Log.Info("Sending request");
            var responseString = await Client.GetStringAsync(Url);
            Debug.WriteLine(responseString);
        }
    }
}
