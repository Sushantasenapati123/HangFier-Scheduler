using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hangfier.Domain.Email
{
    public class MailRequest
    {
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
      

        public IFormFile Attachment { get; set; }
        public byte[] AttachmentBytes { get; set; }
    }
}
//  public List<IFormFile> Attachments { get; set; }
