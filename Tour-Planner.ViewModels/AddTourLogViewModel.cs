using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class AddTourLogViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private IRestService service;
        private Difficulty? _selectedDifficulty;
        private Rating? _ratingItem;
        private string _comment;
        private TimeSpan _totalTime;
        private DateTime _dateAndTime = DateTime.Now;
        private IMediator mediator;
        private Tour? tour;
        public string Error { get; set; } = "";

        bool selectedRatingHasBeenTouched = false;
        bool totalTimeHasBeenTouched = false;
        bool dateAndTimeHasBeenTouched = false;
        bool commentHasBeenTouched = false;
        bool selectedItemHasBeenTouched = false;
        public AddTourLogViewModel(IRestService service, IMediator mediator)
        {
            _selectedDifficulty = null;
            _ratingItem = null;
            _comment = "";
            this.service = service;
            this.mediator = mediator;
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            SaveCommand = new RelayCommand(async _ =>
            {
                List<string> testableProperty = new List<string>() { nameof(SelectedDifficulty), nameof(SelectedRating), nameof(Comment), nameof(TotalTime), nameof(DateAndTime) };
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
                //Enum.TryParse(_selectedItem, out RouteType routeType);
                //Tour newTour = new(_title, _origin, _destination, _description, routeType); // todo
                //var result = await service.AddTour(newTour);
                //Debug.WriteLine(result);

            });
            mediator.Subscribe(SetSelectedTour, DataModels.Enums.ViewModelMessage.SelectTour);
        }


        private void SetSelectedTour(object? obj = null)
        {
            tour = (Tour)obj!;
        }

        public string this[string propertyName]
        {
            get
            {
                return GetErrorForProperty(propertyName, false);
            }
        }
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

        public DateTime DateAndTime
        {
            get => _dateAndTime;
            set
            {
                if (_dateAndTime == value) return;
                _dateAndTime = value;
                RaisePropertyChangedEvent();
            }
        }

        private string GetErrorForProperty(string propertyName, bool onSubmit)
        {

            Error = "";
            switch (propertyName)
            {
                case "SelectedRating":
                    if (_ratingItem == null && (selectedRatingHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(SelectedRating));
                        }
                        Error = "Rating cannot be empty!";
                        return Error;
                    }
                    selectedRatingHasBeenTouched = true;
                    break;
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
                case "DateAndTime":
                    if ((string.IsNullOrEmpty(_dateAndTime.ToString()) || _dateAndTime.ToString().Trim().Length == 0) && (dateAndTimeHasBeenTouched || onSubmit))
                    {
                        Error = "Date and time cannot be empty!";
                        return Error;
                    }
                    dateAndTimeHasBeenTouched = true;
                    break;
                case "Comment":
                    if (!string.IsNullOrEmpty(_comment) && _comment.Trim().Length == 0 && commentHasBeenTouched)
                    {

                        Error = "Comment cannot be only spaces!";
                        return Error;
                    }
                    commentHasBeenTouched = true;
                    break;
                case "SelectedDifficulty":
                    if (_selectedDifficulty == null && (selectedItemHasBeenTouched || onSubmit))
                    {
                        if (onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(SelectedDifficulty));
                        }
                        Error = "Difficulty cannot be empty!";
                    }
                    selectedItemHasBeenTouched = true;
                    return Error;
            }
            return Error;
        }

        //public string this[string columnName] => throw new NotImplementedException();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        public event EventHandler<DialogCloseRequestedEventArgs>? CloseRequested;
    }
}
