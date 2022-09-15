using eLog.HeavyTools.InterfaceCaller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Action
{
    public partial class OlcAction
    {
        public override void SetDefaultValues()
        {
            //Discountcalculationtype = (int)Action.Discountcalculationtype.Division;
            Purchasetype = (int)Action.Purchasetype.All;
            Discountforfree = 0;
            Discountfreetransportation = 0;
            Validforsaleproducts = 0;
            Isactive = 1;
            Isextcondition = 0;
            Isextdiscount = 0;
            Filtercustomerstype = (int)eLog.HeavyTools.Sales.Action.FilterCustomersType.All;
        }

        public override void PostSave()
        {
            base.PostSave();
            new InterfaceCallerBL().ResetAction(Aid);
        }
    }
}
