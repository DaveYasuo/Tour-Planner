using System;
using System.Collections;
using System.Collections.Generic;
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
        private IList<string> _list = new List<string>();
        private readonly IDialogService _dialogService;
        List<Tour> result;
        Tour selectedTour  = new Tour();
        public ListToursViewModel(IDialogService dialogService)
        {
            _description = "hi";
            _dialogService = dialogService;
            ShowTours = new RelayCommand(async _ => {
                await PerformShowToursAsync();
                RaisePropertyChangedEvent(nameof(ListTours));
            });
            DisplayMessageCommand = new RelayCommand(_ => DisplayMessage());
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
            PerformShowToursAsync();

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

        private async Task<List<string>> UpdateTours()
        {
            result = await RestService.GetTour();
            List<string> titles = new List<string>();
            foreach (var item in result)
            {
                titles.Add(item.Title);
            }

            return titles;
        }
        private String _selectedTour;
        public String SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if(_selectedTour != value && value != null)
                {
                    _selectedTour = value;
                    selectedTour = GetTourFromTitle(_selectedTour);
                    Description = selectedTour.Description;
                    RaisePropertyChangedEvent(nameof(SelectedTour));
                    RaisePropertyChangedEvent(nameof(FullTour));
                }
            }
        }


        public Tour FullTour
        {
            get { return selectedTour; }

        }

        private String _description;
        public String Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChangedEvent(); 
            }
        }

        public Tour SingleTour
        {
            get
            {
                Description = selectedTour.Description;
                return selectedTour;
            }
        }
        
        private Tour GetTourFromTitle(string title)
        {
            foreach(Tour tour in result)
            {
                if(tour.Title == title)
                {
                    return tour;
                }
            }
            return null!;
        }
        public ICommand DisplayMessageCommand { get; }
        public ICommand CreatePdfCommand { get; }
        public ICommand ShowTours { get; }

        private async Task PerformShowToursAsync()
        {
            _list = await UpdateTours();
            RaisePropertyChangedEvent(nameof(ListTours));
        }

        public IList<string> ListTours
        {
            get => _list;
        }
    }
}
