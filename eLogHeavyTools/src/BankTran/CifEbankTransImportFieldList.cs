using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.BankTran
{
    public class CifEbankTransImportFieldList : TypeListProvider
    {
        public static readonly string ID = typeof(CifEbankTransImportFieldList).FullName;

        public CifEbankTransImportFieldList() : base("cif_ebank_trans.importfields") { }
    }
}
