using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private Tour? _tour;
        private TourLog? _selectedTourLog;
        private readonly IMediator _mediator;
        private readonly IRestService _service;
        private readonly IDialogService _dialogService;
        private List<TourLog> _allTourLogs = new();

        public TourLogsViewModel(IDialogService dialogService, IRestService service, IMediator mediator)
        {
            _mediator = mediator;
            _service = service;
            _dialogService = dialogService;
            ListToursLogs = new ObservableCollection<TourLog>();
            mediator.Subscribe(UpdateTourLogsFromNewTour, ViewModelMessage.SelectTour);
            mediator.Subscribe(UpdateTourLogsFromNewTour, ViewModelMessage.UpdateTourLogList);
            mediator.Subscribe(DisplayEditTourLog, ViewModelMessage.EditTourLog);
            _tour = null;
            DisplayAddTourLogCommand = new RelayCommand(_ => DisplayAddTourLog());
            DeleteTourLogCommand = new RelayCommand(ExecuteDeleteTourLog);
            DisplayEditTourLogCommand = new RelayCommand(_ => DisplayEditTourLog());
        }

        private async void ExecuteDeleteTourLog(object _)
        {
            await DeleteTourLog();
        }

        private void DisplayAddTourLog()
        {
            if (_tour == null)
            {
                MessageBox.Show("Select tour before adding a tourlog", "Error");
                return;
            }
            var viewModel = new AddTourLogViewModel(_service, _mediator, _tour);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                _ = UpdateTourLogs();
            }
        }
        private void DisplayEditTourLog(object? obj = null)
        {
            if (SelectedTourLog == null)
            {
                MessageBox.Show("Select tourlog before editing a tourlog", "Error");
                return;
            }
            var viewModel = new EditTourLogViewModel(_service, _mediator, SelectedTourLog);
            bool? result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                _ = UpdateTourLogs();
            }
        }

        private void UpdateTourLogsFromNewTour(object? obj = null)
        {
            if (obj != null)
            {
                _tour = (Tour)obj!;
            }
            _ = UpdateTourLogs();
        }
        private async Task UpdateTourLogs()
        {
            if (_tour is null) return;
            List<TourLog>? tourLogs = await _service.GetAllTourLogsFromTour(_tour);
            if (tourLogs is not null)
            {
                ListToursLogs.Clear();
                _allTourLogs = tourLogs;
                foreach (var item in _allTourLogs)
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

            bool result = await _service.DeleteTourLog(SelectedTourLog.Id);
            if (result)
            {
                await UpdateTourLogs();
                SelectedTourLog = null;
            }
        }

        public TourLog? SelectedTourLog
        {
            get => _selectedTourLog;
            set
            {
                if (_selectedTourLog == value) return;
                _selectedTourLog = value;
                _mediator.Publish(ViewModelMessage.SelectTourLog, SelectedTourLog);
                RaisePropertyChangedEvent();
            }
        }

        public ICommand DisplayAddTourLogCommand { get; }
        public ICommand DeleteTourLogCommand { get; }
        public ICommand DisplayEditTourLogCommand { get; }
    }
}
