using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Purchase.Pinv
{
    public class PinvHeadBL3 : CodaInt.Base.Purchase.Pinv.PinvHeadBL2
    {
        public static new PinvHeadBL3 New()
        {
            return (PinvHeadBL3)ObjectFactory.New<Base.Purchase.Pinv.PinvHeadBL>();
        }

        public Base.Common.XCustValue.XCVal LoadCustomPartnerData(int? pinvId)
        {
            var key = new Key
            {
                [Base.Common.XCustValue.XCVal.FieldXcvcode.Name] = "olc_pinvhead",
                [Base.Common.XCustValue.XCVal.FieldXcvrid.Name] = pinvId
            };
            var xcVal = Base.Common.XCustValue.XCVal.Load(key);

            return xcVal;
        }

        public override void Validate(BLObjectMap objects)
        {
            base.Validate(objects);

            var xcVal = objects.Get<Base.Common.XCustValue.XCVal>();
            if (xcVal != null)
            {
                RuleServer.Validate<Base.Common.XCustValue.XCVal, PinvCustomPartnerRules>(objects);
            }
        }

        protected override void BeforeDeleteHead(Base.Purchase.Pinv.PinvHead pinvHead)
        {
            base.BeforeDeleteHead(pinvHead);

            var xcVal = this.LoadCustomPartnerData(pinvHead.Pinvid);
            if (xcVal != null)
            {
                xcVal.State = DataRowState.Deleted;
                xcVal.Save();
            }
        }

        protected override bool PreSave(BLObjectMap objects, Entity e)
        {
            var b = base.PreSave(objects, e);

            if (b)
            {
                var pinvHead = objects.Default as Base.Purchase.Pinv.PinvHead;
                var xcVal = e as Base.Common.XCustValue.XCVal;
                if (pinvHead != null && xcVal?.State == DataRowState.Added)
                {
                    xcVal.Xcvcode = "olc_pinvhead";
                    xcVal.Xcvrid = pinvHead.Pinvid;
                }
            }

            return b;
        }

    }
}