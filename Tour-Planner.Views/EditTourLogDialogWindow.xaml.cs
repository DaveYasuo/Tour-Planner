using System.Text.RegularExpressions;
using System.Windows.Input;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.Views
{
    /// <summary>
    /// Interaction logic for EditTourLogDialogWindow.xaml
    /// </summary>
    public partial class EditTourLogDialogWindow : IDialog
    {
        public EditTourLogDialogWindow()
        {
            InitializeComponent();
        }
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
