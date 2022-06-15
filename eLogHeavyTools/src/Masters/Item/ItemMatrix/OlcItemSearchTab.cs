using eLog.Base.Common;
using eLog.Base.Masters.Item;
using eLog.Base.Setup.Company;
using eLog.HeavyTools.Common.Matrix;
using eLog.HeavyTools.Masters.Item.Color;
using eLog.HeavyTools.Masters.Item.MainGroup;
using eLog.HeavyTools.Masters.Item.Model;
using eLog.HeavyTools.Masters.Item.Season;
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

namespace eLog.HeavyTools.Masters.Item.ItemMatrix
{
    class OlcItemSearchTab : MatrixTabPage2<ItemBL, OlcItemModel, OlcItemModelSeasons, OlcItemSizeRangeLines>
    {
        public static OlcItemSearchTab New()
        {
            var t = (OlcItemSearchTab)ObjectFactory.New(typeof(OlcItemSearchTab));
            t.Initialize("OlcItemModelSeason");
            return t;
        }
        protected override void Initialize(string itemmatrix)
        {
            SkipRootKey = true;
            base.Initialize(itemmatrix);
        }
        public override void InitalizeMatrix(MatrixTabPageData<ItemBL, OlcItemModel, OlcItemModelSeasons, OlcItemSizeRangeLines> matrixTabPageData)
        { 
            matrixTabPageData.Setup(OlcItemModel.FieldImid,
                    new MatrixField(OlcItemModelSeason.FieldImsid, OlcItemModelSeason.FieldImsid),
                    new MatrixField(OlcItemSizeRangeLine.FieldIsrlid, OlcItemSizeRangeLine.FieldSize),
                    ItemBL.ID
                    );
            var c1 = new MatrixEditItem<Combo>(new Field("colortype1", FieldType.Integer), ColorSetup1) { Mandatory = true, ReadOnly = true };
            matrixTabPageData.EditItems.Add(c1);
            matrixTabPageData.EditItems.Add(new MatrixEditItem<Combo>(new Field("colortype2", FieldType.Integer), ColorSetup));
            matrixTabPageData.EditItems.Add(new MatrixEditItem<Combo>(new Field("colortype3", FieldType.Integer), ColorSetup));

            matrixTabPageData.EditItems.Add(new MatrixEditItem<Textbox>(new Field("colorname", FieldType.String), ColorTextSetup) { Mandatory = true, ReadOnly = true, WriteValueFromComboOnChange = c1 });
            matrixTabPageData.EditItems.Add(new MatrixEditItem<Combo>(new Field("patterntype", FieldType.Integer), PatternSetup) { Mandatory = true, ReadOnly = true });
            matrixTabPageData.EditItems.Add(new MatrixEditItem<Combo>(new Field("patterntype2", FieldType.Integer), PatternSetup));
             
            matrixTabPageData.EditItemsTop.Add(new MatrixEditItem<Combo>(new Field("materialtype", FieldType.Integer), MaterialSetup));
            matrixTabPageData.EditItemsTop.Add(new MatrixEditItem<Combo>(new Field("custtarid", FieldType.Integer), CusttarSetup));


            MatrixFirstCellWidth = 115;
        }

        private void ColorTextSetup(Textbox obj)
        {
            lastcolor.SetOnChanged(
                delegate (PageUpdateArgs args) {
                    var icname = "";
                    try
                    {
                        icname = (string)SqlDataAdapter.ExecuteSingleValue(DB.Main, @"select name from ols_typeline where typegrpid=507 and value=" + args.Control.Value);
                    }
                    catch (Exception)
                    { 
                    }
                    
                    obj.Value = icname;
                    this.MatrixControlFix(args);
             });
             
        }

        private Combo lastcolor;
        private void ColorSetup1(Combo obj)
        {
            obj.ListID = ColorTypeList.ID;
            lastcolor = obj; 
        }

