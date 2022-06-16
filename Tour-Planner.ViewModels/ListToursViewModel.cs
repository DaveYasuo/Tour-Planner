using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class ListToursViewModel : BaseViewModel
    {
        TourReport tr = new TourReport();
        private readonly IDialogService _dialogService;
        private ICommand? _listTours;
        List<Tour> result = new List<Tour>();


        // private constructor called by the async method
        public ListToursViewModel(IDialogService dialogService)
        {
            ListTours = new ObservableCollection<string>();
            _dialogService = dialogService;

            DisplayMessageCommand = new RelayCommand(_ => DisplayMessage());
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
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
            Tour tour = new Tour(1, "Dages Reise ins Zauberland", "Wien", "Linz", 40, "Ich bin geil weil ich so weit Fahrrad fahren kann!", new TimeSpan(2, 14, 18), "ImagePath");
            tr.CreatePdf(tour);
        }

        private async Task UpdateTours()
        {
            List<Tour>? tours = await RestService.GetTour();
            if (tours is not null)
            {
                result = tours;
                foreach (var item in result)
                {
                    ListTours.Add(item.Title);
                }
            }
        }


        public ICommand DisplayMessageCommand { get; }
        public ICommand CreatePdfCommand { get; }
        public ICommand ShowTours
        {
            get
            {
                if (_listTours != null) return _listTours;
                _listTours = new RelayCommand(async _ => await UpdateTours());
                return _listTours;
            }
        }

        public ObservableCollection<string> ListTours { get; set; }
    }
}
