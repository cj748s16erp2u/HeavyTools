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
        protected Button btnReadBankFiles2;
        protected DialogBox dlgReadBankFiles2;

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

            btnReadBankFiles2 = new Button("readbankfiles2", 9999);
            AddCmd(btnReadBankFiles2);
            SetButtonAction(btnReadBankFiles2.ID, new ControlEvent(ReadBankFiles2));

            dlgReadBankFiles2 = new DialogBox(DialogBoxType.InputOkCancel);
            dlgReadBankFiles2.OnButton1Clicked += dlgReadBankFiles2_OnButton1Clicked;
            RegisterDialog(dlgReadBankFiles2);
        }

        protected void ReadBankFiles2(PageUpdateArgs args)
        {
            var tbl = new LayoutTable(new TableColumn[]
            {
                new TableColumn(100, TableColumnFlags.None),
                new TableColumn(250, TableColumnFlags.None),
            })
            { ControlGroup = "dlg01" };

            tbl.AddControl(new Combo()
            {
                Field = "cmpcode",
                ListID = $"{CodaInt.Base.Setup.Company.CompanyList.ID}#sessioncmp",
                Flags = ComboFlags.AutoSelectOne,
                Mandatory = true,
                //Value = cmpId,
                //Disabled = true
            });
            tbl.AddControl(new Combo()
            {
                Field = "bankid",
                ListID = U4Ext.Bank.Base.Setup.Bank.EfxBankListProvider.ID,
                Flags = ComboFlags.AutoSelectOne,
                Mandatory = true,
                DependentCtrlID = "dlg01.cmpcode",
                DependentField = "cmpcode",
                DependentAllowNullField = "cmpcode"
            });
            tbl.AddControl(new Combo()
            {
                // ha a fajlt archivaltuk es mar nincs a konyvtarban, akkor a listaban se jelenjen meg, ezert:
                // a random lista parameter miatt mindig olvassa ujra az elemeket
                Field = "filename",
                ListID = U4Ext.Bank.Base.Transaction.EfxBankFileList.ID + "#" + Guid.NewGuid().ToString("n"),
                Flags = ComboFlags.AutoSelectOne,
                Mandatory = true,
                DependentCtrlID = "dlg01.cmpcode,dlg01.bankid",
                DependentField = "cmpcode,bankid",
            });
            tbl.AddControl(new Checkbox()
            {
                Field = "movethefile",
                Value = true,
            });

            args.ShowInputDialog(dlgReadBankFiles2, null, null, tbl);
        }

        protected void dlgReadBankFiles2_OnButton1Clicked(PageUpdateArgs args)
        {
            var cmpCode = dlgReadBankFiles.GetStringValue("cmpcode");
            var bankId = dlgReadBankFiles.GetInputValue<Int32>("bankid");
            var fileName = dlgReadBankFiles.GetStringValue("filename");
            var moveFile = dlgReadBankFiles.GetInputValue<Boolean>("movethefile");
            if (string.IsNullOrEmpty(cmpCode) || !bankId.HasValue || string.IsNullOrEmpty(fileName))
                return;

            var bl = U4Ext.Bank.Base.Transaction.EfxBankTranHeadBL.New();

            List<string> errors = new List<string>();
            try
            {
                errors = bl.BankDocProcess(cmpCode, bankId, fileName, (bool)moveFile, null);
            }
            catch (Exception ex)
            {
                //throw new MessageException(ex.Message);
                Log.Error(ex.Message);
                args.ShowDialog(dlgSimpleMessage, "$msg_readbankfiles_title", "$msg_bankdocprocess_error");
            }

            if (errors.Count > 0)
            {
                args.ShowDialog(dlgSimpleMessage, null, string.Join("<br/>", errors));
                //args.ShowDialog(dlgSimpleMessage, "$msg_calc_title", "$msg_calcstep_finished");
            }
            else
            {
                args.Continue = true;
                SearchResults.KeysToRefresh.Add(new eProjectWeb.Framework.Data.Key
                {
                    ["cmpcode"] = cmpCode,
                    ["bankid"] = bankId,
                    ["filename"] = fileName,
                });
            }
        }

    }
}