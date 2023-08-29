

using Hangfier.Domain.Email;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Hangfier.Irepo.Email
{
    public interface IMailService
    {
        public Task SendEmailAsync(MailRequest mailRequest);
        public Task SendEmailAsyncFromAPath(MailRequest mailRequest);
        
    }
}
