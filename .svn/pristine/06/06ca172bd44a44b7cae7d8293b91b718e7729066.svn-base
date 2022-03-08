#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> GmailProvider.cs </Name>
//         <Created> 22/04/2018 8:55:18 PM </Created>
//         <Key> 82c088db-f815-466b-9509-c20b76844838 </Key>
//     </File>
//     <Summary>
//         GmailProvider.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.Net;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Threading;
using System.Threading.Tasks;
using TIGE.Core.Utils;

namespace TIGE.Core.EmailProvider.GmailProvider
{
    public class GmailProvider : IEmailProvider
    {
        private readonly GmailOptions _options;

        public GmailProvider(IOptions<GmailOptions> configuration)
        {
            _options = configuration.Value;
        }

        public async Task SendAsync(string email, string subject, string html, string ccEmail = null, CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(email))
            {
                string mainEmail = _options.UserName;
                string mainEmailPass = _options.Password;
                string mainEmailTitle = _options.DisplayName;

                var emailMessage = new MimeMessage();

                emailMessage.From.Add(new MailboxAddress(mainEmailTitle, mainEmail));

                emailMessage.To.Add(new MailboxAddress(email, email));

                if (subject.Contains("Buy Token") ||
                    subject.Contains("Verify Withdraw") ||
                    subject.Contains("Verify Sell Token") ||
                    subject.Contains("Verify Deposit"))
                {
                    emailMessage.Cc.Add(new MailboxAddress(SystemHelper.Setting.NotifyEmail, SystemHelper.Setting.NotifyEmail));
                }

                emailMessage.Subject = subject;

                emailMessage.Body = new TextPart("html")
                {
                    Text = html
                };

                using (var client = new SmtpClient())
                {

                    await client.ConnectAsync("smtp.gmail.com", 465, true, cancellationToken).ConfigureAwait(false);
                    await client.AuthenticateAsync(mainEmail, mainEmailPass, cancellationToken).ConfigureAwait(false);

                    var check = client.IsSecure;
                    await client.SendAsync(emailMessage, cancellationToken).ConfigureAwait(true);

                    //await client.ConnectAsync("smtpout.secureserver.net", 465, true, cancellationToken).ConfigureAwait(false);
                    //await client.AuthenticateAsync(mainEmail, mainEmailPass, cancellationToken).ConfigureAwait(false);

                    //var check = client.IsSecure;
                    //await client.SendAsync(emailMessage, cancellationToken).ConfigureAwait(true);
                }
            }
        }
    }
}