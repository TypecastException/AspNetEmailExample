using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace AspNetEmailExample.Models
{
    public partial class MailRecipient
    {
        public MailRecipient()
        {
            this.SentMails = new HashSet<SentMail>();
        }
        
        [Key]
        [Required]
        public int MailRecipientId { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string FirstName { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }

        public string FullName
        {
            get
            {
                return string.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }


        public DateTime? getLastEmailDate()
        {
            var top = (from m in this.SentMails
                       orderby m.SentDate descending
                       select m).Take(1);
            if (top.Count() > 0)
            {
                return top.ElementAt(0).SentDate;
            }
            else
            {
                return null;
            }
        }
    
        public virtual ICollection<SentMail> SentMails { get; set; }
    }
}
