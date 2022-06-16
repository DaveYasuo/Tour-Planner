using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Tour_Planner.Models;

namespace Tour_Planner.Services
{
    public class ExportTour
    {
        public void ExportTourFromInstance(Tour tour)
        {
            string path = "./../../../../Tour-Planner.Services/Files/" + tour.Title + ".txt";
            if (File.Exists(path))
            {

            }
            using (StreamWriter sw = File.CreateText(path));
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
                }*/
        }
    }
}
