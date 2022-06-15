using System.Collections.Generic;
using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Masters.Partner.Import
{
    public class PartnerRowContext : RowContextBase
    {
        public TableEntry Partner { get; internal set; }
        public TableEntry OlcPartner { get; internal set; }
        public List<TableEntry> PartnCmps { get; internal set; } = new List<TableEntry>();
        public List<TableEntry> PartnAddrs { get; internal set; } = new List<TableEntry>();
        public List<TableEntry> OlcPartnAddrs { get; internal set; } = new List<TableEntry>();
        public List<TableEntry> PartnAddrCmps { get; internal set; } = new List<TableEntry>();
        public List<TableEntry> PartnBanks { get; internal set; } = new List<TableEntry>();
        public List<TableEntry> PartnBankCmps { get; internal set; } = new List<TableEntry>();
        public List<TableEntry> Employees { get; internal set; } = new List<TableEntry>();

        public override void Dispose()
        {
            base.Dispose();

            this.Employees?.Clear();
            this.Employees = null;

            this.PartnBankCmps?.Clear();
            this.PartnBankCmps = null;

            this.PartnBanks?.Clear();
            this.PartnBanks = null;

            this.PartnAddrCmps?.Clear();
            this.PartnAddrCmps = null;

            this.OlcPartnAddrs?.Clear();
            this.OlcPartnAddrs = null;

            this.PartnAddrs?.Clear();
            this.PartnAddrs = null;

            this.PartnCmps?.Clear();
            this.PartnCmps = null;

            this.OlcPartner = null;

            this.Partner = null;
        }
    }
}
