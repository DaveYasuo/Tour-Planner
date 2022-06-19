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
            mediator.Subscribe(UpdateTourLogs, ViewModelMessage.SelectTour);
            mediator.Subscribe(UpdateTourLogs, ViewModelMessage.UpdateTourLogList);
            tour = null;
            DisplayAddTourLogCommand = new RelayCommand(_ => DisplayAddTourLog());
            DeleteTourLogCommand = new RelayCommand(async _ => await DeleteTourLog());
            DisplayEditTourLogCommand = new RelayCommand(_ => DisplayEditTourLog());
        }

        private void DisplayAddTourLog()
        {
            if (tour == null)
            {
                MessageBox.Show("Select tour before adding a tourlog", "Error");
                return;
            }
            var viewModel = new AddTourLogViewModel(service, mediator, tour);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                _ = UpdateTourLogs();
            }
            else
            {
                // cancelled
            }
        }
        private void DisplayEditTourLog()
        {
            if (SelectedTourLog == null)
            {
                MessageBox.Show("Select tourlog before editing a tourlog", "Error");
                return;
            }
            var viewModel = new EditTourLogViewModel(service, mediator, SelectedTourLog);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                _ = UpdateTourLogs();
            }
            else
            {
                // cancelled
            }
        }

        private void UpdateTourLogs(object? obj = null)
        {
            if (obj != null)
            {
                tour = (Tour)obj!;
            }
            _ = UpdateTourLogs();
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

        private async Task DeleteTourLog()
        {
            if (SelectedTourLog == null)
            {
                MessageBox.Show("Please select a tourlog to delete!", "Error");
                return;
            }
            else
            {
                bool result = await service.DeleteTourLog(SelectedTourLog.Id);
                if (result)
                {
                    await UpdateTourLogs();
                    SelectedTourLog = null;
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
        public ICommand DeleteTourLogCommand { get; }
        public ICommand DisplayEditTourLogCommand { get; }
    }
}
