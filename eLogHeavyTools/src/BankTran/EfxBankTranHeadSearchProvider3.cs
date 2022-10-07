using CODALink;
using com.coda.efinance.schemas.common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using U4Ext.Bank.Base.Transaction;

namespace eLog.HeavyTools.BankTran
{
    public class EfxBankTranHeadSearchProvider3 : U4Ext.Bank.Base.Transaction.EfxBankTranHeadSearchProvider
    {
        #region IXmlObjectName
        public override string GetNamespaceName()
        {
            return this.GetType().BaseType.Namespace;
        }

        protected override string GetSearchXmlFileName()
        {
            return GetNamespaceName() + "." + this.GetType().BaseType.Name;
        }
        #endregion

        protected override void ModifyQueryString(Dictionary<string, object> args, bool fmtonly, ref string sql3)
        {
            base.ModifyQueryString(args, fmtonly, ref sql3);

            sql3 = sql3.Replace("--$$morefields$$", @",
            bank_lines.line_db, bank_lines.sum_origvalue, bank_lines.sum_valueacc--$$morefields$$");

            sql3 = sql3.Replace("--$$morejoins$$", @"
outer apply (
     select sum(origvalue) sum_origvalue, sum(valueacc) sum_valueacc, count(id) line_db
       from cif_ebank_trans cbt (nolock)
      where cbt.cmpcode = bth.cmpcode and cbt.fileid = bth.filename and cbt.interfaceid = b.interfaceid and
            cbt.ownacnum = ob.sort+ob.acnum) bank_lines--$$morejoins$$");
        }

    }
}
