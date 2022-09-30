using eLog.Base.Common;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Masters.Item.Assortment
{
    public class ItemAssortmentmatrixEntity : VirtualEntity<ItemAssortmentmatrixEntity>
    {
        [Field("tmpitemid", DataType = FieldType.Integer)]
        public static Field FieldTmpitemid { get; protected set; }
        public int? Tmpitemid
        {
            get { return ConvertUtils.ToInt32(this[FieldTmpitemid]); }
            set { this[FieldTmpitemid] = value; }
        }


        [Field("isrlid", DataType = FieldType.Integer)]
        public static Field FieldIsrlid { get; protected set; } 
        public int? Isrlid
        {
            get { return ConvertUtils.ToInt32(this[FieldIsrlid]); }
            set { this[FieldIsrlid] = value; }
        }

        [Field("isoid", DataType = FieldType.Integer)]
        public static Field FieldIsoid { get; protected set; }
        public int? Isoid
        {
            get { return ConvertUtils.ToInt32(this[FieldIsoid]); }
            set { this[FieldIsoid] = value; }
        }

        [Field("count", DataType = FieldType.Integer)]
        public static Field FieldCount { get; protected set; }
        public int? Count
        {
            get { return ConvertUtils.ToInt32(this[FieldCount]); }
            set { this[FieldCount] = value; }
        }


        [Field("itemid", DataType = FieldType.Integer)]
        public static Field FieldItemid { get; protected set; }
        public int? Itemid
        {
            get { return ConvertUtils.ToInt32(this[FieldItemid]); }
            set { this[FieldItemid] = value; }
        }

        [Field("imid", DataType = FieldType.Integer)]
        public static Field FieldImid { get; protected set; }
        public int? Imid
        {
            get { return ConvertUtils.ToInt32(this[FieldImid]); }
            set { this[FieldImid] = value; }
        }

        public static ItemAssortmentmatrixEntity CreateNew()
        {
            ItemAssortmentmatrixEntity val = New();
            val.DefaultVersion = DataRowVersion.Original;
            val.SetDefaultValues();
            val.DefaultVersion = DataRowVersion.Current;
            val.State = DataRowState.Added;
            return val;
        }


        public override void Save()
        {
            if (Isoid.HasValue)
            {
                var oia=OlcItemAssortment.Load(Isoid);
                oia.Count = Count;
                oia.Save();
            } else
            {
                var oia = OlcItemAssortment.CreateNew();
                oia.Count = Count;
                oia.Assortmentitemid = Itemid;
                oia.Itemid = Tmpitemid;
                oia.Save();
            }
        }
    }

    public class ItemAssortmentmatrixEntities : EntityCollection<ItemAssortmentmatrixEntity, ItemAssortmentmatrixEntities>
    {
        public static ItemAssortmentmatrixEntities Load(string sql)
        {
            var lines = New();
            SqlDataAdapter.FillDataSet(DB.Main, lines, sql);
            return lines;
        }
    }

    public class ItemAssortmentmatrixEntityBL : DefaultBL1<ItemAssortmentmatrixEntity, ItemAssortmentmatrixEntityRules>
    {
        public static readonly string ID = typeof(ItemAssortmentmatrixEntityBL).FullName;

        protected ItemAssortmentmatrixEntityBL()
            : base(DefaultBLFunctions.BasicHideUnhide)
        {
        }

        public static OlcItemAssortmentBL New()
        {
            return (OlcItemAssortmentBL)ObjectFactory.New(typeof(OlcItemAssortmentBL));
        }
        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            object x = objects[typeof(OlcItemAssortment).Name];
            if (x != null)
                RuleServer.Validate(objects, typeof(OlcItemAssortmentRules)); 
        }
    }


    public class ItemAssortmentmatrixEntityRules : TypedBaseRuleSet<ItemAssortmentmatrixEntity>
    {
        public ItemAssortmentmatrixEntityRules()
            : base(true)
        {

        }
    }

}
