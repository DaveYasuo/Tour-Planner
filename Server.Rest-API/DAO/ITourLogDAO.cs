using System;
using System.Collections.Generic;
using Tour_Planner.Models;

namespace Server.Rest_API.DAO
{
    public interface ITourLogDAO
    {
        TourLog AddNewTourLog(TourLog newTourLog);
        IEnumerable<TourLog> GetAllTourLogs();
        void DeleteTourLog(int id);
    }
}
