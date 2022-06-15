using eLog.Base.Masters.Item;
using eProjectWeb.Framework;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Model
{
    class ItemSearchTab3 : DetailSearchTabTemplate1
    {
        public static DefaultPageSetup ItemSearchTab3Setup = new DefaultPageSetup("Item", ItemBL.ID,
            ItemSearchProvider3.ID, ItemEditPage.ID, typeof(ItemRules));


        public static ItemSearchTab3 New3()
        {
            ItemSearchTab3 t = (ItemSearchTab3)ObjectFactory.New(typeof(ItemSearchTab3));
            t.Initialize("itemsearch", ItemSearchTab3Setup, "$noroot", DefaultActions.Basic | DefaultActions.HideUnhide);
            return t;
        }

        protected Combo cbDelStat;
        protected Control ctrlFilterRelDateFrom;
        protected Control ctrlFilterRelDateTo;
        protected Control ctrlFilterCmpCodes;
         

        protected ItemSearchTab3()
        {
        }

        protected Button m_genNextCmd;

        protected override void CreateBase()
        {
            base.CreateBase();
             
            cbDelStat = (Combo)this.SrcBar["delstat"];
            ctrlFilterRelDateFrom = this.SrcBar["reldatefrom"];
            ctrlFilterRelDateTo = this.SrcBar["reldateto"];
            ctrlFilterCmpCodes = this.SrcBar["cmpcodes"];

            if (ItemBL.ReleaseDateUsage == ItemBL.ItemReleaseDateUsage.NotUsed)
            {
                if (ctrlFilterRelDateFrom != null)
                    this.SrcBar.ReplaceControl(ctrlFilterRelDateFrom, new Empty());
                if (ctrlFilterRelDateTo != null)
                    this.SrcBar.ReplaceControl(ctrlFilterRelDateTo, new Empty());
            }

            var btnCopy = AddCmd(new Button("copy"));
            var mraCopy = new eProjectWeb.Framework.UI.Actions.ModifyRecordAction(eLog.Base.Masters.Item.ItemEditPage.ID, string.Empty, SearchGridID, false);
            mraCopy.ActionIDForOpenPage = eProjectWeb.Framework.BL.ActionID.New;
            mraCopy.Shortcut = eProjectWeb.Framework.UI.Commands.DefaultShortcut.None;
            SetButtonAction(btnCopy.ID, mraCopy);

            if (ItemBL.AllowGenNextItem)
            {
                AddCmd(m_genNextCmd = new Button(ItemBL.GENNEXT_ACTIONID, 90));
                var loadArgs = "this.getObjectValue(\"" + SearchGridID + "\")";
                SetButtonAction(m_genNextCmd.ID, new eProjectWeb.Framework.UI.Actions.NewRecordAction(Setup.EditPageName, string.Empty, loadArgs, (m_ActionNewEventHandler != null)), m_ActionNewEventHandler);
                m_genNextCmd.Shortcut = eProjectWeb.Framework.UI.Commands.ShortcutKeys.Key_F9;
            }
        }
    }
}