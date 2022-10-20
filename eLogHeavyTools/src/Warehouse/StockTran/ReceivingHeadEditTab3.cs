using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.Base.Warehouse.Common;
using eLog.HeavyTools.Warehouse.WhZone.WhZTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    public class ReceivingHeadEditTab3 : CodaInt.Base.Warehouse.StockTran.ReceivingHeadEditTab2, eProjectWeb.Framework.Xml.IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(Base.Warehouse.StockTran.ReceivingHeadEditTab);

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        protected override string GetPageXmlFileName()
        {
            return $"{this.GetNamespaceName()}.{this.XmlObjectName}";
        }

        public override string XmlObjectName => baseType.Name;

        #endregion

        private const string NEEDWHZONETRANHANDLING = "NEEDWHZONETRANHANDLING";

        protected Control ctrlTowhzid;

        protected override void CreateBase()
        {
            base.CreateBase();

            this.ctrlTowhzid = this.EditGroup1["towhzid"];
        }

        private OlcWhZTranHead LoadWhZTranHead(Base.Warehouse.StockTran.StHead stHead)
        {
            var key = new Key
            {
                [OlcWhZTranHead.FieldStid.Name] = stHead.Stid
            };

            return OlcWhZTranHead.Load(key);
        }

        protected override Base.Warehouse.StockTran.StHead DefaultPageLoad(PageUpdateArgs args)
        {
            var stHead = base.DefaultPageLoad(args);
            if (stHead == null)
            {
                return null;
            }

            if (stHead.Stid != null)
            {
                var bl = ReceivingHeadBL3.New3();
                var needWhZoneTranHandling = bl.CheckWhNeedZoneTranHandling(stHead.Towhid);

                var olcWhZTranHead = this.LoadWhZTranHead(stHead);
                this.ctrlTowhzid.SetValue(olcWhZTranHead?.Towhzid);
                
                needWhZoneTranHandling |= olcWhZTranHead != null;
                this.ctrlTowhzid.SetMandatory(needWhZoneTranHandling);

                this.SetNeedWhZoneTranHandling(args, needWhZoneTranHandling, olcWhZTranHead?.Whztid);

                var stLineExists = WarehouseSqlFunctions.StockTranLineExistsStrict(stHead.Stid.GetValueOrDefault());
                this.ctrlTowhzid.SetDisabled(stLineExists || stHead.Retorigstid.HasValue);
            }

            return stHead;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, Base.Warehouse.StockTran.StHead stHead)
        {
            var map = base.SaveControlsToBLObjectMap(args, stHead);

            var toWhzid = this.ctrlTowhzid.GetValue<int>();
            if (args.ActionID == ActionID.Modify)
            {
                var needWhZoneTranHandling = this.GetNeedWhZoneTranHandling(args);
                if (needWhZoneTranHandling)
                {
                    eProjectWeb.Framework.Rules.RuleUtils.CheckMandatory(toWhzid, "$towhzid");
                }
            }

            stHead.SetCustomData("towhzid", toWhzid);

            return map;
        }

        protected override void cbToWarehouse_OnChanged(PageUpdateArgs args)
        {
            base.cbToWarehouse_OnChanged(args);

            var towhid = this.cbToWarehouse.GetStringValue();

            var bl = ReceivingHeadBL3.New3();
            var needWhZoneTranHandling = bl.CheckWhNeedZoneTranHandling(towhid);
            var whztid = this.GetWhztid(args);
            needWhZoneTranHandling |= whztid != null;

            this.ctrlTowhzid.SetMandatory(needWhZoneTranHandling);

            this.SetNeedWhZoneTranHandling(args, needWhZoneTranHandling, whztid);
        }

        private bool GetNeedWhZoneTranHandling(PageUpdateArgs args)
        {
            if (args.PageData?.TryGetValue(NEEDWHZONETRANHANDLING, out var o) == true)
            {
                return ConvertUtils.ToInt32(o).GetValueOrDefault() != 0;
            }

            return false;
        }

        private int? GetWhztid(PageUpdateArgs args)
        {
            if (args.PageData?.TryGetValue("whztid", out var o) == true)
            {
                return ConvertUtils.ToInt32(o);
            }

            return null;
        }

        private void SetNeedWhZoneTranHandling(PageUpdateArgs args, bool needWhZoneTranHandling, int? whztid)
        {
            if (needWhZoneTranHandling)
            {
                args.PageData[NEEDWHZONETRANHANDLING] = Convert.ToInt32(needWhZoneTranHandling);
            }
            else if (args.PageData.ContainsKey(NEEDWHZONETRANHANDLING))
            {
                args.PageData[NEEDWHZONETRANHANDLING] = null;
            }

            args.PageData["whztid"] = whztid;
        }
    }
}
