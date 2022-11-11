using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U4Ext.Bank.Base.Bookkeeping.Booking;

namespace eLog.HeavyTools.Bookkeeping.Booking
{
    public class BankCreationContext3 : BankCreationContext
    {
        public IEnumerable<BankBookingCreator3.ElmBankList3> PartnerCodes { get; set; }
    }
}
