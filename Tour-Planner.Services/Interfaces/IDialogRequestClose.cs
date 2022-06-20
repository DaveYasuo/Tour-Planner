using System;

namespace Tour_Planner.Services.Interfaces
{
    public interface IDialogRequestClose
    {
        event EventHandler<DialogCloseRequestedEventArgs> CloseRequested;
    }
}