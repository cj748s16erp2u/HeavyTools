using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Site
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var sid = Request.Params["sessid"];
            try
            {
                eProjectWeb.Framework.Session.SetCurrentSession(sid);
            }
            catch
            {
                //Response.Redirect("Login.aspx");
            }
        }

        private string GetUrl(string scheme)
        {
            var segments = new string[Request.Url.Segments.Length - 1];
            Array.Copy(Request.Url.Segments, segments, segments.Length);
            var url = string.Format("{0}://{1}{2}", scheme, Request.Url.Authority, string.Join("", segments));
            return url;
        }

        protected string GetUrl()
        {
            return GetUrl(Request.Url.Scheme);
        }

        //protected string GetSocketUrl()
        //{
        //    return GetUrl("ws");
        //}

        protected string GetStartupOrder()
        {
            var res = new List<string>();
            if (!eProjectWeb.Framework.Globals.UseCODAStyle)
            {
                res.AddRange(new string[] { "createMenu", "createPageMaster", "createLogoutButton", "startClock" });
            }
            else
            {
                res.AddRange(new string[] { "createPageMaster", "createMenu", "createLogoutButton" });
            }

            res = res.ConvertAll<string>(s => "\"" + s + "\"");

            return string.Join(",", res.ToArray());
        }

        protected string GetSettings()
        {
            var dict = new Dictionary<string, object>();

            dict["allowXlsxExport"] = eProjectWeb.Framework.Globals.GetBool("allowXlsx", false);

            return eProjectWeb.Framework.Utils.ToJSON(dict);
        }

        protected bool HasCustomCSS()
        {
            return System.IO.File.Exists(System.IO.Path.Combine(eProjectWeb.Framework.Globals.SiteRoot, "Custom.css"));
        }

        protected bool jsDevelop()
        {
            return eProjectWeb.Framework.Globals.GetBool("jsDevelop", false);
        }
    }
}