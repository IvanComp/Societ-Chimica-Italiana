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
    public class StampesController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: Stampes
        public ActionResult Index()
        {
            ViewBag.Filtri = db.Filtri.Select(q => new SelectListItem { Value = q.IDFiltro.ToString(), Text = q.NomeFiltro }).ToList();
            ViewBag.Ordinamenti = db.Ordinamenti.Select(q => new SelectListItem { Value = q.IDO.ToString(), Text = q.NomeOrdinamentoO }).ToList();
            var stampe = db.Stampe.Include(s => s.Filtri);
            stampe = stampe.Include(s => s.Ordinamenti);
            return View(stampe.ToList());
        }

        // GET: Stampes/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stampe stampe = db.Stampe.Find(id);
            if (stampe == null)
            {
                return HttpNotFound();
            }
            return View(stampe);
        }

        // GET: Stampes/Create
        public ActionResult Create()
        {
            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro");
            ViewBag.IDOrdinamento = new SelectList(db.Ordinamenti, "IDO", "NomeOrdinamentoO");
            return View();
        }

        // POST: Stampes/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Nome,NomeReport,Descrizione,IDFiltro,Filtro,IDOrdinamento,Ordinamento,Filtrabile,Ordinabile,Selezionato,QueryAmmesse")] Stampe stampe)
        {
            if (ModelState.IsValid)
            {
                db.Stampe.Add(stampe);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro");
            ViewBag.IDOrdinamento = new SelectList(db.Ordinamenti, "IDO", "NomeOrdinamentoO");
            return View(stampe);
        }

        // GET: Stampes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stampe stampa = db.Stampe.Find(id);
            if (stampa == null)
            {
                return HttpNotFound();
            }

            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro");
            ViewBag.IDOrdinamento = new SelectList(db.Ordinamenti, "IDO", "NomeOrdinamentoO");
            return View(stampa);
        }

        // POST: Stampes/Seleziona/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]

        public ActionResult Seleziona([Bind(Include = "ID,Nome,NomeReport,Descrizione,IDFiltro,Filtro,IDOrdinamento,Ordinamento,Filtrabile,Ordinabile,Selezionato,QueryAmmesse")] Stampe stampe)
         {
      
            if (ModelState.IsValid)
            {
                var sql = (from r in db.Filtri
                           where r.IDFiltro == stampe.IDFiltro
                           select r.SQL).SingleOrDefault();
                stampe.Filtro = sql.ToString();

                var criterioo = (from r in db.Ordinamenti
                        where r.IDO == stampe.IDOrdinamento
                        select r.CriterioO).SingleOrDefault();
                stampe.Ordinamento = criterioo.ToString();

                db.Entry(stampe).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index", "Filtri_Parametri", new { id = stampe.IDFiltro,idStampa = stampe.ID});

//                return RedirectToAction("Rpt","RptMems",new { query=stampe.QueryAmmesse, reportname=stampe.NomeReport, filter=stampe.Filtro, orderby=stampe.Ordinamento});

            }
            ViewBag.Filtri = db.Filtri.Select(q => new SelectListItem { Value = q.IDFiltro.ToString(), Text = q.NomeFiltro }).ToList();
            ViewBag.Ordinamenti = db.Ordinamenti.Select(q => new SelectListItem { Value = q.IDO.ToString(), Text = q.NomeOrdinamentoO }).ToList();
            return View(stampe);
        }


        // POST: Stampes/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,NomeReport,Descrizione,IDFiltro,Filtro,IDOrdinamento,Ordinamento,Filtrabile,Ordinabile,Selezionato,QueryAmmesse")] Stampe stampe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stampe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro");
            ViewBag.IDOrdinamento = new SelectList(db.Ordinamenti, "IDO", "NomeOrdinamentoO");
            return View(stampe);
        }

        // POST: Stampes/RptSel/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RptSel([Bind(Include = "ID,Nome,NomeReport,Descrizione,IDFiltro,Filtro,IDOrdinamento,Ordinamento,Filtrabile,Ordinabile,Selezionato,QueryAmmesse")] Stampe stampe)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stampe).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro");
            ViewBag.IDOrdinamento = new SelectList(db.Ordinamenti, "IDO", "NomeOrdinamentoO");
            return View(stampe);
        }

        // GET: Stampes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stampe stampe = db.Stampe.Find(id);
            if (stampe == null)
            {
                return HttpNotFound();
            }
            return View(stampe);
        }

        // POST: Stampes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Stampe stampe = db.Stampe.Find(id);
            db.Stampe.Remove(stampe);
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
