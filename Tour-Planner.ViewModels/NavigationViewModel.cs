using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Extensions;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class NavigationViewModel
    {
        private readonly IMediator mediator;

        public NavigationViewModel(IMediator mediator)
        {
            this.mediator = mediator;
            DisplayAddTourCommand = new RelayCommand(_ =>
            {
                mediator.Publish(ViewModelMessage.AddTour, null);
            });
            DisplayEditTourCommand = new RelayCommand(_ =>
            {
                mediator.Publish(ViewModelMessage.EditTour, null);
            });
        }


        public ICommand DisplayAddTourCommand { get; }
        public ICommand DisplayEditTourCommand { get; }
    }
}
