using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using URFX.Data.Resources;

namespace URFX.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }
        public ActionResult Home()
        {
            string language = "en";
            if (Request.QueryString["ln"] != null)
            {
                language = Request.QueryString["ln"];                               
            }
            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            
            
            if (User != null && User.Identity.IsAuthenticated)
            {
                return View("Index");
            }
            ViewBag.Title = "Home Page";
            return View();
        }
        public ActionResult FAQ()
        {
            ViewBag.Title = "FAQ Page";

            return View();
        }
        public ActionResult Help()
        {
            ViewBag.Title = "Help Page";

            return View();
        }
        public ActionResult TermsOfUse()
        {
            ViewBag.Title = "TermsOfUse Page";

            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Title = "Privacy Policy";

            return View();
        }
        public ActionResult NotFound()
        {
            return View("NotFound");
        }
    }
}
