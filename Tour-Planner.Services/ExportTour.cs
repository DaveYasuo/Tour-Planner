using System.IO;
using System.Reflection;
using System.Text.Json;
using log4net;
using Tour_Planner.Models;

namespace Tour_Planner.Services
{
    public class ExportTour
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        public bool ExportSingleTour(Tour tour)
        {
            string json = JsonSerializer.Serialize(tour);
            // Configure save file dialog box
            var dialog = new Microsoft.Win32.SaveFileDialog
            {
                FileName = tour.Title, // Default file name
                DefaultExt = ".json", // Default file extension
                Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt" // Filter files by extension
            };

            // Show save file dialog box
            bool? result = dialog.ShowDialog();

            // Process save file dialog box results
            if (result != true) return true;
            // Save document
            string filename = dialog.FileName;
            using StreamWriter sw = new StreamWriter(filename, false);
            sw.Write(json);
            Log.Info("Export tour success");
            return true;
        }
    }
}
