using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace sci.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autherize(sci.Models.Credenziali CredenzialiModel)
        {
            using (Models.sci_newEntities1 db = new Models.sci_newEntities1())
            {
                var userDetails = db.Credenziali.Where(x => x.username == CredenzialiModel.username && x.password == CredenzialiModel.password).FirstOrDefault();
                if (userDetails == null)
                {
                    CredenzialiModel.LoginErrorMessage = "Username o password errati";
                    
                    return View("Index", CredenzialiModel);
                }
                else
                {
                    Session["UserID"] = userDetails.UserID;
                    Session["Username"] = userDetails.username;
                    return RedirectToAction("Index", "Home");

                    int userid = (int)Session["UserID"];
                }

            }
        }

        public ActionResult LogOut()
        {

            Session.Abandon();
            return RedirectToAction("Index", "Login");
        }
    }
}