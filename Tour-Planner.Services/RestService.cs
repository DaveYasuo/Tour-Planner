using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.Services
{
    public class RestService : IRestService
    {
        public readonly string Url = "http://localhost:8888/";

        private static readonly HttpClient Client = new();
        public RestService()
        {
            Test();
        }

        public async void Test()
        {
            var responseString = await Client.GetStringAsync(Url);
        }
    }
}
