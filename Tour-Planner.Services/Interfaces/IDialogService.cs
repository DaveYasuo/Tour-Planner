namespace Tour_Planner.Services.Interfaces
{
    public interface IDialogService
    {
        void Register<TViewModel, TView>() where TViewModel : IDialogRequestClose
            where TView : IDialog;

        bool? ShowDialog<TViewModel>(TViewModel viewModel) where TViewModel : IDialogRequestClose;

        void ShowMessageBox(string messageBoxText, string caption);
        void ShowMessageBox(string messageBoxText);
    }
}