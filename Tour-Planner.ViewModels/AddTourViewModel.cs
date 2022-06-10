using System;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class AddTourViewModel : BaseViewModel, IDialogRequestClose
    {
        private const string PlaceHolder = "";
        private string _title;
        private string _origin;
        private string _destination;
        private string _description;

        public AddTourViewModel()
        {
            SaveCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true)));
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            _description = PlaceHolder;
            _title = PlaceHolder;
            _origin = PlaceHolder;
            _destination = PlaceHolder;
        }

        public string Title
        {
            get => _title;
            set
            {
                if (_title == value) return;
                _title = value;
                RaisePropertyChangedEvent();
            }
        }
        public string Origin
        {
            get => _origin;
            set
            {
                if (_origin == value) return;
                _origin = value;
                RaisePropertyChangedEvent();
            }
        }
        public string Destination
        {
            get => _destination;
            set
            {
                if (_destination == value) return;
                _destination = value;
                RaisePropertyChangedEvent();
            }
        }
        public string Description
        {
            get => _description;
            set
            {
                if (_description == value) return;
                _description = value;
                RaisePropertyChangedEvent();
            }
        }

        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}