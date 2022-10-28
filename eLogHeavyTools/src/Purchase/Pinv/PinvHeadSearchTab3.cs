using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvHeadSearchTab3 : CodaInt.Base.Purchase.Pinv.PinvHeadSearchTab2
    {
        public override string GetNamespaceName()
        {
            return typeof(CodaInt.Base.Purchase.Pinv.PinvHeadSearchTab2).Namespace;
        }

        public override string XmlObjectName { get { return typeof(CodaInt.Base.Purchase.Pinv.PinvHeadSearchTab2).Name; } }

        protected override void CreateBase()
        {
            base.CreateBase();

            var btnViewAttachment = new Button("viewattachment", GetCmd(eProjectWeb.Framework.BL.ActionID.View).Order + 1);
            AddCmd(btnViewAttachment);
            SetButtonAction(btnViewAttachment.ID, new ControlEvent(btnViewAttachment_OnClick));
        }

        protected void btnViewAttachment_OnClick(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPK == null)
                return;

            var pinvId = Convert.ToInt32(SearchResults.SelectedPK[Base.Purchase.Pinv.PinvHead.FieldPinvid.Name]);

            var lst = Base.Purchase.Pinv.PinvHeadBL.LoadAttachments(pinvId);
            var at = lst.OrderByDescending(a => a.Def.GetValueOrDefault(0) != 0).ThenBy(a => a.Attid.Value).FirstOrDefault();
            if (at != null)
            {
                var sett = new eProjectWeb.Framework.UI.Script.OpenPopupSettings
                {
                    ServicePrefix = "Services/DownloadService.ashx",
                    RealFileName = at.Filename,
                    StoredFileName = at.Storedfilename,
                    ContentType = !at.Mimetype.IsNullOrEmpty() ? at.Mimetype.Value : "",
                    Store = "attach"
                };

                args.AddExecCommand(new eProjectWeb.Framework.UI.Script.OpenPopupStep(sett));
            }
        }

    }
}