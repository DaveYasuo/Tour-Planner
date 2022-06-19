using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Models;

namespace Tour_Planner.Services
{

    public class TourReport
    {
        string folderPath = ".\\..\\..\\..\\..\\Reports/";

        public void CreateTourReport(Tour tour ,List<TourLog> tourLogs)
        {
            //const string LOREM_IPSUM_TEXT = "Lorem ipsum dolor sit amet, consectetur adipisici elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquid ex ea commodi consequat. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            string TARGET_PDF = ".\\..\\..\\..\\..\\Reports/" + tour.Title + ".pdf";
            string imagePath = ".\\..\\..\\..\\..\\RouteImages/";


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            PdfWriter writer = new PdfWriter(TARGET_PDF);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph TitleHeader = new Paragraph("Tour Details of: " + tour.Title)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.RED);
            document.Add(TitleHeader);
            document.Add(new Paragraph(tour.Description));
            document.Add(new Paragraph("Start: " + tour.Origin));
            document.Add(new Paragraph("Destination: " + tour.Destination));
            document.Add(new Paragraph("Tour distance: " + tour.Distance));

            //Image
            Paragraph imageHeader = new Paragraph("Tour Map")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18)
                    .SetBold()
                    .SetFontColor(ColorConstants.GREEN);
            document.Add(imageHeader);
            ImageData imageData = ImageDataFactory.Create(imagePath + tour.ImagePath);
            Image image = new Image(imageData);
            document.Add(image.SetAutoScale(true));

            //TourLogs
            Paragraph TourlogsHeader = new Paragraph("Tourlogs")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18)
                .SetBold();
            document.Add(TourlogsHeader);
            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            table.AddHeaderCell(getHeaderCell("Date and time"));
            table.AddHeaderCell(getHeaderCell("Total time"));
            table.AddHeaderCell(getHeaderCell("Rating"));
            table.AddHeaderCell(getHeaderCell("Difficulty"));
            table.AddHeaderCell(getHeaderCell("Comment"));
            table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);
            foreach (TourLog tourLog in tourLogs)
            {
                table.AddCell(tourLog.DateTime.ToString());
                table.AddCell(tourLog.TotalTime.ToString());
                table.AddCell(tourLog.Rating.ToString());
                table.AddCell(tourLog.Difficulty.ToString());
                table.AddCell(tourLog.Comment);
            }

            document.Add(table);
            Console.WriteLine("Pdf Created");

            document.Close();
        }

        public void CreateSummaryReport(List<Tour> tours,List<TourLog> tourLogs)
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string TARGET_PDF = ".\\..\\..\\..\\..\\Reports/"+"Summary_" + ".pdf";

            PdfWriter writer = new PdfWriter(TARGET_PDF);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph TitleHeader = new Paragraph("Summary")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.RED);

            document.Add(TitleHeader);
            Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
            table.AddHeaderCell(getHeaderCell("Title"));
            table.AddHeaderCell(getHeaderCell("Average time"));
            table.AddHeaderCell(getHeaderCell("Average distance"));
            table.AddHeaderCell(getHeaderCell("Rating"));
            table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);
            foreach (Tour tour in tours)
            {
                List<TourLog> tourLogsFromTour = GetAllTourLogsFromTour(tourLogs, tour);
                table.AddCell(tour.Title);
                if(tourLogsFromTour.Count != 0)
                {
                    table.AddCell(GetAverageTime(tourLogsFromTour).ToString());
                    table.AddCell(GetAverageDistance(tourLogsFromTour).ToString());
                    table.AddCell(GetAverageRating(tourLogsFromTour).ToString());
                }
                else
                {
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                }
            }

            document.Add(table);
            document.Close();
        }
        
        private TimeSpan GetAverageTime(List<TourLog> tourLogs)
        {
            TimeSpan averageTime = TimeSpan.Zero;
            TimeSpan addedTime = TimeSpan.Zero;
            foreach(TourLog tourLog in tourLogs)
            {
                addedTime = addedTime.Add(tourLog.TotalTime);
            }
            averageTime = addedTime.Divide(tourLogs.Count());
            return averageTime;
        }
        private Rating GetAverageRating(List<TourLog> tourLogs)
        {
            Rating rating;
            float averageRating = 0;
            int addedRating = 0;
            foreach (TourLog tourLog in tourLogs)
            {
                addedRating += (int) tourLog.Rating;
            }
            averageRating = addedRating / tourLogs.Count();
            Math.Round(averageRating);
            switch (averageRating)
            {
                case 0: rating = Rating.very_good; break;
                case 1: rating = Rating.good; break;
                case 2: rating = Rating.medium; break;
                case 3: rating = Rating.bad; break;
                case 4: rating = Rating.very_bad; break;
                default: rating = Rating.very_bad; break;
            }
            return rating;
        }

        private double GetAverageDistance(List<TourLog> tourLogs)
        {
            double averageDistance = 0;
            double addedDistance = 0;
            foreach(TourLog tourLog in tourLogs)
            {
                addedDistance += tourLog.Distance;
            }
            averageDistance = addedDistance / tourLogs.Count();
            averageDistance = Math.Round(averageDistance,3);
            return averageDistance;
        }

        private List<TourLog> GetAllTourLogsFromTour(List<TourLog> tourLogs, Tour tour)
        {
            List<TourLog> tourLogsForTour = new();
            foreach(TourLog tourLog in tourLogs)
            {
                if(tour.Id == tourLog.TourId) tourLogsForTour.Add(tourLog); 
            }
            return tourLogsForTour;
        }

        private static Cell getHeaderCell(String s)
        {
            return new Cell().Add(new Paragraph(s)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}

/*
Paragraph listHeader = new Paragraph("Lorem Ipsum ...")
        .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD))
        .SetFontSize(14)
        .SetBold()
        .SetFontColor(ColorConstants.BLUE);
List list = new List()
        .SetSymbolIndent(12)
        .SetListSymbol("\u2022")
        .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_BOLD));
list.Add(new ListItem("lorem ipsum 1"))
        .Add(new ListItem("lorem ipsum 2"))
        .Add(new ListItem("lorem ipsum 3"))
        .Add(new ListItem("lorem ipsum 4"))
        .Add(new ListItem("lorem ipsum 5"))
        .Add(new ListItem("lorem ipsum 6"));
document.Add(listHeader);
document.Add(list);

Paragraph tableHeader = new Paragraph("Lorem Ipsum Table ...")
        .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
        .SetFontSize(18)
        .SetBold()
        .SetFontColor(ColorConstants.GREEN);
document.Add(tableHeader);
Table table = new Table(UnitValue.CreatePercentArray(4)).UseAllAvailableWidth();
table.AddHeaderCell(getHeaderCell("Ipsum 1"));
table.AddHeaderCell(getHeaderCell("Ipsum 2"));
table.AddHeaderCell(getHeaderCell("Ipsum 3"));
table.AddHeaderCell(getHeaderCell("Ipsum 4"));
table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);
table.AddCell("lorem 1");
table.AddCell("lorem 2");
table.AddCell("lorem 3");
table.AddCell("lorem 4");
document.Add(table);

Console.WriteLine("Pdf Created");


document.Add(new AreaBreak());
*/