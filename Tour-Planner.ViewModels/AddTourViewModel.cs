using System;
using System.ComponentModel;
using System.Windows;
using System.Diagnostics;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;
using System.Collections.Generic;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;

namespace Tour_Planner.ViewModels
{
    public class AddTourViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private const string PlaceHolder = null;
        private string _title;
        private string _origin;
        private string _destination;
        private string _description;
        private RouteType? _routeType;

        public string Error { get; set; } = "";

        bool titleHasBeenTouched = false;
        bool originHasBeenTouched = false;
        bool destinationHasBeenTouched = false;
        bool descriptionHasBeenTouched = false;
        bool selectedItemHasBeenTouched = false;
        public AddTourViewModel(IRestService service, IMediator mediator)
        {
            SaveCommand = new RelayCommand(async _ =>
                       {
                           List<string> testableProperty = new() { nameof(Title), nameof(Origin), nameof(Destination), nameof(Description), nameof(SelectedRouteType) };
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
                       });
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            _description = PlaceHolder;
            _title = PlaceHolder;
            _origin = PlaceHolder;
            _destination = PlaceHolder;
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


        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName, false);
            }
        }

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
                    if ((string.IsNullOrEmpty(Title) || Title.Trim().Length == 0) && (titleHasBeenTouched || onSubmit))
                    {
                        Title = "";
                        Error = "Title cannot be empty!";
                        return Error;
                    }
                    titleHasBeenTouched = true;
                    break;
                case "Origin":
                    if ((string.IsNullOrEmpty(Origin) || Origin.Trim().Length == 0) && (originHasBeenTouched || onSubmit))
                    {
                        Origin = "";
                        Error = "Origin cannot be empty!";
                        return Error;
                    }
                    originHasBeenTouched = true;
                    break;
                case "Destination":
                    if ((string.IsNullOrEmpty(Destination) || Destination.Trim().Length == 0) && (destinationHasBeenTouched || onSubmit))
                    {
                        Destination = "";
                        Error = "Destination cannot be empty!";
                        return Error;
                    }
                    destinationHasBeenTouched = true;
                    break;
                case "Description":
                    if (!string.IsNullOrEmpty(Description) && Description.Trim().Length == 0 && descriptionHasBeenTouched)
                    {
                        Error = "Description cannot be only spaces!";
                        return Error;
                    }
                    descriptionHasBeenTouched = true;
                    break;
                case "SelectedRouteType":
                    if (SelectedRouteType == null && (selectedItemHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(SelectedRouteType));
                        }
                        Error = "Route Type cannot be empty!";
                    }
                    selectedItemHasBeenTouched = true;
                    return Error;
            }
            return Error;
        }



        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;


        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }
    }
}