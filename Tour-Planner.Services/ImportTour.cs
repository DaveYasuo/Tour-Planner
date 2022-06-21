using Microsoft.Win32;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;

namespace Tour_Planner.Services
{
    public class ImportTour
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod()!.DeclaringType);

        private readonly IRestService _service;
        public ImportTour(IRestService service)
        {
            _service = service;
        }

        public async Task ImportSingleTour()
        {
            // Configure open file dialog box
            var dialog = new OpenFileDialog
            {
                Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt" // Filter files by extension
            };
            //dialog.InitialDirectory = folderPath2;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                string json = await File.ReadAllTextAsync(filename);
                Tour? tour = JsonSerializer.Deserialize<Tour>(json);
                if (tour != null)
                    if (await _service.AddTour(tour) != null)
                        Log.Info("Tour has been imported");
                    else
                    {
                        Log.Error("Tour cannot be imported");
                    }
            }
        }
    }
}
