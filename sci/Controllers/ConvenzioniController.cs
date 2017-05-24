using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace sci.Models
{
    public class ConvenzioniController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: Convenzioni
        public ActionResult Index()
        {
            var convenzioni = db.Convenzioni.Include(c => c.Pag).Include(c => c.TabConv);
            return View(convenzioni.ToList());
        }

        // GET: Convenzioni/Details/5
        public ActionResult Details(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenzioni convenzioni = db.Convenzioni.Find(id);
            if (convenzioni == null)
            {
                return HttpNotFound();
            }
            return View(convenzioni);
        }

        // GET: Convenzioni/Create
        public ActionResult Create()
        {
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr");
            ViewBag.AnnoP = new SelectList(db.TabConv, "AnnoConv", "DescrConv");
            return View();
        }

        // POST: Convenzioni/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnnoP,CodP,CodConv,Ncopie,DatConv")] Convenzioni convenzioni)
        {
            if (ModelState.IsValid)
            {
                db.Convenzioni.Add(convenzioni);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", convenzioni.CodP);
            ViewBag.AnnoP = new SelectList(db.TabConv, "AnnoConv", "DescrConv", convenzioni.AnnoP);
            return View(convenzioni);
        }

        // GET: Convenzioni/Edit/5
        public ActionResult Edit(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenzioni convenzioni = db.Convenzioni.Find(id);
            if (convenzioni == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", convenzioni.CodP);
            ViewBag.AnnoP = new SelectList(db.TabConv, "AnnoConv", "DescrConv", convenzioni.AnnoP);
            return View(convenzioni);
        }

        // POST: Convenzioni/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnnoP,CodP,CodConv,Ncopie,DatConv")] Convenzioni convenzioni)
        {
            if (ModelState.IsValid)
            {
                db.Entry(convenzioni).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", convenzioni.CodP);
            ViewBag.AnnoP = new SelectList(db.TabConv, "AnnoConv", "DescrConv", convenzioni.AnnoP);
            return View(convenzioni);
        }

        // GET: Convenzioni/Delete/5
        public ActionResult Delete(short? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Convenzioni convenzioni = db.Convenzioni.Find(id);
            if (convenzioni == null)
            {
                return HttpNotFound();
            }
            return View(convenzioni);
        }

        // POST: Convenzioni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(short id)
        {
            Convenzioni convenzioni = db.Convenzioni.Find(id);
            db.Convenzioni.Remove(convenzioni);
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