        private void CusttarSetup(Combo obj)
        {
            obj.ListID = "eLog.Base.Setup.CustTariff.CustTariffList"; // CustTariffList.ID;
        }
        private void PatternSetup(Combo obj)
        {
            obj.ListID = PatternTypeList.ID;
        }

        private void MaterialSetup(Combo obj)
        {
            obj.ListID = MaterialTypeList.ID;
        }

        private void ColorSetup(Combo obj)
        {
            obj.ListID = ColorTypeList.ID;
        }

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

            return OlcItemSizeRangeLines.Load(new Key() {
                { OlcItemSizeRangeLine.FieldDelstat.Name, 0 },
                { OlcItemSizeRangeLine.FieldIsrhid.Name, img.Isrhid} });
        }

        private const string SesionImid = "SesionImid";


        protected override void BeforeBuildMatrix(PageUpdateArgs args, bool save, bool renderControls)
        {
            if (session.Value != null)
            {
                Session.Current[sessionValue] = session.Value;
            }
        }


        protected override OlcItemModelSeasons SetupRows(OlcItemModel im)
        {

            var isid= Session.Current[sessionValue];
            int? imid;
 

            if (im == null)
            {
                imid = ConvertUtils.ToInt32(Session.Current[SesionImid]);
            } else
            {
                imid = im.Imid;
            }

            var k = new Key() {
                { OlcItemModelSeason.FieldDelstat.Name, 0 } ,
                { OlcItemModelSeason.FieldImid.Name, imid },
                { OlcItemModelSeason.FieldIsid.Name, isid }
            };


            Session.Current[SesionImid] = imid;
            return OlcItemModelSeasons.Load(k);

        }

        Textbox codetextbox = new Textbox("code") { Disabled = true };
        Textbox nametextbox = new Textbox("name") { Disabled = true };
        Combo session = new Combo("isid", OlcItemSeasonList.ID);
      
        protected override void SetupHeader(LayoutTable layout)
        {
            layout.AddControl(codetextbox);
            layout.AddControl(nametextbox);
            layout.AddControl(session);

        }
        protected override void SetupRootKey(OlcItemModel rootentity)
        {
            codetextbox.Value = rootentity.Code;
            nametextbox.Value = rootentity.Name;
        }

        private const string sessionValue = "session.Value";

