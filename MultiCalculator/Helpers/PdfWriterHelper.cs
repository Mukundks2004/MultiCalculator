﻿using MultiCalculator.Database.Models;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using System.Net.Mail;
using System.Net;

namespace MultiCalculator.Helpers
{
    public class PdfWriterHelper
    {
        private Document GeneratePdf(List<(string, string)> questionAndAnswer, UserModel user)
        {
            var document = new Document();
            var listOfPages = new List<Page>();
            for (var i = 0; i < questionAndAnswer.Count / 5; i++)
            {
                var page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
                listOfPages.Add(page);
                document.Pages.Add(page);
            }
            StarterPageInformation(listOfPages[0], user);
            PdfWriter(questionAndAnswer, listOfPages);

            return document;
        }

        public string GenerateQuestionSheet(List<(string, string)> questionAndAnswer, UserModel user, bool sendToEmail = false)
        {
            var questionSheet = GeneratePdf(questionAndAnswer, user);
            var path = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var fileName = user.FirstName + user.LastName + "QuestionSheet" + user.AmountOfGeneratedPdfs;
            var filePath = path + "\\" + fileName + ".pdf";
            user.AmountOfGeneratedPdfs++;
            user.GeneratedPdfLocations.Add(filePath);

            questionSheet.Draw(filePath);

            if (sendToEmail)
            {
                SendQuestionSheetToEmail(user, filePath);
            }
            return filePath;
        }

        private void PdfWriter(List<(string, string)> questionAndAnswer, List<Page> pages)
        {
            for (var i = 0; i < questionAndAnswer.Count; i++)
            {
                pages[i/5].Elements.Add(new Label($"Question {i + 1}:\t\t{questionAndAnswer[i].Item1}", 0, 40 + (i%5 != 0 ? (700/5) * (i%5) : 0), 504, 100, Font.Helvetica, 16, TextAlign.Left));
                pages[i/5].Elements.Add(new Label($"Answer {i + 1}:\t\t{questionAndAnswer[i].Item2}", 0, 90 + (i % 5 != 0 ? (700 / 5)*(i%5) : 0), 504, 100, Font.Helvetica, 16, TextAlign.Left));
                if (i % 5 == 0)
                {
                    pages[i / 5].Elements.Add(new Label($"Page {i / 5 + 1}", 0, 0, 504, 100, Font.Helvetica, 16, TextAlign.Left));
                    pages[i / 5].Elements.Add(new Label("By MDM Calculator", 0, 670, 504, 100, Font.Helvetica, 16, TextAlign.Right));
                }
            }
        }

        private void StarterPageInformation(Page page, UserModel user)
        {
            Label creatingUserLabel = new Label($"{user.FirstName} {user.LastName}'s Question Sheet! ({user.AmountOfGeneratedPdfs})", 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(creatingUserLabel);
        }

        private void SendQuestionSheetToEmail(UserModel user, string filePath)
        {
            string fromEmail = "dennishsuhospitalmanagesystem@gmail.com";
            string password = "kwfhmblprldwwxbc";

            SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
            {
                Credentials = new NetworkCredential(fromEmail, password),
                EnableSsl = true
            };

            MailMessage mailMessage = new MailMessage()
            {
                From = new MailAddress(fromEmail),
                Subject = $"Question Sheet - {user.FirstName} {user.LastName}",
                Body = $"Dear {user.FirstName} {user.LastName},\n\nHere is the question sheet you generated. Please preview attached documents.\n\nThank You,\nMDM Calculator",
                IsBodyHtml = false,
            };

            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(filePath);
            mailMessage.Attachments.Add(attachment);
            mailMessage.To.Add(user.Email);

            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            } // Catches any errors to prevent the entire program from stopping.
        }
    }
}
