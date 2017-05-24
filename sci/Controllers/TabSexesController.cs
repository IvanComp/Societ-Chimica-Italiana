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
    public class TabSexesController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: TabSexes
        public ActionResult Index()
        {
            return View(db.TabSex.ToList());
        }

        // GET: TabSexes/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabSex tabSex = db.TabSex.Find(id);
            if (tabSex == null)
            {
                return HttpNotFound();
            }
            return View(tabSex);
        }

        // GET: TabSexes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TabSexes/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodSex,DescrSex,SelSex")] TabSex tabSex)
        {
            if (ModelState.IsValid)
            {
                db.TabSex.Add(tabSex);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tabSex);
        }

        // GET: TabSexes/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabSex tabSex = db.TabSex.Find(id);
            if (tabSex == null)
            {
                return HttpNotFound();
            }
            return View(tabSex);
        }

        // POST: TabSexes/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodSex,DescrSex,SelSex")] TabSex tabSex)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tabSex).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tabSex);
        }

        // GET: TabSexes/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TabSex tabSex = db.TabSex.Find(id);
            if (tabSex == null)
            {
                return HttpNotFound();
            }
            return View(tabSex);
        }

        // POST: TabSexes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            TabSex tabSex = db.TabSex.Find(id);
            db.TabSex.Remove(tabSex);
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
