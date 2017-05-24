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
    public class TabNazsController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: TabNazs
        public ActionResult Index()
        {
            return View(db.TabNaz.ToList());
        }

        // GET: TabNazs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabNaz tabNaz = db.TabNaz.Find(id);
            if (tabNaz == null)
            {
                return HttpNotFound();
            }
            return View(tabNaz);
        }

        // GET: TabNazs/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TabNazs/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodNaz,DescrNaz,SelNaz")] TabNaz tabNaz)
        {
            if (ModelState.IsValid)
            {
                db.TabNaz.Add(tabNaz);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tabNaz);
        }

        // GET: TabNazs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabNaz tabNaz = db.TabNaz.Find(id);
            if (tabNaz == null)
            {
                return HttpNotFound();
            }
            return View(tabNaz);
        }

        // POST: TabNazs/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodNaz,DescrNaz,SelNaz")] TabNaz tabNaz)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabNaz).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tabNaz);
        }

        // GET: TabNazs/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabNaz tabNaz = db.TabNaz.Find(id);
            if (tabNaz == null)
            {
                return HttpNotFound();
            }
            return View(tabNaz);
        }

        // POST: TabNazs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TabNaz tabNaz = db.TabNaz.Find(id);
            db.TabNaz.Remove(tabNaz);
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
