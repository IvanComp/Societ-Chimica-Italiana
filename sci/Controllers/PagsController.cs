using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using sci.Models;
using System.Data.SqlClient;

namespace sci.Controllers
{
    public class PagsController : Controller
    {
        private sci_newEntities db = new sci_newEntities();
        private void UpdateDovuto(int anno, int cod)
        {
            var result = 0;
            using (var context = new sci_newEntities())
            {
                SqlParameter param1 = new SqlParameter("@AnnoP", anno);
                SqlParameter param2 = new SqlParameter("@CodP", cod);
                result = context.Database.ExecuteSqlCommand("up_Dovuto @AnnoP, @CodP", param1, param2);
            }
            if (result != -1) {
                TempData["msg"] = "Aggiornamento Importo Dovuto non riuscito";
            };
        }


        // GET: O_Pag_Mem
        public ActionResult Index(int? page)
        {
            var pag = db.Pag.Include(p => p.Mem).Include(p => p.TabCarica).Include(p => p.TabCat).Include(p => p.TabEnt).Include(p => p.TabPos).Include(p => p.TabSet).Include(p => p.TabSez).Include(p => p.TabTIscr).Include(p => p.TabTPag).OrderBy(p => p.CodP);
            var paginatedPag = pag.Skip((page ?? 0) * 10).Take(10).ToList();

            return View(paginatedPag);
        }

        // GET: Pags/Details/5
        public ActionResult Details(int codp, int annop)
        {
            Pag pag = db.Pag.Find(codp,annop);
            if (pag == null)
            {
                return HttpNotFound();
            }
            return View(pag);
        }

        // GET: Pags/Create
        public ActionResult Create()
        {
            ViewBag.CodP = new SelectList(db.Mem, "Cod", "Cod");
            ViewBag.AnnoP = new SelectList(db.TabCat, "AnnoCat", "AnnoCat");
            ViewBag.Cat = new SelectList(db.TabCat, "CodCat", "DescrCat");
            ViewBag.Carica = new SelectList(db.TabCarica, "CodCarica", "DescrCarica");

            ViewBag.Ent = new SelectList(db.TabEnt, "CodEnt", "DescrEnt");
            ViewBag.Pos = new SelectList(db.TabPos, "CodPos", "DescrPos");
            ViewBag.Sett = new SelectList(db.TabSet, "CodSet", "DescrSet");
            ViewBag.Sett = new SelectList(db.TabSex, "CodSex", "DescrSex");
            ViewBag.Sez = new SelectList(db.TabSez, "CodSez", "DescrSez");
            ViewBag.Tiscr = new SelectList(db.TabTIscr, "CodTIscr", "DescrTIscr");
            ViewBag.Tpag = new SelectList(db.TabTPag, "CodTPag", "DescrTPag");
            return View();
        }

        // POST: Pags/Create
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CodP,AnnoP,Tiscr,Cat,Pagato,Dovuto,DatPag,DatReg,DatCassa,Tpag,Fuori,Lode,Carica,Sez,Sett,Pos,Ent,CodSocCol")] Pag pag)
        {
            if (ModelState.IsValid)
            {
                db.Pag.Add(pag);
                db.SaveChanges();
                UpdateDovuto(pag.AnnoP, pag.CodP);
                return RedirectToAction("Edit", new { codp = pag.CodP, annop = pag.AnnoP });
            }

            ViewBag.CodP = new SelectList(db.Mem, "Cod", "Cod", pag.Mem.Cod);
            ViewBag.AnnoP = new SelectList(db.TabCat, "AnnoCat", "AnnoCat", pag.AnnoP);
            ViewBag.Cat = new SelectList(db.TabCat, "CodCat", "DescrCat", pag.Cat);
            ViewBag.Carica = new SelectList(db.TabCarica, "CodCarica", "DescrCarica", pag.Carica);
            ViewBag.Ent = new SelectList(db.TabEnt, "CodEnt", "DescrEnt", pag.Ent);
            ViewBag.Pos = new SelectList(db.TabPos, "CodPos", "DescrPos", pag.Pos);
            ViewBag.Sett = new SelectList(db.TabSet, "CodSet", "DescrSet", pag.Sett);
            ViewBag.Sez = new SelectList(db.TabSez, "CodSez", "DescrSez", pag.Sez);
            ViewBag.Tiscr = new SelectList(db.TabTIscr, "CodTIscr", "DescrTIscr", pag.Tiscr);
            ViewBag.Tpag = new SelectList(db.TabTPag, "CodTPag", "DescrTPag", pag.Tpag);
            return View(pag);
        }

