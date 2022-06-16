using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.Services
{
    public class RestService : IRestService
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public readonly string BaseUrl = "http://localhost:8888/api";

        private static readonly HttpClient Client = new();
        public RestService()
        {
            Test();
        }

        public async void Test()
        {
            Log.Info("Sending request");
            try
            {
                var responseString = await Client.GetStringAsync(BaseUrl);
                Debug.WriteLine(responseString);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

        public async Task<bool> AddTour(Tour tour)
        {
            var result = await Client.PostAsync($"{BaseUrl}/Tour", new StringContent(JsonSerializer.Serialize(tour), Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> GetAllTours(Tour tour)
        {
            var result = await Client.GetAsync($"{BaseUrl}/Tour");

            return result.IsSuccessStatusCode;
        }
    }
}
