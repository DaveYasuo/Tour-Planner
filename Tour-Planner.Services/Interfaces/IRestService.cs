using System.Threading.Tasks;
using Tour_Planner.Models;

namespace Tour_Planner.Services.Interfaces
{
    public interface IRestService
    {
        Task<bool> AddTour(Tour tour);
    }
}