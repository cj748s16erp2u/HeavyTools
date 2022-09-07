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
    public class ApprovedPinvLinePackageSearchTab : eProjectWeb.Framework.UI.Templates.DetailSearchTabTemplate1
    {
        public static ApprovedPinvLinePackageSearchTab New(eProjectWeb.Framework.UI.Templates.DefaultPageSetup line)
        {
            var t = ObjectFactory.New<ApprovedPinvLinePackageSearchTab>();
            t.Initialize("apprplpackage", line, "$noroot_apprphead",
                    eProjectWeb.Framework.UI.Templates.DefaultActions.None);
            return t;
        }

    }
}
