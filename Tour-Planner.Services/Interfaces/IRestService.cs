using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Planner.Models;

namespace Tour_Planner.Services.Interfaces
{
    public interface IRestService
    {
        Task<Tour?> AddTour(Tour tour);
        Task<bool> AddTourLog(TourLog tourLog);
        Task<List<Tour>?> GetTour();
        Task<bool> DeleteTour(int id);
    }
}