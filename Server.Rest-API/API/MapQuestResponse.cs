using System;

namespace Server.Rest_API.API
{
    public class MapQuestResponse
    {
        public TimeSpan Time { get; }
        public double Distance { get; }
        public string BoundingBox { get; }
        public string SessionId { get; }

        public MapQuestResponse(TimeSpan time, double distance, string boundingBox, string sessionId)
        {
            Time = time;
            Distance = distance;
            BoundingBox = boundingBox;
            SessionId = sessionId;
        }
    }
}
