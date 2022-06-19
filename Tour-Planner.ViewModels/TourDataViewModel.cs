using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.ViewModels
{
    public class TourDataViewModel : BaseViewModel
    {
        private readonly IMediator mediator;
        private IRestService service;
        private string _description;
        private string _title;
        private string _origin;
        private string _destination;
        private string? _routeImagePath;
        private RouteType _routeType;
        private double _distance;
        private TimeSpan _duration;

        public TourDataViewModel(IRestService service, IMediator mediator)
        {
            this.mediator = mediator;
            this.service = service;
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
            Tour tour = (Tour)o!;
            if (tour == null) return;
            Title = tour.Title;
            Origin = tour.Origin;
            Destination = tour.Destination;
            Description = tour.Description;
            RouteType = tour.RouteType;
            Distance = tour.Distance;
            Duration = tour.Duration;
            if (string.IsNullOrEmpty(tour.ImagePath))
            {
                RouteImagePath = null;
            }
            else
            {
                RouteImagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, ".\\..\\..\\..\\..\\RouteImages\\" + tour.ImagePath);
            }
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
                if (_distance == value) return;
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

