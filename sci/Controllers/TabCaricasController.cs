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
    public class TabCaricasController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: TabCaricas
        public ActionResult Index()
        {
            return View(db.TabCarica.ToList());
        }

        // GET: TabCaricas/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabCarica tabCarica = db.TabCarica.Find(id);
            if (tabCarica == null)
            {
                return HttpNotFound();
            }
            return View(tabCarica);
        }

        // GET: TabCaricas/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TabCaricas/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodCarica,DescrCarica,SelCarica")] TabCarica tabCarica)
        {
            if (ModelState.IsValid)
            {
                db.TabCarica.Add(tabCarica);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tabCarica);
        }

        // GET: TabCaricas/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabCarica tabCarica = db.TabCarica.Find(id);
            if (tabCarica == null)
            {
                return HttpNotFound();
            }
            return View(tabCarica);
        }

        // POST: TabCaricas/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodCarica,DescrCarica,SelCarica")] TabCarica tabCarica)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabCarica).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tabCarica);
        }

        // GET: TabCaricas/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabCarica tabCarica = db.TabCarica.Find(id);
            if (tabCarica == null)
            {
                return HttpNotFound();
            }
            return View(tabCarica);
        }

        // POST: TabCaricas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TabCarica tabCarica = db.TabCarica.Find(id);
            db.TabCarica.Remove(tabCarica);
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
