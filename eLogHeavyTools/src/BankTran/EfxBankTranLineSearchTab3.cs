using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Actions;
using eProjectWeb.Framework.UI.Commands;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.BankTran
{
    public class EfxBankTranLineSearchTab3 : U4Ext.Bank.Base.Transaction.EfxBankTranLineSearchTab, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        protected PopupButton btnBankStatementImport;
        protected UploadButton btnFoxPostHUFImportButton;
        protected UploadButton btnGLSHUFImportButton;
        protected UploadButton btnGLSEURImportButton;
        protected UploadButton btnGLSRONImportButton;
        protected UploadButton btnGLSCZKImportButton;
        protected UploadButton btnSprinterHUHImportButton;

        #region IXmlObjectName
        protected static Type baseType = typeof(U4Ext.Bank.Base.Transaction.EfxBankTranLineSearchTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        public string XmlObjectName
        {
            get { return baseType.Name; }
        }
        #endregion

        protected override void CreateBase()
        {
            base.CreateBase();

            // import
            btnBankStatementImport = new PopupButton("bankstatement_import");
            btnBankStatementImport.Order = 9999;
            btnBankStatementImport.Default = null;
            btnBankStatementImport.AllowDropDown = true;
            btnBankStatementImport.ForceDropDown = true;
            AddCmd(btnBankStatementImport);

            btnFoxPostHUFImportButton = new UploadButton("foxpost_huf_import");
            btnBankStatementImport.Add(btnFoxPostHUFImportButton);
            SetButtonAction(btnFoxPostHUFImportButton.ID, new ControlEvent(btnFoxPost_HUF_Import_OnClick));

            btnGLSHUFImportButton = new UploadButton("gls_huf_import");
            btnBankStatementImport.Add(btnGLSHUFImportButton);
            SetButtonAction(btnGLSHUFImportButton.ID, new ControlEvent(btnGLS_HUF_Import_OnClick));

            btnGLSEURImportButton = new UploadButton("gls_eur_import");
            btnBankStatementImport.Add(btnGLSEURImportButton);
            SetButtonAction(btnGLSEURImportButton.ID, new ControlEvent(btnGLS_EUR_Import_OnClick));

            btnGLSRONImportButton = new UploadButton("gls_ron_import");
            btnBankStatementImport.Add(btnGLSRONImportButton);
            //SetButtonAction(btnGLSEURImportButton.ID, new ControlEvent(btnGLS_EUR_Import_OnClick));

            btnGLSCZKImportButton = new UploadButton("gls_czk_import");
            btnBankStatementImport.Add(btnGLSCZKImportButton);
            //SetButtonAction(btnGLSEURImportButton.ID, new ControlEvent(btnGLS_EUR_Import_OnClick));

            btnSprinterHUHImportButton = new UploadButton("sprinter_czk_import");
            btnBankStatementImport.Add(btnSprinterHUHImportButton);
            //SetButtonAction(btnGLSEURImportButton.ID, new ControlEvent(btnGLS_EUR_Import_OnClick));
    }

        #region Fox Post HUF
        private void btnFoxPost_HUF_Import_OnClick(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPKS.Count > 1)
                return;

            if (SearchResults.SelectedPK == null)
                return;

            var cifTrId = Convert.ToInt32(SearchResults.SelectedPK[U4Ext.Bank.Base.Transaction.CifEbankTrans.FieldId.Name]);

            var uploadInfo = this.btnFoxPostHUFImportButton.GetUploadData(args);

            var ciftransBL = (CifEbankTransBL3)CifEbankTransBL3.New3();
            var processResult = ciftransBL.FoxPostBankStatementImport(uploadInfo, cifTrId);
        }
        #endregion Fox Post HUF

        #region GLS HUF
        private void btnGLS_HUF_Import_OnClick(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPKS.Count > 1)
                return;

            if (SearchResults.SelectedPK == null)
                return;

            var cifTrId = Convert.ToInt32(SearchResults.SelectedPK[U4Ext.Bank.Base.Transaction.CifEbankTrans.FieldId.Name]);

            var uploadInfo = this.btnGLSHUFImportButton.GetUploadData(args);

            var ciftransBL = (CifEbankTransBL3)CifEbankTransBL3.New3();
            var processResult = ciftransBL.GLSHUFBankStatementImport(uploadInfo, cifTrId);
        }
        #endregion GLS HUF

        #region GLS EUR
        private void btnGLS_EUR_Import_OnClick(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPKS.Count > 1)
                return;

            if (SearchResults.SelectedPK == null)
                return;

            var cifTrId = Convert.ToInt32(SearchResults.SelectedPK[U4Ext.Bank.Base.Transaction.CifEbankTrans.FieldId.Name]);

            var uploadInfo = this.btnGLSHUFImportButton.GetUploadData(args);

            var ciftransBL = (CifEbankTransBL3)CifEbankTransBL3.New3();
            var processResult = ciftransBL.GLSHUFBankStatementImport(uploadInfo, cifTrId);
        }
        #endregion GLS EUR
    }
}
