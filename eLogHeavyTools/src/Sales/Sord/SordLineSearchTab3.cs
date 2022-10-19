using eLog.Base.CostPrice.Accounts;
using eLog.Base.Sales.Sord;
using eLog.Base.Setup.SordDoc;
using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Script;
using eProjectWeb.Framework.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Sord
{
    public class SordLineSearchTab3 : SordLineSearchTab, IXmlObjectName
    {
        public override string GetNamespaceName()
        {
            return typeof(SordLineSearchTab).Namespace;
        }
        protected override string GetPageXmlFileName()
        {
            return GetNamespaceName() + "." + typeof(SordLineSearchTab).Name;
        }
        #region IXmlObjectName Members

        private string m_xmlObjectName = null;

        public string XmlObjectName
        {
            get { if (string.IsNullOrEmpty(m_xmlObjectName)) m_xmlObjectName = typeof(SordLineSearchTab).Name; return m_xmlObjectName; }
        }

        #endregion

        Button preorderButton = new Button("preorderButton");

        protected override void CreateBase()
        {
            base.CreateBase();
            OnPageTaskActivate += this.SordLineSearchTab3_OnPageTaskActivate;
            AddCmd(preorderButton); 
             
            SetButtonAction(preorderButton.ID, new ControlEvent(m_sordStCmd_Partner_OnClick));
        }

        private void SordLineSearchTab3_OnPageTaskActivate(PageUpdateArgs args)
        {
            var pbvisible = false;
            var sordid = GetSordId(args);

            var sh = SordHead.Load(sordid);

            if (sh!=null)
            {
                var sd = SordDoc.Load(sh.Sorddocid);

                if (sd != null)
                {
                    if (sd.Type == 2)
                    {
                        pbvisible = true;
                    }
                }
            }

            preorderButton.Visible = pbvisible;
            GetCmd("create").Visible = !pbvisible;
            GetCmd("modify").Visible = !pbvisible;

        }
        protected virtual void m_sordStCmd_Partner_OnClick(PageUpdateArgs args)
        {
            DoCreateSordSord(args);
        }
        protected virtual void DoCreateSordSord(PageUpdateArgs args)
        {
            try
            { 
                var bl = SordSordBL.New();
                int sordid = GetSordId(args).GetValueOrDefault();
                
                Guid g = bl.CreateOlcTmpSordSord(sordid);
                Key k = new Key(new string[] { "ssid", "sordid" }, new object[] { g, sordid });

                args.AddExecCommand(new OpenEditPageStep(SordSordPage.ID, args.Control.ID, 
                    OpenEditPageStep.KeyType.Custom, k));
            }
            catch (Exception e)
            {
                args.ShowDialog(dlgSimpleMessage, "$sordst_result_title", e.ToString());
            }
        }

        private int? GetSordId(PageUpdateArgs args)
        {
            var rek = args.PageData[Consts.RootEntityKey];
            var pk = new Key(rek);
            if (pk != null)
            {
                return ConvertUtils.ToInt32(pk[SordHead.FieldSordid.Name]);
            }
            return null;
        }
    }
}
