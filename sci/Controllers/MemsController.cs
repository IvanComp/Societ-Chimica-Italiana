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
using System.Collections;

namespace sci.Controllers
{
    public class MemsController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
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
            var data = db.Mem.Select(m => m).ToList();
            ViewBag.data = data;

            if (!String.IsNullOrEmpty(searchString))
            {
                if (searchString.All(char.IsDigit))
                {
                    mems = mems.Where(m => m.Cod.ToString().StartsWith(searchString));
                }

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
                default: 
                    mems = mems.OrderBy(m => m.Cognome);
                    break;
            }

            int pageSize = 20;
            int pageNumber = (page ?? 1);
            return View(mems.ToPagedList(pageNumber, pageSize));
        }

        [HttpPost]
        public ActionResult Index(string searchString)
        {
            ViewBag.CurrentFilter = searchString;

            var mems = db.Mem.Where(m => (m.Cognome.StartsWith(searchString)) || (m.Nome.StartsWith(searchString)) || (m.Cod.ToString().StartsWith(searchString))).Take(10);

            return View(mems);
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
