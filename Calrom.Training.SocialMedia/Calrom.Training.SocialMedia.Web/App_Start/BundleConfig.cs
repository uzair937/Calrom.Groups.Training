using System.Web;
using System.Web.Optimization;

namespace Calrom.Training.SocialMedia.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/site.css"));
            bundles.Add(new ScriptBundle("~/Content/account/scripts").Include("~/Content/Scripts/account.js"));
            bundles.Add(new ScriptBundle("~/Content/home/scripts").Include("~/Content/Scripts/home.js"));
            bundles.Add(new ScriptBundle("~/scripts").Include("~/Scripts/jquery-3.4.1.min.js"));
        }
    }
}
