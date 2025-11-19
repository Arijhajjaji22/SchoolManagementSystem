using System.Net.Mail;
using System.Net;

namespace ProjetBrima.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpClient = new SmtpClient(_configuration["SmtpConfig:Host"])
            {
                Port = int.Parse(_configuration["SmtpConfig:Port"]),
                Credentials = new NetworkCredential(
                    _configuration["SmtpConfig:Username"],
                    _configuration["SmtpConfig:Password"]),
                EnableSsl = true
            };

            var fromAddress = _configuration["SmtpConfig:From"];
            var mailMessage = new MailMessage
            {
                From = new MailAddress(fromAddress),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            // ✅ Vérification de l'email destinataire
            if (!MailAddress.TryCreate(toEmail, out var toAddress))
                throw new FormatException("Adresse email de destination invalide : " + toEmail);

            mailMessage.To.Add(toAddress);

            await smtpClient.SendMailAsync(mailMessage);
        }

    }

}
