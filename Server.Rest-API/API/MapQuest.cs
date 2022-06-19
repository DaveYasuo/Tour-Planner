using log4net;
using Server.Rest_API.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Text.Json.Nodes;
using System.Threading.Tasks;
using System.Web;
using Tour_Planner.Models;

namespace Server.Rest_API.API
{
    public class MapQuest : IMapQuest
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        public readonly string RouteUrl = "http://open.mapquestapi.com/directions/v2/route";
        public readonly string RouteImageUrl = "http://www.mapquestapi.com/staticmap/v5/map";
        private static readonly HttpClient Client = new();
        private readonly string _mapQuestKey;

        public MapQuest(string key)
        {
            _mapQuestKey = key;
        }



        public async Task<MapQuestResponse> GetRoute(Tour tour)
        {
            var builder = new UriBuilder(RouteUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = _mapQuestKey;
            query["from"] = tour.Origin;
            query["to"] = tour.Destination;
            query["unit"] = "k";
            query["routeType"] = tour.RouteType.ToString();
            builder.Query = query.ToString()!;
            string uri = builder.ToString();
            var response = await Client.GetAsync(uri);
            Debug.WriteLine(await response.Content.ReadAsStringAsync());
            try
            {
                if (response.IsSuccessStatusCode)
                {
                    var jsonString = response.Content.ReadAsStringAsync().Result;
                    JsonObject mapQuest = JsonNode.Parse(jsonString)!.AsObject();
                    if (mapQuest["route"]?["distance"] is { } tmpDistance &&
                        mapQuest["route"]?["boundingBox"] is { } tmpBoundingBox &&
                        mapQuest["route"]?["sessionId"] is { } tmpSessionId &&
                        mapQuest["route"]?["formattedTime"] is { } tmpTime)
                    {
                        TimeSpan time = TimeSpan.Parse(tmpTime.ToString());
                        double distance = tmpDistance.GetValue<double>();
                        string boundingBox =
                            tmpBoundingBox["ul"]!["lat"]!.ToString().Replace(",", ".") + ","
                            + tmpBoundingBox["ul"]!["lng"]!.ToString().Replace(",", ".") + ","
                            + tmpBoundingBox["lr"]!["lat"]!.ToString().Replace(",", ".") + ","
                            + tmpBoundingBox["lr"]!["lng"]!.ToString().Replace(",", ".");
                        string sessionId = tmpSessionId.GetValue<string>();
                        Log.Info("Get route from Mapquest successfully");
                        return new MapQuestResponse(time, distance, boundingBox, sessionId);
                    }
                }
                Log.Error("MapQuest returned invalid route.");
                return null;
            }
            catch (Exception)
            {
                Log.Error("Parsing Bounding Box failed.");
                return null;
            }
        }
        public async Task<string> GetRouteImagePath(string boundingBox, string sessionID)
        {
            var builder = new UriBuilder(RouteImageUrl);
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["key"] = _mapQuestKey;
            query["session"] = sessionID;
            query["size"] = "1920,1440";
            query["boundingBox"] = boundingBox;
            builder.Query = query.ToString()!;
            string uri = builder.ToString();
            try
            {
                var response = await Client.GetAsync(uri);

                if (response.IsSuccessStatusCode)
                {
                    string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".\\..\\..\\..\\..\\RouteImages");
                    Directory.CreateDirectory(folderPath);
                    string imageName = Guid.NewGuid() + ".jpeg";
                    await using (var fs = new FileStream(Path.Combine(folderPath, imageName), FileMode.OpenOrCreate))
                    {
                        await response.Content.CopyToAsync(fs);
                    }
                    Log.Info("Saved image as " + imageName);
                    return imageName;
                }
                Log.Error("Could not request image from MapQuest.");
            }
            catch (Exception)
            {
                Log.Error("Could not download image from MapQuest.");
            }
            return null;
        }
    }
}
