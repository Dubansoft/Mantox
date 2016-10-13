using System.Web;
using System.Web.Optimization;

namespace MantoxWebApp
{
    public class BundleConfig
    {
        // Para obtener más información sobre Bundles, visite http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/adminLte/css/bootstrap/").Include(
                "~/Content/adminLte/bootstrap/css/bootstrap.min.css"));

            bundles.Add(new StyleBundle("~/adminLte/css/fontawesome/").Include(
                "~/Content/adminLte/font-awesome-master/css/font-awesome.min.css"));

            bundles.Add(new StyleBundle("~/adminLte/css/ionicons/").Include(
                "~/Content/adminLte/ionicons/css/ionicons.min.css"));

            bundles.Add(new StyleBundle("~/adminLte/css/adminLte/").Include(
                "~/Content/adminLte/dist/css/AdminLTE.min.css"));

            bundles.Add(new StyleBundle("~/adminLte/css/adminLteSkin/").Include(
                "~/Content/adminLte/dist/css/skins/_all-skins.min.css"));

            bundles.Add(new StyleBundle("~/adminLte/css/icheck/").Include(
                "~/Content/adminLte/plugins/iCheck/square/blue.css"));

            bundles.Add(new StyleBundle("~/adminLte/css/select2/").Include(
                "~/Content/adminLte/plugins/select2/select2.min.css",
                "~/Content/adminLte/plugins/select2/select2.custom.css"));


            bundles.Add(new StyleBundle("~/adminLte/css/jqGrid/").Include(
                "~/Content/adminLte/jqtable/assets/css/ui.jqgrid.min.css",
                "~/Content/adminLte/jqtable/assets/css/ace.min - copia.css"));

            bundles.Add(new StyleBundle("~/adminLte/css/Datepicker/").Include(
               "~/Content/adminLte/plugins/datepicker/datepicker3.css"));





            bundles.Add(new ScriptBundle("~/adminLte/js/htmlshiv/").Include(
                "~/Content/adminLte/plugins/html5shiv/dist/html5shiv.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/respondjs/").Include(
                "~/Content/adminLte/plugins/respond/dest/respond.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/jquery/").Include(
                "~/Content/adminLte/plugins/jQuery/jquery-2.2.3.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/bootstrap/").Include(
                "~/Content/adminLte/bootstrap/js/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/slimscroll/").Include(
                "~/Content/adminLte/plugins/slimScroll/jquery.slimscroll.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/fastclick/").Include(
                "~/Content/adminLte/plugins/fastclick/fastclick.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/lteapp/").Include(
                "~/Content/adminLte/dist/js/app.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/select2/").Include(
                "~/Content/adminLte/plugins/select2/select2.full.min.js"));

            bundles.Add(new StyleBundle("~/adminLte/js/icheck/").Include(
                "~/Content/adminLte/plugins/iCheck/icheck.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/validate/").Include(
                "~/Content/adminLte/plugins/validate/jquery.validate.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/msAjax/").Include(
                "~/Content/adminLte/plugins/msAjax/jquery.unobtrusive-ajax.min.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/jqGrid/").Include(
                "~/Content/adminLte/jqtable/assets/js/jquery.jqGrid.min.js",
                "~/Content/adminLte/jqtable/assets/js/grid.locale-en.js",
                "~/Content/adminLte/jqtable/assets/js/jquery.table.defaultSetup.js"));

            bundles.Add(new ScriptBundle("~/adminLte/js/Datepicker/").Include(
              "~/Content/adminLte/plugins/datepicker/bootstrap-datepicker.js"));

            

        }


    }

}