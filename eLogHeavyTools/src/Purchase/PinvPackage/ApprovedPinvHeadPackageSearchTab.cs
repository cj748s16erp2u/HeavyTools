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

namespace eLog.HeavyTools.Purchase.PinvPackage
{
    public class ApprovedPinvHeadPackageSearchTab : eProjectWeb.Framework.UI.Templates.SearchTabTemplate1
    {
        public static ApprovedPinvHeadPackageSearchTab New(eProjectWeb.Framework.UI.Templates.DefaultPageSetup head)
        {
            var t = ObjectFactory.New<ApprovedPinvHeadPackageSearchTab>();
            t.Initialize("apprphpackage", head, eProjectWeb.Framework.UI.Templates.DefaultActions.None);
            return t;
        }

    }
}
