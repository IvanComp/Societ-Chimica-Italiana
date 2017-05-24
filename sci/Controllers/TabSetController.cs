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
    public class TabSetController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: TabSet
        public ActionResult Index()
        {
            return View(db.TabSet.ToList());
        }

        // GET: TabSet/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabSet tabSet = db.TabSet.Find(id);
            if (tabSet == null)
            {
                return HttpNotFound();
            }
            return View(tabSet);
        }

        // GET: TabSet/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TabSet/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodSet,NCodSet,DescrSet,SelSet")] TabSet tabSet)
        {
            if (ModelState.IsValid)
            {
                db.TabSet.Add(tabSet);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tabSet);
        }

        // GET: TabSet/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabSet tabSet = db.TabSet.Find(id);
            if (tabSet == null)
            {
                return HttpNotFound();
            }
            return View(tabSet);
        }

        // POST: TabSet/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodSet,NCodSet,DescrSet,SelSet")] TabSet tabSet)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabSet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tabSet);
        }

        // GET: TabSet/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabSet tabSet = db.TabSet.Find(id);
            if (tabSet == null)
            {
                return HttpNotFound();
            }
            return View(tabSet);
        }

        // POST: TabSet/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TabSet tabSet = db.TabSet.Find(id);
            db.TabSet.Remove(tabSet);
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
