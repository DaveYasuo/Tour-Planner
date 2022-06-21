using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
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

namespace Tour_Planner.ViewModels.TourLogs
{
    public class AddTourLogViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private readonly IDialogService _dialogService;
        private Difficulty? _selectedDifficulty; private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);
        private Rating? _ratingItem;
        private string _comment;
        private string _distance;
        private TimeSpan _totalTime;
        private DateTime _dateTime = DateTime.Now;
        public string Error { get; set; } = "";

        private bool _distanceHasBeenTouched;
        private bool _selectedRatingHasBeenTouched;
        private bool _totalTimeHasBeenTouched;
        private bool _dateAndTimeHasBeenTouched;
        private bool _commentHasBeenTouched;
        private bool _selectedItemHasBeenTouched;

        public AddTourLogViewModel(IDialogService dialogService, IRestService service, IMediator mediator, Tour tour)
        {
            _dialogService = dialogService;
            _selectedDifficulty = null;
            _ratingItem = null;
            _comment = "";
            _distance = "0";
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

            async void ExecuteSave(object _)
            {
                Log.Debug("Execute Save Tour Log Button");
                List<string> testableProperty = new()
                {
                    nameof(SelectedDifficulty),
                    nameof(SelectedRating),
                    nameof(Comment),
                    nameof(TotalTime),
                    nameof(DateTime),
                    nameof(Distance)
                };
                bool hasError = false;
                foreach (var unused in testableProperty.Where(item => GetErrorForProperty(item, true) is not ""))
                {
                    hasError = true;
                }

                if (hasError)
                {
                    Log.Info("Execute Save Tour Log Button has form error");
                    _dialogService.ShowMessageBox("Please fill out the form before submitting");
                    return;
                }

                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                TourLog newTour = new(tour.Id, DateTime, TotalTime, (Rating)SelectedRating!, (Difficulty)SelectedDifficulty!, double.Parse(Distance), Comment); // Muss noch id holen
                var result = await service.AddTourLog(newTour);
                mediator.Publish(ViewModelMessage.UpdateTourLogList, null);
            }

            SaveCommand = new RelayCommand(ExecuteSave);
        }


        public string this[string propertyName] => GetErrorForProperty(propertyName, false);

        public Difficulty? SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty == value) return;
                _selectedDifficulty = value;
                RaisePropertyChangedEvent();
            }
        }
        public Rating? SelectedRating
        {
            get => _ratingItem;
            set
            {
                if (_ratingItem == value) return;
                _ratingItem = value;
                RaisePropertyChangedEvent();
            }
        }
        public string Comment
        {
            get => _comment;
            set
            {
                if (_comment == value) return;
                _comment = value;
                RaisePropertyChangedEvent();
            }
        }
        public TimeSpan TotalTime
        {
            get => _totalTime;
            set
            {
                if (_totalTime == value) return;
                _totalTime = value;
                RaisePropertyChangedEvent();
            }
        }
        public DateTime DateTime
        {
            get => _dateTime;
            set
            {
                if (_dateTime == value) return;
                _dateTime = value;
                RaisePropertyChangedEvent();
            }
        }
        public string Distance
        {
            get => _distance;
            set
            {
                if (_distance == value) return;
                _distance = value;
                RaisePropertyChangedEvent();
            }
        }
        private string GetErrorForProperty(string propertyName, bool onSubmit)
        {

            Error = "";
            switch (propertyName)
            {
                case "SelectedRating":
                    if (SelectedRating == null && (_selectedRatingHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(SelectedRating));
                        }
                        Error = "Rating cannot be empty!";
                        Log.Info(Error);
                        return Error;
                    }
                    _selectedRatingHasBeenTouched = true;
                    break;
                case "TotalTime":
                    if (TotalTime == TimeSpan.Zero && (_totalTimeHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(TotalTime));
                        }
                        Error = "Total time cannot be zero!";
                        Log.Info(Error);
                        return Error;
                    }
                    _totalTimeHasBeenTouched = true;
                    break;
                case "DateTime":
                    if ((string.IsNullOrEmpty(DateTime.ToString(CultureInfo.InvariantCulture)) || DateTime.ToString(CultureInfo.InvariantCulture).Trim().Length == 0) && (_dateAndTimeHasBeenTouched || onSubmit))
                    {
                        Error = "Date and time cannot be empty!";
                        Log.Info(Error);
                        return Error;
                    }
                    _dateAndTimeHasBeenTouched = true;
                    break;
                case "Distance":
                    if (Distance == "0" && (_distanceHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(Distance));
                        }
                        Error = "Distance cannot be 0!";
                        Log.Info(Error);
                        return Error;
                    }
                    _distanceHasBeenTouched = true;
                    break;
                case "Comment":
                    if (!string.IsNullOrEmpty(Comment) && Comment.Trim().Length == 0 && _commentHasBeenTouched)
                    {

                        Error = "Comment cannot be only spaces!";
                        Log.Info(Error);
                        return Error;
                    }
                    _commentHasBeenTouched = true;
                    break;
                case "SelectedDifficulty":
                    if (SelectedDifficulty == null && (_selectedItemHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(SelectedDifficulty));
                        }
                        Error = "Difficulty cannot be empty!";
                        Log.Info(Error);
                    }
                    _selectedItemHasBeenTouched = true;
                    return Error;
            }
            return Error;
        }
        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }

        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
    }
}
