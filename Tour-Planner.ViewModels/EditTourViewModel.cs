using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class EditTourViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        public string this[string columnName] => throw new NotImplementedException();

        public string Error => throw new NotImplementedException();

        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;

        private Tour selectedTour;
        public EditTourViewModel(IRestService service, Tour tour)
        {
            selectedTour = tour;
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));


        }

        public string Title
        {
            get => selectedTour.Title;
            set
            {
                if (selectedTour.Title == value) return;
                selectedTour.Title = value;
                RaisePropertyChangedEvent();
            }
        }

        public string Description
        {
            get => selectedTour.Description;
            set
            {
                if (selectedTour.Description == value) return;
                selectedTour.Description = value;
                RaisePropertyChangedEvent();
            }
        }

        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }

    }
}
