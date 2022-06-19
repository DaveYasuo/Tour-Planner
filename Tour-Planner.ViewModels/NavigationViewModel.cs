using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class NavigationViewModel
    {
        private readonly IMediator _mediator;
        private Tour? _selectedTour;
        private readonly ExportTour _exporter = new();
        private readonly ImportTour _importer;
        private readonly TourReport _tr = new();
        private readonly IRestService _service;

        public NavigationViewModel(IMediator mediator, IRestService service)
        {
            _mediator = mediator;
            _service = service;
            _importer = new ImportTour(service);
            mediator.Subscribe(SetSelectedTour, ViewModelMessage.SelectTour);
            DisplayAddTourCommand = new RelayCommand(_ => mediator.Publish(ViewModelMessage.AddTour, null));
            DisplayEditTourCommand = new RelayCommand(_ => mediator.Publish(ViewModelMessage.EditTour, null));
            DisplayEditTourLogCommand = new RelayCommand(_ => mediator.Publish(ViewModelMessage.EditTourLog, null));
            ExportTourCommand = new RelayCommand(_ => ExportTour());
            ImportTourCommand = new RelayCommand(ExecuteImportTour);
            CreateTourReportCommand = new RelayCommand(ExecuteCreateTourReport);
            ShowHelpCommand = new RelayCommand(_ => ShowHelp());
            CreateSummaryReportCommand = new RelayCommand(ExecuteCreateSummaryReport);
            ExitCommand = new RelayCommand(CloseApplication);
        }

        private async void ExecuteCreateSummaryReport(object _)
        {
            await CreateSummaryReport();
        }

        private async void ExecuteCreateTourReport(object _)
        {
            await CreateTourReport();
        }

        private async void ExecuteImportTour(object _)
        {
            await ImportTour();
        }

        private void CloseApplication(object obj)
        {
            Application.Current.Shutdown();
        }

        private void ShowHelp()
        {
            const string url = "https://github.com/DaveYasuo/Tour-Planner";
            try
            {
                ProcessStartInfo psi = new()
                {
                    FileName = url,
                    UseShellExecute = true
                };
                Process.Start(psi);
            }
            catch (Exception)
            {
                MessageBox.Show("Could not open browser for the help page.\n" + $"Please visit '{url}' manually.", "Tour-Planner - Help");
            }
        }
        private void SetSelectedTour(object? obj = null)
        {
            _selectedTour = (Tour)obj!;
        }
        private void ExportTour()
        {
            if (_selectedTour != null)
            {
                _exporter.ExportSingleTour(_selectedTour);
                return;
            }
            MessageBox.Show("Please select a tour to export!");
        }

        private async Task ImportTour()
        {
            await _importer.ImportSingleTour();
            _mediator.Publish(ViewModelMessage.UpdateTourList, true);
        }

        private async Task CreateTourReport()
        {
            if (_selectedTour != null)
            {
                List<TourLog>? tourLogs = await _service.GetAllTourLogsFromTour(_selectedTour);
                _tr.CreateTourReport(_selectedTour, tourLogs!);
            }
            else
            {
                MessageBox.Show("Select a tour to report");
            }
        }
        private async Task CreateSummaryReport()
        {
            List<TourLog>? tourLogs = await _service.GetAllTourLogs();
            List<Tour>? tours = await _service.GetTours();
            _tr.CreateSummaryReport(tours, tourLogs);
        }


        public ICommand DisplayAddTourCommand { get; }
        public ICommand DisplayEditTourCommand { get; }
        public ICommand DisplayEditTourLogCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ICommand ImportTourCommand { get; }
        public ICommand CreateTourReportCommand { get; }
        public ICommand ShowHelpCommand { get; }
        public ICommand CreateSummaryReportCommand { get; }
        public ICommand ExitCommand { get; }
    }
}
