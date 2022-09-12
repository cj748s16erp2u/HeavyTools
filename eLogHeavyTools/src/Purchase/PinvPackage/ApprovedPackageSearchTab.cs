using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPackageSearchTab : eProjectWeb.Framework.UI.Templates.SearchTabTemplate1
    {
        public static ApprovedPackageSearchTab New(eProjectWeb.Framework.UI.Templates.DefaultPageSetup package)
        {
            var t = ObjectFactory.New<ApprovedPackageSearchTab>();
            t.Initialize("apprpackage", package, eProjectWeb.Framework.UI.Templates.DefaultActions.None);
            return t;
        }

        protected override void CreateBase()
        {
            RefreshGridOnActivate = false;

            base.CreateBase();

            SearchResults.MergePageData = "pinvid";

            OnPageActivate += ApprovedPackageSearchTab_OnPageActivate;
        }

        protected void ApprovedPackageSearchTab_OnPageActivate(eProjectWeb.Framework.PageUpdateArgs args)
        {
            args.PageData["pinvid"] = new Key("pinvid", 0);
        }

    }
}
