﻿using System;
using Tour_Planner.Models;

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
                RaisePropertyChangedEvent(nameof(inputTitle));
                Console.WriteLine(inputTitle);
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