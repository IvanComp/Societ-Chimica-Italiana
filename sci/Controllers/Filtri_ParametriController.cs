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
    public class Filtri_ParametriController : Controller
    {
        private sci_newEntities db = new sci_newEntities();

        // GET: Filtri_Parametri
        public ActionResult Index(int ID, int? idStampa)
        {
            var filtri_Parametri = db.Filtri_Parametri.Include(f => f.Filtri).Include(f => f.TabParametriFiltri).Where(f => f.IDFiltro == ID);
            ViewBag.idStampa = idStampa;
            return View(filtri_Parametri.ToList());
        }



        [HttpPost]
        public ActionResult Index(List<Filtri_Parametri> filtri_parametri, int idStampa, string RptFormat)
        {
            int ID = 0;
            int i = 0;
            object[] args = new object[filtri_parametri.Count()];

            foreach (Filtri_Parametri Fp in filtri_parametri)
            {
                Filtri_Parametri Existed_Fp = db.Filtri_Parametri.Find(Fp.IDFiltro,Fp.Ordine,Fp.IDP);
                Existed_Fp.Valore = Fp.Valore;
                if (Existed_Fp.TabParametriFiltri.TipoParametro == "Date")
                {
                    Fp.Valore = "DateTime(" + Convert.ToDateTime(Fp.Valore).Date.Year.ToString()
                        + "," + Convert.ToDateTime(Fp.Valore).Date.Month.ToString()
                        + "," + Convert.ToDateTime(Fp.Valore).Date.Day.ToString()
                        + ")";
                }

                ID = Fp.IDFiltro;
                args[i++] = Fp.Valore;
            }
            db.SaveChanges();
            Stampe stampa = db.Stampe.Find(idStampa);
            String filter = String.Format(stampa.Filtro, args);
            // richiama la produzione del report
            return RedirectToAction("Rpt","Stampe",new { id=RptFormat, query=stampa.QueryAmmesse, reportname=stampa.NomeReport, filter=filter, orderby=stampa.Ordinamento});
        }

        // GET: Filtri_Parametri/Details/5
        public ActionResult Details(int? IDFiltro, int? IDP, int? Ordine)
        {
            if (IDFiltro == null || IDP == null || Ordine == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filtri_Parametri filtri_Parametri = db.Filtri_Parametri.Find(IDFiltro, IDP, Ordine);
            if (filtri_Parametri == null)
            {
                return HttpNotFound();
            }
            return View(filtri_Parametri);
        }

        // GET: Filtri_Parametri/Create
        public ActionResult Create()
        {
            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro");
            ViewBag.IDP = new SelectList(db.TabParametriFiltri, "Idp", "NomeParametro");
            return View();
        }

        // POST: Filtri_Parametri/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "IDFiltro,IDP,Prompt,Ordine,Valore")] Filtri_Parametri filtri_Parametri)
        {
            if (ModelState.IsValid)
            {
                db.Filtri_Parametri.Add(filtri_Parametri);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro", filtri_Parametri.IDFiltro);
            ViewBag.IDP = new SelectList(db.TabParametriFiltri, "Idp", "NomeParametro", filtri_Parametri.IDP);
            return View(filtri_Parametri);
        }

        // GET: Filtri_Parametri/Edit/5
        public ActionResult Edit(int IDFiltro, int IDP, int Ordine)
        {
            if (IDFiltro == null || IDP == null || Ordine == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filtri_Parametri filtri_Parametri = db.Filtri_Parametri.Find(IDFiltro, IDP, Ordine);
            if (filtri_Parametri == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro", filtri_Parametri.IDFiltro);
            ViewBag.IDP = new SelectList(db.TabParametriFiltri, "Idp", "NomeParametro", filtri_Parametri.IDP);
            return View(filtri_Parametri);
        }

        // POST: Filtri_Parametri/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "IDFiltro,IDP,Prompt,Ordine,Valore")] Filtri_Parametri filtri_Parametri)
        {
            if (ModelState.IsValid)
            {
                db.Entry(filtri_Parametri).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.IDFiltro = new SelectList(db.Filtri, "IDFiltro", "NomeFiltro", filtri_Parametri.IDFiltro);
            ViewBag.IDP = new SelectList(db.TabParametriFiltri, "Idp", "NomeParametro", filtri_Parametri.IDP);
            return View(filtri_Parametri);
        }

        // GET: Filtri_Parametri/Delete/5
        public ActionResult Delete(int? IDFiltro, int IDP, int Ordine)
        {
            if (IDFiltro == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Filtri_Parametri filtri_Parametri = db.Filtri_Parametri.Find(IDFiltro, IDP, Ordine);
            if (filtri_Parametri == null)
            {
                return HttpNotFound();
            }
            return View(filtri_Parametri);
        }

        // POST: Filtri_Parametri/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Filtri_Parametri filtri_Parametri = db.Filtri_Parametri.Find(id);
            db.Filtri_Parametri.Remove(filtri_Parametri);
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
