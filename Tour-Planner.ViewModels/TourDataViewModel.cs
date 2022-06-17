using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.ViewModels
{
    public class TourDataViewModel : BaseViewModel
    {
        List<Tour>? result;
        private IRestService service;

        public TourDataViewModel(IRestService service)
        {
            this.service = service;
            GetTours();
        }
        public async void GetTours()
        {
            result = await service.GetTour();
        }

    }
}

