using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.Extensions;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Models;

namespace Tour_Planner.ViewModels
{
    public class AddTourLogViewModel : BaseViewModel, IDialogRequestClose, IDataErrorInfo
    {
        private IRestService service;
        private string _selectedItem;
        private string _ratingItem;
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
        bool totalTimeRaiseProperty = true;
        public AddTourLogViewModel(IRestService service, IMediator mediator)
        {
            this.service = service;
            this.mediator = mediator;
            CancelCommand = new RelayCommand(_ => CloseRequested?.Invoke(this, new DialogCloseRequestedEventArgs(false)));
            SaveCommand = new RelayCommand(async _ =>
            {
                List<string> testableProperty = new List<string>() { nameof(SelectedItem), nameof(SelectedRating), nameof(Comment), nameof(TotalTime), nameof(DateAndTime) };
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
                Enum.TryParse(_selectedItem, out DifficultyType difficultyType);
                Enum.TryParse(_ratingItem, out RatingType ratingType);
                TourLog newTour = new(1, _dateAndTime, _totalTime, ratingType, difficultyType,_comment); // Muss noch id holen
                var result = await service.AddTour(newTour);
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

        public string SelectedRating
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
                    if ((string.IsNullOrEmpty(_ratingItem) || _ratingItem.Trim().Length == 0) && (selectedRatingHasBeenTouched || onSubmit))
                    {
                        SelectedRating = "";
                        Error = "Rating cannot be empty!";
                        return Error;
                    }
                    selectedRatingHasBeenTouched = true;
                    break;
                case "TotalTime":
                    if (TotalTime == TimeSpan.Zero && (totalTimeHasBeenTouched || onSubmit))
                    {
                        if (totalTimeRaiseProperty && onSubmit)
                        {
                            RaisePropertyChangedEvent(nameof(TotalTime));
                            totalTimeRaiseProperty = false;
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
                case "SelectedItem":
                    if (string.IsNullOrEmpty(_selectedItem) && (selectedItemHasBeenTouched || onSubmit))
                    {
                        SelectedItem = "";
                        Error = "Difficulty cannot be empty!";
                    }
                    else
                    {
                        selectedItemHasBeenTouched = true;
                        return Error;
                    }
                    break;
            }
            return Error;
        }

        //public string this[string columnName] => throw new NotImplementedException();

        public ICommand SaveCommand { get; }
        public ICommand CancelCommand { get; }


        public event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}
