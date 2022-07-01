using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
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

        protected Control ctrlImportNumberFormat;
        protected Control ctrlImportFields;
        protected Control ctrlImportText;
        protected Button btnImport;

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

            /*
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
            /*/

            LayoutTable tbl = (LayoutTable)this["multi"];
            if (tbl != null)
            {
                ctrlImportNumberFormat = tbl["importnumberformat"];
                ctrlImportText = tbl["multipaste"];
                ctrlImportFields = tbl["importfields"];
            }

            if (tbl != null)
            {
                btnImport = new Button("importfunc", 9999);
                AddCmd(btnImport);
                SetButtonAction(btnImport.ID, new ControlEvent(btnImport_OnClick));
            }

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

        protected override void EfxBankTranLineSearchTab_OnPageActivate(PageUpdateArgs args)
        {
            base.EfxBankTranLineSearchTab_OnPageActivate(args);

            SetImportColumnsText();
        }

        #region Import
        protected void btnImport_OnClick(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPKS.Count > 1)
                return;

            if (SearchResults.SelectedPK == null)
                return;

            var cifTrId = Convert.ToInt32(SearchResults.SelectedPK[U4Ext.Bank.Base.Transaction.CifEbankTrans.FieldId.Name]);

            string importText = ctrlImportText.GetStringValue();
            if (string.IsNullOrEmpty(importText))
                return;

            int? fieldListId = null;
            if (ctrlImportFields != null && ctrlImportFields.Value != null)
                fieldListId = Convert.ToInt32(ctrlImportFields.Value);

            var numberFormat = new eLog.Base.Setup.Parameters.NumberFormatTypeList().GetFormat(ConvertUtils.ToInt32(ctrlImportNumberFormat?.Value));

            var ciftransBL = (CifEbankTransBL3)CifEbankTransBL3.New3();
            List<Key> generatedRecords = null;
            string wrongLines = ciftransBL.ImportRecords(cifTrId, importText, numberFormat, fieldListId, out generatedRecords);

            if (string.IsNullOrEmpty(wrongLines))
            {
                SetImportColumnsText();
            }
            else
                ctrlImportText.Value = wrongLines;

            if (generatedRecords != null && generatedRecords.Count() > 0)
            {
                U4Ext.Bank.Base.Transaction.CifEbankTrans ct = U4Ext.Bank.Base.Transaction.CifEbankTrans.Load(generatedRecords.First());
                if (ct != null)
                {
                    var msg = eProjectWeb.Framework.Lang.Translator.Translate("$msg_ciftrans_import_success", ct.Fileid, generatedRecords.Count());
                    args.ShowDialog(dlgSimpleMessage, "$msg_ciftransimport_success_title", msg);
                }

                SearchResults.KeysToRefresh.AddRange(generatedRecords);
                RefreshTabInfoPart(args);
            }
        }

        protected virtual void SetImportColumnsText()
        {
            ctrlImportText.SetValue(null);
        }
        #endregion Import
    }
}
