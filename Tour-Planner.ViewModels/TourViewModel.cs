using System;
using System.Windows.Input;
using Tour_Planner.Models;
using Tour_Planner.ViewModels;

//AddTourViewModel

namespace Tour_Planner.ViewModels
{
    public class TourViewModel:BaseViewModel
    {
        private string inputTitle;
        private string inputDescription;
        private string inputSource;
        private string inputDestination;


        public string InputTitle
        {
            get => inputTitle;
            set
            {
                if (inputTitle == value) return;
                inputTitle = value;
                RaisePropertyChangedEvent(nameof(InputTitle));
            }
        }

        public string InputDescription
        {
            get => inputDescription;
            set
            {
                if (inputDescription == value) return;
                inputDescription = value;
                //OnPropertyChanged();Changed();
            }
        }

        public string InputSource
        {
            get => inputSource;
            set
            {
                if (inputSource == value) return;
                inputSource = value;
                //OnPropertyChanged();Changed();
            }
        }

        public string InputDestination
        {
            get => inputDestination;
            set
            {
                if (inputDestination == value) return;
                inputDestination = value;
                //OnPropertyChanged();Changed();
            }
        }


        Tour singleTour = new Tour();

    }
}