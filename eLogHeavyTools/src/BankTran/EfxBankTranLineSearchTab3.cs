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
        protected UploadButton btnFoxPostImportButton;

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
            //btnBankStatementImport = new PopupButton("bankstatement_import");
            //btnBankStatementImport.Order = 9999998;
            //btnBankStatementImport.Default = null;
            //btnBankStatementImport.AllowDropDown = true;
            //btnBankStatementImport.ForceDropDown = true;
            //AddCmd(btnBankStatementImport);

            //var btnFoxPostImport = new PopupChildButton("foxpost_import");
            //btnBankStatementImport.Add(btnFoxPostImport);
            //SetButtonAction(btnFoxPostImport.ID, new ControlEvent(btnFoxPost_Import_OnClick));

            //var btnFoxPostImport = new UploadButton("foxpost_import");
            //btnBankStatementImport.Add(btnFoxPostImport);
            //SetButtonAction(btnFoxPostImport.ID, new ControlEvent(btnFoxPost_Import_OnClick));

            this.btnFoxPostImportButton = this.AddCmd(new UploadButton("foxpost_import", 950));
            this.SetButtonAction(this.btnFoxPostImportButton.ID, this.btnFoxPost_Import_OnClick);
        }

        private void btnFoxPost_Import_OnClick(PageUpdateArgs args)
        {
            if (SearchResults.SelectedPKS.Count > 1)
                return;

            if (SearchResults.SelectedPK == null)
                return;

            var cifTrId = Convert.ToInt32(SearchResults.SelectedPK[U4Ext.Bank.Base.Transaction.CifEbankTrans.FieldId.Name]);

            var uploadInfo = this.btnFoxPostImportButton.GetUploadData(args);

            var partnerBL = (CifEbankTransBL3)CifEbankTransBL3.New3();
            var processResult = partnerBL.FoxPostBankStatementImport(uploadInfo, cifTrId);
        }

    }
}
