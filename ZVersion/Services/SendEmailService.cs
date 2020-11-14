using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MimeKit;
using Models.Model;
using System;
using System.Net;
using System.Net.Mail;

namespace ZVersion.Services
{
    public class SendEmailService
    {
        private readonly ILogger<SendEmailService> _logger;
        private readonly IConfiguration _configuration;

        public SendEmailService(ILogger<SendEmailService> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        public void SendEmailDefault()
        {
            try
            {
                MailMessage message = new MailMessage();
                message.IsBodyHtml = true;
                message.From = new MailAddress("andrewgopanchuk@gmail.com", "Це я Андрій");
                message.To.Add("andrewgopanchuk@gmail.com");
                message.Subject = "Сообщение System.Net.Mail";
                message.Body = "<div>Сообщение System.Net.Mail </div>";

                using (SmtpClient client = new SmtpClient("smtp.gmail.com"))
                {
                    string login = _configuration.GetSection("GoogleAccount:login").Value;
                    string password = _configuration.GetSection("GoogleAccount:password").Value;
                    client.Credentials = new NetworkCredential(login, password);
                    client.Port = 587;
                    client.EnableSsl = true;
                    client.Send(message);
                    _logger.LogInformation("Повідомлення успішно відправлено");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
            }
        }

        [Obsolete]
        public void SendEmailCustom(Appointment appointment, string fromTitle, string fromEmail, string subject)
        {
            try
            {
                MimeMessage message = new MimeMessage();
                message.From.Add(new MailboxAddress(fromTitle, fromEmail));
                message.To.Add(new MailboxAddress(appointment.Email.ToLower().ToString()));
                message.Subject = subject; //тема листа
                message.Body = new BodyBuilder()
                {
                    HtmlBody = appointment.Name + " ви записались на прийом до Гоголь Л.В." + appointment.DateWhenAdded
                }.ToMessageBody();

                using (MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient())
                {
                    string login = _configuration.GetSection("GoogleAccount:login").Value;
                    string password = _configuration.GetSection("GoogleAccount:password").Value;
                    client.Connect("smtp.gmail.com", 465, true);
                    client.Authenticate(login, password);
                    client.Send(message);
                    client.Disconnect(true);
                    _logger.LogInformation("Повідомлення відправлено");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.GetBaseException().Message);
            }
        }
    }
}
