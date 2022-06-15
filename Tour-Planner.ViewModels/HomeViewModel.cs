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
        private readonly IDialogService _dialogService;
        public HomeViewModel(IDialogService dialogService)
        {
            DisplayMessageCommand = new RelayCommand(_ => DisplayMessage());
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
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
            Tour tour = new Tour(1,"Wien","Linz","Dages Reise ins Zauberland",40,"Ich bin geil weil ich so weit Fahrrad fahren kann!");
            TourReport tr = new TourReport();
            tr.CreatePdf(tour);
        }

        public ICommand DisplayMessageCommand { get; }
        public ICommand CreatePdfCommand { get; }
    }
}
