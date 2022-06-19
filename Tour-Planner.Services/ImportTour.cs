using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Tour_Planner.Models;
using Tour_Planner.Services.Interfaces;


namespace Tour_Planner.Services
{
    public class ImportTour
    {
        //Could do MultiSelect
        IRestService service;
        public ImportTour(IRestService service)
        {
            this.service = service;
        }

        public async Task ImportSingleTour()
        {
            // Configure open file dialog box
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.Filter = "Json files (*.json)|*.json|Text files (*.txt)|*.txt"; // Filter files by extension
            //dialog.InitialDirectory = folderPath2;

            // Show open file dialog box
            bool? result = dialog.ShowDialog();

            // Process open file dialog box results
            if (result == true)
            {
                // Open document
                string filename = dialog.FileName;
                string json = File.ReadAllText(filename);
                Tour? tour = JsonSerializer.Deserialize<Tour>(json);
                if(tour!= null)
                await service.AddTour(tour);
            }
        }
    }
}
