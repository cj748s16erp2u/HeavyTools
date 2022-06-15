<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="Site.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><% Response.Write(eProjectWeb.Framework.ModuleRegistry.AppTitle); %></title>

    <meta charset="utf-8" />
    <!-- Viewport mobile tag for sensible mobile support -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta name="description" content="-" />
    <meta http-equiv="x-ua-compatible" content="IE=edge" />
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />

    <link rel="SHORTCUT ICON" href="favicon.ico" />
    <% if (!jsDevelop())
        { %>
    <link rel="stylesheet" href="Styles/style.min.css" />
    <% if (eProjectWeb.Framework.Globals.UseCODAStyle)
        { %>
    <link rel="stylesheet" href="Styles/CODAstyle.min.css" />
    <% } %>
    <% }
        else
        { %>
    <link rel="stylesheet" href="Styles/style.css" />
    <% if (eProjectWeb.Framework.Globals.UseCODAStyle)
        { %>
    <link rel="stylesheet" href="Styles/CODAstyle.css" />
    <% } %>
    <% } %>
    <%--<link rel="stylesheet" href="Styles/theme-default.css" />--%>
    <%--<link rel="stylesheet" href="Styles/ball-atom.min.css" />--%>
    <link rel="stylesheet" href="Styles/ball-spin-clockwise.min.css" />
    <link rel="stylesheet" href="Styles/Colorizer.css" />
    <% if (HasCustomCSS())
        {
            Response.Write("<link rel=\"stylesheet\" href=\"Custom.css\" />");
        } %>
