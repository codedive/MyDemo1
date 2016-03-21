using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using URFX.Web.Infrastructure;

namespace URFX.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AutoMapperWebApiProfile.Run();
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            TaskManager.Initialize(new MyRegistry());
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.PreserveReferencesHandling =
            //    Newtonsoft.Json.PreserveReferencesHandling.All;
        }
        protected void Application_BeginRequest()
        {
            string language = "en";

            var cookie = Request.Cookies.Get("APPLICATION_LANGUAGE");
            if (cookie != null)
                language = cookie.Value;

            Thread.CurrentThread.CurrentCulture = new CultureInfo(language);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
        }
        protected void Application_End()
        {
            TaskManager.Stop();
            
            
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Elmah.ErrorSignal.FromCurrentContext().Raise(Server.GetLastError());
        }

    }
}
