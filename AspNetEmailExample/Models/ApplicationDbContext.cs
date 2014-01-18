using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace AspNetEmailExample.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection2")
        {

        }

        public System.Data.Entity.DbSet<AspNetEmailExample.Models.MailRecipient> MailRecipients { get; set; }
        public System.Data.Entity.DbSet<AspNetEmailExample.Models.SentMail> SentMails { get; set; }
    }
}