using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sci.Models;
using sci.Util;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.AspNet.SignalR;
using Program;

namespace sci.Controllers
{
    public class MailingController : Controller
    {
        private sci_newEntities db = new sci_newEntities();
        // GET: Mailing
        public ActionResult Index()
        {
            return View(db.Posta_Prototipi.ToList());
        }
        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult ExampleDemo()
        {
            Thread.Sleep(1000);
            string status = "Task Completed Successfully";
            return Json(status, JsonRequestBehavior.AllowGet);
        }

        // GET: Mailing/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posta_Prototipi posta_prototipi = db.Posta_Prototipi.Find(id);
            if (posta_prototipi == null)
            {
                return HttpNotFound();
            }

            return View(posta_prototipi);
        }

        // POST: Mailing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codice,CodiceConn,Sender,NotificationTO,ReceiptTo,Subject,Header,Message,Footer,CC,BCC,emailtest,Allegati,Azione,FullMessage")] Posta_Prototipi posta_prototipi, string submit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(posta_prototipi).State = EntityState.Modified;
                db.SaveChanges();
                switch (submit)
                {
                    case "Salva":
                        return RedirectToAction("Edit", "Mailing", new { id = posta_prototipi.Codice });
                    case "SendMail":
                       return RedirectToAction("SendMail","Mailing", new { id = posta_prototipi.Codice});
                    default:
                        throw new Exception();
                }
                return RedirectToAction("Index");
            }

            return View(posta_prototipi);
        }

        // POST: Mailing/SendMail/5

        public JsonResult LongRunningProcess(int? id)
        {

            Posta_Prototipi posta_prototipi = db.Posta_Prototipi.Find(id);
            TabPosta tabposta = db.TabPosta.Find(posta_prototipi.CodiceConn);

            int itemsCount = posta_prototipi.Posta_Messaggi.Count();


            foreach (var item in posta_prototipi.Posta_Messaggi) {
                var x = item.Nom;
            }


            for (int i = 0; i <= itemsCount; i++)
            {

                var body = posta_prototipi.Message;
                var message = new MailMessage();
                // SEND MAIL
                message.To.Add(new MailAddress(posta_prototipi.emailtest));  // replace with valid value 
                message.From = new MailAddress(posta_prototipi.Sender);  // replace with valid value
                message.Bcc.Add(new MailAddress(posta_prototipi.Sender));
                message.Subject = posta_prototipi.Subject;

                message.Body = string.Format(body, posta_prototipi.Subject, posta_prototipi.Header, posta_prototipi.Message, posta_prototipi.Footer);
                message.IsBodyHtml = true;
                message.Sender = new MailAddress(posta_prototipi.Sender); ;

                var smtp = new SmtpClient();
                smtp.EnableSsl = true;
                var credential = new NetworkCredential
                {
                    UserName = tabposta.SendUsername,
                    Password = tabposta.SendPassword
                };
                smtp.UseDefaultCredentials = false;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Credentials = credential;
                //smtp.Host = "smtp.gmail.com";
                smtp.Host = tabposta.smtpServer;
                smtp.Port = (int)tabposta.smtpServerPort;
                smtp.Send(message);

                //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
                Functions.SendProgress("Process in progress...", i, itemsCount);
            }

            return Json("", JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> SendMail(int? id)

        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Posta_Prototipi posta_prototipi = db.Posta_Prototipi.Find(id);
            if (posta_prototipi == null)
            {
                return HttpNotFound();
            }

            TabPosta tabposta = db.TabPosta.Find(posta_prototipi.CodiceConn);
            if (tabposta == null)
            {
                return HttpNotFound();
            }

            // SEND MAIL
            var body = posta_prototipi.Message;
            var message = new MailMessage();
            message.To.Add(new MailAddress(posta_prototipi.emailtest));  // replace with valid value 
            message.From = new MailAddress(posta_prototipi.Sender);  // replace with valid value
            message.Bcc.Add(new MailAddress(posta_prototipi.Sender));
            message.Subject = posta_prototipi.Subject;
                
            message.Body = string.Format(body, posta_prototipi.Subject, posta_prototipi.Header, posta_prototipi.Message, posta_prototipi.Footer);
            message.IsBodyHtml = true;
            message.Sender = new MailAddress(posta_prototipi.Sender); ;

            var smtp = new SmtpClient();
            smtp.EnableSsl = true;
            var credential = new NetworkCredential
            {
                UserName = tabposta.SendUsername,
                Password = tabposta.SendPassword
            };
            smtp.UseDefaultCredentials = false;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Credentials = credential;
            //smtp.Host = "smtp.gmail.com";
            smtp.Host = tabposta.smtpServer;
            smtp.Port = (int)tabposta.smtpServerPort;

            await smtp.SendMailAsync(message);
                        

            return RedirectToAction("Edit", "Mailing", new { id = posta_prototipi.Codice });
        }
    }
}