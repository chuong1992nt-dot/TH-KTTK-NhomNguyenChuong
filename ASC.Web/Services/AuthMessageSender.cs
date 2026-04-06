using System.Threading.Tasks;
using ASC.Web.Configuration;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace ASC.Web.Services
{
    public class AuthMessageSender : ASC.Web.Services.IEmailSender, Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, ISmsSender
    {
        private readonly EmailSettings _emailSettings;

        // Constructor để gọi cấu hình EmailSettings từ appsettings.json
        public AuthMessageSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            // Code dùng MailKit để đóng gói và gửi Email
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
            mimeMessage.To.Add(new MailboxAddress("", email)); // Gửi đến email người dùng đăng ký
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder { HtmlBody = message };
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            // Kết nối, đăng nhập và gửi qua server Gmail
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_emailSettings.SenderEmail, _emailSettings.SenderPassword);
            await client.SendAsync(mimeMessage);
            await client.DisconnectAsync(true);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // SMS tạm thời chưa học tới, cứ để Task.FromResult(0) để không bị lỗi
            return Task.FromResult(0);
        }
    }
}