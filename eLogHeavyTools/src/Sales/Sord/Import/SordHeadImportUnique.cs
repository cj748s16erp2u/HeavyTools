using eLog.Base.Sales.Sord;
using System;

namespace eLog.HeavyTools.Sales.Sord.Import
{
    /// <summary>
    /// SordId, PartnerId, PartnAddrId es ref alapjan kulcsot vissza ado osztaly
    /// </summary>
    public class SordHeadImportUnique : IEquatable<SordHeadImportUnique>
    {
        public int SordHeadId { get; set; }

        public int PartnerId { get; set; }

        public int PartnAddrId { get; set; }

        public string Ref { get; set; }

        public string UniqId => $"{PartnerId}{PartnAddrId}{Ref}";

        public SordHeadImportUnique(SordHead sordHead)
        {
            this.SordHeadId = sordHead.Sordid ?? 0;
            this.PartnerId = sordHead.Partnid ?? 0;
            this.PartnAddrId = sordHead.Addrid ?? 0;
            this.Ref = sordHead.Ref1;
        }

        public bool Equals(SordHeadImportUnique other)
        {
            return other != null && other.UniqId.Equals(UniqId);
        }
    }
}