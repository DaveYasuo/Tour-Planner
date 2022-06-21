using Server.Rest_API.API;
using System.Threading.Tasks;
using Tour_Planner.Models;

namespace Server.Rest_API.Common
{
    public interface IMapQuest
    {
        Task<MapQuestResponse> GetRoute(Tour tour);
        Task<string> GetRouteImagePath(string boundingBox, string sessionId);
    }
}
