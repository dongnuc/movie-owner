using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Movie_StructureCode.Application.Abstractions.Services.Email;
using Movie_StructureCode.Contract.Emails;

namespace Movie_StructureCode.Infracstructure.Services.Email
{
    /// <summary>
    /// Email Service Implementation - triển khai các phương thức gửi email
    /// </summary>
    public sealed class EmailServices : IEmailServices
    {
        private readonly EmailConfiguration _emailConfig;

        public EmailServices(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            var builder = new BodyBuilder
            {
                HtmlBody = message.Content
            };

            emailMessage.Body = builder.ToMessageBody();
            return emailMessage;
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_emailConfig.SmtpServer, _emailConfig.Port, SecureSocketOptions.StartTls, CancellationToken.None);
                await client.AuthenticateAsync(_emailConfig.UserName, _emailConfig.Password, CancellationToken.None);
                await client.SendAsync(mailMessage, CancellationToken.None);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
                throw;
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }
        }

        public async Task SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            await SendAsync(emailMessage);
        }

        public Task SendMailConfirmAsync(string email, string urlUser)
        {
            throw new NotImplementedException();
        }

        public Task SendMailForgotPassWord(string email, string token)
        {
            throw new NotImplementedException();
        }
    }
}
