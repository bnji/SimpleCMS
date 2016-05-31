using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace SimpleCMS
{
    public class SimpleCMSBundleConfig
    {
        public static void LoadMobileBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/jqueryMobile")
                .Include("~/SimpleCMS/Content/jqueryMobile/jquery.mobile-{version}.js"));

            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                        "~/SimpleCMS/Scripts/jquery-{version}.js"));

            bundles.Add(new StyleBundle("~/css/Mobile")
                .Include("~/SimpleCMS/Content/Site.Mobile.css"));

            bundles.Add(new StyleBundle("~/css/jqueryMobile")
                .Include("~/SimpleCMS/Content/jqueryMobile/jquery.mobile-{version}.css")
                .Include("~/SimpleCMS/Content/jqueryMobile/jquery.mobile.structure-{version}.css")
                .Include("~/SimpleCMS/Content/jqueryMobile/jquery.mobile.theme-{version}.css")
                );
        }

        public static void LoadBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js/datatables").Include(
                        "~/SimpleCMS/Scripts/DataTables/jquery.dataTables.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.bootstrap.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.autoFill.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.buttons.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.responsive.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.colReorder.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.rowReorder.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.fixedColumns.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.fixedHeader.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.scroller.js",
                        "~/SimpleCMS/Scripts/DataTables/dataTables.select.js",
                        "~/SimpleCMS/Scripts/DataTables/autoFill.bootstrap.js",
                        "~/SimpleCMS/Scripts/DataTables/buttons.bootstrap",
                        "~/SimpleCMS/Scripts/DataTables/responsive.bootstrap.js"
                        ));


            bundles.Add(new StyleBundle("~/css/datatables").Include(
                      "~/SimpleCMS/Content/DataTables/css/dataTables.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/autoFill.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/buttons.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/responsive.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/colReorder.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/fixedColumns.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/fixedHeader.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/keyTable.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/rowReorder.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/scroller.bootstrap.css",
                      "~/SimpleCMS/Content/DataTables/css/select.bootstrap.css"
                      ));


            bundles.Add(new ScriptBundle("~/js/jquery").Include(
                        "~/SimpleCMS/Scripts/jquery-{version}.js"));

            // http://jqueryvalidation.org/
            bundles.Add(new ScriptBundle("~/js/jqueryval").Include(
                        "~/SimpleCMS/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/js/signalr").Include(
                        "~/SimpleCMS/Scripts/jquery.signalR-*"));

            //bundles.Add(new ScriptBundle("~/js/jqueryui").Include(
            //            "~/SimpleCMS/Scripts/jquery-ui-{version}.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/js/modernizr").Include(
                        "~/SimpleCMS/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/js/serialize").Include(
                        "~/SimpleCMS/Scripts/jquery.serialize-object.js",
                        "~/SimpleCMS/Scripts/jquery.todictionary.js"));

            // https://github.com/jquery/globalize  
            //bundles.Add(new ScriptBundle("~/js/globalize").Include(
            //        "~/SimpleCMS/Scripts/globalize/globalize.js"));

            // https://github.com/scottjehl/Respond
            bundles.Add(new ScriptBundle("~/js/bootstrap").Include(
                      "~/SimpleCMS/Scripts/bootstrap.js",
                      "~/SimpleCMS/Scripts/respond.js",
                      "~/SimpleCMS/Scripts/bootstrap-dashboard.js"));

            // https://github.com/eternicode/bootstrap-datepicker/
            //bundles.Add(new ScriptBundle("~/js/bootstrap-datepicker").Include(
            //          "~/SimpleCMS/Scripts/bootstrap-datepicker.js",
            //          "~/SimpleCMS/Scripts/bootstrap-datepicker-globalize.js",
            //          "~/SimpleCMS/Scripts/locales/bootstrap-datepicker.da.js",
            //          "~/SimpleCMS/Scripts/locales/bootstrap-datepicker.fo.js"));

            // http://underscorejs.org/
            bundles.Add(new ScriptBundle("~/js/underscore").Include(
                      "~/SimpleCMS/Scripts/underscore.js",
                      "~/SimpleCMS/Scripts/underscore.string.js"));

            // https://github.com/douglascrockford/JSON-js
            bundles.Add(new ScriptBundle("~/js/json2").Include(
                    "~/SimpleCMS/Scripts/json2.js"));

            // http://fancyapps.com/fancybox/
            bundles.Add(new ScriptBundle("~/js/fancybox").Include(
                    "~/SimpleCMS/Scripts/jquery.fancybox.js",
                    "~/SimpleCMS/Scripts/jquery.fancybox-buttons.js",
                    "~/SimpleCMS/Scripts/jquery.fancybox-media.js",
                    "~/SimpleCMS/Scripts/jquery.fancybox-thumbs.js"
                    ));

            bundles.Add(new ScriptBundle("~/js/cms.general").Include(
                      "~/SimpleCMS/Scripts/cms.general.js"
                //"~/SimpleCMS/Scripts/cms.knockout.extensions.js"
                      ));

            bundles.Add(new ScriptBundle("~/js/cms-knockout-extensions").Include(
                      "~/SimpleCMS/Scripts/cms.knockout.extensions.js"));

            bundles.Add(new ScriptBundle("~/js/cms.file").Include(
                      "~/SimpleCMS/Scripts/cms.file.upload.handler.js"
                      ));

            // http://www.openjs.com/scripts/events/keyboard_shortcuts/
            // https://github.com/tzuryby/jquery.hotkeys
            bundles.Add(new ScriptBundle("~/js/shortcuts").Include(
                      "~/SimpleCMS/Scripts/shortcut.js"
                // bug: jquery.hotkeys.js somehow breaks kendo ui autocomplete, but only searches (works) on copy paste.
                //"~/SimpleCMS/Scripts/jquery.hotkeys.js"
                      ));

            //bundles.Add(new ScriptBundle("~/js/editable").Include(
            //            "~/SimpleCMS/Content/bootstrap3-editable/js/bootstrap-editable.js"));

            bundles.Add(new ScriptBundle("~/js/knockout").Include(
                        "~/SimpleCMS/Scripts/knockout-{version}.js",//3.2.0
                        "~/SimpleCMS/Scripts/knockout.mapping-latest.js"));

            bundles.Add(new ScriptBundle("~/js/sammy").Include(
                        "~/SimpleCMS/Scripts/sammy-{version}.js"));//0.7.5

            //bundles.Add(new ScriptBundle("~/js/fuelux").Include(
            //            "~/SimpleCMS/Content/fuelux/js/fuelux.js"));

            // http://momentjs.com/
            bundles.Add(new ScriptBundle("~/js/moment").Include(
                        "~/SimpleCMS/Scripts/moment-with-locales.js"));

            bundles.Add(new ScriptBundle("~/js/linq").Include(
                        "~/SimpleCMS/Scripts/rx.lite.js"
                        ));

            bundles.Add(new ScriptBundle("~/js/maskedinput").Include(
                        "~/SimpleCMS/Scripts/jquery.maskedinput.js"
                        ));

            //bundles.Add(new ScriptBundle("~/js/inputmask").Include(
            //            "~/SimpleCMS/Scripts/jquery.inputmask/jquery.inputmask.js",
            //            "~/SimpleCMS/Scripts/jquery.inputmask/jquery.inputmask.extensions.js",
            //            "~/SimpleCMS/Scripts/jquery.inputmask/jquery.inputmask.date.extensions.js",
            //            "~/SimpleCMS/Scripts/jquery.inputmask/jquery.inputmask.numeric.extensions.js"));

            //bundles.Add(new ScriptBundle("~/js/datatable").Include(
            //            "~/SimpleCMS/Scripts/DataTables-1.10.3/jquery.dataTables.js"));

            //bundles.Add(new StyleBundle("~/css/datatable").Include(
            //          "~/SimpleCMS/Content/DataTables-1.10.3/css/jquery.dataTables.css"));

            //bundles.Add(new StyleBundle("~/SimpleCMS/Content/css").Include("~/SimpleCMS/Content/site.css"));
            bundles.Add(new StyleBundle("~/css/cms.general").Include(
                      "~/SimpleCMS/Content/general.css"));

            bundles.Add(new StyleBundle("~/css/fancybox").Include(
                      "~/SimpleCMS/Content/jquery.fancybox.css",
                      "~/SimpleCMS/Content/jquery.fancybox-buttons.css",
                      "~/SimpleCMS/Content/jquery.fancybox-thumbs.css"
                      ));

            bundles.Add(new StyleBundle("~/css/bootstrap").Include(
                      "~/SimpleCMS/Content/bootstrap/bootstrap.css",
                      "~/SimpleCMS/Content/bootstrap-extensions.css",
                //"~/SimpleCMS/Content/bootstrap-theme.css",
                      "~/SimpleCMS/Content/bootstrap-dashboard.css"));

            //bundles.Add(new StyleBundle("~/css/bootstrap-datepicker").Include(
            //          "~/SimpleCMS/Content/bootstrap-datepicker.css"));

            //bundles.Add(new StyleBundle("~/css/jqueryui").Include(
            //            "~/SimpleCMS/Content/themes/base/jquery.ui.css",
            //            "~/SimpleCMS/Content/themes/base/jquery.ui.core.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.resizable.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.selectable.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.accordion.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.autocomplete.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.button.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.dialog.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.slider.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.tabs.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.datepicker.css",
            //    //"~/SimpleCMS/Content/themes/base/jquery.ui.progressbar.css",
            //            "~/SimpleCMS/Content/themes/base/jquery.ui.theme.css"
            //            ));

            //bundles.Add(new StyleBundle("~/css/pagedlist").Include(
            //            "~/SimpleCMS/Content/PagedList.css"));

            //bundles.Add(new StyleBundle("~/css/editable").Include(
            //            "~/SimpleCMS/Content/bootstrap3-editable/css/bootstrap-editable.css"));

            //bundles.Add(new StyleBundle("~/css/fuelux").Include(
            //            "~/SimpleCMS/Content/fuelux/css/fuelux.css"));

            bundles.Add(new ScriptBundle("~/js/kendoaspnetmvc").Include(
            //"~/SimpleCMS/Content/kendo/js/kendo.all.min.js",
            "~/SimpleCMS/Content/kendo/js/kendo.ui.core.min.js",
                // "~/SimpleCMS/Scripts/kendo/kendo.timezones.min.js", // uncomment if using the Scheduler
            "~/SimpleCMS/Content/kendo/js/kendo.culture.da.min.js",
            "~/SimpleCMS/Content/kendo/js/kendo.culture.en.min.js",
            "~/SimpleCMS/Content/kendo/js/kendo.culture.fo.min.js",
            "~/SimpleCMS/Content/kendo/js/kendo.aspnetmvc.min.js"));

            bundles.Add(new StyleBundle("~/Content/kendo/styles/kendocss").Include(
            "~/SimpleCMS/Content/kendo/css/kendo.common.min.css",
            "~/SimpleCMS/Content/kendo/css/kendo.default.min.css",
            "~/SimpleCMS/Content/kendo/css/kendo.dataviz.min.css",
            "~/SimpleCMS/Content/kendo/css/kendo.dataviz.default.min.css",
            "~/SimpleCMS/Content/kendo/css/kendo.dataviz.mobile.min.css",
            "~/SimpleCMS/Content/kendo/css/kendo.common-bootstrap.min.css",
            "~/SimpleCMS/Content/kendo/css/kendo.uniform.min.css"));

            bundles.Add(new StyleBundle("~/SimpleCMS/Content/fontawesome").Include(
            "~/SimpleCMS/Content/font-awesome.css"));

            bundles.Add(new ScriptBundle("~/js/summernote").Include(
            "~/SimpleCMS/Content/summernote-editor/summernote.js"));

            bundles.Add(new StyleBundle("~/css/summernote").Include(
            "~/SimpleCMS/Content/summernote-editor/summernote.css"));

            // Set EnableOptimizations to false for debugging. For more information,
            // visit http://go.microsoft.com/fwlink/?LinkId=301862
            // If is not debug mode then enable optimizations
            BundleTable.EnableOptimizations = !General.Helpers.Debug.IsDebugMode;
        }
    }
}