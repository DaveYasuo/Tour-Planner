using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Tour_Planner.ViewModels.Commands
{
    class UpdateViewCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        private readonly MainViewModel _viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            switch (parameter?.ToString())
            {
                case "Home":
                case null:
                    _viewModel.SelectedViewModel = new HomeViewModel();
                    break;
                case "AddTour":
                    _viewModel.SelectedViewModel = new TourViewModel();
                    break;
            }
        }
    }
}
