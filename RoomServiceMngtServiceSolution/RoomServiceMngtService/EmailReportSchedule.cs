using iTextSharp.text;
using iTextSharp.text.pdf;
using RoomServiceMngtService.DataAccess;
using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using TCPServer;

namespace RoomServiceMngtService
{
    public class EmailReportSchedule
    {
        private static System.Timers.Timer Timer;

        private static bool sent;
        private static DateTime lastDateTime;

        public static void Start() {
           // Timer = new System.Timers.Timer(1000 * 60 * 60);
            Timer = new System.Timers.Timer(20000);
            Timer.Elapsed += OnTimedEvent;
            Timer.AutoReset = true;
            Timer.Start();
        }

        public static void Stop() {
            Timer?.Stop();
        }
        private static void createReport() {
            try
            {

            Document document = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            MemoryStream mem = new MemoryStream();

            //string folderPath = "C:\\Users\\Milan Chinthaka\\Desktop\\PDF\\";
            //if (!Directory.Exists(folderPath))
            //{
            //    Directory.CreateDirectory(folderPath);
            //}
            //FileStream stream = new FileStream(folderPath + "DataGridViewExport"+ DateTime.Now.Ticks+".pdf", FileMode.Create);

            PdfWriter writer = PdfWriter.GetInstance(document, mem);          
            document.Open();

            //Report Header
            BaseFont bfntHead = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntHead = new Font(bfntHead, 16, 1, BaseColor.BLACK);
            Paragraph prgHeading = new Paragraph();
            prgHeading.Alignment = Element.ALIGN_CENTER;
            prgHeading.Add(new Chunk("Daily report for the Steward Call system: " + DateTime.Now.ToString("yyyy-MM-dd"), fntHead));
            document.Add(prgHeading);

            //Add line break
            document.Add(new Chunk("\n", fntHead));

            Font fntTableHead = new Font(bfntHead, 12, 1, BaseColor.BLACK);
            Paragraph prgTableHeading = new Paragraph();
            prgTableHeading.Alignment = Element.ALIGN_CENTER;
            prgTableHeading.Add(new Chunk("Repeaters", fntTableHead));
            document.Add(prgTableHeading);

            //Add line break
            document.Add(new Chunk("\n", fntTableHead));

            DataTable repeatertable = new DataTable();

            //Define columns
            repeatertable.Columns.Add("Repeater");
            repeatertable.Columns.Add("Status");
            repeatertable.Columns.Add("Uptime");
            repeatertable.Columns.Add("IP Address");

            ConcurrentDictionary<string, TcpServerConnection> Connections = RepeaterTCPServer.Instance.Connections;          
            foreach (var item in Connections)
            {
                    string AppId = item.Key;

                    TimeSpan timespan;
                    bool b = RepeaterTCPServer.Instance.ConnectionsTimespans.TryGetValue(AppId, out timespan);                
                    string Status = "Running";
                    string IpAddress = item.Value.IpAddress;
                    string Uptime = (!b) ?
                      "00:00:00:00" : timespan.ToString(@"dd\.hh\:mm\:ss");

                    repeatertable.Rows.Add(AppId, Status, Uptime, IpAddress);              
            }

            //Write the table
            PdfPTable table1 = new PdfPTable(repeatertable.Columns.Count);
            //Table header
            BaseFont btnColumnHeader = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader = new Font(btnColumnHeader, 12, 1, BaseColor.WHITE);
            Font fntRow = new Font(btnColumnHeader, 10, 1, BaseColor.BLACK);
                for (int i = 0; i < repeatertable.Columns.Count; i++)
            {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = BaseColor.GRAY;
                    Paragraph p = new Paragraph(repeatertable.Columns[i].ColumnName, fntColumnHeader);
                        p.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(p);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table1.AddCell(cell);
            }
            //table Data
            for (int i = 0; i < repeatertable.Rows.Count; i++)
            {
                for (int j = 0; j < repeatertable.Columns.Count; j++)
                {
                        PdfPCell cell = new PdfPCell();
                        Paragraph p = new Paragraph(repeatertable.Rows[i][j].ToString(), fntRow);
                        p.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(p);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table1.AddCell(cell);
                }
            }

            document.Add(table1);

            //Add line break
            document.Add(new Chunk("\n", fntHead));

            Paragraph prgTableHeading2 = new Paragraph();
            prgTableHeading2.Alignment = Element.ALIGN_CENTER;
            prgTableHeading2.Add(new Chunk("Users", fntTableHead));
            document.Add(prgTableHeading2);

            //Add line break
            document.Add(new Chunk("\n", fntTableHead));

            DataTable mobiletable = new DataTable();

            //Define columns
            mobiletable.Columns.Add("User");
            mobiletable.Columns.Add("Status");
            mobiletable.Columns.Add("Uptime");
            mobiletable.Columns.Add("Accepted Calls");

            ConcurrentDictionary<int, TcpServerConnection> Connections2 = MobileTCPServer.Instance.Connections;

            DateTime fromDate = DateTime.Now;
            fromDate = fromDate.AddDays(-1);

            DateTime toDate = DateTime.Now;

            foreach (var item in EmployeeFactory.GetInstance().GetAll())
            {
                TimeSpan timespan;
                bool b = MobileTCPServer.Instance.ConnectionsTimespans.TryGetValue(item.Id, out timespan);

                string Employee = item.Name;
                string Status = "Off-Line";
                string accepts = "" + CallData.GetInstance().GetDailyAcceptedCallsByEmployee(item.Id, fromDate, toDate).Count;
                string Uptime = (!b) ?
                  "00:00:00:00" : timespan.ToString(@"dd\.hh\:mm\:ss");

                if (Connections2.ContainsKey(item.Id))
                {
                     Status = "Running";
                }
                mobiletable.Rows.Add(Employee, Status, Uptime, accepts);
            }

            //Write the table
            PdfPTable table2 = new PdfPTable(mobiletable.Columns.Count);
               
            //Table header
            BaseFont btnColumnHeader2 = BaseFont.CreateFont(BaseFont.HELVETICA, BaseFont.CP1252, BaseFont.NOT_EMBEDDED);
            Font fntColumnHeader2 = new Font(btnColumnHeader2, 12, 1, BaseColor.WHITE);
            for (int i = 0; i < mobiletable.Columns.Count; i++)
            {
                    PdfPCell cell = new PdfPCell();
                    cell.BackgroundColor = BaseColor.GRAY;
                    Paragraph p = new Paragraph(mobiletable.Columns[i].ColumnName, fntColumnHeader2);
                    p.Alignment = Element.ALIGN_CENTER;
                    cell.AddElement(p);
                    cell.HorizontalAlignment = Element.ALIGN_CENTER;
                    cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    table2.AddCell(cell);
            }
              
                //table Data
            for (int i = 0; i < mobiletable.Rows.Count; i++)
            {
                for (int j = 0; j < mobiletable.Columns.Count; j++)
                {
                        PdfPCell cell = new PdfPCell();
                        Paragraph p = new Paragraph(mobiletable.Rows[i][j].ToString(), fntRow);
                        p.Alignment = Element.ALIGN_CENTER;
                        cell.AddElement(p);
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.VerticalAlignment = Element.ALIGN_MIDDLE;
                        table2.AddCell(cell);
                }
            }

            document.Add(table2);

            writer.CloseStream = false;
            document.Close();

            mem.Position = 0;

            //stream.Close();

             EmailNotification.SendAsync("Steward Call Daily Report: " + DateTime.Now.ToString("yyyy-MM-dd"), "", mem);

            }
            catch (Exception ee)
            {
                Console.WriteLine(ee.Message);
                Console.WriteLine(ee.StackTrace);
            }
        }

        private static void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            try { 
            if (lastDateTime == null)
            {
                sent = false;
            }
            else
            {
                if (lastDateTime.Day < DateTime.Now.Day)
                {
                    sent = false;
                }
                else
                {
                    sent = true;
                }
            }
            if (!sent)
            {
                //Send Email
                createReport();
                sent = true;
                lastDateTime = DateTime.Now;
            }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
