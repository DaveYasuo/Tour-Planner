using System.Windows.Input;
using Tour_Planner.Services.Interfaces;
using System.Text.RegularExpressions;

namespace Tour_Planner.Views
{
    /// <summary>
    /// Interaction logic for AddTourDialogWindow.xaml
    /// </summary>
    public partial class AddTourLogDialogWindow : IDialog
    {
        public AddTourLogDialogWindow()
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
