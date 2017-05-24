using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using sci.Models;
using Microsoft.Reporting.WebForms;
using System.IO;


namespace sci.Controllers
{


    public class RptMemsController : Controller
    {
        List<SearchFieldMutator<Mem, MemsSearchViewModel>> SearchFieldMutators { get; set; }

        private sci_newEntities db = new sci_newEntities();        // GET: RptMems
        public ActionResult Index()
        {

            var mems = from m in db.Mem select m;
            mems = mems.Where(m => m.Cognome.Contains("Sco"));
            return View();
        }
        public ActionResult Rpt(string id, string query, string reportname, string filter, string orderby)

        {
            ViewBag.reportname = reportname;
            ViewBag.query = query;
            ViewBag.filter = filter;
            ViewBag.orderby = orderby;

            var mems = from m in db.Q_Pag_Mem select m;
            
            
//                        filter = "Annop = 2016 And Cat = \"J\" And DatReg = DateTime(2016, 1, 14)";
//            filter = "Annop = 2016 And Cat = \"J\" And DatReg = \"20160114\"";
            var x = Convert.ToDateTime("14/01/2016").Date.Year;
            var y = Convert.ToDateTime("14/01/2016").Date.Month;
            var z = Convert.ToDateTime("14/01/2016").Date.Day;
            mems = mems.Where(filter);
            mems = mems.OrderBy(orderby);

            if (id != "PDF" && id != "Excel" && id != "Word")
            {
                return View(mems);
            }

            LocalReport lr = new LocalReport();
            string path = Path.Combine(Server.MapPath("~/RptReports"), reportname);

            // esempio report a matrice 
            // string path = Path.Combine(Server.MapPath("~/RptReports"), "Report2.rdlc");
            
            if (System.IO.File.Exists(path))
            {
                lr.ReportPath = path;
            }
 
            ReportDataSource rd = new ReportDataSource(query, mems);
 //           ReportDataSource rd = new ReportDataSource("DataSet1", mems);
            // dataset per report2.rdlc            ReportDataSource rd = new ReportDataSource("DataSet1", mems);
            lr.DataSources.Add(rd);

            string reportType = id;
            string mimeType;
            string encoding;
            string fileNameExtension;
            string deviceInfo =
            "<DeviceInfo>" +
            "  <OutputFormat>" + id +"</OutputFormat>" +
            "  <PageWidth>21cm</PageWidth>" +
            "  <PageHeight>29</PageHeight>" +
            "  <MarginTop>1cm</MarginTop>" +
            "  <MarginLeft>1cm</MarginLeft>" +
            "  <MarginRight>1cm</MarginRight>" +
            "  <MarginBottom>1cm</MarginBottom>" +
            "</DeviceInfo>"
            ;

            Warning[] warnings;
            string[] streams;
            byte[] renderedBytes;

            renderedBytes = lr.Render(
                reportType,
                deviceInfo,
                out mimeType,
                out encoding,
                out fileNameExtension,
                out streams,
                out warnings
                );

            return File(renderedBytes, mimeType);
        }
    }

}