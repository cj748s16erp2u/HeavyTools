<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MSSRSViewer.aspx.cs" Inherits="Site.MSSRSViewer" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=12.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><% Response.Write(eProjectWeb.Framework.ModuleRegistry.AppTitle); %></title>

    <meta charset="utf-8" />
    <!-- Viewport mobile tag for sensible mobile support -->
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <meta http-equiv="x-ua-compatible" content="IE=edge" />
    <meta http-equiv="Cache-Control" content="no-cache, no-store, must-revalidate" />
    <meta http-equiv="Pragma" content="no-cache" />
    <meta http-equiv="Expires" content="0" />

    <link rel="SHORTCUT ICON" href="favicon.png" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
        </asp:ScriptManager>
        <rsweb:ReportViewer ID="ReportViewer1" runat="server"
            Width="100%" Height="100%"
            ZoomMode="PageWidth" ShowCredentialPrompts="false" Style="display: table;"
            SizeToReportContent="true">
        </rsweb:ReportViewer>
        <asp:Label ID="msg" BackColor="#ffff99" runat="server" />
    </form>
</body>
</html>