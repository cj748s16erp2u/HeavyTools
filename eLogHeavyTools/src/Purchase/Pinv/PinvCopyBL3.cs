using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Purchase.Pinv
{
    internal class PinvCopyBL3 : CodaInt.Base.Purchase.Pinv.PinvCopyBL2
    {
        protected override void SetPinvHead(CopyContext cc)
        {
            base.SetPinvHead(cc);

            var xcVal = cc.OrigMap.Get<Base.Common.XCustValue.XCVal>();
            if (xcVal != null)
            {
                var newXcVal = Base.Common.XCustValue.XCVal.CreateNew();
                xcVal.CopyTo(newXcVal);

                newXcVal.Xcvid = null;
                newXcVal.Xcvrid = null;
                newXcVal.Addusrid = null;
                newXcVal.Adddate = null;

                cc.NewMap.Add(newXcVal);
            }
        }

    }
}
