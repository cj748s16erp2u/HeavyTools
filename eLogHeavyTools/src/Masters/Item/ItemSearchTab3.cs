using eLog.Base.Masters.Item;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Lang;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using eProjectWeb.Framework.UI.Script;
using eProjectWeb.Framework.UI.Templates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item
{
    public enum ItemSearchTabMode
    { 
        Master,
        Detail
    }

    class ItemSearchTab3 : ItemSearchTab
    { 
        protected string DetailEntityKey;
        protected string ParentEntityKey;

        protected string NoRootMsg;

        protected ItemSearchTabMode SearchTabMode = ItemSearchTabMode.Master;
     
        public ItemSearchTab3()
        {
            DetailEntityKey = Consts.DetailEntityKey;
            ParentEntityKey = Consts.RootEntityKey;
            ShowEditedRecordInGrid = true;
        }
        string TabID;
        public override void Initialize(string tabName, DefaultPageSetup setup, DefaultActions actions, PageMode pageMode)
        {
            TabID = tabName;
            base.Initialize(tabName, setup, actions, pageMode);
        }

        protected override void CreateDefaultEvents()
        {
            if (this.SearchTabMode == ItemSearchTabMode.Master)
            {
                base.CreateDefaultEvents();
            } else
            {
                base.OnActivate.AddStep(new PreInitStep());
                CreateCheckForRootEntityKey();
                base.OnActivate.NewGroup();
                if (RefreshGridOnActivate)
                {
                    base.OnActivate.AddStep(new ShowObjectStep(SearchGridID, ParentEntityKey));
                } 
                if (ShowEditedRecordInGrid)
                {
                    base.OnActivate.AddStep(new ShowEditedObjectStep(SearchGridID));
                } else
                {
                    base.OnActivate.AddStep(new StoreEntityKeyStep(null, "EditedPK"));
                }

                if (!string.IsNullOrEmpty(DetailEntityKey)) 
                { 
                    base.OnDeactivate.AddStep(new StoreGridToEntityKeyStep(SearchGridID, DetailEntityKey));
                } 
            } 
        }

        protected bool ShowEditedRecordInGrid;

        protected virtual void CreateCheckForRootEntityKey()
        {
            this.OnActivate.AddStep(new CheckEntityKeyStep(TabID, ParentEntityKey));
        }

        internal static TabPage NewDetail(PageMode mode, DefaultPageSetup setup, string noRootMsg)
        {
            ItemSearchTab3 t = (ItemSearchTab3)ObjectFactory.New(typeof(ItemSearchTab3));
            t.SearchTabMode = ItemSearchTabMode.Detail;
            t.Initialize("itemsearch", setup, DefaultActions.Basic | DefaultActions.HideUnhide, mode);
            t.NoRootMsg = noRootMsg;
            return t;
        }

        protected override void RenderTextResources()
        {
            base.RenderTextResources();
            base.TabSettings.TextResource[TabID + "_noroot"] = Translator.Translate(NoRootMsg); 
        }


    }
}