        // GET: Pags/Edit/5
        public ActionResult Edit(int? codp, int? annop)
        {
            if (codp == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Pag pag = db.Pag.Find(codp,annop);

            if (annop == null)
            {
                var query = db.Pag.Max(x => x.AnnoP);
                annop = query;
            }

            if (pag == null)
            {
                return View("Error404");
            }

            // anni dei pagamenti
            var anniPag = db.Pag.Where(d => d.CodP == codp && d.AnnoP != annop).Select(d => d.AnnoP.ToString()).ToList();
            if (anniPag.Any())
            {
                ViewBag.anniPag = anniPag;
            }

            // per le divisioni
            var divAlreadyPresent = db.Divisioni.Where(d => d.AnnoP == annop && d.CodP == codp).Select(d => d.CodDiv.ToString()).ToList();
            var listDiv = db.TabDiv.Where(m => !divAlreadyPresent.Contains(m.CodDiv ) && m.AnnoDiv == annop);
            if (listDiv.Any())
            {
                ViewBag.CodDiv = listDiv.Select(q => new SelectListItem { Value = q.CodDiv, Text = q.DescrDiv }).ToList();
            }

            // per i gruppi
            var gruAlreadyPresent = db.Gruppi.Where(d => d.AnnoP == annop && d.CodP == codp).Select(d => d.CodGru).ToList();
            var listGru = db.TabGru.Where(m => !gruAlreadyPresent.Contains(m.CodGru) && m.AnnoGru == annop);
            if (listGru.Any())
            {
                ViewBag.CodGru = listGru.Select(q => new SelectListItem { Value = q.CodGru, Text = q.DescrGruppo }).ToList();
            }
            
            // per le Riviste
            var rivAlreadyPresent = db.Riviste.Where(d => d.AnnoP == annop && d.CodP == codp).Select(d => d.CodRiv).ToList();
            var listRiv = db.TabRiv.Where(m => !rivAlreadyPresent.Contains(m.CodRiv) && m.AnnoRiv == annop);
            if (listRiv.Any())
            {
                ViewBag.CodRiv = listRiv.Select(q => new SelectListItem { Value = q.CodRiv, Text = q.DescrRiv }).ToList();
            }

            // per le Convenzioni
            var convAlreadyPresent = db.Convenzioni.Where(d => d.AnnoP == annop && d.CodP == codp).Select(d => d.CodConv).ToList();
            var listConv = db.TabConv.Where(m => !convAlreadyPresent.Contains(m.CodConv) && m.AnnoConv == annop);
            if (listConv.Any())
            {
                ViewBag.CodConv = listConv.Select(q => new SelectListItem { Value = q.CodConv, Text = q.DescrConv }).ToList();
            }

            ViewBag.Cat = new SelectList(db.TabCat.Where(m => m.AnnoCat == annop), "CodCat", "DescrCat", pag.Cat);
            ViewBag.Carica = new SelectList(db.TabCarica, "CodCarica", "DescrCarica", pag.Carica);
            ViewBag.Ent = new SelectList(db.TabEnt, "CodEnt", "DescrEnt", pag.Ent);
            ViewBag.Pos = new SelectList(db.TabPos, "CodPos", "DescrPos", pag.Pos);
            ViewBag.Sett = new SelectList(db.TabSet, "CodSet", "DescrSet", pag.Sett);
            ViewBag.Sez = new SelectList(db.TabSez, "CodSez", "DescrSez", pag.Sez);
            ViewBag.Tiscr = new SelectList(db.TabTIscr, "CodTIscr", "DescrTIscr", pag.Tiscr);
            ViewBag.Tpag = new SelectList(db.TabTPag, "CodTPag", "DescrTPag", pag.Tpag);
            return View(pag);
        }

        // POST: Pags/Edit/5
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CodP,AnnoP,Tiscr,Cat,Pagato,Dovuto,DatPag,DatReg,DatCassa,Tpag,Fuori,Lode,Carica,Sez,Sett,Pos,Ent,CodSocCol")] Pag pag)
        {
            if (ModelState.IsValid)
            {
                db.Entry(pag).State = EntityState.Modified;
                db.SaveChanges();
                UpdateDovuto(pag.AnnoP, pag.CodP);
                ViewBag.Cat = new SelectList(db.TabCat.Where(m => m.AnnoCat == pag.AnnoP), "CodCat", "DescrCat", pag.Cat);
                ViewBag.Carica = new SelectList(db.TabCarica, "CodCarica", "DescrCarica", pag.Carica);
                ViewBag.Ent = new SelectList(db.TabEnt, "CodEnt", "DescrEnt", pag.Ent);
                ViewBag.Pos = new SelectList(db.TabPos, "CodPos", "DescrPos", pag.Pos);
                ViewBag.Sett = new SelectList(db.TabSet, "CodSet", "DescrSet", pag.Sett);
                ViewBag.Sez = new SelectList(db.TabSez, "CodSez", "DescrSez", pag.Sez);
                ViewBag.Tiscr = new SelectList(db.TabTIscr, "CodTIscr", "DescrTIscr", pag.Tiscr);
                ViewBag.Tpag = new SelectList(db.TabTPag, "CodTPag", "DescrTPag", pag.Tpag);
                TempData["msg-ok"] = "Aggiornamento effettuato";
            } else { TempData["msg"] = "Aggiornamento non riuscito"; }

            return RedirectToAction("Edit", new { codp = pag.CodP, annop = pag.AnnoP });
        }

        // GET: Pags/Delete/5
        public ActionResult Delete(int? codp, int? annop)
        {
            if (codp == null || annop == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Pag pag = db.Pag.Find(codp,annop);
            if (pag == null)
            {
                return HttpNotFound();
            }
            return View(pag);
        }

        // POST: Pags/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int codp, int annop)
        {
            Pag pag = db.Pag.Find(codp,annop);
            db.Pag.Remove(pag);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        
        // POST: Pags/DeleteDivisioni/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteDivisioni(int AnnoP, int CodP, string CodDiv)
        {
            Divisioni divisioni = db.Divisioni.Find(AnnoP, CodP, CodDiv);
            db.Divisioni.Remove(divisioni);
            db.SaveChanges();
            UpdateDovuto(AnnoP, CodP);
            TempData["msg-ok"] = "Cancellata Divisione " + divisioni.CodDiv.ToString();

            return RedirectToAction("Edit", new { codp = divisioni.CodP, annop = divisioni.AnnoP });
        }


        // POST: Pags/CreateDivisioni
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateDivisioni([Bind(Include = "AnnoP,CodP,CodDiv,Effettiva")] Divisioni divisioni)
        {
            if (ModelState.IsValid)
            {
                db.Divisioni.Add(divisioni);
                db.SaveChanges();
                UpdateDovuto(divisioni.AnnoP, divisioni.CodP);
                TempData["msg-ok"] = "Aggiunta Divisione " + divisioni.CodDiv.ToString();
            }

            return RedirectToAction("Edit", new { codp = divisioni.CodP, annop = divisioni.AnnoP });
        }

        // POST: Pags/DeleteGruppi/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteGruppi(int? AnnoP, int? CodP, string CodGru)
        {
            Gruppi gruppi = db.Gruppi.Find(AnnoP, CodP, CodGru);
            db.Gruppi.Remove(gruppi);
            db.SaveChanges();
            UpdateDovuto(gruppi.AnnoP, gruppi.CodP);
            TempData["msg-ok"] = "Cancellata Divisione";
            return RedirectToAction("Edit", new { codp = gruppi.CodP, annop = gruppi.AnnoP });
        }

        // POST: Pags/CreateGruppi
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGruppi([Bind(Include = "AnnoP,CodP,CodGru")] Gruppi gruppi)
        {
            if (ModelState.IsValid)
            {
                db.Gruppi.Add(gruppi);
                db.SaveChanges();
                UpdateDovuto(gruppi.AnnoP, gruppi.CodP);
                return RedirectToAction("Edit", new { codp = gruppi.CodP, annop = gruppi.AnnoP });
            }

            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", gruppi.CodP);
            ViewBag.AnnoP = new SelectList(db.TabGru, "AnnoGru", "NCodGru", gruppi.AnnoP);
            return RedirectToAction("Edit", new { codp = gruppi.CodP, annop = gruppi.AnnoP });
        }

        // POST: Pags/DeleteRiviste/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteRiviste(int AnnoP, int CodP, string CodRiv)
        {
            Riviste riviste = db.Riviste.Find(AnnoP, CodP, CodRiv);
            db.Riviste.Remove(riviste);
            db.SaveChanges();
            UpdateDovuto(AnnoP, CodP);
            return RedirectToAction("Edit", new { codp = riviste.CodP, annop = riviste.AnnoP });
        }

        // POST: Pags/CreateRiviste
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRiviste([Bind(Include = "AnnoP,CodP,CodRiv,Ncopie")] Riviste riviste)
        {
            if (ModelState.IsValid)
            {
                db.Riviste.Add(riviste);
                db.SaveChanges();
                UpdateDovuto(riviste.AnnoP, riviste.CodP);
                return RedirectToAction("Edit", new { codp = riviste.CodP, annop = riviste.AnnoP });
            }

            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", riviste.CodP);
            ViewBag.AnnoP = new SelectList(db.TabRiv, "AnnoRiv", "NCodRiv", riviste.AnnoP);
            return RedirectToAction("Edit", new { codp = riviste.CodP, annop = riviste.AnnoP });
        }

        // POST: Pags/DeleteConvenzioni/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConvenzioni(int? AnnoP, int? CodP, string CodConv)
        {
            Convenzioni convenzioni = db.Convenzioni.Find(AnnoP, CodP, CodConv);
            db.Convenzioni.Remove(convenzioni);
            db.SaveChanges();
            UpdateDovuto(convenzioni.AnnoP, convenzioni.CodP);
            return RedirectToAction("Edit", new { codp = convenzioni.CodP, annop = convenzioni.AnnoP });
        }

        // POST: Pags/CreateConvenzioni
        // Per proteggere da attacchi di overposting, abilitare le proprietà a cui eseguire il binding. 
        // Per ulteriori dettagli, vedere http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateConvenzioni([Bind(Include = "AnnoP,CodP,CodConv,Ncopie,DatConv")] Convenzioni convenzioni)
        {
            if (ModelState.IsValid)
            {
                db.Convenzioni.Add(convenzioni);
                db.SaveChanges();
                UpdateDovuto(convenzioni.AnnoP, convenzioni.CodP);
                return RedirectToAction("Edit", new { codp = convenzioni.CodP, annop = convenzioni.AnnoP });
            }
            ViewBag.CodP = new SelectList(db.Pag, "CodP", "Tiscr", convenzioni.CodP);
            ViewBag.AnnoP = new SelectList(db.TabConv, "AnnoConv", "DescrConv", convenzioni.AnnoP);
            return RedirectToAction("Edit", new { codp = convenzioni.CodP, annop = convenzioni.AnnoP });
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
