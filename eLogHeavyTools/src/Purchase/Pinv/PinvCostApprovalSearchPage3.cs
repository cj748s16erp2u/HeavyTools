using eProjectWeb.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvCostApprovalSearchPage3 : Base.Purchase.Pinv.PinvCostApprovalSearchPage
    {
        public PinvCostApprovalSearchPage3() : base()
        {
            if (eLog.Base.Common.SqlFunctions.IsAttachmentTabVisible("pinv"))
            {
                string noroot = eProjectWeb.Framework.Lang.Translator.TranslateNspace("$noroot_pinvhead_attachment", this.GetNamespaceName());
                Tabs.AddTab(() => Base.Attach.AttachmentTab.New(Base.Purchase.Pinv.PinvHeadBL.ATTACHMENTSOURCE, noroot, GetAttachmentRefid));
            }
        }

        // Csatolt dokumentum ful mukodesehez szukseges refid meghatarozasa
        protected object GetAttachmentRefid(PageUpdateArgs args)
        {
            var pinvId = -1;

            var o = args.PageData[Consts.RootEntityKey];
            try
            {
                var k = new eProjectWeb.Framework.Data.Key(o);
                if (k.ContainsKey(Base.Purchase.Pinv.PinvApproval.FieldPinvapprid.Name))
                {
                    var pinvApprId = Convert.ToInt32(k[Base.Purchase.Pinv.PinvApproval.FieldPinvapprid.Name]);
                    var pinvApproval = Base.Purchase.Pinv.PinvApproval.Load(pinvApprId);
                    if (pinvApproval != null && pinvId == -1)
                        pinvId = pinvApproval.Pinvid.Value;
                }
            }
            catch (Exception)
            {
                // ha tobb sort jelolt ki akkor elszall
            }

            // tobbes kijeloles eseten, de nincs ertelme
            /*
            foreach (Dictionary<string, object> oo in ((List<object>)args.PageData[Consts.RootEntityKey]))
            {
                var k = new eProjectWeb.Framework.Data.Key(oo);
                if (k.ContainsKey(Base.Purchase.Pinv.PinvApproval.FieldPinvapprid.Name))
                {
                    var pinvApprId = Convert.ToInt32(k[Base.Purchase.Pinv.PinvApproval.FieldPinvapprid.Name]);
                    var pinvApproval = Base.Purchase.Pinv.PinvApproval.Load(pinvApprId);
                    if (pinvApproval != null && pinvId == -1)
                        pinvId = pinvApproval.Pinvid.Value;
                }
            }
            */

            return new eProjectWeb.Framework.Data.Key() { { "pinvid", pinvId } };
        }

    }
}
