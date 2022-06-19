using System;
using System.Collections.ObjectModel;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.DataModels.Collections
{
    public class RatingTypes : ObservableCollection<Rating>
    {
        public RatingTypes()
        {
            foreach (Rating rating in Enum.GetValues(typeof(Rating)))
            {
                Add(rating);
            }
        }
    }
}
