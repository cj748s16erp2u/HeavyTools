using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPinvHeadPackageSearchTab : eProjectWeb.Framework.UI.Templates.DetailSearchTabTemplate1
    {
        public static ApprovedPinvHeadPackageSearchTab New(eProjectWeb.Framework.UI.Templates.DefaultPageSetup head)
        {
            var t = ObjectFactory.New<ApprovedPinvHeadPackageSearchTab>();
            t.Initialize("apprphpackage", head, "$noroot_apprpackage",
                    eProjectWeb.Framework.UI.Templates.DefaultActions.None);
            return t;
        }

        protected override void CreateBase()
        {
            base.CreateBase();

            SetupClientsideSum();
        }

        void SetupClientsideSum()
        {
            SearchResults.AddSumArray("net");
        }

    }
}