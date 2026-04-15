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

        /// <summary>
        /// Gửi email xác nhận tài khoản
        /// </summary>
        public async Task SendMailConfirmAsync(string email, string confirmationLink)
        {
            var htmlContent = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <style>
                        body {{ font-family: Arial, sans-serif; background-color: #f4f4f4; }}
                        .container {{ max-width: 600px; margin: 0 auto; background-color: #ffffff; padding: 20px; border-radius: 8px; box-shadow: 0 2px 4px rgba(0,0,0,0.1); }}
                        .header {{ text-align: center; color: #333; margin-bottom: 20px; }}
                        .content {{ color: #666; line-height: 1.6; }}
                        .button-container {{ text-align: center; margin: 30px 0; }}
                        .confirm-button {{ background-color: #007bff; color: white; padding: 12px 30px; text-decoration: none; border-radius: 5px; display: inline-block; font-weight: bold; }}
                        .confirm-button:hover {{ background-color: #0056b3; }}
                        .warning {{ background-color: #fff3cd; border: 1px solid #ffc107; padding: 10px; border-radius: 5px; margin: 20px 0; color: #856404; }}
                        .footer {{ text-align: center; color: #999; font-size: 12px; margin-top: 30px; border-top: 1px solid #ddd; padding-top: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>Xác Thực Địa Chỉ Email</h2>
                        </div>
                        <div class='content'>
                            <p>Xin chào,</p>
                            <p>Cảm ơn bạn đã đăng ký tài khoản Movie Application. Vui lòng xác thực địa chỉ email của bạn bằng cách nhấp vào nút dưới đây:</p>
                            
                            <div class='button-container'>
                                <a href='{confirmationLink}' class='confirm-button'>Xác Thực Email</a>
                            </div>

                            <p>Hoặc sao chép link này vào trình duyệt của bạn:</p>
                            <p style='word-break: break-all; background-color: #f5f5f5; padding: 10px; border-radius: 5px;'>{confirmationLink}</p>

                            <div class='warning'>
                                <strong>Lưu ý:</strong> Link xác thực này sẽ hết hạn sau 24 giờ. Nếu link đã hết hạn, vui lòng đăng nhập và yêu cầu email xác thực mới.
                            </div>

                            <p>Nếu bạn không yêu cầu tạo tài khoản này, vui lòng bỏ qua email này.</p>
                        </div>
                        <div class='footer'>
                            <p>© 2024 Movie Application. Bản quyền được bảo vệ.</p>
                        </div>
                    </div>
                </body>
                </html>";

            var message = new Message(
                to: new[] { email },
                subject: "Xác Thực Địa Chỉ Email - Movie Application",
                content: htmlContent
            );

            await SendEmail(message);
        }

        public Task SendMailForgotPassWord(string email, string token)
        {
            throw new NotImplementedException();
        }
    }
}
