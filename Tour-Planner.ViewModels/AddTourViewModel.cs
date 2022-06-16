using System;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;
using System.Collections.Generic;

namespace Tour_Planner.ViewModels
{
    public class AddTourViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private const string PlaceHolder = "";
        private string _title;
        private string _origin;
        private string _destination;
        private string _description;
        private string _selectedItem;
        public string Error { get; set; } = "";
        //public string Error => throw new NotImplementedException();s

        bool titleHasBeenTouched = false;
        bool originHasBeenTouched = false;
        bool destinationHasBeenTouched = false;
        bool descriptionHasBeenTouched = false;
        public AddTourViewModel()
        {
            SaveCommand = new RelayCommand(async _ =>
                       {
                           List<string> testableProperty = new List<string>() { Title, Origin, Destination, Description };
                           foreach (var item in testableProperty)
                           {

                               if (GetErrorForProperty(nameof(item)) is not null)
                               {
                                   MessageBox.Show("Please fill out the form before submitting");
                                   return;
                               }
                           }
                           CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                           Tour newTour = new(_title, _origin, _destination, _description); // todo
                           var result = await RestService.AddTour(newTour);
                           Debug.WriteLine(result);

                       });
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            _description = PlaceHolder;
            _title = PlaceHolder;
            _origin = PlaceHolder;
            _destination = PlaceHolder;
            _selectedItem = PlaceHolder;
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

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName);
            }
        }

        public string SelectedItem
        {
            get => _selectedItem;
            set
            {
                if (_selectedItem == value) return;
                _selectedItem = value;
                RaisePropertyChangedEvent();
            }
        }

        private string GetErrorForProperty(string propertyName)
        {

            Error = "";
            switch (propertyName)
            {
                case "Title":
                    if (string.IsNullOrEmpty(_title) && titleHasBeenTouched)
                    {
                        Error = "Title can not be empty!";
                        return Error;
                    }
                    else if (titleHasBeenTouched == true && _title.Trim().Length == 0)
                    {
                        Error = "Title can not be empty!";
                        return Error;
                    }
                    titleHasBeenTouched = true;
                    break;
                case "Origin":
                    if (string.IsNullOrEmpty(_origin) && originHasBeenTouched)
                    {
                        Error = "Origin can not be empty!";
                        return Error;
                    }
                    else if (_origin.Trim().Length == 0 && originHasBeenTouched)
                    {
                        Error = "Origin can not be empty!";
                        return Error;
                    }
                    originHasBeenTouched = true;
                    break;
                case "Destination":
                    if (string.IsNullOrEmpty(_destination) && destinationHasBeenTouched)
                    {
                        Error = "Destination can not be empty!";
                        return Error;
                    }
                    else if (_destination.Trim().Length == 0 && destinationHasBeenTouched)
                    {
                        Error = "Destination can not be empty!";
                        return Error;
                    }
                    destinationHasBeenTouched = true;
                    break;
                case "Description":
                    if (_description.Trim().Length == 0 && descriptionHasBeenTouched)
                    {
                        if (string.IsNullOrEmpty(_description) && descriptionHasBeenTouched)
                        {
                            Error = "";
                        }
                        else
                        {
                            Error = "Description can not be only spaces!";
                            return Error;
                        }
                    }
                    descriptionHasBeenTouched = true;
                    break;
            }
            return string.Empty;
        }



        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}