        protected override MatrixStoredEntity GetStoredEntityCollection(Key key)
        {
             
            var k = new Key
            {
                { OlcItemModelSeason.FieldIsid.Name, Session.Current[sessionValue]},
                { OlcItemModelSeason.FieldImid.Name, ConvertUtils.ToInt32(Session.Current[SesionImid])},

            };

            var sql = string.Format(@"select ci.* 
                                      from olc_item ci
                                      join olc_itemmodelseason ims on ims.imsid=ci.imsid 
                                        where {0}", k.ToSql());

            var ics = OlcItems.New();
            SqlDataAdapter.FillDataSet(DB.Main, ics, sql);



            sql = string.Format(@"select i.* from olc_item c 
                                    join ols_item i on i.itemid=c.itemid
	                                join olc_itemmodelseason ims on ims.imsid=c.imsid 
                                    where {0}", k.ToSql());

            var ic = Items.New();
            SqlDataAdapter.FillDataSet(DB.Main, ic, sql);

            var msec = new MatrixStoredEntityConverter(typeof(OlcItem), typeof(Base.Masters.Item.Item));
            msec.Add(ics, typeof(OlcItem));
            msec.Add(ic, typeof(Base.Masters.Item.Item), typeof(OlcItem), Base.Masters.Item.Item.FieldItemid);

            var mse = new MatrixStoredEntity(msec);
            return mse;
        }
         

        protected override void Presave(BLObjectMap map)
        {
            var item = (Base.Masters.Item.Item)map.Default;
            
            if (item.State == DataRowState.Added)
            {
                var olcitem = (OlcItem)map[typeof(OlcItem).Name];

                var ms = OlcItemModelSeason.Load(olcitem.Imsid);
                var m = OlcItemModel.Load(ms.Imid);
                var mf = OlcItemMainGroup.Load(m.Imgid);
    
                if (!olcitem.Colortype1.HasValue)
                {
                    throw new MessageException("$missingcolortype1");
                }
                 

                var srl = OlcItemSizeRangeLine.Load(olcitem.Isrlid);

                var p = SqlDataAdapter.ExecuteSingleValue(DB.Main, @"select name from ols_typeline where typegrpid=501 and value=" + olcitem.Patterntype).ToString();
                var pN = ConvertUtils.ToInt32(SqlDataAdapter.ExecuteSingleValue(DB.Main, @"select VALUE from ols_typeline where typegrpid=501 and value=" + olcitem.Patterntype)).Value;

                if (pN == 0)
                {
                    p = "";
                }

                item.Name01 = string.Format("{0} {1} {2} {3}", m.Name, olcitem.Colorname, srl.Size, p);
                item.Itemcode = string.Format("{0}{1}{2}.{3}", m.Code, ms.Isid, ms.Icid, srl.Size);
                item.Itemgrpid = mf.Itemgrpid;
                item.Unitid = m.Unitid;
                item.Cmpcodes = "*";
                item.Cmpid = -1;

                var ics = ItemCmps.New();
                foreach (Company c in Companies.GetAll())
                {
                    var ic = ItemCmp.CreateNew();
                    ic.Cmpid = c.Cmpid;
                    ics.Add(ic);
                }
                map.Add<ItemCmps>(ics);
            }
        }

        protected override void Postsave(BLObjectMap map)
        {
            var item = (Base.Masters.Item.Item)map.Default;

            var bl = ItemExtBL.New();
            var imap = bl.CreateBLObjects();
            var ie = ItemExt.CreateNew();
            ie.Extcode = GetNewEAN();
            ie.Type = 2;
            ie.Def = 1;
            ie.Itemid = item.Itemid;
            imap.Default = ie;
            bl.Save(imap);

        }

        private string GetNewEAN()
        {
            string sql =
                @"
declare @d varchar(15), @EAN varchar(100)
exec @d=sp_olc_generatae_ean  @isStore=1,  @EAN=@EAN OUTPUT 
select @d, @EAN as num";

            string number = null;
            eLog.Base.Common.SqlFunctions.QueryData(DB.Main, sql,
                    delegate (System.Data.SqlClient.SqlDataReader dr, int index)
                    {
                        number = ConvertUtils.ToString(dr["num"]);
                    });

            return number;
        }



        #region Title
        public override string GetMatrixTitle()
        {
            return @"$itemmatrixtitle";
        }

        public override string GetMatrixGroupTitle()
        {
            return @"$itemgrouptitle";
        }

        public override string GetMatrixsingTitle()
        {
            return @"$itemmatrixsingtitle";
        }

        public override string GetTootTitle()
        {
            return @"$itemtoottitle";
        }

        public override string GetResultTitle()
        {
            return @"$modelMatrixResultTitle";
        }

        public override string GetMatrixResult()
        {
            return @"$modelMatrixResult";
        }
        #endregion

        protected override string BeforeColNameShow(string rowkey)
        {
            return ColNameCache.GetName(rowkey);
        }

        private class ColNameCache
        {
            static object lo = new object();

            private static Dictionary<string, string> Cache = new Dictionary<string, string>();

            public static string GetName(string id)
            {
                lock (lo)
                {
                    if (!Cache.ContainsKey(id))
                    {
                        var v = SqlDataAdapter.ExecuteSingleValue(DB.Main, @"select name from olc_itemmodelseason ms (nolock) join olc_itemcolor c  (nolock) on c.icid=ms.icid where imsid=" + id).ToString();
                        Cache.Add(id, v);
                    }
                    return Cache[id];
                } 
            }
        }
    }
}