using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class NavigationViewModel
    {
        private readonly IMediator mediator;
        Tour _selectedTour ;
        ExportTour exporter = new ExportTour();

        public NavigationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            DisplayAddTourCommand = new RelayCommand(_ =>
            {
                mediator.Publish(ViewModelMessage.AddTour, null);
            });
            DisplayEditTourCommand = new RelayCommand(_ =>
            {
                mediator.Publish(ViewModelMessage.EditTour, null);
            });
            ExportTourCommand = new RelayCommand(_ => ExportTour());
        }

        private void SetSelectedTour(object? obj = null)
        {
            _selectedTour = (Tour)obj!;
        }
        private void ExportTour()
        {
            mediator.Subscribe(SetSelectedTour, ViewModelMessage.SelectTour);
            if(_selectedTour != null)
            {
                exporter.ExportSingleTour(_selectedTour);
                return;
            }
            MessageBox.Show("Please select a tour to export!");
        }


        public ICommand DisplayAddTourCommand { get; }
        public ICommand DisplayEditTourCommand { get; }
        public ICommand ExportTourCommand { get; }
    }
}
