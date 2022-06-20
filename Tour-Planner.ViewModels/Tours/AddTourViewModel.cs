using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using log4net;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels.Tours
{
    public class AddTourViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private string _title;
        private string _origin;
        private string _destination;
        private string _description;
        private RouteType? _routeType;

        public string Error { get; set; } = "";

        private bool _titleHasBeenTouched;
        private bool _originHasBeenTouched;
        private bool _destinationHasBeenTouched;
        private bool _descriptionHasBeenTouched;
        private bool _selectedItemHasBeenTouched;
        public AddTourViewModel(IRestService service, IMediator mediator)
        {
            async void Execute(object _)
            {
                List<string> testableProperty = new()
                {
                    nameof(Title),
                    nameof(Origin),
                    nameof(Destination),
                    nameof(Description),
                    nameof(SelectedRouteType)
                };
                bool hasError = false;
                foreach (var item in testableProperty.Where(item => GetErrorForProperty(item, true) is not ""))
                {
                    hasError = true;
                }

                if (hasError)
                {
                    MessageBox.Show("Please fill out the form before submitting", "Error");
                    return;
                }

                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                Tour newTour = new(_title, _origin, _destination, _description, (RouteType)_routeType!); // todo
                var result = await service.AddTour(newTour);
                if (result == null)
                {
                    mediator.Publish(ViewModelMessage.UpdateTourList, false);
                    MessageBox.Show($"Cannot find a route for {newTour.Title} from {newTour.Origin} to {newTour.Destination}.", "Check your inputs");
                }
                else
                {
                    mediator.Publish(ViewModelMessage.UpdateTourList, true);
                }
            }

            SaveCommand = new RelayCommand(Execute);
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            _description = "";
            _title = "";
            _origin = "";
            _destination = "";
            _routeType = null;
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


        public string this[string propertyName] => GetErrorForProperty(propertyName, false);

        public RouteType? SelectedRouteType
        {
            get => _routeType;
            set
            {
                if (_routeType == value) return;
                _routeType = value;
                RaisePropertyChangedEvent();
            }
        }

        private string GetErrorForProperty(string propertyName, bool onSubmit)
        {

            Error = "";
            switch (propertyName)
            {
                case "Title":
                    if ((string.IsNullOrEmpty(Title) || Title.Trim().Length == 0) && (_titleHasBeenTouched || onSubmit))
                    {
                        Title = "";
                        Error = "Title cannot be empty!";
                        Log.Info(Error);
                        return Error;
                    }
                    _titleHasBeenTouched = true;
                    break;
                case "Origin":
                    if ((string.IsNullOrEmpty(Origin) || Origin.Trim().Length == 0) && (_originHasBeenTouched || onSubmit))
                    {
                        Origin = "";
                        Error = "Origin cannot be empty!";
                        Log.Info(Error);
                        return Error;
                    }
                    _originHasBeenTouched = true;
                    break;
                case "Destination":
                    if ((string.IsNullOrEmpty(Destination) || Destination.Trim().Length == 0) && (_destinationHasBeenTouched || onSubmit))
                    {
                        Destination = "";
                        Error = "Destination cannot be empty!";
                        Log.Info(Error);
                        return Error;
                    }
                    _destinationHasBeenTouched = true;
                    break;
                case "Description":
                    if (!string.IsNullOrEmpty(Description) && Description.Trim().Length == 0 && _descriptionHasBeenTouched)
                    {
                        Error = "Description cannot be only spaces!";
                        Log.Info(Error);
                        return Error;
                    }
                    _descriptionHasBeenTouched = true;
                    break;
                case "SelectedRouteType":
                    if (SelectedRouteType == null && (_selectedItemHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(SelectedRouteType));
                        }
                        Error = "Route Type cannot be empty!";
                        Log.Info(Error);
                    }
                    _selectedItemHasBeenTouched = true;
                    return Error;
            }
            return Error;
        }



        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}