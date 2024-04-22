using JobSeeking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace JobSeeking.Ultility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var mail = "namvhgcd210429@fpt.edu.vn";
            var pw = "0833458845"; 
            var client = new SmtpClient("smtp.gmail.com", 587) 
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(mail, pw)
            };

            var mailMessage = new MailMessage(from: mail, to: email)
            {
                Subject = subject,
                Body = message,
                IsBodyHtml = true 
            };

            return client.SendMailAsync(mailMessage);
        }

       
    }
}
