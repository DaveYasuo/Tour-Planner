using log4net;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.Services
{
    public class RestService : IRestService
    {

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public readonly string BaseUrl = "http://localhost:8888/api";

        private static readonly HttpClient Client = new();


        public async Task<Tour?> AddTour(Tour tour)
        {
            try
            {
                var httpResponseMessage = await Client.PostAsync($"{BaseUrl}/Tour", new StringContent(JsonSerializer.Serialize(tour), Encoding.UTF8, "application/json"));
                if (!httpResponseMessage.IsSuccessStatusCode) return null;
                string result = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<Tour>(result);
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot add tour from Server " + ex.Message);
                return null;
            }
        }

        public async Task<List<Tour>?> GetTours()
        {
            try
            {
                var result = await Client.GetFromJsonAsync<List<Tour>>($"{BaseUrl}/Tour");
                return result ?? null;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot get all tours from Server " + ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteTour(int id)
        {
            var result = await Client.DeleteAsync($"{BaseUrl}/Tour/" + id);
            return result.IsSuccessStatusCode;
        }

        public async Task<TourLog?> AddTourLog(TourLog tourLog)
        {
            try
            {
                var httpResponseMessage = await Client.PostAsync($"{BaseUrl}/TourLog", new StringContent(JsonSerializer.Serialize(tourLog), Encoding.UTF8, "application/json"));
                if (!httpResponseMessage.IsSuccessStatusCode) return null;
                string result = await httpResponseMessage.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<TourLog>(result);
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot add tour from Server " + ex.Message);
                return null;
            }

        }

        public async Task<bool> UpdateTour(Tour tour)
        {
            var result = await Client.PatchAsync($"{BaseUrl}/Tour", new StringContent(JsonSerializer.Serialize(tour), Encoding.UTF8, "application/json"));
            return result.IsSuccessStatusCode;
        }

        public async Task<List<TourLog>?> GetAllTourLogs()
        {
            try
            {
                var result = await Client.GetStringAsync($"{BaseUrl}/TourLog");
                return result != "" ? JsonSerializer.Deserialize<List<TourLog>>(result) : null;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot get all tours logs from Server " + ex.Message);
                return null;
            }
        }

        public async Task<List<TourLog>?> GetAllTourLogsFromTour(Tour tour)
        {
            try
            {
                var result = await Client.GetStringAsync($"{BaseUrl}/TourLog/" + tour.Id);
                return result != "" ? JsonSerializer.Deserialize<List<TourLog>>(result) : null;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot get all tours logs from one tour from Server " + ex.Message);
                return null;
            }
        }

        public async Task<bool> DeleteTourLog(int id)
        {
            var result = await Client.DeleteAsync($"{BaseUrl}/TourLog/" + id);
            return result.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateTourLog(TourLog newTour)
        {
            try
            {
                var responseMessage = await Client.PatchAsync($"{BaseUrl}/TourLog", new StringContent(JsonSerializer.Serialize(newTour), Encoding.UTF8, "application/json"));
                return responseMessage.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Log.Warn("Cannot update a tour log from Server " + ex.Message);
                return false;
            }
        }
    }
}
