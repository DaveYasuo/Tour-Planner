using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;

namespace Tour_Planner.DataModels.Collections
{
    public class RouteTypes : ObservableCollection<string>
    {
        public RouteTypes()
        {
            foreach (string routeType in Enum.GetNames(typeof(RouteType)))
            {
                Add(routeType);
            }
        }
    }
}
