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
using System.Linq.Dynamic;

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
            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro");
            return View(posta_prototipi);
        }

        // POST: Mailing/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codice,CodiceConn,Sender,NotificationTO,ReceiptTo,Subject,Header,Message,Footer,CC,BCC,emailtest,Allegati,Azione,FullMessage,IDFiltro")] Posta_Prototipi posta_prototipi, string submit)
        {
            if (ModelState.IsValid)
            {




                var sql = (from r in db.Filtri
                           where r.IDFiltro == posta_prototipi.IDFiltro
                           select r.SQL).SingleOrDefault();

                posta_prototipi.Filtro = sql.ToString();

                db.Entry(posta_prototipi).State = EntityState.Modified;
                db.SaveChanges();
                switch (submit)
                {
                    case "Salva e Aggiorna Lista Destinatari":
                        return RedirectToAction("SetFilterParameters", "Mailing", new { Codice = posta_prototipi.Codice, IDFiltro = posta_prototipi.IDFiltro });
                    case "SendMail":
                        return RedirectToAction("SendMail", "Mailing", new { id = posta_prototipi.Codice });
                    default:
                        throw new Exception();
                }
                return RedirectToAction("Index");
            }

            return View(posta_prototipi);
        }

        // mostra la maschera in cui inserire i valori dei parametri del filtro
        [HttpGet]
        public ActionResult SetFilterParameters(int Codice, int IDFiltro)
        {
            // Inutile rileggere Posta_Prototipi per conoscere il codice del filtro IDFiltro 
            // Posta_Prototipi posta = db.Posta_Prototipi.Find(Codice);

            var filtri_Parametri = db.Filtri_Parametri.Include(f => f.Filtri).Include(f => f.TabParametriFiltri).Where(f => f.IDFiltro == IDFiltro);
            ViewBag.Codice = Codice;
            return View(filtri_Parametri.ToList());
        }

        // Ritorna dalla maschera in cui si impostano i valore dei paramentri del filtro
        // e quindi in questa fase si ricostruisce il dataset dei destinatari del messaggio
        [HttpPost]
        public ActionResult SetFilterParameters(List<Filtri_Parametri> filtri_parametri, int Codice)
        {
            int ID = 0;
            int i = 0;
            object[] args = new object[filtri_parametri.Count()];

            foreach (Filtri_Parametri Fp in filtri_parametri)
            {
                Filtri_Parametri Existed_Fp = db.Filtri_Parametri.Find(Fp.IDFiltro, Fp.IDP, Fp.Ordine);
                Existed_Fp.Valore = Fp.Valore;
                if (Existed_Fp.TabParametriFiltri.TipoParametro == "Date")
                {
                    Fp.Valore = "DateTime(" + Convert.ToDateTime(Fp.Valore).Date.Year.ToString()
                        + "," + Convert.ToDateTime(Fp.Valore).Date.Month.ToString()
                        + "," + Convert.ToDateTime(Fp.Valore).Date.Day.ToString()
                        + ")";
                }

                ID = Fp.IDFiltro;
                args[i++] = Fp.Valore;
            }
            db.SaveChanges();

            Posta_Prototipi posta = db.Posta_Prototipi.Find(Codice);

            String filter = String.Format(posta.Filtro, args);

            // seleziona dal dataset Q_Pag_Mem i destinatari dei messaggi
            var mailingdataset = from m in db.Q_Pag_Mem select m;
            mailingdataset = mailingdataset.Where(filter);
            // elimina quelli senza email
            mailingdataset = mailingdataset.Where("Email != null");

            // pulisce la lista dei destinatari
            var postamessaggi = db.Posta_Messaggi.Where(m => m.Codice == Codice).ToList();
            foreach (var pm in postamessaggi)
                db.Posta_Messaggi.Remove(pm);
            db.SaveChanges();

            // riscrive il dataset dei destinatari
            foreach (var mds in mailingdataset)
            {
                Posta_Messaggi pm = new Posta_Messaggi();
                pm.Codice = Codice;
                pm.CodP = mds.CodP;
                pm.AnnoP = mds.AnnoP;
                pm.Nom = mds.Nom;
                pm.FlagInvio = true;
                pm.E_Mail = mds.Email;
                db.Posta_Messaggi.Add(pm);

            }
            db.SaveChanges();

            // richiama la Mailing selezionata con il nuovo data set
            return RedirectToAction("Edit", "Mailing", new { id = Codice });
        }




        // POST: Mailing/SendMail/5

        public JsonResult SendMail(int? id)
        {

            if (id == null)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            Posta_Prototipi posta_prototipi = db.Posta_Prototipi.Find(id);

            int itemsCount = posta_prototipi.Posta_Messaggi.Where(p => p.FlagInvio == true && p.E_Mail != null && p.FlagSpedito == false).Count();
            if (itemsCount == 0)
            {
                return Json("", JsonRequestBehavior.AllowGet);
            }

            TabPosta tabposta = db.TabPosta.Find(posta_prototipi.CodiceConn);

            int i = 0;

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
            smtp.Timeout = 1000;

            foreach (var item in posta_prototipi.Posta_Messaggi)
            {
                if (item.FlagInvio == true && item.E_Mail != null && item.FlagSpedito == false)
                {
                    // SEND MAIL
                    try
                    {
                        var message = new MailMessage();
                        var body = posta_prototipi.Header + posta_prototipi.Message + posta_prototipi.Footer;

                        message.To.Add(new MailAddress(item.E_Mail));  // replace with valid value 
                        message.From = new MailAddress(posta_prototipi.Sender);  // replace with valid value
                        message.Bcc.Add(new MailAddress(posta_prototipi.BCC));
                        message.CC.Add(new MailAddress(posta_prototipi.CC));
                        message.Subject = posta_prototipi.Subject;
                        message.Body = string.Format(body, item.Nom);
                        message.IsBodyHtml = false;
                        message.Sender = new MailAddress(posta_prototipi.Sender);

                        smtp.Send(message);


                        // aggiorna flag spedito
                        Posta_Messaggi current_msg = db.Posta_Messaggi.Find(item.Codice, item.CodP, item.AnnoP);
                        current_msg.FlagSpedito = true;
                        db.SaveChanges();
                        //CALLING A FUNCTION THAT CALCULATES PERCENTAGE AND SENDS THE DATA TO THE CLIENT
                        Functions.SendProgress("Invio Messaggi in corso ...", ++i, itemsCount);
                    }
                    catch (Exception ex)
                    {
                        Functions.SendProgress(ex.Message, ++i, itemsCount);
                    }
                }
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }

    }
}