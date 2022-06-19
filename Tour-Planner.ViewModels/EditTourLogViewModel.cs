using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private IRestService service;
        private IMediator mediator;
        private TourLog selectedTourLog;
        private Difficulty _selectedDifficulty;
        private Rating _ratingItem;
        private string _comment;
        private TimeSpan _totalTime;
        private DateTime _dateTime;
        private double _distance;
        public string Error { get; set; } = "";

        bool distanceHasBeenTouched = false;
        bool totalTimeHasBeenTouched = false;
        bool dateAndTimeHasBeenTouched = false;
        bool commentHasBeenTouched = false;

        public EditTourLogViewModel(IRestService service, IMediator mediator, TourLog selectedTourLog)
        {
            this.service = service;
            this.mediator = mediator;
            this.selectedTourLog = selectedTourLog;
            _selectedDifficulty = selectedTourLog.Difficulty;
            _ratingItem = selectedTourLog.Rating;
            _comment = selectedTourLog.Comment;
            _totalTime = selectedTourLog.TotalTime;
            _dateTime = selectedTourLog.DateTime;
            _distance = selectedTourLog.Distance;
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            SaveCommand = new RelayCommand(async _ =>
            {
                List<string> testableProperty = new() { nameof(Comment), nameof(TotalTime), nameof(DateTime), nameof(Distance) };
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
                CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(true));
                TourLog newTour = new(selectedTourLog.Id, selectedTourLog.TourId, DateTime, TotalTime, SelectedRating, SelectedDifficulty, Distance, Comment);
                var result = await service.UpdateTourLog(newTour);
                mediator.Publish(ViewModelMessage.UpdateTourLogList, null);
            });
        }


        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName, false);
            }
        }
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
        private string GetErrorForProperty(string propertyName, bool onSubmit)
        {

            Error = "";
            switch (propertyName)
            {
                case "TotalTime":
                    if (TotalTime == TimeSpan.Zero && (totalTimeHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(TotalTime));
                        }
                        Error = "Total time cannot be zero!";
                        return Error;
                    }
                    totalTimeHasBeenTouched = true;
                    break;
                case "DateTime":
                    if ((string.IsNullOrEmpty(_dateTime.ToString()) || _dateTime.ToString().Trim().Length == 0) && (dateAndTimeHasBeenTouched || onSubmit))
                    {
                        Error = "Date and time cannot be empty!";
                        return Error;
                    }
                    dateAndTimeHasBeenTouched = true;
                    break;
                case "Distance":
                    if (Distance <= 0 && (distanceHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(Distance));
                        }
                        Error = "Distance cannot be empty!";
                        return Error;
                    }
                    distanceHasBeenTouched = true;
                    break;
                case "Comment":
                    if (!string.IsNullOrEmpty(_comment) && _comment.Trim().Length == 0 && commentHasBeenTouched)
                    {

                        Error = "Comment cannot be only spaces!";
                        return Error;
                    }
                    commentHasBeenTouched = true;
                    break;
            }
            return Error;
        }

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
    }
}
