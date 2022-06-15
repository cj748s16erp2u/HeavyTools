using System.Collections.Generic;
using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Masters.Partner.Import
{
    public class PartnerImportResultSet : ImportResultSetBase
    {
        public TableEntry Partner { get; set; }
        public TableEntry OlcPartner { get; set; }
        public IEnumerable<TableEntry> PartnCmps { get; set; }
        public IEnumerable<TableEntry> PartnAddrs { get; set; }
        public IEnumerable<TableEntry> OlcPartnAddrs { get; set; }
        public IEnumerable<TableEntry> PartnAddrCmps { get; set; }
        public IEnumerable<TableEntry> PartnBanks { get; set; }
        public IEnumerable<TableEntry> PartnBankCmps { get; set; }
        public IEnumerable<TableEntry> Employees { get; set; }
    }
}
