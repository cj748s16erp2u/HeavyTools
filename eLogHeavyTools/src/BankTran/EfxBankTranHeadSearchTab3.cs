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
    public class EfxBankTranHeadSearchTab3 : U4Ext.Bank.Base.Transaction.EfxBankTranHeadSearchTab, eProjectWeb.Framework.Xml.IXmlObjectName
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
            btnBankStatementImport = new PopupButton("bankstatement_import");
            btnBankStatementImport.Order = 9999998;
            btnBankStatementImport.Default = null;
            btnBankStatementImport.AllowDropDown = true;
            btnBankStatementImport.ForceDropDown = true;
            AddCmd(btnBankStatementImport);

            //var btnFoxPostImport = new PopupChildButton("foxpost_import");
            //btnBankStatementImport.Add(btnFoxPostImport);
            //SetButtonAction(btnFoxPostImport.ID, new ControlEvent(btnFoxPost_Import_OnClick));

            var btnFoxPostImport = new UploadButton("foxpost_import");
            btnBankStatementImport.Add(btnFoxPostImport);
            //SetButtonAction(btnFoxPostImport.ID, new ControlEvent(btnFoxPost_Import_OnClick));

            this.btnFoxPostImportButton = this.AddCmd(new UploadButton("partnerimport", 950));
            this.SetButtonAction(this.btnFoxPostImportButton.ID, this.btnFoxPost_Import_OnClick);

        }

        private void btnFoxPost_Import_OnClick(PageUpdateArgs args)
        {
            //var bl = (ProjectBL3)ProjectBL3.New();
            //bool doRefresh = bl.GetEPProjectIssueFile();

            var uploadInfo = this.btnFoxPostImportButton.GetUploadData(args);

            var partnerBL = (CifEbankTransBL3)CifEbankTransBL3.New3();
            //var processResult = CifEbankTransBL3.FoxPostBankStatementImport(uploadInfo);

        }

    }
}