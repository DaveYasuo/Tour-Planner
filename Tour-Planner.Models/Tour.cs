namespace Tour_Planner.Models
{
    public class Tour
    {
        //Single Tour class Images will be stored externally and only the path of the image will be saved
        public Tour(int id, string source, string destination, string name, double distance, string description)
        {
            _id = id;
            _source = source;
            _destination = destination;
            _name = name;
            _distance = distance;
            _description = description;
            //_picture = picture;
        }

        public Tour(string source, string destination, string name, double distance, string description)
        {
            _source = source;
            _destination = destination;
            _name = name;
            _distance = distance;
            _description = description;
            _id = 0;
            //_picture = picture;
        }

        public Tour()
        {

        }

        public int _id { get; set; }
        public string _source { get; set; }
        public string _destination { get; set; }
        public string _name { get; set; }
        public double _distance { get; set; }
        public string _description { get; set; }
        //public byte[] _picture { get; set; }
        }
}