</head>
<body>
    <div class="navbar">
        <div class="container">

            <div class="navbar-clock"><span id="clockdate"></span><span id="clocktime"></span></div>

            <div class="navbar-collapse collapse">
                <div class="nav navbar-header">
                    <% if (!eProjectWeb.Framework.Globals.UseCODAStyle)
                        { %>
                    <div><a class="navbar-brand"><% Response.Write(eProjectWeb.Framework.ModuleRegistry.AppTitle); %></a></div>
                    <div>
                        <div class="navbar-version"><% Response.Write(eProjectWeb.Framework.ModuleRegistry.AppVersion); %></div>
                    </div>
                    <% }
                        else
                        { %>
                    <img class="x-img u4-applicationlogo-image x-img-default" src="images/logo_unit4codafinancials.png" />
                    <% } %>
                </div>
                <div class="nav navbar-nav navbar-right">
                    <div>
                        <div class="navbar-login-info">
                            <% try { Response.Write(eProjectWeb.Framework.Session.UserID + " / <span class=\"navbar-company\" title=\"" + eProjectWeb.Framework.Session.CompanyCodes + "\">" + eProjectWeb.Framework.Session.CompanyCodes + "</span> (" + eProjectWeb.Framework.Session.Catalog + ")"); }
                                catch { } %>
                        </div>
                    </div>
                    <div><a class="navbar-logout" href="#"><% Response.Write(eProjectWeb.Framework.Lang.Translator.Translate("$logout")); %></a></div>
                </div>
            </div>
        </div>
    </div>

    <% if (!eProjectWeb.Framework.Globals.UseCODAStyle)
        {
            Response.Write("<div id=\"treeMenuContainer\"></div>");
        }%>

    <div id="pageMaster">
        <% if (eProjectWeb.Framework.Globals.UseCODAStyle)
            {
                Response.Write("<div id=\"codaMenuContainer\"></div>");
            }%>
    </div>

    <!-- normal: Mozilla/5.0 (Windows NT 6.3; WOW64; Trident/7.0; rv:11.0) like Gecko -->
    <!-- compatible: Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 6.3; WOW64; Trident/7.0; .NET4.0E; .NET4.0C; .NET CLR 3.5.30729; .NET CLR 2.0.50727; .NET CLR 3.0.30729) -->
    <%--<% if (!Request.UserAgent.Contains("MSIE"))
       {
           Response.Write("<script src=\"Scripts/vendor/jQuery-2.1.3.min.js\"></script>");
       }
       else
       {
           Response.Write("<script src=\"Scripts/vendor/jQuery-1.11.3.min.js\"></script>");
           Response.Write("<script src=\"Scripts/vendor/jQuery.json.min.js\"></script>");
       } %>--%>

    <script src="Scripts/vendor/jQuery-3.1.0.min.js"></script>
    <script src="Scripts/vendor/modernizr-3.3.1.min.js"></script>
    <!-- validation -->
    <script src="Scripts/vendor/validator/jQuery.form-validator.min.js"></script>
    <!-- jQuery UI plugins -->
    <script src="Scripts/vendor/jQuery-ui.1.12.0.min.js"></script>
    <% if (jsDevelop())
        { %>
    <script src="Scripts/vendor/jQuery-migrate-3.0.0.js"></script>
    <% }
        else
        { %>
    <script src="Scripts/vendor/jQuery-migrate-3.0.0.min.js"></script>
    <% } %>

    <!-- for IE6-8 -->
    <script src="Scripts/vendor/respond.js"></script>

    <!-- attrchange: allow to detect attribute changes, used for detect element resize event -->
    <script src="Scripts/vendor/attrchange.js"></script>
    <script src="Scripts/vendor/attrchange_ext.js"></script>

    <!-- crypto -->
    <script src="Scripts/crypto/BigInt.js"></script>
    <script src="Scripts/crypto/Barrett.js"></script>
    <script src="Scripts/crypto/RSA.js"></script>
    <script src="Scripts/crypto/md5.js"></script>
    <script src="Scripts/crypto/base64.js"></script>

    <!-- storage -->
    <script src="Scripts/vendor/jquery.cookie.js"></script>
    <script src="Scripts/vendor/jquery.storageapi.min.js"></script>

    <!-- eLog -->
    <% if (jsDevelop())
        { %>
    <script src="Scripts.dev/010_utils.js"></script>
    <script src="Scripts.dev/020_connection.js"></script>
    <script src="Scripts.dev/030_listCache.js"></script>
    <script src="Scripts.dev/040_validator.js"></script>
    <script src="Scripts.dev/050_dialogBox.js"></script>
    <script src="Scripts.dev/100_layout.js"></script>
    <script src="Scripts.dev/105_layoutCtrlSetup.js"></script>
    <script src="Scripts.dev/106_vertSplitter.js"></script>
    <script src="Scripts.dev/110_textbox.js"></script>
    <script src="Scripts.dev/111_filteredtextbox.js"></script>
    <script src="Scripts.dev/120_numberbox.js"></script>
    <script src="Scripts.dev/139_calendar.js"></script>
    <script src="Scripts.dev/140_datebox.js"></script>
    <script src="Scripts.dev/150_checkbox.js"></script>
    <script src="Scripts.dev/160_empty.js"></script>
    <script src="Scripts.dev/170_flagselector.js"></script>
    <script src="Scripts.dev/180_selector.js"></script>
    <script src="Scripts.dev/190_groupseparator.js"></script>
    <script src="Scripts.dev/200_table.js"></script>
    <script src="Scripts.dev/349_gridRow.js"></script>
    <script src="Scripts.dev/350_grid.js"></script>
    <script src="Scripts.dev/355_stateGrid.js"></script>
    <script src="Scripts.dev/356_customGrid.js"></script>
    <script src="Scripts.dev/357_treeControl.js"></script>
    <script src="Scripts.dev/360_combo.js"></script>
    <script src="Scripts.dev/361_childcombo.js"></script>
    <script src="Scripts.dev/365_datespecial.js"></script>
    <script src="Scripts.dev/366_HTMLEditor.js"></script>
    <script src="Scripts.dev/367_CustomControl.js"></script>
    <script src="Scripts.dev/369_uploader.js"></script>
    <script src="Scripts.dev/370_button.js"></script>
    <script src="Scripts.dev/380_cmdBar.js"></script>
    <script src="Scripts.dev/390_controlMaster.js"></script>
    <script src="Scripts.dev/400_tabBar.js"></script>
    <script src="Scripts.dev/470_tab.js"></script>
    <script src="Scripts.dev/480_page.js"></script>
    <script src="Scripts.dev/490_pageMaster.js"></script>
    <script src="Scripts.dev/700_treeMenu.js"></script>
    <% if (eProjectWeb.Framework.Globals.UseCODAStyle)
        { %>
    <script src="Scripts.dev/710_CODAMenu.js"></script>
    <% } %>
    <script src="Scripts.dev/790_login.js"></script>
    <script src="Scripts.dev/800_app.js"></script>
    <script src="Scripts.dev/A01_CODALogin.js"></script>
    <script src="Scripts.dev/A02_IFrame.js"></script>
    <script src="Scripts.dev/B00_stupidGrid.js"></script>
    <% }
        else
        { %>
    <%--<script src="Scripts/cal.js"></script>--%>
    <script src="Scripts/cal.min.js"></script>
    <% } %>

    <script>
        $(window).on("load", function () {

            $("body").calApp({
                baseUrl: "<% Response.Write(GetUrl()); %>",
                sid: "<% Response.Write(Request.Params["sessid"]); %>",
                clock: {
                    show: <% Response.Write(Convert.ToInt32(eProjectWeb.Framework.Globals.ShowClock)); %>,
                    dateSelector: "#clockdate",
                    timeSelector: "#clocktime"
                },
                menu: {
                    containerSelector: "<% Response.Write(!eProjectWeb.Framework.Globals.UseCODAStyle ? "#treeMenuContainer" : "#codaMenuContainer"); %>",
                    <% if (!eProjectWeb.Framework.Globals.UseCODAStyle)
        {
            Response.Write("buttonSelector: \".navbar-brand\"");
        }%>
                },
                page: {
                    selector: "#pageMaster"
                },
                close: {
                    navbarSelector: ".navbar",
                    navbarLoginInfoSelector: ".navbar-login-info",
                    selector: ".navbar-logout"
                },
                allowAnimate: <% Response.Write(Convert.ToInt32(eProjectWeb.Framework.Globals.AllowAnimate)); %>,
                speed: "<% Response.Write(eProjectWeb.Framework.Globals.AnimationSpeed); %>",
                cultureInfo: <% Response.Write(eProjectWeb.Framework.Session.CultureInfoJSON); %>,
                useCODAStyle: <% Response.Write(Convert.ToInt32(eProjectWeb.Framework.Globals.UseCODAStyle)); %>,
                showDevTools: <% Response.Write(Convert.ToInt32(eProjectWeb.Framework.Globals.GetBool("showDevTools", false))); %>,
                settings: <% Response.Write(GetSettings()); %>,

                startupOrder: [<% Response.Write(GetStartupOrder()); %>],

                authorizer: "<% Response.Write(eProjectWeb.Framework.Globals.Authorizer); %>",
                loginTo: "<% Response.Write(eProjectWeb.Framework.Globals.LoginTo); %>",
                login_comboForCompany: <% Response.Write(Convert.ToInt32(eProjectWeb.Framework.Globals.GetBool("Login-comboForCompany", false))); %>,
                allowLDAP: <% Response.Write(Convert.ToInt32(eProjectWeb.Framework.Globals.AllowLDAP)); %>,
                LDAPInfo: "<% Response.Write(HttpUtility.UrlEncode(HttpContext.Current.User.Identity.Name)); %>",

                <% if (eProjectWeb.Framework.Globals.UseCODAStyle)
        {
            Response.Write("defaultRowHeight: 21");
        }%>
            });

        });
    </script>

</body>
</html>
