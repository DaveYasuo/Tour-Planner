using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.Models;
using Tour_Planner.Services;

namespace Tour_Planner.ViewModels
{
    public class TourDataViewModel :BaseViewModel
    {
        List<Tour> result;
        public TourDataViewModel()
        {
            GetTours();
        }
        public async void GetTours()
        {
            var result =  await RestService.GetTour();
        }

    }
}

