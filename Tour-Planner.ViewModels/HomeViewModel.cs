using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        TourReport tr = new TourReport();
        private readonly IDialogService _dialogService;
        public List<Tour> result = new List<Tour> ();
        public List<string> titles = new List<string>();

        public HomeViewModel (IDialogService dialogService)
        {         
            result =  UpdateTours();
            foreach(Tour tour in result)
            {
                titles.Add (tour.Title);
            }

            DisplayMessageCommand = new RelayCommand(_ => DisplayMessage());
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
            GetTourTitlesCommand = new RelayCommand(async _ =>
            {
                result = await RestService.GetTour();
            });
            _dialogService = dialogService;
        }

        private void DisplayMessage()
        {
            var viewModel = new AddTourViewModel();
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                // accepted
            }
            else
            {
                // cancelled
            }
        }

        private void CreatePdf()
        {
            Tour tour = new Tour(1, "Dages Reise ins Zauberland", "Wien", "Linz", 40, "Ich bin geil weil ich so weit Fahrrad fahren kann!", new TimeSpan(2, 14, 18), "ImagePath"
);

            tr.CreatePdf(tour);
        }

        private List<Tour> UpdateTours()
        {
            return RestService.GetTour().Result;
        }


        public ICommand DisplayMessageCommand { get; }
        public ICommand CreatePdfCommand { get; }
        public ICommand GetTourTitlesCommand { get; }
    }
}
