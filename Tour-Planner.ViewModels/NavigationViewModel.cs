using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
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
        private readonly IMediator mediator;
        Tour? _selectedTour;
        ExportTour exporter = new ExportTour();
        ImportTour importer;
        TourReport tr = new TourReport();
        IRestService service;

        public NavigationViewModel(IMediator mediator, IRestService service)
        {
            this.mediator = mediator;
            this.service = service;
            importer = new ImportTour(service);
            mediator.Subscribe(SetSelectedTour, ViewModelMessage.SelectTour);
            DisplayAddTourCommand = new RelayCommand(_ => mediator.Publish(ViewModelMessage.AddTour, null));
            DisplayEditTourCommand = new RelayCommand(_ => mediator.Publish(ViewModelMessage.EditTour, null));
            DisplayEditTourLogCommand = new RelayCommand(_ => mediator.Publish(ViewModelMessage.EditTourLog, null));
            ExportTourCommand = new RelayCommand(_ => ExportTour());
            ImportTourCommand = new RelayCommand(async (_) => await ImportTour());
            CreateTourReportCommand = new RelayCommand(async _ => await CreateTourReport());
            ShowHelpCommand = new RelayCommand(_ => ShowHelp());
        }

        private void ShowHelp()
        {
            string url = "https://github.com/DaveYasuo/Tour-Planner";
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
                mediator.Publish(ViewModelMessage.EditTour, null);
            });
            ExportTourCommand = new RelayCommand(_ => ExportTour());
            ImportTourCommand = new RelayCommand(async (_) => await ImportTour());
            CreateTourReportCommand = new RelayCommand(_ => CreateTourReport());
            CreateSummaryReportCommand = new RelayCommand(_ => CreateSummaryReport());
        }

        private void SetSelectedTour(object? obj = null)
        {
            _selectedTour = (Tour)obj!;
        }
        private void ExportTour()
        {
            if (_selectedTour != null)
            {
                exporter.ExportSingleTour(_selectedTour);
                return;
            }
            MessageBox.Show("Please select a tour to export!");
        }

        private async Task ImportTour()
        {
            await importer.ImportSingleTour();
            mediator.Publish(ViewModelMessage.UpdateTourList, true);
        }

        private async Task CreateTourReport()
        {
            if (_selectedTour != null)
            {
                List<TourLog>? tourLogs = await service.GetAllTourLogsFromTour(_selectedTour);
                tr.CreateTourReport(_selectedTour, tourLogs);
            }
            else
            {
                MessageBox.Show("Select a tour to report");
            }
        }
        private async Task CreateSummaryReport()
        {
            List<TourLog> tourLogs = await service.GetAllTourLogs();
            List<Tour> tours = await service.GetTours();
            tr.CreateSummaryReport(tours, tourLogs);
        }


        public ICommand DisplayAddTourCommand { get; }
        public ICommand DisplayEditTourCommand { get; }
        public ICommand DisplayEditTourLogCommand { get; }
        public ICommand ExportTourCommand { get; }
        public ICommand ImportTourCommand { get; }
        public ICommand CreateTourReportCommand { get; }
        public ICommand ShowHelpCommand { get; }
        public ICommand CreateSummaryReportCommand { get; }
    }
}
