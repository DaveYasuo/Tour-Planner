using System.Collections.Generic;
using Tour_Planner.Models;

namespace Server.Rest_API.DAO
{
    public interface ITourDAO
    {
        Tour FindById(int tourId);
        Tour AddNewTour(Tour newTour);
        void UpdateTour(Tour newTour);
        IEnumerable<Tour> GetTours();
        void DeleteTour(int tourId);
    }
}