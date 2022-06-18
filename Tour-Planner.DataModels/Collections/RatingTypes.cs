using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.DataModels.Collections
{
    public class RatingTypes : ObservableCollection<string>
    {
        public RatingTypes()
        {
            foreach (string ratingType in Enum.GetNames(typeof(RatingType)))
            {
                Add(ratingType);
            }
        }
    }
}
