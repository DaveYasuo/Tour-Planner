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

        private MainViewModel viewModel;

        public UpdateViewCommand(MainViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter?.ToString() == "Home" || parameter?.ToString() == null)
            {
                viewModel.SelectedViewModel = new HomeViewModel();
            }
            else if(parameter?.ToString() == "AddTour")
            {
                viewModel.SelectedViewModel = new TourViewModel();
            }
        }
    }
}
