using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using log4net;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.ViewModels.Tours
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
        private int _childFriendliness;
        private int _popularity;
        private TimeSpan _duration;
        private readonly IRestService _service;
        private Tour? _tour;
        private readonly string _folderLocation;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public TourDataViewModel(IRestService service, IMediator mediator, string? tmp)
        {
            if (tmp == null)
            {
                Log.Fatal("Key: RouteImagePath not found for displaying Route Images.");
                throw new KeyNotFoundException("Key: RouteImagePath not found for displaying Route Images.");
            }
            _folderLocation = tmp;
            _service = service;
            _tour = null;
            _duration = default;
            _distance = default;
            _title = "";
            _destination = "";
            _description = "";
            _origin = "";
            _routeImagePath = null;
            _childFriendliness = 0;
            _popularity = 0;
            mediator.Subscribe(ShowTourData, ViewModelMessage.SelectTour);
            mediator.Subscribe(ShowComputedAttributes, ViewModelMessage.UpdateComputedTourAttributes);
        }

        private void ShowComputedAttributes(object? obj = null)
        {
            try
            {
                if (obj != null)
                {
                    List<TourLog> logsList = (List<TourLog>)obj;
                    if (logsList.Count == 0) return;
                    _ = CalculateTourAttributes(logsList);
                }
                else
                {
                    _ = CalculateTourAttributes();
                }
            }
            catch (Exception)
            {
                Log.Error("Cannot get List of tour logs. Check if tour logs are present.");
            }
        }

        private async Task CalculateTourAttributes(List<TourLog>? logsFromTour = null)
        {
            if (_tour == null) return;
            List<TourLog>? allTourLogs = await _service.GetAllTourLogs();
            if (allTourLogs == null)
            {
                Popularity = 0;
            }
            else
            {
                if (logsFromTour == null)
                {
                    logsFromTour = await _service.GetAllTourLogsFromTour(_tour);
                    if (logsFromTour == null || logsFromTour.Count == 0)
                    {
                        Popularity = 0;
                        ChildFriendliness = 0;
                        return;
                    }
                }
                Popularity = (int)Math.Round((double)logsFromTour.Count / allTourLogs.Count * 100);
            }
            // avr Difficulty / max Difficulty => the greater the harder (max. 4)
            double difficulty = logsFromTour!.Sum(tourLog => (int)tourLog.Difficulty) / (float)logsFromTour!.Count;
            TimeSpan avrTime = TourReport.GetAverageTime(logsFromTour);
            // avr Time / pre-calculated Time => the greater the harder
            if (_tour != null)
            {
                double timeDif = avrTime.Divide(_tour.Duration);
                double avrDistance = TourReport.GetAverageDistance(logsFromTour);
                // avr Distance / pre-calculated Distance => the greater the harder
                double distanceDif = avrDistance / _tour.Distance;
                int tmp = (int)(1 / ((difficulty + timeDif + distanceDif) / 3 / 4) * 100);
                if (tmp >= 100) tmp = 100;
                if (tmp <= 0) tmp = 0;
                ChildFriendliness = tmp;
            }
            Log.Debug("Calculated Tour Attributes");
        }

        private void ShowTourData(object? o)
        {
            if (o == null) return;
            _tour = (Tour)o;
            Title = _tour.Title;
            Origin = _tour.Origin;
            Destination = _tour.Destination;
            Description = _tour.Description;
            RouteType = _tour.RouteType;
            Distance = _tour.Distance;
            Duration = _tour.Duration;
            RouteImagePath = _tour.ImagePath;
            ShowComputedAttributes();
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
                if (!string.IsNullOrEmpty(value))
                {
                    string imagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, _folderLocation + value);
                    _routeImagePath = File.Exists(imagePath) ? imagePath : null;
                }
                else
                {
                    _routeImagePath = null;
                }

                if (_routeImagePath == null)
                {
                    Log.Info("Image " + value + " does not exists.");
                }
                RaisePropertyChangedEvent();
            }
        }
        public int ChildFriendliness
        {
            get => _childFriendliness;
            set
            {
                if (_childFriendliness == value) return;
                _childFriendliness = value;
                RaisePropertyChangedEvent();
            }
        }
        public int Popularity
        {
            get => _popularity;
            set
            {
                if (_popularity == value) return;
                _popularity = value;
                RaisePropertyChangedEvent();
            }
        }
    }
}

