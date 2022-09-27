using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework;

namespace eLog.HeavyTools.Warehouse.WhLocLink
{
    public class OlcWhLocLinkLineSearchTab : InlineEditAdditionalTabTemplate1<OlcWhLocLinkLine, OlcWhLocLinkLineRules, OlcWhLocLinkLineBL>
    {
        public static OlcWhLocLinkLineSearchTab New(DefaultPageSetup setup)
        {
            var tab = ObjectFactory.New<OlcWhLocLinkLineSearchTab>();
            tab.Initialize("OlcWhLocLinkLine", setup, null, "$noroot_olcwhloclinkline", DefaultActions.New | DefaultActions.Delete);
            return tab;
        }

        protected override void OnCmdIleOK(PageUpdateArgs args)
        {
            base.OnCmdIleOK(args);
            RefreshTabInfoPart(args);
        }

        protected override void Delete_ConfirmYes(PageUpdateArgs args)
        {
            base.Delete_ConfirmYes(args);
            RefreshTabInfoPart(args);
        }
    }
}
