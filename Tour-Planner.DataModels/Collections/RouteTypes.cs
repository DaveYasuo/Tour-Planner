using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tour_Planner.DataModels.Collections
{
    public class RouteTypes : ObservableCollection<string>
    {
        public RouteTypes()
        {
            Add("fastest");
            Add("shortest");
            Add("pedestrian");
            Add("bicycle");
        }
    }
}
