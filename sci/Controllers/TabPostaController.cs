using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sci.Models;

namespace sci.Controllers
{
    public class TabPostaController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: TabPosta
        public ActionResult Index()
        {
            return View(db.TabPosta.ToList());
        }

        // GET: TabPosta/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabPosta tabPosta = db.TabPosta.Find(id);
            if (tabPosta == null)
            {
                return HttpNotFound();
            }
            return View(tabPosta);
        }

        // GET: TabPosta/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TabPosta/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Codice,Descrizione,smtpServer,smtpServerPort,sendUsing,AutenticationType,SendUsername,SendPassword,Sender,NotificationTo,ReceiptTo")] TabPosta tabPosta)
        {
            if (ModelState.IsValid)
            {
                db.TabPosta.Add(tabPosta);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tabPosta);
        }

        // GET: TabPosta/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabPosta tabPosta = db.TabPosta.Find(id);
            if (tabPosta == null)
            {
                return HttpNotFound();
            }
            return View(tabPosta);
        }

        // POST: TabPosta/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Codice,Descrizione,smtpServer,smtpServerPort,sendUsing,AutenticationType,SendUsername,SendPassword,Sender,NotificationTo,ReceiptTo")] TabPosta tabPosta)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabPosta).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tabPosta);
        }

        // GET: TabPosta/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabPosta tabPosta = db.TabPosta.Find(id);
            if (tabPosta == null)
            {
                return HttpNotFound();
            }
            return View(tabPosta);
        }

        // POST: TabPosta/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TabPosta tabPosta = db.TabPosta.Find(id);
            db.TabPosta.Remove(tabPosta);
            db.SaveChanges();
            return RedirectToAction("Index");
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
