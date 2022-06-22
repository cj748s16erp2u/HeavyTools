using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.BankTran.Import
{
    public class CifEbankTransRowContext : RowContextBase
    {
        public TableEntry CifEbankTrans { get; internal set; }

        public override void Dispose()
        {
            base.Dispose();

            this.CifEbankTrans = null;
        }
    }
}
