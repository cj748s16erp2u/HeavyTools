using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Extensions;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.UI.Templates;

namespace eLog.HeavyTools.Warehouse.StockTranLocation
{
    public class ReceivingStLocCustomEditTab : EditTabTemplate1<ReceivingStLocCustomDto, ReceivingStLocCustomRules, ReceivingStLocCustomBL>
    {
        public static ReceivingStLocCustomEditTab New(DefaultPageSetup setup)
        {
            var t = ObjectFactory.New<ReceivingStLocCustomEditTab>();
            t.Initialize(setup.MainEntity, setup);
            return t;
        }

        protected ReceivingStLocCustomEditTab()
        {
        }

        protected IEnumerable<Control> StockControls => this.EditGroup1?.Controls.Cast<Control>().Where(c => c.CustomData == "stock");

        protected override ReceivingStLocCustomDto DefaultPageLoad(PageUpdateArgs args)
        {
            var e = base.DefaultPageLoad(args);
            if (e is null)
            {
                return e;
            }

            var bl = ReceivingStLocCustomBL.New();
            var stock = bl.GetStock(e.Whid, e.Whzoneid, e.Itemid, e.Whlocid);
            if (stock != null)
            {
                var props = typeof(WhZone.WhZTranService.WhZStockMapQDto)
                    .GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                foreach (var p in props)
                {
                    var ctrl = this.StockControls.FirstOrDefault(c => string.Equals(c.DataField, p.Name, StringComparison.InvariantCultureIgnoreCase));
                    if (ctrl != null)
                    {
                        var value = p.GetValue(stock);
                        ctrl.SetValue(value);
                    }
                }
            }

            return e;
        }
    }
}
