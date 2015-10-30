using System.Web.Optimization;

namespace URFX.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));
            bundles.Add(new ScriptBundle("~/bundles/angular").Include(
                     "~/Scripts/angular.js",
                     "~/Scripts/ui-router.min.js","~/Scripts/angular-animate.js",
                     "~/Scripts/angular-sanitize.js", "~/Scripts/angular-local-storage.min.js", 
                     "~/Scripts/angular-resource.js", "~/Scripts/angular-cookies.js", "~/Scripts/angular-validation.js", "~/Scripts/angular-validation-rule.js")); //"~/Scripts/angular-translate-loader-url.js", "~/Scripts/angular-translate.js"
            bundles.Add(new ScriptBundle("~/bundles/app").Include(
                  "~/app/app.core.js"));

            bundles.Add(new ScriptBundle("~/bundles/Services").Include(
                    "~/app/Services/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Controllers").Include(
                    "~/app/Controllers/*.js"));

            bundles.Add(new ScriptBundle("~/bundles/Directives").Include(
                    "~/app/Directives/*.js"));
            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css", "~/Content/URFXTheme/css/bootstrap.min.css", "~/Content/URFXTheme/css/style.css", 
                      "~/Content/URFXTheme/css/responsive.css", "~/Content/URFXTheme/css/font-awesome.min.css", 
                      "~/Content/URFXTheme/css/animate.css", "~/Content/URFXTheme/css/simple-sidebar.css", "~/Content/URFXTheme/css/sb-admin.css",
                      "~/Content/main.css"));
        }
    }
}
