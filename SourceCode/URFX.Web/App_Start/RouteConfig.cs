using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace URFX.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {            
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            //  routes.IgnoreRoute("");            
            routes.MapRoute(
                name: "Application_Home",
                url: "App",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Home",
                url: "",
                defaults: new { controller = "Home", action = "Home", id = UrlParameter.Optional }
            );
            routes.MapRoute(
               name: "Application_FAQ",
               url: "FAQ",
               defaults: new { controller = "Home", action = "FAQ", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Application_Help",
               url: "HelpPage",
               defaults: new { controller = "Home", action = "Help", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Application_TermsOfUse",
               url: "TermsOfUse",
               defaults: new { controller = "Home", action = "TermsOfUse", id = UrlParameter.Optional }
           );
            routes.MapRoute(
               name: "Application_PrivacyPolicy",
               url: "PrivacyPolicy",
               defaults: new { controller = "Home", action = "PrivacyPolicy", id = UrlParameter.Optional }
           );

            routes.MapRoute(
            "PageNotFound",
            "{*url}",
            new { controller = "Home", action = "NotFound" }
        );
            routes.LowercaseUrls = true;
        }
    }
}
