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
        }
    }
}