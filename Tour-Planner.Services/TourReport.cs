using System;
using System.Collections.Generic;
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
using Tour_Planner.Models;

namespace Tour_Planner.Services
{

    public class TourReport
    {

        public void CreatePdf(Tour tour)
        {
            //const string LOREM_IPSUM_TEXT = "Lorem ipsum dolor sit amet, consectetur adipisici elit, sed eiusmod tempor incidunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquid ex ea commodi consequat. Quis aute iure reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint obcaecat cupiditat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
            string TARGET_PDF = "C:/Users/le0_/Source/Repos/DaveYasuo/Tour-Planner/Tour-Planner.ViewModels/PDFs/" + tour._name + ".pdf";

            PdfWriter writer = new PdfWriter(TARGET_PDF);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf);

            Paragraph loremIpsumHeader = new Paragraph("Tour Details of: " + tour._name)
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.HELVETICA))
                    .SetFontSize(14)
                    .SetBold()
                    .SetFontColor(ColorConstants.RED);
            document.Add(loremIpsumHeader);
            document.Add(new Paragraph(tour._description));
            document.Add(new Paragraph("Start: " + tour._source));
            document.Add(new Paragraph("Destination: " + tour._destination));
            document.Add(new Paragraph("Tour distance: " + tour._distance));
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

            Paragraph imageHeader = new Paragraph("Lorem Ipsum Image ...")
                    .SetFont(PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN))
                    .SetFontSize(18)
                    .SetBold()
                    .SetFontColor(ColorConstants.GREEN);
            document.Add(imageHeader);
            ImageData imageData = ImageDataFactory.Create(GOOGLE_MAPS_PNG);
            document.Add(new Image(imageData));*/
            document.Close();
        }

        private static Cell getHeaderCell(String s)
        {
            return new Cell().Add(new Paragraph(s)).SetBold().SetBackgroundColor(ColorConstants.GRAY);
        }
    }
}

