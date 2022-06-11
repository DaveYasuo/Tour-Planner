using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IDialogService _dialogService;
        public HomeViewModel(IDialogService dialogService)
        {
            DisplayMessageCommand = new RelayCommand(_ => DisplayMessage());
            _dialogService = dialogService;
        }

        private void DisplayMessage()
        {
            var viewModel = new AddTourViewModel();
            var result = _dialogService.ShowDialog(viewModel);
            if (!result.HasValue) return;
            if (result.Value)
            {
                // accepted
            }
            else
            {
                // cancelled
            }
        }

        public ICommand DisplayMessageCommand { get; }
    }
}
