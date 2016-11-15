using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;
using HelpdeskViewModels;

namespace HelpdeskWebsite.Reports
{
    public class ReportPDF
    {
        static string mappedPath = System.Web.Hosting.HostingEnvironment.MapPath("~/");
        static Font hdgFont = new Font(Font.FontFamily.HELVETICA, 24, Font.BOLD);
        static Font smallfont = new Font(Font.FontFamily.HELVETICA, 14, Font.BOLD);
        static Font smallfontNOB = new Font(Font.FontFamily.HELVETICA, 12, Font.NORMAL);
        static string IMG = "img/helpdesk.png";
        List<EmployeeViewModel> employees = new List<EmployeeViewModel>();
        List<CallViewModel> calls = new List<CallViewModel>();

        public void GenerateEmployeeReport()
        {
            try
            {

                EmployeeViewModel emp = new EmployeeViewModel();
                employees = emp.GetAll();

                Document document = new Document();
                PdfWriter.GetInstance(document,
                    new FileStream(mappedPath + "pdfs/EmployeeReport.pdf", FileMode.Create));
                document.Open();

                /* Image */
                Paragraph para1 = new Paragraph();
                Image image1 = Image.GetInstance(mappedPath + IMG);
                image1.ScaleToFit(150f, 150f);
                image1.Alignment = Image.ALIGN_CENTER;
                image1.SpacingAfter = 10f;

                document.Add(image1);



                /* Title */
                Paragraph para2 = new Paragraph("Employees", hdgFont);
                para2.Alignment = Element.ALIGN_CENTER;
                para2.SpacingAfter = 10f;
                document.Add(para2);
                
                

                /* Table */
                PdfPTable table = new PdfPTable(3);
                table.WidthPercentage = 50.00F;
                PdfPCell cell1 = new PdfPCell(new Phrase("Title", smallfont));
                cell1.Border = Rectangle.NO_BORDER;
                table.AddCell(cell1);
                PdfPCell cell2 = new PdfPCell(new Phrase("Firstname", smallfont));
                cell2.Border = Rectangle.NO_BORDER;
                table.AddCell(cell2);
                PdfPCell cell3 = new PdfPCell(new Phrase("Lastname", smallfont));
                cell3.Border = Rectangle.NO_BORDER;
                table.AddCell(cell3);
                

                foreach(var employee in employees)
                {
                    PdfPCell cell4 = new PdfPCell(new Phrase(employee.Title, smallfontNOB));
                    PdfPCell cell5 = new PdfPCell(new Phrase(employee.Firstname, smallfontNOB));
                    PdfPCell cell6 = new PdfPCell(new Phrase(employee.Lastname, smallfontNOB));
                    cell4.Border = Rectangle.NO_BORDER;
                    cell5.Border = Rectangle.NO_BORDER;
                    cell6.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell4);
                    table.AddCell(cell5);
                    table.AddCell(cell6);
                }

                table.SpacingAfter = 30f;
                document.Add(table);

                Paragraph date = new Paragraph("Employee report written on - " + System.DateTime.Now, smallfont);
                date.Alignment = Element.ALIGN_CENTER;
                document.Add(date);
                document.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error " + ex.Message);
            }
        }

        public void GenerateCallReport()
        {
            try
            {

                CallViewModel call = new CallViewModel();
                calls = call.GetAll();

                Document document = new Document();
                PdfWriter.GetInstance(document,
                    new FileStream(mappedPath + "pdfs/CallReport.pdf", FileMode.Create));
                document.Open();

                /* Image */
                Paragraph para1 = new Paragraph();
                Image image1 = Image.GetInstance(mappedPath + IMG);
                image1.ScaleToFit(150f, 150f);
                image1.Alignment = Image.ALIGN_CENTER;
                image1.SpacingAfter = 10f;

                document.Add(image1);



                /* Title */
                Paragraph para2 = new Paragraph("Calls", hdgFont);
                para2.Alignment = Element.ALIGN_CENTER;
                para2.SpacingAfter = 10f;
                document.Add(para2);



                /* Table */
                float[] widths = { 5f, 5f, 5f, 5f, 5f, 5f };
                PdfPTable table = new PdfPTable(6);
                table.WidthPercentage = 85.00F;
                PdfPCell cell1 = new PdfPCell(new Phrase("Opened", smallfont));
                cell1.Border = Rectangle.NO_BORDER;
                table.AddCell(cell1);
                PdfPCell cell2 = new PdfPCell(new Phrase("Lastname", smallfont));
                cell2.Border = Rectangle.NO_BORDER;
                table.AddCell(cell2);

                PdfPCell cell3 = new PdfPCell(new Phrase("Tech", smallfont));
                cell3.Border = Rectangle.NO_BORDER;
                table.AddCell(cell3);

                PdfPCell cell4 = new PdfPCell(new Phrase("Problem", smallfont));
                cell4.Border = Rectangle.NO_BORDER;
                table.AddCell(cell4);

                PdfPCell cell5 = new PdfPCell(new Phrase("Status", smallfont));
                cell5.Border = Rectangle.NO_BORDER;
                table.AddCell(cell5);

                PdfPCell cell6 = new PdfPCell(new Phrase("Closed", smallfont));
                cell6.Border = Rectangle.NO_BORDER;
                table.AddCell(cell6);


                foreach (var c in calls)
                {
                    PdfPCell cell7 = new PdfPCell(new Phrase(c.DateOpened.ToString("yyyy-MM-dd"), smallfontNOB));
                    PdfPCell cell8 = new PdfPCell(new Phrase(c.EmployeeName, smallfontNOB));
                    PdfPCell cell9 = new PdfPCell(new Phrase(c.TechName, smallfontNOB));
                    PdfPCell cell10 = new PdfPCell(new Phrase(c.ProblemDescription, smallfontNOB));
                    PdfPCell cell11 = new PdfPCell(new Phrase((c.OpenStatus.ToString() == "True") ? "Open" : "Closed", smallfontNOB));
                    PdfPCell cell12 = new PdfPCell(new Phrase((c.DateClosed != null) ? c.DateClosed.Value.ToString("yyyy-MM-dd") : c.DateClosed.ToString(), smallfontNOB));
                    cell7.Border = Rectangle.NO_BORDER;
                    cell8.Border = Rectangle.NO_BORDER;
                    cell9.Border = Rectangle.NO_BORDER;
                    cell10.Border = Rectangle.NO_BORDER;
                    cell11.Border = Rectangle.NO_BORDER;
                    cell12.Border = Rectangle.NO_BORDER;
                    table.AddCell(cell7);
                    table.AddCell(cell8);
                    table.AddCell(cell9);
                    table.AddCell(cell10);
                    table.AddCell(cell11);
                    table.AddCell(cell12);
                }

                table.SpacingAfter = 30f;
                document.Add(table);

                Paragraph date = new Paragraph("Calls report written on - " + System.DateTime.Now, smallfont);
                date.Alignment = Element.ALIGN_CENTER;
                document.Add(date);
                document.Close();
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Error " + ex.Message);
            }
        }
    }
}