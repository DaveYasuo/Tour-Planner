using System.IO;
using System.Text.Json;
using Tour_Planner.Models;

namespace Tour_Planner.Services
{
    public class ExportTour
    {
        public bool ExportSingleTour(Tour tour)
        {
            string json = JsonSerializer.Serialize(tour);
            // Configure save file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog();
            dialog.FileName = tour.Title; // Default file name
            dialog.DefaultExt = ".json"; // Default file extension
            dialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; // Filter files by extension

            // Show save file dialog box
            bool? result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dialog.FileName; 
                using StreamWriter sw = new StreamWriter(filename, false);
                sw.Write(json);
            }

            return true;
        }
    }
}
