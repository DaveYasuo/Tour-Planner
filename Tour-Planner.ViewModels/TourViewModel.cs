using System;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.ViewModels;

//AddTourViewModel

namespace Tour_Planner.ViewModels
{
    public class TourViewModel : BaseViewModel
    {
        private string _inputTitle;
        private string _inputDescription;
        private string _inputSource;
        private string _inputDestination;


        public string InputTitle
        {
            get => _inputTitle;
            set
            {
                if (_inputTitle == value) return;
                _inputTitle = value;
                RaisePropertyChangedEvent(nameof(InputTitle));
            }
        }

        public string InputDescription
        {
            get => _inputDescription;
            set
            {
                _inputDescription = value;
                RaisePropertyChangedEvent(nameof(InputTitle));
            }
        }

        public string InputSource
        {
            get => _inputSource;
            set
            {
                if (_inputSource == value) return;
                _inputSource = value;
                RaisePropertyChangedEvent(nameof(InputTitle));
            }
        }

        public string InputDestination
        {
            get => _inputDestination;
            set
            {
                if (_inputDestination == value) return;
                _inputDestination = value;
                RaisePropertyChangedEvent(nameof(InputTitle));
            }
        }


        //Tour singleTour = new Tour();

    }
}