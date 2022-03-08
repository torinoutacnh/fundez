#region	License
//--------------------------------------------------
// <License>
//     <Copyright> 2018 © Linh Nguyen </Copyright>
//     <Url> http://linhnguyen.net/ </Url>
//     <Author> Top </Author>
//     <Project> TIGE </Project>
//     <File>
//         <Name> SendGridEmailProvider.cs </Name>
//         <Created> 22/04/2018 1:10:23 PM </Created>
//         <Key> 1ae76e00-2518-4557-b62e-ee5e74f2571e </Key>
//     </File>
//     <Summary>
//         SendGridEmailProvider.cs is a part of TIGE
//     </Summary>
// <License>
//--------------------------------------------------
#endregion License

using System.Collections.Generic;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace TIGE.Core.EmailProvider.SendGridProvider
{
    public class SendGridEmailProvider : IEmailProvider
    {
        private readonly SendGridOptions _options;

        public SendGridEmailProvider(IOptions<SendGridOptions> configuration)
        {
            _options = configuration.Value;
        }

        public Task SendAsync(string email, string subject, string html, string ccEmail = null, CancellationToken cancellationToken = default)
        {
            var from = new EmailAddress(_options.DisplayEmail, _options.DisplayName);

            var to = new EmailAddress(email);


            var text = Regex.Replace(html, "<[^>]*>", string.Empty);

            var msg = MailHelper.CreateSingleEmail(from, to, subject, text, html);


            if (!string.IsNullOrWhiteSpace(ccEmail))
            {
                msg.AddCcs(new List<EmailAddress>()
                {
                    new EmailAddress(ccEmail)
                });
            }

            var client = new SendGridClient(_options.Key);

            return client.SendEmailAsync(msg, cancellationToken);
        }
    }
}