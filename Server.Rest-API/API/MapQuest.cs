using log4net;
using Server.Rest_API.Common;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using Tour_Planner.Models;

namespace Server.Rest_API.API
{
    public class MapQuest : IMapQuest
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public readonly string RouteUrl = "http://open.mapquestapi.com/directions/v2/route";
        public readonly string RouteImageUrl = "http://www.mapquestapi.com/staticmap/v4/getmap";
        private static readonly HttpClient Client = new();
        private readonly string MapQuestKey;

        public MapQuest(string key)
        {
            MapQuestKey = key;
        }



        public async Task GetRoute(Tour tour)
        {
            var builder = new UriBuilder(RouteUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = MapQuestKey;
            query["from"] = tour.Origin;
            query["to"] = tour.Destination;
            builder.Query = query.ToString();
            string uri = builder.ToString();
            var result = await Client.GetAsync(uri);
            Debug.WriteLine(result.ToString());
            Debug.WriteLine(await result.Content.ReadAsStringAsync());
            return;
            // return result.IsSuccessStatusCode;
        }
        public async Task GetRouteImage()
        {
            //var result = await Client.PostAsync($"{RouteUrl}/Tour", new StringContent(JsonSerializer.Serialize(tour), Encoding.UTF8, "application/json"));
            // return result.IsSuccessStatusCode;
        }
    }
}
