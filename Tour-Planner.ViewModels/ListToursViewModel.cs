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
        TourReport tr = new TourReport();
        private readonly IDialogService _dialogService;
        List<Tour> result = new List<Tour>();
        Tour selectedTour  = new Tour();
        private string _searchBarContent;
        public ListToursViewModel(IDialogService dialogService)
        {
            ListTours = new ObservableCollection<string>();
            _dialogService = dialogService;
            ShowTours = new RelayCommand(async (_) => await UpdateTours());
            DisplayMessageCommand = new RelayCommand(_ => DisplayMessage());
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
            DeleteTourCommand = new RelayCommand(_ => DeleteTour());

        }

        private void DisplayMessage()
        {
            var viewModel = new AddTourViewModel();
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
            Tour tour = new Tour(1, "Dages Reise ins Zauberland", "Wien", "Linz", 40, "Ich bin geil weil ich so weit Fahrrad fahren kann!", new TimeSpan(2, 14, 18), "ImagePath", RouteType.pedestrian);
            tr.CreatePdf(tour);
        }

        private async Task UpdateTours()
        {
            List<Tour>? tours = await RestService.GetTour();
            if (tours is not null)
            {
                ListTours.Clear();
                result = tours;
                foreach (var item in result)
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
               // var currentViewModel = new ListToursViewModel();
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
            if(selectedTour.Title != null && selectedTour.Description != null && selectedTour.Origin != null && selectedTour.Destination != null)
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

        private void FilterByText()
        {
            ListTours.Clear();
            /*foreach(Tour tour in result)
            {
                if (tour.Title.Contains(_searchBarContent) || tour.Description.Contains(_searchBarContent) || tour.Origin.Contains(_searchBarContent) || tour.Destination.Contains(_searchBarContent) || tour.RouteType.ToString().Contains(_searchBarContent) || tour.Distance.ToString().Contains(_searchBarContent) || tour.Duration.ToString().Contains(_searchBarContent))
                {
                    ListTours.Add(tour.Title);
                }
            }*/
        }
        public string SearchBarContent
        {
            get => _searchBarContent;
            set
            {
                if (_searchBarContent == value) return;
                _searchBarContent = value;
                //FilterByText();
                RaisePropertyChangedEvent();
                // var currentViewModel = new ListToursViewModel();
            }
        }
        public ICommand DisplayMessageCommand { get; }
        public ICommand CreatePdfCommand { get; }
        public ICommand ShowTours { get; }
        public ICommand DeleteTourCommand { get; }

        public ObservableCollection<string> ListTours { get; set; }
    }
}
