using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class TourLogsViewModel : BaseViewModel
    {
        public ObservableCollection<TourLog> ListToursLogs { get; set; }

        private Tour? tour;
        private TourLog? _selectedTourLog;
        private readonly IMediator mediator;
        private readonly IRestService service;
        private readonly IDialogService _dialogService;
        List<TourLog> AllTourLogs = new();

        public TourLogsViewModel(IDialogService dialogService, IRestService service, IMediator mediator)
        {
            this.mediator = mediator;
            this.service = service;
            _dialogService = dialogService;
            ListToursLogs = new ObservableCollection<TourLog>();
            mediator.Subscribe(SetSelectedTour, ViewModelMessage.SelectTour);
            tour = null;
            DisplayAddTourLogCommand = new RelayCommand(_ =>
            {
                if (tour is null)
                {
                    MessageBox.Show("Select tour before adding a log", "Error");
                    return;
                }
                DisplayAddTourLog();
            });
        }
        private void SetSelectedTour(object? obj = null)
        {
            tour = (Tour)obj!;
            _ = UpdateTourLogs();
        }
        private void DisplayAddTourLog()
        {
            var viewModel = new AddTourLogViewModel(service, mediator, tour!);
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
        private async Task UpdateTourLogs()
        {
            if (tour is null) return;
            List<TourLog>? tourLogs = await service.GetAllTourLogsFromTour(tour);
            if (tourLogs is not null)
            {
                ListToursLogs.Clear();
                AllTourLogs = tourLogs;
                foreach (var item in AllTourLogs)
                {
                    ListToursLogs.Add(item);
                }
            }
        }
        public TourLog? SelectedTourLog
        {
            get => _selectedTourLog;
            set
            {
                if (_selectedTourLog == value) return;
                _selectedTourLog = value;
                mediator.Publish(ViewModelMessage.SelectTourLog, SelectedTourLog);
                RaisePropertyChangedEvent();
            }
        }

        public ICommand DisplayAddTourLogCommand { get; }
    }
}
