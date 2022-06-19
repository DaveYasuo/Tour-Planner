using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using Tour_Planner.Models;

namespace Tour_Planner.Services
{
    public class ExportTour
    {
        /*public void ExportTourFromInstance()
        {
            //Directory Explorer
                /*if (Directory.Exists(folderPath))
                {
                    ProcessStartInfo startInfo = new ProcessStartInfo()
                    {
                        Arguments = folderPath,
                        FileName = "explorer.exe"
                    };

                    Process.Start(startInfo);
                }
                else
                {
                    MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
                }
        }*/


        public bool ExportSingleTour(Tour tour)
        {
            string folderPath = ".\\..\\..\\..\\..\\ExportedTours/";
            string fileName = tour.Title + ".json";
            string json = JsonSerializer.Serialize(tour);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (StreamWriter sw = new StreamWriter(folderPath + fileName, false))
            {
                sw.Write(json);
            };

            return true;
        }
    }
}
