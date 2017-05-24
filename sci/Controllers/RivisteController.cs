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
    public class RivisteController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: Riviste
        public ActionResult Index()
        {
            var riviste = db.Riviste.Include(r => r.Pag).Include(r => r.TabRiv);
            return View(riviste.ToList());
        }

        // GET: Riviste/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Riviste riviste = db.Riviste.Find(id);
            if (riviste == null)
            {
                return HttpNotFound();
            }
            return View(riviste);
        }

        // GET: Riviste/Create
        public ActionResult Create()
        {
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr");
            ViewBag.AnnoP = new SelectList(db.TabRiv, "AnnoRiv", "NCodRiv");
            return View();
        }

        // POST: Riviste/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnnoP,CodP,CodRiv,Ncopie")] Riviste riviste)
        {
            if (ModelState.IsValid)
            {
                db.Riviste.Add(riviste);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", riviste.CodP);
            ViewBag.AnnoP = new SelectList(db.TabRiv, "AnnoRiv", "NCodRiv", riviste.AnnoP);
            return View(riviste);
        }

        // GET: Riviste/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Riviste riviste = db.Riviste.Find(id);
            if (riviste == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", riviste.CodP);
            ViewBag.AnnoP = new SelectList(db.TabRiv, "AnnoRiv", "NCodRiv", riviste.AnnoP);
            return View(riviste);
        }

        // POST: Riviste/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnnoP,CodP,CodRiv,Ncopie")] Riviste riviste)
        {
            if (ModelState.IsValid)
            {
                db.Entry(riviste).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", riviste.CodP);
            ViewBag.AnnoP = new SelectList(db.TabRiv, "AnnoRiv", "NCodRiv", riviste.AnnoP);
            return View(riviste);
        }

        // GET: Riviste/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Riviste riviste = db.Riviste.Find(id);
            if (riviste == null)
            {
                return HttpNotFound();
            }
            return View(riviste);
        }

        // POST: Riviste/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Riviste riviste = db.Riviste.Find(id);
            db.Riviste.Remove(riviste);
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
