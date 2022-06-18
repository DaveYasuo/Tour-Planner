using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.DataModels.Collections
{
    public class RouteTypes : ObservableCollection<RouteType>
    {
        public RouteTypes()
        {
            foreach (RouteType routeType in Enum.GetValues(typeof(RouteType)))
            {
                Add(routeType);
            }
        }
    }
}
