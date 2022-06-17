using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class ListToursViewModel : BaseViewModel
    {
        public ObservableCollection<Tour> ListTours { get; set; }

        private readonly IRestService service;
        private string _description;
        private Tour? _selectedTour;

        private readonly IDialogService _dialogService;
        List<Tour> AllTours = new();
        public ListToursViewModel(IDialogService dialogService, IRestService service)
        {
            this.service = service;
            _description = "hi";
            ListTours = new ObservableCollection<Tour>();
            _dialogService = dialogService;
            ShowTours = new RelayCommand(async (_) => await UpdateTours());
            DisplayAddTourCommand = new RelayCommand(_ => DisplayAddTour());
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
            DeleteTourCommand = new RelayCommand(_ => DeleteTour());
            _selectedTour = null;
        }


        public string Description
        {
            get => _description;
            set
            {
                _description = value;
                RaisePropertyChangedEvent();
            }
        }
        public Tour? SelectedTour
        {
            get => _selectedTour;
            set
            {
                if (_selectedTour == value) return;
                _selectedTour = value;
                RaisePropertyChangedEvent();
            }
        }

        private void DisplayAddTour()
        {
            var viewModel = new AddTourViewModel(service);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                _ = UpdateTours();
            }
            else
            {
                // cancelled
            }
        }
        private void CreatePdf()
        {
            TourReport tr = new TourReport();
            Tour tour = new Tour(1, "Dages Reise ins Zauberland", "Wien", "Linz", 40, "Ich bin geil weil ich so weit Fahrrad fahren kann!", new TimeSpan(2, 14, 18), "ImagePath", RouteType.pedestrian);
            tr.CreatePdf(tour);
        }
        private async Task UpdateTours()
        {
            List<Tour>? tours = await service.GetTour();
            if (tours is not null)
            {
                ListTours.Clear();
                AllTours = tours;
                foreach (var item in AllTours)
                {
                    ListTours.Add(item.Title);
                }
            }
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
        private String _selectedTour;
        public String SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != value && value != null)
                {
                    _selectedTour = value;
                    selectedTour = GetTourFromTitle(_selectedTour);
                    Description = selectedTour.Description;
                    RaisePropertyChangedEvent(nameof(SelectedTour));
                    RaisePropertyChangedEvent(nameof(Description));
                }
            }
        }
        private async Task DeleteTour()
        {
            if (selectedTour.Title != null && selectedTour.Description != null && selectedTour.Origin != null && selectedTour.Destination != null)
            {
                bool result = await RestService.DeleteTour(selectedTour.Id);
                if (result)
                {
                    await UpdateTours();
                    selectedTour = new Tour();
                }
            }
            else
            {
                MessageBox.Show("Please select a Tour to delete!");
            }

        }

        public ICommand DisplayMessageCommand { get; }
        public ICommand CreatePdfCommand { get; }
        public ICommand ShowTours { get; }
        public ICommand DeleteTourCommand { get; }

    }
}
