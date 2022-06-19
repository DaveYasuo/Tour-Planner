using System.Collections.Generic;
using System.Threading.Tasks;
using Tour_Planner.Models;

namespace Tour_Planner.Services.Interfaces
{
    public interface IRestService
    {
        Task<Tour?> AddTour(Tour tour);
        Task<bool> AddTourLog(TourLog tourLog);
        Task<List<Tour>?> GetTours();
        Task<bool> DeleteTour(int id);
        Task<bool> DeleteTourLog(int id);
        Task<bool> UpdateTour(Tour tour);
        Task<List<TourLog>?> GetAllTourLogs();
        Task<List<TourLog>?> GetAllTourLogsFromTour(Tour tour);
        Task<bool> UpdateTourLog(TourLog newTour);
    }
}