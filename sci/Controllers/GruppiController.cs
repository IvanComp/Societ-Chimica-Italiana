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
    public class GruppiController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: Gruppi
        public ActionResult Index()
        {
            var gruppi = db.Gruppi.Include(g => g.Pag).Include(g => g.TabGru);
            return View(gruppi.ToList());
        }

        // GET: Gruppi/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gruppi gruppi = db.Gruppi.Find(id);
            if (gruppi == null)
            {
                return HttpNotFound();
            }
            return View(gruppi);
        }

        // GET: Gruppi/Create
        public ActionResult Create()
        {
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr");
            ViewBag.AnnoP = new SelectList(db.TabGru, "AnnoGru", "NCodGru");
            return View();
        }

        // POST: Gruppi/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnnoP,CodP,CodGru")] Gruppi gruppi)
        {
            if (ModelState.IsValid)
            {
                db.Gruppi.Add(gruppi);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", gruppi.CodP);
            ViewBag.AnnoP = new SelectList(db.TabGru, "AnnoGru", "NCodGru", gruppi.AnnoP);
            return View(gruppi);
        }

        // GET: Gruppi/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gruppi gruppi = db.Gruppi.Find(id);
            if (gruppi == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", gruppi.CodP);
            ViewBag.AnnoP = new SelectList(db.TabGru, "AnnoGru", "NCodGru", gruppi.AnnoP);
            return View(gruppi);
        }

        // POST: Gruppi/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnnoP,CodP,CodGru")] Gruppi gruppi)
        {
            if (ModelState.IsValid)
            {
                db.Entry(gruppi).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", gruppi.CodP);
            ViewBag.AnnoP = new SelectList(db.TabGru, "AnnoGru", "NCodGru", gruppi.AnnoP);
            return View(gruppi);
        }

        // GET: Gruppi/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Gruppi gruppi = db.Gruppi.Find(id);
            if (gruppi == null)
            {
                return HttpNotFound();
            }
            return View(gruppi);
        }

        // POST: Gruppi/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Gruppi gruppi = db.Gruppi.Find(id);
            db.Gruppi.Remove(gruppi);
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
