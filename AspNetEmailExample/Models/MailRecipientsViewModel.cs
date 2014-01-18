using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AspNetEmailExample.Models
{
    public class MailRecipientsViewModel
    {
        public List<SelectRecipientEditorViewModel> MailRecipients { get; set; }
        public MailRecipientsViewModel()
        {
            this.MailRecipients = new List<SelectRecipientEditorViewModel>();
        }


        public IEnumerable<int> getSelectedRecipientIds()
        {
            return (from r in this.MailRecipients 
                    where r.Selected 
                    select r.MailRecipientId).ToList();
        }
    }
}