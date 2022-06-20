using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using log4net;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels.Tours
{
    public class EditTourViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public string Error { get; set; } = "";
        private readonly Tour _selectedTour;

        public EditTourViewModel(IRestService service, IMediator mediator, Tour tour)
        {
            _selectedTour = tour;
            string? title = tour.Title;
            string? description = tour.Description;
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

            async void ExecuteSave(object _)
            {
                List<string> testableProperty = new List<string>() { nameof(Title), nameof(Description) };
                bool hasError = false;
                foreach (var unused in testableProperty.Where(item => GetErrorForProperty(item) is not ""))
                {
                    hasError = true;
                }

                if (hasError)
                {
                    MessageBox.Show("Please fill out the form before submitting");
                    Log.Error("Please fill out the form before submitting");
                    return;
                }

                if (_selectedTour.Title == title && _selectedTour.Description == description)
                {
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                    return;
                }

                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                await service.UpdateTour(_selectedTour);
            }

            SaveCommand = new RelayCommand(ExecuteSave);

        }

        public string Title
        {
            get => _selectedTour.Title;
            set
            {
                if (_selectedTour.Title == value) return;
                _selectedTour.Title = value;
                RaisePropertyChangedEvent();
            }
        }

        public string Description
        {
            get => _selectedTour.Description;
            set
            {
                if (_selectedTour.Description == value) return;
                _selectedTour.Description = value;
                RaisePropertyChangedEvent();
            }
        }

        public string this[string propertyName] => GetErrorForProperty(propertyName);

        private string GetErrorForProperty(string propertyName)
        {

            Error = "";
            switch (propertyName)
            {
                case "Title":
                    if (string.IsNullOrEmpty(_selectedTour.Title) || _selectedTour.Title.Trim().Length == 0)
                    {
                        Title = "";
                        Error = "Title cannot be empty!";
                        Log.Info(Error);
                        return Error;
                    }
                    break;
                case "Description":
                    if (!string.IsNullOrEmpty(_selectedTour.Description) && _selectedTour.Description.Trim().Length == 0)
                    {
                        Error = "Description cannot be only spaces!";
                        Log.Info(Error);
                        return Error;
                    }
                    break;
            }
            return Error;
        }


        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
        public ICommand CancelCommand { get; }
        public ICommand SaveCommand { get; }

    }
}
