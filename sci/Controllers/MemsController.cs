using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sci.Models;
using PagedList;

namespace sci.Controllers
{
    public class MemsController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

       
            // GET: Mems
            public ActionResult Index(string sortOrder, string currentFilter, string searchString,string prefix, int? page)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.DateSortParm = sortOrder == "Cod" ? "cod_desc" : "Cod";

            if (searchString != null)
            {
                page = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            ViewBag.CurrentFilter = searchString;
            var mems = from m in db.Mem select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString.All(char.IsDigit))
                {
                    mems = mems.Where(m => m.Cod.ToString().Equals(searchString));
                    if (mems.Count() == 1)
                    {
                        var pags = db.Pag.Where(p => p.CodP.ToString().Equals(searchString)).Select(d => d.AnnoP);
                        if (pags.Any())
                        {
                            var last = pags.Max();
                            var lastcod = Convert.ToInt32(searchString); ;
                            return RedirectToAction("Edit", "Pags", new { codp = lastcod, annop = last });
                        }
                        else
                            return RedirectToAction("Edit/" + mems.Select(m => m.Cod).Max().ToString(), "Mems", null);
                    }
                }
                     
                else
                    mems = mems.Where(m => m.Cognome.Contains(searchString));
            }


            switch (sortOrder)
            {
                case "name_desc":
                    mems = mems.OrderByDescending(m => m.Cognome);
                    break;
                case "Cod":
                    mems = mems.OrderBy(m => m.Cod);
                    break;
                case "cod_desc":
                    mems = mems.OrderByDescending(m => m.Cod);
                    break;
                default:  // Name ascending 
                    mems = mems.OrderBy(m => m.Cognome);
                    break;
            }

            int pageSize = 50;
            int pageNumber = (page ?? 1);
            return View(mems.ToPagedList(pageNumber, pageSize));
        }

        

    // GET: Mems/Details/5
    public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Mem mem = db.Mem.Find(id);
            var query = db.Pag.Max(x => x.AnnoP);

            if (mem == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Edit", "Pags", new { codp = id, annop = query });
        }

        // GET: Mems/Create
        public ActionResult Create()
        {
            ViewBag.Naz = new SelectList(db.TabNaz, "CodNaz", "DescrNaz");
            ViewBag.Sesso = new SelectList(db.TabSex, "CodSex", "DescrSex");
            ViewBag.CodTitStu = new SelectList(db.TabTitStu, "TCodTitStu", "DescrTitStu");
            return View();
        }

        // POST: Mems/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Cod,Sesso,Nom,Titolo,Nome,Cognome,Ist,Via,CAP,Cit,Naz,DaNa,Tel,Fax,Email,DimAn,NoStampa,Modificato,PresentatoDa,FlagRegalo,PresentatoDa2,FlagRegalo2,AnnoPresentazione,CodTitStu")] Mem mem)
        {
            if (ModelState.IsValid)
            {
                db.Mem.Add(mem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Naz = new SelectList(db.TabNaz, "CodNaz", "DescrNaz", mem.Naz);
            ViewBag.Sesso = new SelectList(db.TabSex, "CodSex", "DescrSex", mem.Sesso);
            ViewBag.CodTitStu = new SelectList(db.TabTitStu, "TCodTitStu", "DescrTitStu", mem.CodTitStu);
            return View(mem);
        }

        // GET: Mems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mem mem = db.Mem.Find(id);
            if (mem == null)
            {
                return HttpNotFound();
            }

            // anni dei pagamenti
            var anniPag = db.Pag.Where(d => d.CodP == id ).Select(d => d.AnnoP.ToString()).ToList();
            if (anniPag.Any())
            {
                ViewBag.anniPag = anniPag;
            }

            ViewBag.Naz = new SelectList(db.TabNaz, "CodNaz", "DescrNaz", mem.Naz);
            ViewBag.Sesso = new SelectList(db.TabSex, "CodSex", "DescrSex", mem.Sesso);
            ViewBag.CodTitStu = new SelectList(db.TabTitStu, "TCodTitStu", "DescrTitStu", mem.CodTitStu);
            return View(mem);
        }

        // POST: Mems/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Cod,Sesso,Nom,Titolo,Nome,Cognome,Ist,Via,CAP,Cit,Naz,DaNa,Tel,Fax,Email,DimAn,NoStampa,Modificato,PresentatoDa,FlagRegalo,PresentatoDa2,FlagRegalo2,AnnoPresentazione,CodTitStu")] Mem mem)
        {
            if (ModelState.IsValid)
            {
                db.Entry(mem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Naz = new SelectList(db.TabNaz, "CodNaz", "DescrNaz", mem.Naz);
            ViewBag.Sesso = new SelectList(db.TabSex, "CodSex", "DescrSex", mem.Sesso);
            ViewBag.CodTitStu = new SelectList(db.TabTitStu, "TCodTitStu", "DescrTitStu", mem.CodTitStu);
            return View(mem);
        }

        // GET: Mems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Mem mem = db.Mem.Find(id);
            if (mem == null)
            {
                return HttpNotFound();
            }
            return View(mem);
        }

        // POST: Mems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Mem mem = db.Mem.Find(id);
            db.Mem.Remove(mem);
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
