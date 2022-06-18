using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;

namespace Tour_Planner.Models
{
    public class TourLog
    {
        public TourLog(int id, int tourId, DateTime dateTime, TimeSpan totalTime, Rating rating, Difficulty difficulty, string comment)
        {
            Id = id;
            TourId = tourId;
            DateTime = dateTime;
            TotalTime = totalTime;
            Rating = rating;
            Difficulty = difficulty;
            Comment = comment;
        }

        public TourLog(int tourId, DateTime dateTime, TimeSpan totalTime, Rating rating, Difficulty difficulty, string comment)
        {
            Id = default;
            TourId = tourId;
            DateTime = dateTime;
            TotalTime = totalTime;
            Rating = rating;
            Difficulty = difficulty;
            Comment = comment;
        }
        public TourLog() { }

        public int Id { get; set; }//Default
        public int TourId { get; set; }
        public DateTime DateTime { get; set; }//Default aus User perspektive wenn er angeben will wann er war is relevanter

        [JsonConverter(typeof(TimeSpanConverter))]
        public TimeSpan TotalTime { get; set; }
        public Rating Rating { get; set; }
        public Difficulty Difficulty { get; set; }
        public string Comment { get; set; }

    }
}
