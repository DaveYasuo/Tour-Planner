namespace Tour_Planner.Models
{
    public class Tour
    {
        public Tour(int id, string title, string origin, string destination, double distance, string description)
        {
            Id = id;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = distance;
            Description = description;
        }

        public Tour(string title, string origin, string destination, double distance, string description)
        {
            Id = 0;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = distance;
            Description = description;
        }

        public Tour(string title, string origin, string destination, string description)
        {
            Id = 0;
            Title = title;
            Origin = origin;
            Destination = destination;
            Distance = 0;
            Description = description;
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
    }
}
