using System;

namespace Tour_Planner.Models
{
    public class Tour
    {
        // Single Tour class Images will be stored externally and only the path of the image will be saved
        public Tour(int id, string title, string origin, string destination, double distance, string description, TimeSpan duration, string imagePath)
        {
            Id = id;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = distance;
            Description = description;
            Duration = duration;
            ImagePath = imagePath;
        }

        public Tour(string title, string origin, string destination, double distance, string description, TimeSpan duration, string imagePath)
        {
            Id = default;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = distance;
            Description = description;
            Duration = duration;
            ImagePath = imagePath;
        }

        public Tour(string title, string origin, string destination, string description)
        {
            Id = default;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = default;
            Description = description;
            Duration = default;
            ImagePath = default;
        }

        public Tour()
        {

        }

        public int Id { get; }
        public string Title { get; }
        public string Origin { get; }
        public string Destination { get; }
        public double Distance { get; }
        public string Description { get; }
        public TimeSpan Duration { get; }
        public string ImagePath { get; }
    }
}
