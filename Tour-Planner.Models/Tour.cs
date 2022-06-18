using System;
using System.Text.Json.Serialization;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;

namespace Tour_Planner.Models
{
    public class Tour
    {
        // Single Tour class Images will be stored externally and only the path of the image will be saved
        public Tour(int id, string title, string origin, string destination, double distance, string description, TimeSpan duration, string imagePath, RouteType routeType)
        {
            Id = id;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = distance;
            Description = description;
            Duration = duration;
            ImagePath = imagePath;
            RouteType = routeType;
        }

        public Tour(string title, string origin, string destination, double distance, string description, TimeSpan duration, string imagePath, RouteType routeType)
        {
            Id = default;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = distance;
            Description = description;
            Duration = duration;
            ImagePath = imagePath;
            RouteType = routeType;
        }

        public Tour(string title, string origin, string destination, string description, RouteType routeType)
        {
            Id = default;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = default;
            Description = description;
            Duration = default;
            ImagePath = "";
            RouteType = routeType;
        }

        public Tour()
        {

        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Origin { get; set; }
        public string Destination { get; set; }
        public double Distance { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan Duration { get; set; }
        public string ImagePath { get; set; }
        public RouteType RouteType { get; set; }
    }
}
