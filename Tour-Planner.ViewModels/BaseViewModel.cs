using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Tour_Planner.Services;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.ViewModels
{
    public abstract class BaseViewModel : INotifyPropertyChanged
    {
        public static IRestService RestService = DependencyService.GetInstance<IRestService>();

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void RaisePropertyChangedEvent([CallerMemberName] string propertyName = "")
        {
            ValidatePropertyName(propertyName);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected void ValidatePropertyName(string propertyName)
        {
            if (TypeDescriptor.GetProperties(this)[propertyName] == null)
            {
                throw new ArgumentException("Invalid property name: " + propertyName);
            }
        }
        #endregion
    }
}
