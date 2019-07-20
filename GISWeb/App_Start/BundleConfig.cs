﻿using System.Web.Optimization;

namespace GISWeb.App_Start
{
    public class BundleConfig
    {

        public static void RegisterBundles(BundleCollection bundles)
        {
            
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jQuery/jquery-{version}.js"));





            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jQuery/jquery.validate*",
                        "~/Scripts/jQuery/jquery.unobtrusive*"));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/jQuery/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/ace_js_master").Include(
                        "~/Scripts/Ace/bootstrap.js",
                        "~/Scripts/Ace/bootbox.js",
                        "~/Scripts/Ace/ace.js",
                        "~/Scripts/Ace/ace.sidebar.js",
                        "~/Scripts/Ace/ace.sidebar-scroll-1.js",
                        "~/Scripts/Ace/elements.scroller.js",
                        "~/Scripts/Ace/ace.ajax-content.js",
                        "~/Scripts/Ace/elements.aside.js",
                        "~/Scripts/Custom/jquery.ui.datepicker-pt-BR.js",
                        "~/Scripts/Ace/jQ/jquery-ui.js",
                        "~/Scripts/Ace/jQ/jquery-ui.custom.js",
                        "~/Scripts/Ace/jQ/jquery.ui.touch-punch.js",                        
                        "~/Scripts/Ace/jQ/jquery.gritter.js"));


            bundles.Add(new ScriptBundle("~/bundles/ace_js_extra").Include(
                        "~/Scripts/Ace/ace-extra.js"));

            bundles.Add(new StyleBundle("~/bundles/ace_css_master").Include(
                        "~/Content/Ace/css/bootstrap.css",
                        "~/Content/Ace/FontAwesome/css/font-awesome.css",
                        "~/Content/Ace/css/jquery-ui.custom.css",
                        "~/Content/Ace/css/jquery-ui.css",
                        "~/Content/Ace/css/jquery.gritter.css",
                        "~/Content/Ace/css/ace-fonts.css",
                        "~/Content/Ace/css/ace.css"));





            bundles.Add(new ScriptBundle("~/bundles/js_common_end").Include(
                        "~/Scripts/Ace/ace.widget-box.js",
                        "~/Scripts/Ace/chosen.jquery.js",
                        "~/Scripts/Ace/bootstrap-multiselect.js",
                        "~/Scripts/Ace/moment.js",
                        "~/Scripts/Ace/daterangepicker.js",
                        "~/Scripts/Ace/bootstrap-datepicker.js",
                        "~/Scripts/Ace/bootstrap-tag.js",
                        "~/Scripts/Ace/elements.typeahead.js",
                        "~/Scripts/Ace/jQ/jquery.maskedinput.js",
                        "~/Scripts/Ace/jQ/jquery.nestable.js",
                        "~/Scripts/Ace/jQ/jquery.colorbox.js"));

            bundles.Add(new StyleBundle("~/bundles/css_common_end").Include(
                        "~/Content/Ace/css/chosen.css",
                        "~/Content/Ace/css/bootstrap-multiselect.css",
                        "~/Content/Ace/css/daterangepicker.css",
                        "~/Content/Ace/css/datepicker.css",
                        "~/Content/Ace/css/colorbox.css"));




            bundles.Add(new ScriptBundle("~/bundles/jQ_dataTable").Include(
                        "~/Scripts/Ace/dataTables/jquery.dataTables.js",
                        "~/Scripts/Ace/dataTables/jquery.dataTables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jQ_tableTools").Include(
                        "~/Scripts/Ace/dataTables/dataTables.tableTools.js",
                        "~/Scripts/Ace/dataTables/dataTables.colVis.js"));

            bundles.Add(new ScriptBundle("~/bundles/ace_js_fileUpload").Include(
                        "~/Scripts/Ace/dropzone.js"));

            bundles.Add(new StyleBundle("~/bundles/ace_css_fileUpload").Include(
                        "~/Content/Ace/css/dropzone.css"));

            bundles.Add(new ScriptBundle("~/bundles/croppie_js").Include(
                        "~/Scripts/Croppie/croppie.js",
                        "~/Scripts/Ace/elements.fileinput.js"));

            bundles.Add(new StyleBundle("~/bundles/croppie_css").Include(
                        "~/Content/Croppie/croppie.css"));
            bundles.Add(new StyleBundle("~/bundles/inputmask").Include(
                        "~/Content/jQuery/inputmask"));


            bundles.Add(new ScriptBundle("~/bundles/inbox/pessoal_js").Include(
                        "~/Scripts/Custom/Inbox/Inbox.js",
                        "~/Scripts/Custom/Inbox/Pessoal.js"));

            bundles.Add(new ScriptBundle("~/bundles/inbox/grupos_js").Include(
                        "~/Scripts/Custom/Inbox/Inbox.js",
                        "~/Scripts/Custom/Inbox/MeusGrupos.js"));

            bundles.Add(new ScriptBundle("~/bundles/custom/profile").Include(
                        "~/Scripts/Custom/Account/custom-profile.js"));

        }

    }
}