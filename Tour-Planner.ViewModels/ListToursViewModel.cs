using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class ListToursViewModel : BaseViewModel
    {
        public ObservableCollection<Tour> ListTours { get; set; }

        private readonly IMediator mediator;
        private readonly IRestService service;
        private readonly IDialogService _dialogService;
        private ImageSource? _loadingImage;
        private readonly Tuple<ImageSource, ImageSource> loadedImage;
        private string _searchBarContent;
        private Tour? _selectedTour;

        List<Tour> AllTours = new();

        public ListToursViewModel(IDialogService dialogService, IRestService service, IMediator mediator)
        {
            this.mediator = mediator;
            this.service = service;
            _loadingImage = null;
            loadedImage = new(GetBitmapImage("\\refresh.gif"), GetBitmapImage("\\refresh.png"));
            ListTours = new ObservableCollection<Tour>();
            _dialogService = dialogService;
            ShowTours = new RelayCommand(async (_) => await UpdateTours());
            DisplayAddTourCommand = new RelayCommand(_ => DisplayAddTour());
            DisplayAddTourLogCommand = new RelayCommand(_ =>
            {
                if (SelectedTour is null) return;
                DisplayAddTourLog();
            });
            DisplayAddTourLogCommand = new RelayCommand(_ =>
            {
                if (SelectedTour is null) return;
                DisplayAddTourLog();
            });
            CreatePdfCommand = new RelayCommand(_ => CreatePdf());
            DeleteTourCommand = new RelayCommand(async _ => await DeleteTour());
            _selectedTour = null;
            _searchBarContent = "";
            mediator.Subscribe(DisplayAddTour, ViewModelMessage.AddTour);
            mediator.Subscribe(RefreshTour, ViewModelMessage.UpdateTourList);
        }


        public Tour? SelectedTour
        {
            get => _selectedTour;
            set
            {
                if (_selectedTour == value) return;
                _selectedTour = value;
                mediator.Publish(ViewModelMessage.SelectTour, SelectedTour);
                RaisePropertyChangedEvent();
            }
        }
        public ImageSource? LoadingImage
        {
            get { return _loadingImage; }
            set
            {
                if (value == _loadingImage) return;
                _loadingImage = value;
                RaisePropertyChangedEvent();
            }
        }
        public string SearchBarContent
        {
            get => _searchBarContent;
            set
            {
                if (_searchBarContent == value) return;
                _searchBarContent = value;
                FilterByText();
                RaisePropertyChangedEvent();
            }
        }

        private void RefreshTour(object? obj)
        {
            bool isSuccess = (bool)obj!;
            if (isSuccess)
            {
                _ = UpdateTours();
            }
            else
            {
                LoadingImage = loadedImage.Item2;
            }
        }
        private void DisplayAddTour(object? obj = null)
        {
            var viewModel = new AddTourViewModel(service, mediator);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                LoadingImage = loadedImage.Item2;
                LoadingImage = loadedImage.Item1;
            }
            else
            {
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
            LoadingImage = loadedImage.Item2;
            LoadingImage = loadedImage.Item1;
            List<Tour>? tours = await service.GetTour();
            if (tours is not null)
            {
                ListTours.Clear();
                AllTours = tours;
                foreach (var item in AllTours)
                {
                    ListTours.Add(item);
                }
            }
            await Task.Delay(1000);
            LoadingImage = loadedImage.Item2;
        }
        private void UpdateTourLogs()
        {

        }
        private async Task DeleteTour()
        {
            if (SelectedTour is null)
            {
                MessageBox.Show("Please select a Tour to delete!");
            }
            else
            {
                bool result = await service.DeleteTour(SelectedTour.Id);
                if (result)
                {
                    await UpdateTours();
                    SelectedTour = null;
                }
            }

        }
        private void FilterByText()
        {
            ListTours.Clear();
            foreach (Tour tour in AllTours)
            {
                if (tour.Title.ToLower().Contains(_searchBarContent) ||
                    tour.Description.ToLower().Contains(_searchBarContent) ||
                    tour.Origin.ToLower().Contains(_searchBarContent) ||
                    tour.Destination.ToLower().Contains(_searchBarContent) ||
                    tour.RouteType.ToString().Contains(_searchBarContent) ||
                    tour.Distance.ToString().Contains(_searchBarContent) ||
                    tour.Duration.ToString().Contains(_searchBarContent))
                {
                    ListTours.Add(tour);
                }
            }
        }
        public BitmapImage GetBitmapImage(string location)
        {
            try
            {
                var image = new BitmapImage(new Uri(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".\\..\\..\\..\\Images" + location), UriKind.RelativeOrAbsolute));
                return image;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            return new BitmapImage();
        }
        private void DisplayAddTourLog()
        {
            var viewModel = new AddTourLogViewModel(service, mediator, SelectedTour!);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                //_ = UpdateTourLogs();
            }
            else
            {
                // cancelled
            }
        }
        private void EditTour()
        {
            var viewModel = new EditTourViewModel(service, SelectedTour!);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                //_ = UpdateTourLogs();
            }
            else
            {
                // cancelled
            }
        }

        public ICommand DisplayAddTourCommand { get; }
        public ICommand CreatePdfCommand { get; }
        public ICommand ShowTours { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand DisplayAddTourLogCommand { get; }
        public ICommand EditTourCommand { get; }


    }
}
