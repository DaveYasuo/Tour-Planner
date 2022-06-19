using System;
using System.IO;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.ViewModels
{
    public class TourDataViewModel : BaseViewModel
    {
        private string _description;
        private string _title;
        private string _origin;
        private string _destination;
        private string? _routeImagePath;
        private RouteType _routeType;
        private double _distance;
        private TimeSpan _duration;

        public TourDataViewModel(IMediator mediator)
        {
            _duration = default;
            _distance = default;
            _title = "";
            _destination = "";
            _description = "";
            _origin = "";
            _routeImagePath = null;
            mediator.Subscribe(ShowTourData, ViewModelMessage.SelectTour);
        }

        private void ShowTourData(object? o)
        {
            if (o == null) return;
            Tour tour = (Tour)o;
            Title = tour.Title;
            Origin = tour.Origin;
            Destination = tour.Destination;
            Description = tour.Description;
            RouteType = tour.RouteType;
            Distance = tour.Distance;
            Duration = tour.Duration;
            RouteImagePath = string.IsNullOrEmpty(tour.ImagePath) ? null : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".\\..\\..\\..\\..\\RouteImages\\" + tour.ImagePath);
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
        public RouteType RouteType
        {
            get => _routeType;
            set
            {
                if (_routeType == value) return;
                _routeType = value;
                RaisePropertyChangedEvent();
            }
        }
        public double Distance
        {
            get => _distance;
            set
            {
                if (Math.Abs(_distance - value) < 0.001) return;
                _distance = value;
                RaisePropertyChangedEvent();
            }
        }
        public TimeSpan Duration
        {
            get => _duration;
            set
            {
                if (_duration == value) return;
                _duration = value;
                RaisePropertyChangedEvent();
            }
        }
        public string? RouteImagePath
        {
            get => _routeImagePath;
            set
            {
                if (_routeImagePath == value) return;
                _routeImagePath = value;
                RaisePropertyChangedEvent();
            }
        }
    }
}

