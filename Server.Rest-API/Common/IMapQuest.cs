using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.Models;

namespace Server.Rest_API.Common
{
    public interface IMapQuest
    {
        Task GetRoute(Tour tour);
    }
}
