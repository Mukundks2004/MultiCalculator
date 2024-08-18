using MultiCalculator.Database.Models;
using ceTe.DynamicPDF;
using ceTe.DynamicPDF.PageElements;
using System.Net.Mail;
using System.Net;
using System.ComponentModel.DataAnnotations;

namespace MultiCalculator.Helpers
{
    public class PdfWriterHelper
    {
        private Document GeneratePdf(List<(string, string)> questionAndAnswer, UserModel user)
        {
            var document = new Document();
            var listOfPages = new List<Page>();
            for (var i = 0; i < (questionAndAnswer.Count / 5 + 1) * 2; i++)
            {
                var page = new Page(PageSize.Letter, PageOrientation.Portrait, 54.0f);
                listOfPages.Add(page);
                document.Pages.Add(page);
            }
            StarterPageInformation(listOfPages[0], user);
            PdfWriter(questionAndAnswer, listOfPages);

            return document;
        }

        public string GenerateQuestionSheet(List<(string, string)> questionAndAnswer, UserModel user, bool sendToEmail = false, string email = null)
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
                SendQuestionSheetToEmail(user, filePath, email);
            }
            return filePath;
        }

        private void PdfWriter(List<(string, string)> questionAndAnswer, List<Page> pages)
        {
            var answerStart = questionAndAnswer.Count / 5 + 1;
            for (var i = 0; i < questionAndAnswer.Count; i++)
            {
                pages[i/5].Elements.Add(new Label($"Question {i + 1}:\t\t{questionAndAnswer[i].Item1}", 0, 40 + (i%5 != 0 ? (700/5) * (i%5) : 0), 504, 100, Font.Helvetica, 16, TextAlign.Left));
                pages[i/5].Elements.Add(new Label($"Answer {i + 1}:", 0, 90 + (i % 5 != 0 ? (700 / 5)*(i%5) : 0), 504, 100, Font.Helvetica, 16, TextAlign.Left));
                pages[i/5 + answerStart].Elements.Add(new Label($"Answer {i + 1}:\t\t{questionAndAnswer[i].Item2}", 0, 40 + (i % 5 != 0 ? (700 / 5) * (i % 5) : 0), 504, 100, Font.Helvetica, 16, TextAlign.Left));

                if (i % 5 == 0)
                {
                    pages[i / 5].Elements.Add(new Label($"Page {i / 5 + 1}", 0, 0, 504, 100, Font.Helvetica, 16, TextAlign.Right));
                    pages[i / 5].Elements.Add(new Label("By MDM Calculator", 0, 670, 504, 100, Font.Helvetica, 16, TextAlign.Right));
                    pages[i / 5 + answerStart].Elements.Add(new Label($"Page {i / 5 + 1}", 0, 0, 504, 100, Font.Helvetica, 16, TextAlign.Right));
                    pages[i / 5 + answerStart].Elements.Add(new Label("By MDM Calculator", 0, 670, 504, 100, Font.Helvetica, 16, TextAlign.Right));
                }
            }
        }

        private void StarterPageInformation(Page page, UserModel user)
        {
            Label creatingUserLabel = new Label($"{user.FirstName} {user.LastName}'s Question Sheet! ({user.AmountOfGeneratedPdfs})", 0, 0, 504, 100, Font.Helvetica, 18, TextAlign.Center);
            page.Elements.Add(creatingUserLabel);
        }

        private void SendQuestionSheetToEmail(UserModel user, string filePath, string email = null)
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
                Body = GenerateDynamicEmailBody(user),
				IsBodyHtml = true,
            };

            System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(filePath);
            mailMessage.Attachments.Add(attachment);
            mailMessage.To.Add(email ?? user.Email);

            try
            {
                client.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            } // Catches any errors to prevent the entire program from stopping.
        }

		private string GenerateDynamicEmailBody(UserModel user)
		{
			return $@"
<!DOCTYPE html>
<html lang='en'>
<head>
    <meta charset='UTF-8'>
    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .email-container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            padding: 20px;
            border-radius: 10px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            background-color: #007bff;
            color: #ffffff;
            padding: 10px 20px;
            border-radius: 10px 10px 0 0;
            text-align: center;
        }}
        .content {{
            padding: 20px;
            line-height: 1.6;
        }}
        .content p {{
            margin: 10px 0;
        }}
        .footer {{
            margin-top: 20px;
            text-align: center;
            font-size: 12px;
            color: #777777;
        }}
        .footer strong {{
            color: #333333;
        }}
    </style>
</head>
<body>
    <div class='email-container'>
        <div class='header'>
            <h1>MDM Calculator</h1>
        </div>
        <div class='content'>
            <p>Dear {user.FirstName} {user.LastName},</p>
            <p>Here is the question sheet you generated. Please preview attached documents.</p>
            <p>Kind Regards,</p>
            <p><strong>MDM Calculator Team</strong></p>
        </div>
        <div class='footer'>
            <p><strong>PLEASE DO NOT REPLY.</strong> THIS IS AN AUTOGENERATED EMAIL.</p>
        </div>
    </div>
</body>
</html>";
		}
	}
}
