using System.Web;
using System.Web.Optimization;

namespace PlanetWeb
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //            "~/Scripts/jquery-1.10.2.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));


            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            //bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
            //            "~/Scripts/modernizr-*"));

            //bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
            //          "~/Scripts/bootstrap.js",
            //          "~/Scripts/respond.js"));
            bundles.Add(new StyleBundle("~/Content/login").Include(
                              "~/Content/bootstrap-social.css",
                              "~/Content/login.css"
                              ));
            bundles.Add(new StyleBundle("~/Content/register").Include(
                              "~/Content/dropdown.css",
                              "~/Content/perfect-scrollbar.min.css",
                              "~/Content/flags.css"
                              ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/flag-icon.min.css",
                      "~/Content/bootstrap-social.css",
                      "~/Content/site.css",
                      "~/Content/timeline.css",
                      "~/Content/dropdown.css",
                      "~/Content/chat.css",
                      "~/Content/flags.css",
                      "~/Content/customfont.css",
                      "~/Content/perfect-scrollbar.min.css",
                      "~/Content/simple-slider.css",
                      "~/Content/bootstrap-tour.min.css",
                      "~/Content/bootstrap-datetimepicker.min.css",
                      "~/Content/ItemsByCategory.css"));

            bundles.Add(new ScriptBundle("~/bundles/vm").Include(
             "~/Scripts/ViewModel/page-*",
             "~/Scripts/ViewModel/postcommentvm-*",
             "~/Scripts/ViewModel/friendvm-*",
             "~/Scripts/ViewModel/usernotificationvm-*",
             "~/Scripts/ViewModel/usertaskvm-*"
                ));
            bundles.Add(new ScriptBundle("~/bundles/login").Include(
           "~/Scripts/dropdown.js",
              "~/Scripts/perfect-scrollbar.min.js",
             "~/Scripts/ViewModel/loginvm-1.0.0.js"
                ));

            bundles.Add(new ScriptBundle("~/bundles/app").Include(
              "~/Scripts/dropdown.js",
              "~/Scripts/perfect-scrollbar.min.js",
                "~/Scripts/moment.min.js",
                "~/Scripts/numeral.min.js",
              "~/Scripts/bootstrap.touchspin.js",
              "~/Scripts/jquery.knob.js",
              "~/Scripts/simple-slider.min.js",
              "~/Scripts/bootstrap-datetimepicker.min.js",
              "~/Scripts/jquery.typing-0.2.0.min.js",
              "~/Scripts/jquery.bootstrap-growl.min.js",
              "~/Scripts/bootstrap-tour.min.js"
              ));

            //bundles.Add(new ScriptBundle("~/bundles/knockout").Include(
            //"~/Scripts/knockout-3.0.0.js",
            //"~/Scripts/knockout.mapping-latest.js"

            //));

        }
    }
}
