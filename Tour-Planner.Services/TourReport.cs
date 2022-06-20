using iText.IO.Font.Constants;
using iText.IO.Image;
using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Tour_Planner.DataModels.Enums;
using Tour_Planner.Models;

namespace Tour_Planner.Services
{

    public class TourReport
    {
        string folderPath = ".\\..\\..\\..\\..\\Reports/";

        public void CreateTourReport(Tour tour, List<TourLog> tourLogs)
        {
            //const string LOREM_IPSUM_TEXT = "Lorem ipsum dolor sit amet, consectetur adipisici elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquid ex ea commodi consequat. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            string targetPdf = ".\\..\\..\\..\\..\\Reports/" + tour.Title + ".pdf";
            const string imagePath = ".\\..\\..\\..\\..\\RouteImages/";


            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            PdfWriter writer = new PdfWriter(targetPdf);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph titleHeader = new Paragraph("Tour Details of: " + tour.Title)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.RED);
            document.Add(titleHeader);
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
            Paragraph tourlogsHeader = new Paragraph("Tourlogs")
                .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                .SetFontSize(18)
                .SetBold();
            document.Add(tourlogsHeader);
            Table table = new Table(UnitValue.CreatePercentArray(7)).UseAllAvailableWidth();
            table.AddHeaderCell(GetHeaderCell("Date and time"));
            table.AddHeaderCell(GetHeaderCell("Total time"));
            table.AddHeaderCell(GetHeaderCell("Distance"));
            table.AddHeaderCell(GetHeaderCell("Km/h"));
            table.AddHeaderCell(GetHeaderCell("Rating"));
            table.AddHeaderCell(GetHeaderCell("Difficulty"));
            table.AddHeaderCell(GetHeaderCell("Comment"));
            table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);
            foreach (TourLog tourLog in tourLogs)
            {
                table.AddCell(tourLog.DateTime.ToString(CultureInfo.InvariantCulture));
                table.AddCell(tourLog.TotalTime.ToString());
                table.AddCell(tourLog.Distance.ToString(CultureInfo.InvariantCulture));
                table.AddCell(Math.Round((tourLog.Distance / tourLog.TotalTime.TotalHours), 1).ToString(CultureInfo.InvariantCulture));
                table.AddCell(tourLog.Rating.ToString());
                table.AddCell(tourLog.Difficulty.ToString());
                table.AddCell(tourLog.Comment);
            }

            document.Add(table);
            Console.WriteLine("Pdf Created");

            document.Close();
        }

        public void CreateSummaryReport(List<Tour>? tours, List<TourLog>? tourLogs)
        {
            if (tours == null || tourLogs == null) return;
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            string targetPdf = ".\\..\\..\\..\\..\\Reports/" + "Summary_" + ".pdf";

            PdfWriter writer = new PdfWriter(targetPdf);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph titleHeader = new Paragraph("Summary")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.RED);

            document.Add(titleHeader);
            Table table = new Table(UnitValue.CreatePercentArray(5)).UseAllAvailableWidth();
            table.AddHeaderCell(GetHeaderCell("Title"));
            table.AddHeaderCell(GetHeaderCell("Average time"));
            table.AddHeaderCell(GetHeaderCell("Average distance"));
            table.AddHeaderCell(GetHeaderCell("Average Km/h"));
            table.AddHeaderCell(GetHeaderCell("Rating"));
            table.SetFontSize(14).SetBackgroundColor(ColorConstants.WHITE);
            foreach (Tour tour in tours)
            {
                List<TourLog> tourLogsFromTour = GetAllTourLogsFromTour(tourLogs, tour);
                table.AddCell(tour.Title);
                TimeSpan averageTime = GetAverageTime(tourLogsFromTour);
                double averageDistance = GetAverageDistance(tourLogsFromTour);
                double averageSpeed = averageDistance / averageTime.TotalHours;
                if (tourLogsFromTour.Count != 0)
                {
                    table.AddCell(averageTime.ToString());
                    table.AddCell(averageDistance.ToString(CultureInfo.InvariantCulture));
                    table.AddCell(Math.Round(averageSpeed, 2).ToString(CultureInfo.InvariantCulture));
                    table.AddCell(GetAverageRating(tourLogsFromTour).ToString());
                }
                else
                {
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                    table.AddCell("");
                }
            }

            document.Add(table);
            document.Close();
        }

        public static TimeSpan GetAverageTime(List<TourLog> tourLogs)
        {
            TimeSpan addedTime = tourLogs.Aggregate(TimeSpan.Zero, (current, tourLog) => current.Add(tourLog.TotalTime));
            var averageTime = addedTime.Divide(tourLogs.Count);
            return averageTime;
        }
        private Rating GetAverageRating(List<TourLog> tourLogs)
        {
            int addedRating = tourLogs.Sum(tourLog => (int)tourLog.Rating);
            float averageRating = addedRating / (float)tourLogs.Count;
            double roundAverageRating = Math.Round(averageRating);
            var rating = roundAverageRating switch
            {
                0 => Rating.very_good,
                1 => Rating.good,
                2 => Rating.medium,
                3 => Rating.bad,
                4 => Rating.very_bad,
                _ => Rating.very_bad
            };
            return rating;
        }

        public static double GetAverageDistance(List<TourLog> tourLogs)
        {
            double addedDistance = tourLogs.Sum(tourLog => tourLog.Distance);
            double averageDistance = addedDistance / tourLogs.Count();
            averageDistance = Math.Round(averageDistance, 3);
            return averageDistance;
        }

        private List<TourLog> GetAllTourLogsFromTour(List<TourLog> tourLogs, Tour tour)
        {
            return tourLogs.Where(tourLog => tour.Id == tourLog.TourId).ToList();
        }

        private static Cell GetHeaderCell(string s)
        {
            return new Cell().Add(new Paragraph(s)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}