using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class EditTourLogViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private Difficulty _selectedDifficulty;
        private Rating _ratingItem;
        private string _comment;
        private TimeSpan _totalTime;
        private DateTime _dateTime;
        private string _distance;
        public string Error { get; set; } = "";

        private bool _distanceHasBeenTouched;
        private bool _totalTimeHasBeenTouched;
        private bool _dateAndTimeHasBeenTouched;
        private bool _commentHasBeenTouched;

        public EditTourLogViewModel(IRestService service, IMediator mediator, TourLog selectedTourLog)
        {
            _selectedDifficulty = selectedTourLog.Difficulty;
            _ratingItem = selectedTourLog.Rating;
            _comment = selectedTourLog.Comment;
            _totalTime = selectedTourLog.TotalTime;
            _dateTime = selectedTourLog.DateTime;
            _distance = selectedTourLog.Distance.ToString(CultureInfo.InvariantCulture);
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));

            async void Execute(object _)
            {
                List<string> testableProperty = new() { nameof(Comment), nameof(TotalTime), nameof(DateTime), nameof(Distance) };
                bool hasError = false;
                foreach (var item in testableProperty.Where(item => GetErrorForProperty(item, true) is not ""))
                {
                    hasError = true;
                }

                if (hasError)
                {
                    MessageBox.Show("Please fill out the form before submitting");
                    return;
                }

                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                TourLog newTour = new(selectedTourLog.Id, selectedTourLog.TourId, DateTime, TotalTime, SelectedRating, SelectedDifficulty, double.Parse(Distance), Comment);
                var result = await service.UpdateTourLog(newTour);
                mediator.Publish(ViewModelMessage.UpdateTourLogList, null);
            }

            SaveCommand = new RelayCommand(Execute);
        }


        public string this[string propertyName] => GetErrorForProperty(propertyName, false);

        public Difficulty SelectedDifficulty
        {
            get => _selectedDifficulty;
            set
            {
                if (_selectedDifficulty == value) return;
                _selectedDifficulty = value;
                RaisePropertyChangedEvent();
            }
        }
        public Rating SelectedRating
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
                RaisePropertyChangedEvent();
                _distance = value;
            }
        }
        private string GetErrorForProperty(string propertyName, bool onSubmit)
        {

            Error = "";
            switch (propertyName)
            {
                case "TotalTime":
                    if (TotalTime == TimeSpan.Zero && (_totalTimeHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(TotalTime));
                        }
                        Error = "Total time cannot be zero!";
                        return Error;
                    }
                    _totalTimeHasBeenTouched = true;
                    break;
                case "DateTime":
                    if ((string.IsNullOrEmpty(DateTime.ToString()) || DateTime.ToString().Trim().Length == 0) && (_dateAndTimeHasBeenTouched || onSubmit))
                    {
                        Error = "Date and time cannot be empty!";
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
                        return Error;
                    }
                    _distanceHasBeenTouched = true;
                    break;
                case "Comment":
                    if (!string.IsNullOrEmpty(Comment) && Comment.Trim().Length == 0 && _commentHasBeenTouched)
                    {

                        Error = "Comment cannot be only spaces!";
                        return Error;
                    }
                    _commentHasBeenTouched = true;
                    break;
            }
            return Error;
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
    }
}
