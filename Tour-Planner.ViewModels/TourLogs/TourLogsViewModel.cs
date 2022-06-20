using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using log4net;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels.TourLogs
{
    public class TourLogsViewModel : BaseViewModel
    {
        public ObservableCollection<TourLog> ListToursLogs { get; set; }
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);


        public Tour? Tour;
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
            Tour = null;
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
            if (Tour == null)
            {
                _dialogService.ShowMessageBox("Select tour before adding a tour log", "Error");
                Log.Error("Select tour before adding a tour log");

                return;
            }
            var viewModel = new AddTourLogViewModel(_dialogService, _service, _mediator, Tour);
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
                _dialogService.ShowMessageBox("Select tour log before editing a tour log", "Error");
                Log.Error("Select tour log before editing a tour log");
                return;
            }
            var viewModel = new EditTourLogViewModel(_dialogService, _service, _mediator, SelectedTourLog);
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
                Tour = (Tour)obj!;
            }
            _ = UpdateTourLogs();
        }
        private async Task UpdateTourLogs()
        {
            if (Tour is null) return;
            List<TourLog>? tourLogs = await _service.GetAllTourLogsFromTour(Tour);
            if (tourLogs is not null)
            {
                ListToursLogs.Clear();
                _allTourLogs = tourLogs;
                foreach (var item in _allTourLogs)
                {
                    ListToursLogs.Add(item);
                }
                _mediator.Publish(ViewModelMessage.UpdateComputedTourAttributes, _allTourLogs);
            }
            Log.Debug("Tour Logs updated");
        }

        private async Task DeleteTourLog()
        {
            if (SelectedTourLog == null)
            {
                _dialogService.ShowMessageBox("Please select a tour log to delete!", "Error");
                Log.Error("Please select a tour log to delete!");
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
