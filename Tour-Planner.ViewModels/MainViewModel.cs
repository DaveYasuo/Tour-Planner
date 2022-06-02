using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Tour_Planner.ViewModels.Commands;

namespace Tour_Planner.ViewModels
{
    public class MainViewModel : BaseViewModel
    {

        private BaseViewModel selectedViewModel;

        public BaseViewModel SelectedViewModel
        {
            get { return selectedViewModel; }
            set { 
                selectedViewModel = value;
                RaisePropertyChangedEvent(nameof(SelectedViewModel));
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }
        public ICommand UpdateViewCommand { get; set; }

        public MainViewModel()
        {
            if(selectedViewModel == null)
            {
                selectedViewModel = new HomeViewModel();
            }
            UpdateViewCommand =new UpdateViewCommand(this);
        }

    }
}
