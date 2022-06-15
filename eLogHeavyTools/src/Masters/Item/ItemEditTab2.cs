using eLog.Base.Masters.Item;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;

namespace eLog.HeavyTools.Masters.Item
{
    public class ItemEditTab2 : ItemEditTab
    {
        protected LayoutTable olcGroup;

        protected Combo imgid;
        public ItemEditTab2()
        {
        }

        protected override void CreateBase()
        {
            base.CreateBase();
            olcGroup = FindRenderable<LayoutTable>("OlcGroup");


            imgid = FindRenderable<Combo>("imgid");
            imgid.SetOnChanged(OnImgidChanged);
        }

        private void OnImgidChanged(PageUpdateArgs args)
        {
            UpdateIsrhid();
        }

        private void UpdateIsrhid()
        {
            int? isrhid = null;

            if (imgid.Value != null)
            {
                var img = OlcItemMainGroup.Load(ConvertUtils.ToInt32(imgid.Value));
                if (img != null)
                {
                    isrhid = img.Isrhid;
                }
            }

            FindRenderable<Intbox>("isrhid").Value = isrhid;
        }

        protected override void LoadOtherTables(eProjectWeb.Framework.Data.Key itemKey)
        {
            base.LoadOtherTables(itemKey);
            var olcItem = OlcItem.Load(itemKey);
            if (olcItem != null)
            {
              
                if (olcItem.Imsid.HasValue)
                {
                    var sql = @"select m.imid, mg.imgid
        from olc_itemmodelseason ms
		join olc_itemmodel m on m.imid=ms.imid
        join olc_itemmaingroup mg on mg.imgid=m.imgid
        where ms.imsid=" + olcItem.Imsid;
                    foreach (var item in SqlDataAdapter.Query(sql).AllRows)
                    {
                        olcGroup.FindRenderable<Combo>("imgid").Value = ConvertUtils.ToInt32(item["imgid"]);
                        olcGroup.FindRenderable<Combo>("imid").Value = ConvertUtils.ToInt32(item["imid"]);
                        UpdateIsrhid();
                        break;
                    }
                }

                olcGroup.DataBind(olcItem, false);


            }
        }
        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, Base.Masters.Item.Item e)
        { 
            var map = base.SaveControlsToBLObjectMap(args, e);
             
            OlcItem olcItem = OlcItem.Load(e.PK);

            if (olcItem == null)
            {
                olcItem = OlcItem.CreateNew();
                olcItem.SetKey(e.PK);
            }
            
            olcGroup.DataBind(olcItem, true);

            map.Add(typeof(OlcItem).Name, olcItem);

            return map;
        }
    }

} 