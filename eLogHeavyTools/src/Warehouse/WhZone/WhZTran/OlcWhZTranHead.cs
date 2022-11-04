using eLog.Base.Warehouse.StockTran;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.WhZone.WhZTran
{
    public partial class OlcWhZTranHead
    {
        public override void SetDefaultValues()
        {
            Whztid = null;
            Cmpid = null;
            Whzttype = null;
            Whztdate = DateTime.Today;
            Fromwhzid = null;
            Towhzid = null;
            Closeusrid = null;
            Closedate = DateTime.Today;
            Whztstat = null;
            Note = null;
            Stid = null;
            Sordid = null;
            Pordid = null;
            Taskid = null;
            Gen = null;
            Addusrid = null;
            Adddate = null;
        }

        //private void AutoFillWarehousePtAddr(StDocType trType)
        //{
        //    switch (trType)
        //    {
        //        case StDocType.Receiving:
        //            Fromwhzid = null;
        //            AutoFillWarehousePtAddr(FieldTowhzid, FieldTopartnid, FieldToaddrid);
        //            break;

        //        case StDocType.Issuing:
        //            Towhzid = null;
        //            AutoFillWarehousePtAddr(FieldFromwhzid, FieldFrompartnid, FieldFromaddrid);
        //            break;
        //        case StDocType.Transfering:
        //            AutoFillWarehousePtAddr(FieldFromwzhid, FieldFrompartnid, FieldFromaddrid);
        //            AutoFillWarehousePtAddr(FieldTowhzid, FieldTopartnid, FieldToaddrid);
        //            break;
        //    }
        //}
    }
}
