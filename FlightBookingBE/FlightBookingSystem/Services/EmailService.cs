using System.Net;
using System.Net.Mail;

namespace FlightBookingSystem.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            _logger.LogInformation("EmailService - SendEmailAsync - Sending email.");

            var smtpHost = _config["SmtpSettings:Host"];
            var smtpPort = _config["SmtpSettings:Port"];
            var smtpUsername = _config["SmtpSettings:Username"];
            var smtpPassword = _config["SmtpSettings:Password"];

            if (
                string.IsNullOrEmpty(smtpHost)
                || string.IsNullOrEmpty(smtpPort)
                || string.IsNullOrEmpty(smtpUsername)
                || string.IsNullOrEmpty(smtpPassword)
            )
            {
                _logger.LogError("SMTP configuration settings are missing or invalid.");
                throw new ArgumentNullException(
                    "SMTP configuration settings are missing or invalid."
                );
            }

            if (!int.TryParse(smtpPort, out int port))
            {
                _logger.LogError("Invalid SMTP port value.");
                throw new ArgumentException("Invalid SMTP port value.");
            }

            _logger.LogInformation(
                $"SMTP Host: {smtpHost}, Port: {port}, Username: {smtpUsername}"
            );

            using var client = new SmtpClient(smtpHost, port)
            {
                Credentials = new NetworkCredential(smtpUsername, smtpPassword),
                EnableSsl = true,
            };

            _logger.LogInformation("EmailService - SendEmailAsync - SmtpClient created.");
            var mailMessage = new MailMessage
            {
                From = new MailAddress(smtpUsername),
                Subject = subject,
                Body = body,
                IsBodyHtml = true,
            };

            _logger.LogInformation("EmailService - SendEmailAsync - MailMessage created.");
            mailMessage.To.Add(to);

            _logger.LogInformation("EmailService - SendEmailAsync - Sending email.");
            await client.SendMailAsync(mailMessage);
        }
    }
}