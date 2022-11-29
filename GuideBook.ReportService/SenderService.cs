using System.Net;
using System.Net.Mail;

namespace GuideBook.ReportService;

public class SenderService
{
    public static void SendMail(string filePath,string email)
    {
        var sc = new SmtpClient
        {
            Port = 587,
            Host = "smtp.gmail.com",
            EnableSsl = true,
            Credentials = new NetworkCredential("rise.assesment@gmail.com", "123")
        };

        var mail = new MailMessage
        {
            Subject = "Contact Report",
            IsBodyHtml = true,
            From = new MailAddress("rise.assesment@gmail.com", "Harun Aydin"),
            Body = "Contact Report Content"
        };

        mail.To.Add(email);         
        mail.Attachments.Add(new Attachment(filePath));

        sc.Send(mail);
    }
}
