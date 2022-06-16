using System;
using System.Collections.Generic;
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
        
        public HomeViewModel(IDialogService dialogService)
        {
            List<Tour> result;
            DisplayMessageCommand = new RelayCommand(_ => DisplayMessage());
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
            GetTourTitlesCommand = new RelayCommand(async _ =>
            {
                result = await RestService.GetTour();
                foreach(var title in result)
                {

                }
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


        public ICommand DisplayMessageCommand { get; }
        public ICommand CreatePdfCommand { get; }
        public ICommand GetTourTitlesCommand { get; }
    }
}
