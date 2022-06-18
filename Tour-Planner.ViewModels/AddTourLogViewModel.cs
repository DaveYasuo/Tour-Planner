using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Tour_Planner.Services.Interfaces;
using Tour_Planner.ViewModels.Commands;

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
        public string Error { get; set; } = "";

        bool selectedRatingHasBeenTouched = false;
        bool totalTimeHasBeenTouched = false;
        bool dateAndTimeHasBeenTouched = false;
        bool commentHasBeenTouched = false;
        bool selectedItemHasBeenTouched = false;
        bool totalTimeRaiseProperty = false;
        public AddTourLogViewModel(IRestService service)
        {
            this.service = service;
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
                //Enum.TryParse(_selectedItem, out RouteType routeType);
                //Tour newTour = new(_title, _origin, _destination, _description, routeType); // todo
                //var result = await service.AddTour(newTour);
                //Debug.WriteLine(result);

            });
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
                        //TotalTime = null;
                        //if (!totalTimeRaiseProperty)
                        //{
                            //RaisePropertyChangedEvent(nameof(TotalTime));
                            //totalTimeRaiseProperty = true;
                        //}
                        Error = "Total time cannot be empty!";
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
