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

        protected override void CreateBase()
        {
            RefreshGridOnActivate = false;

            base.CreateBase();

            //SearchResults.MergePageData = "mypinvkey";

            OnPageActivate += ApprovedPinvLinePackageSearchTab_OnPageActivate;
        }

        protected void ApprovedPinvLinePackageSearchTab_OnPageActivate(eProjectWeb.Framework.PageUpdateArgs args)
        {
            int? pinvId = 0;

            if ((args.PageData?.ContainsKey(Consts.DetailEntityKey)).GetValueOrDefault())
            {
                var detailEntityKey = args.PageData[Consts.DetailEntityKey] as Dictionary<string, object>;

                if (detailEntityKey.ContainsKey("pinvid"))
                {
                    pinvId = ConvertUtils.ToInt32(detailEntityKey["pinvid"]);
                }
            }

            args.PageData["mypinvkey"] = new eProjectWeb.Framework.Data.Key("pinvid", pinvId);

            if (!SearchResults.ForceRefresh)
                SearchResults.ForceRefresh = true;
        }
    }
}
