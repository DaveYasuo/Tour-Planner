﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_Planner.Models.DataControllers
{
    public class Tour
    {
        public Tour(int id, string source, string destination, string name, double distance, string description)
        {
            Id = id;
            Source = source;
            Destination = destination;
            Name = name;
            Distance = distance;
            Description = description;
        }

        public Tour(string source, string destination, string name, double distance, string description)
        {
            Source = source;
            Destination = destination;
            Name = name;
            Distance = distance;
            Description = description;
            Id = 0;
        }

        public Tour()
        {

        }

        public int Id { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public string Name { get; set; }
        public double Distance { get; set; }
        public string Description { get; set; }
        }
}
