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
        }



        public async Task<bool> AddTour(Tour tour)
        {
            var result = await Client.PostAsync($"{BaseUrl}/Tour", new StringContent(JsonSerializer.Serialize(tour), Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        public async Task<List<Tour>?> GetTour()
        {
            try
            {
                var result = await Client.GetStringAsync($"{BaseUrl}/Tour");
                if (result is not null && result != "")
                {
                    return JsonSerializer.Deserialize<List<Tour>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteTour(int id)
        {
            var result = await Client.DeleteAsync($"{BaseUrl}/Tour/" + id);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> AddTourLog(TourLog tourLog)
        {
            var result = await Client.PostAsync($"{BaseUrl}/TourLog", new StringContent(JsonSerializer.Serialize(tourLog), Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTour(Tour tour)
        {
            var result = await Client.PatchAsync($"{BaseUrl}/Tour", new StringContent(JsonSerializer.Serialize(tour),Encoding.UTF8,"application/json"));
            return result.IsSuccessStatusCode;
        }

        public async Task<List<TourLog>?> GetTourLogs()
        {
            try
            {
                var result = await Client.GetStringAsync($"{BaseUrl}/TourLog");
                if (result is not null && result != "")
                {
                    return JsonSerializer.Deserialize<List<TourLog>>(result);
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
