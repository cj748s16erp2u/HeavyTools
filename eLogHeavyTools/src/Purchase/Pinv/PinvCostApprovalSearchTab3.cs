using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using eProjectWeb.Framework;
using eLog.Base.Purchase.Pinv;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvCostApprovalSearchTab3 : Base.Purchase.Pinv.PinvCostApprovalSearchTab, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(PinvCostApprovalSearchTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        public string XmlObjectName => baseType.Name;

        #endregion
        protected override void CreateBase()
        {
            base.CreateBase();

            var btnViewAttachment = new Button("viewattachment", 9000);
            AddCmd(btnViewAttachment);
            SetButtonAction(btnViewAttachment.ID, new ControlEvent(btnViewAttachment_OnClick));
        }

        public override void Initialize(string tabName, DefaultPageSetup setup, DefaultActions actions, PageMode pageMode)
        {
            actions |= eProjectWeb.Framework.UI.Templates.DefaultActions.View;

            base.Initialize(tabName, setup, actions, pageMode);
        }

        protected void btnViewAttachment_OnClick(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPK == null)
                return;

            if (SearchResults.SelectedPKS.Count != 1)
                return;

            var pinvApprId = Convert.ToInt32(SearchResults.SelectedPK[Base.Purchase.Pinv.PinvApproval.FieldPinvapprid.Name]);

            var pinvApproval = Base.Purchase.Pinv.PinvApproval.Load(pinvApprId);
            if (pinvApproval == null)
                return;

            //var pinvId = Convert.ToInt32(SearchResults.SelectedPK[Base.Purchase.Pinv.PinvHead.FieldPinvid.Name]);
            var pinvId = pinvApproval.Pinvid.Value;

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
