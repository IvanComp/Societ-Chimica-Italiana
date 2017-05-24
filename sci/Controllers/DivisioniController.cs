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
    public class DivisioniController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: Divisioni
        public ActionResult Index()
        {
            var divisioni = db.Divisioni.Include(d => d.Pag).Include(d => d.TabDiv).Where(d => d.AnnoP == 2007 && d.CodP >= 3187 && d.CodP <= 3500);
            return View(divisioni.ToList());
        }

        // GET: Divisioni/Details/5
        public ActionResult Details(int? AnnoP, int? CodP, string CodDiv)
        {
            if (AnnoP == null || CodP == null || CodDiv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Divisioni divisioni = db.Divisioni.Find(AnnoP, CodP, CodDiv);
            if (divisioni == null)
            {
                return HttpNotFound();
            }
            return View(divisioni);
        }

        // GET: Divisioni/Create
        public ActionResult Create()
        {
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr");
            ViewBag.AnnoP = new SelectList(db.TabDiv, "AnnoDiv", "AnnoDiv");
            ViewBag.CodDiv = new SelectList(db.TabDiv, "CodDiv", "DescrDiv");
            return View();
        }

        // POST: Divisioni/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AnnoP,CodP,CodDiv,Effettiva")] Divisioni divisioni)
        {
            if (ModelState.IsValid)
            {
                db.Divisioni.Add(divisioni);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", divisioni.CodP);
            ViewBag.AnnoP = new SelectList(db.TabDiv, "AnnoDiv", "NCodDiv", divisioni.AnnoP);
            return View(divisioni);
        }

        // GET: Divisioni/Edit/5
        public ActionResult Edit(int? AnnoP, int? CodP, string CodDiv)
        {
            if (AnnoP == null || CodP == null || CodDiv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Divisioni divisioni = db.Divisioni.Find(AnnoP,CodP,CodDiv);
            if (divisioni == null)
            {
                return HttpNotFound();
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", divisioni.CodP);
            ViewBag.AnnoP = new SelectList(db.TabDiv, "AnnoDiv", "NCodDiv", divisioni.AnnoP);
            return View(divisioni);
        }

        // POST: Divisioni/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AnnoP,CodP,CodDiv,Effettiva")] Divisioni divisioni)
        {
            if (ModelState.IsValid)
            {
                db.Entry(divisioni).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", divisioni.CodP);
            ViewBag.AnnoP = new SelectList(db.TabDiv, "AnnoDiv", "NCodDiv", divisioni.AnnoP);
            return View(divisioni);
        }

        // GET: Divisioni/Delete/5
        public ActionResult Delete(int? AnnoP, int? CodP, string CodDiv)
        {
            if (AnnoP == null || CodP == null || CodDiv == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Divisioni divisioni = db.Divisioni.Find(AnnoP, CodP, CodDiv);
            if (divisioni == null)
            {
                return HttpNotFound();
            }
            return View(divisioni);
        }

        // POST: Divisioni/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int? AnnoP, int? CodP, string CodDiv)
        {
            Divisioni divisioni = db.Divisioni.Find(AnnoP, CodP, CodDiv);
            db.Divisioni.Remove(divisioni);
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
