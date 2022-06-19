using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class EditTourViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {

        public string Error { get; set; } = "";
        private readonly Tour selectedTour;
        private readonly string _title;
        private readonly string _description;
        public EditTourViewModel(IRestService service, IMediator mediator, Tour tour)
        {
            selectedTour = tour;
            _title = tour.Title;
            _description = tour.Description;
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            SaveCommand = new RelayCommand(async _ =>
            {
                List<string> testableProperty = new List<string>() { nameof(Title), nameof(Description) };
                bool hasError = false;
                foreach (var item in testableProperty)
                {

                    if (GetErrorForProperty(item, true) is not "")
                    {
                        hasError = true;
                    }
                }
                if (hasError)
                {
                    MessageBox.Show("Please fill out the form before submitting");
                    return;
                }
                if (selectedTour.Title == _title && selectedTour.Description == _description)
                {
                    CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                    return;
                }
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                await service.UpdateTour(selectedTour);
            });

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

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName, false);
            }
        }

        private string GetErrorForProperty(string propertyName, bool onSubmit)
        {

            Error = "";
            switch (propertyName)
            {
                case "Title":
                    if ((string.IsNullOrEmpty(selectedTour.Title) || selectedTour.Title.Trim().Length == 0))
                    {
                        Title = "";
                        Error = "Title cannot be empty!";
                        return Error;
                    }
                    break;
                case "Description":
                    if (!string.IsNullOrEmpty(selectedTour.Description) && selectedTour.Description.Trim().Length == 0)
                    {
                        Error = "Description cannot be only spaces!";
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
