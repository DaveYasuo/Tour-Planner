namespace Tour_Planner.Services.Interfaces
{
    //https://www.youtube.com/watch?v=OqKaV4d4PXg
    public interface IDialog
    {
        object DataContext { get; set; }
        bool? DialogResult { get; set; }
        void Close();
        bool? ShowDialog();
    }
}