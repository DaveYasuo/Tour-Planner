using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class ListToursViewModel : BaseViewModel
    {
        public ObservableCollection<Tour> ListTours { get; set; }

        private readonly IMediator _mediator;
        private readonly IRestService _service;
        private readonly IDialogService _dialogService;
        private ImageSource? _loadingImage;
        private readonly Tuple<ImageSource, ImageSource> _loadedImage;
        private string _searchBarContent;
        private Tour? _selectedTour;

        private List<Tour> _allTours = new();


        public ListToursViewModel(IDialogService dialogService, IRestService service, IMediator mediator)
        {
            _mediator = mediator;
            _service = service;
            _loadingImage = null;
            _loadedImage = new Tuple<ImageSource, ImageSource>(GetBitmapImage("\\refresh.gif"), GetBitmapImage("\\refresh.png"));
            ListTours = new ObservableCollection<Tour>();
            _dialogService = dialogService;

            async void ExecuteRefresh(object _)
            {
                await UpdateTours();
                mediator.Publish(ViewModelMessage.UpdateTourLogList, null);
            }

            RefreshCommand = new RelayCommand(ExecuteRefresh);
            DisplayAddTourCommand = new RelayCommand(_ => DisplayAddTour());
            DisplayEditTourCommand = new RelayCommand(_ => DisplayEditTour());
            DeleteTourCommand = new RelayCommand(Execute);
            _selectedTour = null;
            _searchBarContent = "";
            mediator.Subscribe(DisplayAddTour, ViewModelMessage.AddTour);
            mediator.Subscribe(DisplayEditTour, ViewModelMessage.EditTour);
            mediator.Subscribe(RefreshTour, ViewModelMessage.UpdateTourList);
        }

        private async void Execute(object _)
        {
            await DeleteTour();
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
                LoadingImage = _loadedImage.Item2;
            }
        }
        private void DisplayAddTour(object? obj = null)
        {
            var viewModel = new AddTourViewModel(_service, _mediator);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (!result.Value) return;
            LoadingImage = _loadedImage.Item2;
            LoadingImage = _loadedImage.Item1;
        }
        private void DisplayEditTour(object? obj = null)
        {
            if (SelectedTour is null)
            {
                MessageBox.Show("Select tour before editing a tour", "Error");
                return;
            }
            var viewModel = new EditTourViewModel(_service, _mediator, SelectedTour!);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                _ = UpdateTours();
            }
        }
        private async Task UpdateTours()
        {
            LoadingImage = _loadedImage.Item2;
            LoadingImage = _loadedImage.Item1;
            List<Tour>? tours = await _service.GetTours();
            if (tours is not null)
            {
                ListTours.Clear();
                _allTours = tours;
                foreach (var item in _allTours)
                {
                    ListTours.Add(item);
                }
            }
            await Task.Delay(1000);
            LoadingImage = _loadedImage.Item2;
        }

        private async Task DeleteTour()
        {
            if (SelectedTour is null)
            {
                MessageBox.Show("Please select a tour to delete!", "Error");
            }
            else
            {
                bool result = await _service.DeleteTour(SelectedTour.Id);
                if (result)
                {
                    await UpdateTours();
                    //File.Delete(".\\..\\..\\..\\..\\RouteImages/" + SelectedTour.ImagePath);
                    SelectedTour = null;
                }
            }
        }
        private async Task FilterByText()
        {
            ListTours.Clear();
            List<TourLog>? tourLogs = await _service.GetAllTourLogs();
            string smallSearchBarContent = SearchBarContent.ToLower();
            bool hasString = false;
            foreach (Tour tour in _allTours)
            {
                if (tourLogs != null)
                {
                    List<TourLog> tourLogsToTour = FindTourLogsToTour(tour, tourLogs);
                    hasString = SearchAllLogs(tourLogsToTour);
                }
                if (tour.Title.ToLower().Contains(smallSearchBarContent) ||
                    tour.Description.ToLower().Contains(smallSearchBarContent) ||
                    tour.Origin.ToLower().Contains(smallSearchBarContent) ||
                    tour.Destination.ToLower().Contains(smallSearchBarContent) ||
                    tour.RouteType.ToString().Contains(smallSearchBarContent) ||
                    tour.Distance.ToString(CultureInfo.InvariantCulture).Contains(smallSearchBarContent) ||
                    tour.Duration.ToString().Contains(smallSearchBarContent) ||
                    hasString)
                {
                    ListTours.Add(tour);
                }
            }
        }

        private List<TourLog> FindTourLogsToTour(Tour tour, List<TourLog> tourLogs)
        {
            return tourLogs.Where(tourLog => tourLog.TourId == tour.Id).ToList();
        }
        private bool SearchAllLogs(List<TourLog> tourLogs)
        {
            string smallSearchBarContent = SearchBarContent.ToLower();
            foreach (TourLog tourLog in tourLogs)
            {
                if (tourLog.TotalTime.ToString().ToLower().Contains(smallSearchBarContent) ||
                    tourLog.Rating.ToString().ToLower().Contains(smallSearchBarContent) ||
                    tourLog.Difficulty.ToString().ToLower().Contains(smallSearchBarContent) ||
                    tourLog.DateTime.ToString(CultureInfo.InvariantCulture).ToLower().Contains(smallSearchBarContent) ||
                    tourLog.Comment.ToLower().Contains(smallSearchBarContent))
                {
                    return true;
                }
            }
            return false;
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

        public Tour? SelectedTour
        {
            get => _selectedTour;
            set
            {
                if (_selectedTour == value) return;
                _selectedTour = value;
                _mediator.Publish(ViewModelMessage.SelectTour, SelectedTour);
                RaisePropertyChangedEvent();
            }
        }
        public ImageSource? LoadingImage
        {
            get => _loadingImage;
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
                _ = FilterByText();
                RaisePropertyChangedEvent();
            }
        }
        public ICommand DisplayAddTourCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand DeleteTourCommand { get; }
        public ICommand DisplayEditTourCommand { get; }
    }
}
