using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Account.Business.Helpers.Interfaces;
using Account.Business.Utils;
using Account.Dto.Shared;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace Account.Business.Helpers
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _emailSettings;

        public EmailService(IOptions<MailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        /// <inheritdoc/>
        public async Task SendVerificationAsync(string to, string link, CancellationToken cancellationToken, string from = null)
        {
            var email = new MimeMessage();
            var bodyBuilder = new BodyBuilder();

            using (StreamReader SourceReader = System.IO.File.OpenText(@"H:\Akroma\AccountService\AccountService\Account.Business\Assets\Html\verify-email.html"))
            {
                bodyBuilder.HtmlBody = SourceReader.ReadToEnd();
            }

            email.From.Add(MailboxAddress.Parse(from ?? _emailSettings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = Constants.VerificationSubject;
            email.Body = bodyBuilder.ToMessageBody();

            await SendEmailAsync(email, cancellationToken);
        }

        /// <inheritdoc/>
        public async Task SendResetCodeAsync(string to, string code, CancellationToken cancellationToken, string from = null)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from ?? _emailSettings.From));
            email.To.Add(MailboxAddress.Parse(to));
            email.Subject = Constants.ResetPasswordSubject;
            //email.Body = new TextPart(TextFormat.Html) { Text = html };
            email.Body = new TextPart(TextFormat.Html) { Text = code};

            await SendEmailAsync(email, cancellationToken);
        }

        private async Task SendEmailAsync(MimeMessage email, CancellationToken cancellationToken)
        {
            using var smtp = new SmtpClient();
            smtp.CheckCertificateRevocation = false;
            smtp.Connect(_emailSettings.Host, _emailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSettings.UserName, _emailSettings.Password);
            await smtp.SendAsync(email, cancellationToken);
            smtp.Disconnect(true);
        }
    }
}
