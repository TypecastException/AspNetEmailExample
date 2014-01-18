using AspNetEmailExample.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Threading;

namespace AspNetEmailExample.Controllers
{
    public class MailRecipientsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [Authorize]
        public async Task<ActionResult> Index()
        {
            var model = new MailRecipientsViewModel();

            // Get a list of all the recipients:
            var recipients = await db.MailRecipients.ToListAsync();
            foreach(var item in recipients)
            {
                // Put the relevant data into the ViewModel:
                var newRecipient = new SelectRecipientEditorViewModel()
                {
                    MailRecipientId = item.MailRecipientId,
                    FullName = item.FullName,
                    Company = item.Company,
                    Email = item.Email,
                    LastMailedDate = item.getLastEmailDate().HasValue ? item.getLastEmailDate().Value.ToShortDateString() : "",
                    Selected = true
                };

                // Add to the list contained by the "wrapper" ViewModel:
                model.MailRecipients.Add(newRecipient);
            }
            // Pass to the view and return:
            return View(model);
        }

        [Authorize]
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailRecipient mailrecipient = await db.MailRecipients.FindAsync(id);
            if (mailrecipient == null)
            {
                return HttpNotFound();
            }
            return View(mailrecipient);
        }

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="LastName,FirstName,Email,Company")] MailRecipient mailrecipient)
        {
            if (ModelState.IsValid)
            {
                db.MailRecipients.Add(mailrecipient);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(mailrecipient);
        }


        [Authorize]
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailRecipient mailrecipient = await db.MailRecipients.FindAsync(id);
            if (mailrecipient == null)
            {
                return HttpNotFound();
            }
            return View(mailrecipient);
        }


        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include=
            "MailRecipientId,LastName,FirstName,Email,Company")] 
            MailRecipient mailrecipient)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mailrecipient).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(mailrecipient);
        }


        [Authorize]
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MailRecipient mailrecipient = await db.MailRecipients.FindAsync(id);
            if (mailrecipient == null)
            {
                return HttpNotFound();
            }
            return View(mailrecipient);
        }


        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            MailRecipient mailrecipient = await db.MailRecipients.FindAsync(id);
            db.MailRecipients.Remove(mailrecipient);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }


        [HttpPost]
        [Authorize]
        public ActionResult SendMail(MailRecipientsViewModel recipients)
        {
            // Retrieve the ids of the recipients selected:
            var selectedIds = recipients.getSelectedRecipientIds();

            // Grab the recipient records:
            var selectedMailRecipients = this.LoadRecipientsFromIds(selectedIds);

            // Build the message container for each:
            var messageContainers = this.createRecipientMailMessages(selectedMailRecipients);

            // Send the mail:
            var sender = new MailSender();
            var sent = sender.SendMail(messageContainers);

            // Save a record of each mail sent:
            this.SaveSentMail(sent);

            // Reload the index form:
            return RedirectToAction("Index");
        }


        IEnumerable<MailRecipient> LoadRecipientsFromIds(IEnumerable<int> selectedIds)
        {
            var selectedMailRecipients = from r in db.MailRecipients
                                         where selectedIds.Contains(r.MailRecipientId)
                                         select r;
            return selectedMailRecipients;
        }


        IEnumerable<Message> createRecipientMailMessages(IEnumerable<MailRecipient> selectedMailRecipients)
        {
            var messageContainers = new List<Message>();
            var currentUser = db.Users.Find(User.Identity.GetUserId());
            foreach (var recipient in selectedMailRecipients)
            {
                var msg = new Message()
                {
                    Recipient = recipient,
                    User = currentUser,
                    Subject = string.Format("Welcome, {0}", recipient.FullName),
                    MessageBody = this.getMessageText(recipient, currentUser)
                };
                messageContainers.Add(msg);
            }
            return messageContainers;
        }


        void SaveSentMail(IEnumerable<SentMail> sentMessages)
        {
            foreach (var sent in sentMessages)
            {
                db.SentMails.Add(sent);
                db.SaveChanges();
            }
        }


        string getMessageText(MailRecipient recipient, ApplicationUser user)
        {
            return ""
            + string.Format("Dear {0}, ", recipient.FullName) + Environment.NewLine
            + "Thank you for your interest in our latest product. Please feel free to contact me for more information!"
            + Environment.NewLine
            + Environment.NewLine
            + "Sincerely, "
            + Environment.NewLine
            + string.Format("{0} {1}", user.FirstName, user.LastName);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
