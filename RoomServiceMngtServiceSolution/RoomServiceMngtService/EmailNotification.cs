using RoomServiceMngtService.Common;
using RoomServiceMngtService.Factories;
using RoomServiceMngtService.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace RoomServiceMngtService
{
    public class EmailNotification
    {
        public static void SendAsync(string subject, string body)
        {
            try { 
            SmtpClient client = new SmtpClient(EmailSettings.SMTP_HOST, EmailSettings.PORT);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = false;
            //client.EnableSsl = true; // True for Gmail
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(EmailSettings.USERNAME, EmailSettings.PASSWORD);

            MailAddress fromAddress = new MailAddress(EmailSettings.FROM_ADDRESS, EmailSettings.DISPLAY_NAME + (char)0xD8, System.Text.Encoding.UTF8);

            MailMessage message = new MailMessage();
            message.From = fromAddress;

                List<Email> emailList = EmailFactory.GetInstance().GetAll();
            foreach (var item in emailList)
            {
                message.To.Add(item.EmailAddress);
            }

            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            client.SendAsync(message, "Notification");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public static void SendAsync(string subject, string body, MemoryStream attachmentStream)
        {
            try { 
            SmtpClient client = new SmtpClient(EmailSettings.SMTP_HOST, EmailSettings.PORT);
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.EnableSsl = false;
            //client.EnableSsl = true; // True for Gmail
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(EmailSettings.USERNAME, EmailSettings.PASSWORD);

            MailAddress fromAddress = new MailAddress(EmailSettings.FROM_ADDRESS, EmailSettings.DISPLAY_NAME + (char)0xD8, System.Text.Encoding.UTF8);

            MailMessage message = new MailMessage();
            message.From = fromAddress;

            foreach (var item in EmailSettings.TO_LIST)
            {
                message.To.Add(item);
            }

            message.Body = body;
            message.BodyEncoding = System.Text.Encoding.UTF8;
            message.Subject = subject;
            message.SubjectEncoding = System.Text.Encoding.UTF8;

            attachmentStream.Position = 0;
            Attachment attachment = new Attachment(attachmentStream, "Attachment.pdf");
            message.Attachments.Add(attachment);

            client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);
            client.SendAsync(message, "Notification");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void SendCompletedCallback(object sender, AsyncCompletedEventArgs e)
        {
            // Get the unique identifier for this asynchronous operation.
            String token = (string)e.UserState;

            if (e.Cancelled)
            {
                Console.WriteLine("Send canceled." + token);
                //status = "Send canceled." + token;
            }
            if (e.Error != null)
            {
                Console.WriteLine("ERROR " + token + " " + e.Error.ToString());
                //status = "ERROR " + token + " " + e.Error.ToString();
            }
            else
            {
                Console.WriteLine("Message sent - " + token);
                //status = "Message sent - " + token;
            }
        }
    }
}
