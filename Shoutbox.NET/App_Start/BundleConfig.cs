using System.Web;
using System.Web.Optimization;

namespace Shoutbox.NET
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new StyleBundle("~/bundles/css").Include(
                    "~/Content/bootstrap.css",
                    "~/Content/gridstack.min.css",
                    "~/Content/font-awesome.min.css",
                    "~/Content/tooltipster.bundle.min.css",
                    "~/Content/tooltipster-sideTip-light.min.css",
                    "~/Content/sweetalert2.min.css",
                    "~/Content/Rabo.css",
                    "~/Content/Chat.css",
                    "~/Content/Shoutbox.css",
                    "~/Content/Sststatus.css",
                    "~/Content/tagcloud.css",
                    "~/Content/Team.css",
                    "~/Content/Masterincidents.css",
                    "~/Content/SOS.css",
                    "~/Content/settings.css",
                    "~/Content/TOPKM.css"));

            bundles.Add(new ScriptBundle("~/bundles/javascript").Include(
                    "~/Scripts/jquery.timeago.js",
                    "~/Scripts/jquery.timeago.nl.js",
                    "~/Scripts/jquery.tagcloud.js",
                    "~/Scripts/core.min.js",
                    "~/Scripts/sweetalert2.min.js",
                    "~/Scripts/tooltipster.bundle.min.js",
                    "~/Scripts/jquery-{version}.js",
                    "~/Scripts/jquery-ui-{version}.js",
                    "~/Scripts/chattile.js",
                    "~/Scripts/tagcloud.js",
                    "~/Scripts/team.js",
                    "~/Scripts/sststatus.js",
                    "~/Scripts/masterincidents.js",
                    "~/Scripts/Preload.js",
                    "~/Scripts/SOS.js",
                    "~/Scripts/settings.js",
                    "~/Scripts/KM.js",
                    "~/Scripts/bootstrap.js",
                    "~/Scripts/respond.js",
                    "~/Scripts/lodash.min.js",
                    "~/Scripts/gridstack.min.js",
                    "~/Scripts/jquery.nicescroll.min.js",
                    "~/Scripts/datepicker.min.js",
                    "~/Scripts/jquery.signalR*"));

        }
    }
}
