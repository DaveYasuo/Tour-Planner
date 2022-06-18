using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.Models
{
    public class TourLog
    {
        public TourLog( int tourId, DateTime dateAndTime,TimeSpan totalTime, Rating rating, Difficulty difficulty,string comment)
        {
            Id = default;
            TourId = tourId;
            DateAndTime = dateAndTime;
            TotalTime = totalTime;
            Rating = rating;
            Difficulty = difficulty;
        }

        public int Id { get; set; }//Default
        public int TourId { get; set; }
        public DateTime DateAndTime { get; set; }//Default aus User perspektive wenn er angeben will wann er war is relevanter
        public TimeSpan TotalTime { get; set; }
        public Rating Rating { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Comment { get; set; }

    }
}
