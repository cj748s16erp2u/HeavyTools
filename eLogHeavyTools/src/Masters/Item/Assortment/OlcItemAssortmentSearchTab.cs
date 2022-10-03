using eLog.HeavyTools.Common.Matrix;
using eLog.HeavyTools.Masters.Item.ItemMatrix;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eLog.HeavyTools.Masters.Item.Model;
using eLog.HeavyTools.Masters.Item.Season;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.PageParts;
using System;

namespace eLog.HeavyTools.Masters.Item.Assortment
{
    public class OlcItemAssortmentSearchTab : MatrixTabPage2<ItemAssortmentmatrixEntityBL, OlcItemModel, Items, OlcItemSizeRangeLines>
    {
        #region Title
        public override string GetMatrixTitle()
        {
            return @"$itemassorimentmatrixtitle";
        }

        public override string GetMatrixGroupTitle()
        {
            return @"$itemassorimentgrouptitle";
        }

        public override string GetMatrixsingTitle()
        {
            return @"$itemassorimentmatrixsingtitle";
        }

        public override string GetTootTitle()
        {
            return @"$itemassorimenttoottitle";
        }

        public override string GetResultTitle()
        {
            return @"$itemassorimentMatrixResultTitle";
        }

        public override string GetMatrixResult()
        {
            return @"$itemassorimentmodelMatrixResult";
        }
        #endregion

        public static OlcItemAssortmentSearchTab New()
        {
            var t = (OlcItemAssortmentSearchTab)ObjectFactory.New(typeof(OlcItemAssortmentSearchTab));
            t.Initialize("OlcItemAssortmentSearchTab");
            return t;
        }
        
        Textbox codetextbox = new Textbox("code") { Disabled = true };
        Textbox nametextbox = new Textbox("name") { Disabled = true };
 
        protected override void SetupHeader(LayoutTable layout)
        {
            layout.AddControl(codetextbox);
            layout.AddControl(nametextbox); 
        }

        protected override void SetupRootKey(OlcItemModel rootentity)
        {
            codetextbox.Value = rootentity.Code;
            nametextbox.Value = rootentity.Name;
        }

        protected override Items SetupRows(OlcItemModel im)
        {
            int? imid;

            if (im == null)
            {
                imid = ConvertUtils.ToInt32(Session.Current[SesionImid]);
            }
            else
            {
                imid = im.Imid;
            }
            if (imid==null || !imid.HasValue)
            {
                imid = -1;
            }

            var sql = $@"select i.* 
  from ols_item i
  join olc_item c on i.itemid=c.itemid
  join olc_itemmodelseason im on c.imsid=im.imsid
 where imid={imid}
   and isnull(c.iscollectionarticlenumber,0)=1";


            Session.Current[SesionImid] = imid;
            return Items.Load(DB.Main, sql);
        }

        private const string SesionImid = "SesionImid";

        protected override OlcItemSizeRangeLines SetupCols(OlcItemModel im)
        {
            if (im == null)
            {
                im = OlcItemModel.Load(ConvertUtils.ToInt32(Session.Current[SesionImid]));
            }
            if (im == null)
            {
                return OlcItemSizeRangeLines.New();
            }
            var img = OlcItemMainGroup.Load(im.Imgid);

            var sizes = OlcItemSizeRangeLines.Load(new Key() {
                { OlcItemSizeRangeLine.FieldDelstat.Name, 0 },
                { OlcItemSizeRangeLine.FieldIsrhid.Name, img.Isrhid} });
             
            return sizes;
        }

        public override void InitalizeMatrix(MatrixTabPageData<ItemAssortmentmatrixEntityBL, OlcItemModel, Items, OlcItemSizeRangeLines> matrixTabPageData)
        {
            matrixTabPageData.Setup(MatrixEditItemType.Intbox, ItemAssortmentmatrixEntity.FieldCount, OlcItemModel.FieldImid,
                    new MatrixField(Base.Masters.Item.Item.FieldItemid, Base.Masters.Item.Item.FieldItemcode),
                    new MatrixField(OlcItemSizeRangeLine.FieldIsrlid, OlcItemSizeRangeLine.FieldSize),
                    ItemAssortmentmatrixEntityBL.ID
                    );
        }

        protected override MatrixStoredEntity GetStoredEntityCollection(Key key)
        { 
            var sql = $@"
  select itemid2 tmpitemid, xx.isrlid, isoid, count, i.itemid, imid
  from ols_item i (nolock)
  join olc_item c (nolock) on i.itemid=c.itemid
  join olc_itemmodelseason im (nolock) on c.imsid=im.imsid
  outer apply (
	select a.count, isoid, ii.itemid itemid2, isrlid
	  from olc_item cc (nolock)
	  join ols_item ii (nolock) on ii.itemid=cc.itemid
	  left join olc_itemassortment a  (nolock) on a.itemid=cc.itemid
	  where cc.iscollectionarticlenumber=0
		and cc.imsid=c.imsid
  ) xx
 where im.imid={key["imid"]}
   and c.iscollectionarticlenumber=1

";

            var md = ItemAssortmentmatrixEntities.Load(sql);

            var msec = new MatrixStoredEntityConverter(
                typeof(ItemAssortmentmatrixEntity), typeof(ItemAssortmentmatrixEntity));

            msec.Add(md, typeof(ItemAssortmentmatrixEntity));

            return new MatrixStoredEntity(msec);
        }


        protected override bool Presave(BLObjectMap map, MatrixStoredEntityValues se)
        {
            var iae = (ItemAssortmentmatrixEntity)map.Default;
            var data = (ItemAssortmentmatrixEntity)se.Entities[typeof(ItemAssortmentmatrixEntity)];

            data.Count = iae.Count;

            if (iae.Isoid.HasValue)
            {
                if (!iae.Count.HasValue)
                {
                    iae.Count = 0;
                }
                data.Save();
            } else
            {
                if (iae.Count.HasValue)
                {
                    data.State = DataRowState.Added;
                    data.Save(); 
                }
            }


            return false;
        }
    }
}
