using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;

namespace NewsPortal
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            //StyleSheet
            bundles.Add(new StyleBundle("~/bundles/stylesheet-Site").Include("~/Content/Site.css"));
            bundles.Add(new StyleBundle("~/bundles/stylesheet-Basic").Include("~/Content/StyleSheet_BasicFormatting.css"));
            bundles.Add(new StyleBundle("~/bundles/stylesheet-Help").Include("~/Content/StyleSheet_HelpStyles.css"));
            bundles.Add(new StyleBundle("~/bundles/stylesheet-Home").Include("~/Content/StyleSheet_HomePage.css"));
            bundles.Add(new StyleBundle("~/bundles/stylesheet-News").Include("~/Content/StyleSheet_NewsPage.css"));
            bundles.Add(new StyleBundle("~/bundles/stylesheet-Service").Include("~/Content/StyleSheet_ServicePages.css"));
            //Script
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include("~/Scripts/jquery.validate*"));

            //bundles.Add(new ScriptBundle("~/bundles/jquery-min").Include("~/Scripts/jquery-3.2.1.min.js"));
            //bundles.Add(new ScriptBundle("~/bundles/jquery-News").Include("~/Scripts/jquery.signalR-2.2.2.min.js*"));
            //bundles.Add(new ScriptBundle("~/bundles/jquery-Hubs").Include("~/signalr/hubs"));
            //bundles.Add(new ScriptBundle("~/bundles/jquery-Comment").Include("~/Scripts/util.js"));
        }
    }
}