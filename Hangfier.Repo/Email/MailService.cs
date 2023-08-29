using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

using System.IO;
using System.Threading.Tasks;
using System.Net.Mail;
using Hangfier.Domain.Email;
using Hangfier.Irepo.Email;
using Org.BouncyCastle.Crypto.Macs;
using Microsoft.Extensions.Configuration;

namespace Hangfier.Repo.Email

{
    public class MailService : IMailService
    {
       
        IConfigurationRoot configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json")
             .Build();
        public async Task SendEmailAsync(MailRequest mailRequest)
        {
            //mailRequest.Attachment = Microsoft.AspNetCore.Http.FormFile;
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(configuration["MailSettings:Mail"]);
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;

            if (mailRequest.Attachment != null && mailRequest.Attachment.Length > 0)
            {
                using (var ms = new MemoryStream())
                {
                    await mailRequest.Attachment.CopyToAsync(ms);
                    var fileBytes = ms.ToArray();
                    builder.Attachments.Add(mailRequest.Attachment.FileName, fileBytes, ContentType.Parse(mailRequest.Attachment.ContentType));
                }
            }


            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(configuration["MailSettings:Host"], int.Parse(configuration["MailSettings:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration["MailSettings:Mail"], configuration["MailSettings:Password"]);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }

        public async Task SendEmailAsyncFromAPath(MailRequest mailRequest)
        {
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(configuration["MailSettings:Mail"]);//_mailSettings.Mail
            email.To.Add(MailboxAddress.Parse(mailRequest.ToEmail));
            email.Subject = mailRequest.Subject;

            var builder = new BodyBuilder();
            builder.HtmlBody = mailRequest.Body;

            if (mailRequest.AttachmentBytes != null && mailRequest.AttachmentBytes.Length > 0)
            {
                using (var ms = new MemoryStream(mailRequest.AttachmentBytes))
                {
                    var fileBytes = ms.ToArray();
                   // builder.Attachments.Add("pro1.jpg", fileBytes, ContentType.Parse("image/jpeg"));
                    builder.Attachments.Add("pro1.pdf", fileBytes, ContentType.Parse("image/pdf"));
                }
            }

            email.Body = builder.ToMessageBody();
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(configuration["MailSettings:Host"], int.Parse(configuration["MailSettings:Port"]), SecureSocketOptions.StartTls);
            smtp.Authenticate(configuration["MailSettings:Mail"], configuration["MailSettings:Password"]);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
        }
    }
}
