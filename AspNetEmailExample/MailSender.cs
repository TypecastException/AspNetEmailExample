using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AspNetEmailExample.Models;
using System.Net.Mail;
using System.Threading.Tasks;

namespace AspNetEmailExample
{
    public class MailSender
    {
        public IEnumerable<SentMail> SendMail(IEnumerable<Message> mailMessages)
        {
            var output = new List<SentMail>();

            // Modify this to suit your business case:
            string mailUser = "yourAddress@maildomain.com";
            string mailUserPwd = "yourPassword";
            SmtpClient client = new SmtpClient("smptp.yourMailHost.com");
            client.Port = 587;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credentials = new System.Net.NetworkCredential(mailUser, mailUserPwd);
            client.EnableSsl = true;
            client.Credentials = credentials;

            foreach (var msg in mailMessages)
            {
                var mail = new MailMessage(msg.User.Email.Trim(), msg.Recipient.Email.Trim());
                mail.Subject = msg.Subject;
                mail.Body = msg.MessageBody;

                try
                {
                    client.Send(mail);
                    var sentMessage = new SentMail()
                    {
                        MailRecipientId = msg.Recipient.MailRecipientId,
                        SentToMail = msg.Recipient.Email,
                        SentFromMail = msg.User.Email,
                        SentDate = DateTime.Now
                    };
                    output.Add(sentMessage);
                }
                catch (Exception ex)
                {
                    throw ex;
                    // Or, more likely, do some logging or something
                }
            }
            return output;
        }
    }
}