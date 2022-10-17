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

        public string Currency { get; set; }

        public string Document { get; set; }

        public string Ref { get; set; }

        public string UniqId => $"{PartnerId}{PartnAddrId}{Ref}{Currency}{Document}";

        public SordHeadImportUnique(SordHead sordHead)
        {
            this.SordHeadId = sordHead.Sordid ?? 0;
            this.PartnerId = sordHead.Partnid ?? 0;
            this.PartnAddrId = sordHead.Addrid ?? 0;
            this.Currency = sordHead.Curid;
            this.Document = sordHead.Sorddocid;
            this.Ref = sordHead.Ref1;
        }

        public bool Equals(SordHeadImportUnique other)
        {
            return other != null && other.UniqId.Equals(UniqId);
        }

        public override bool Equals(object obj)
        {
            if(obj is SordHeadImportUnique)
            {
                return Equals((SordHeadImportUnique)obj);
            }

            return false;
        }

        public override int GetHashCode()
        {
            return SordHeadId.GetHashCode() ^ PartnerId.GetHashCode() ^ PartnAddrId.GetHashCode() ^ Ref.GetHashCode() ^ Currency.GetHashCode() ^ Document.GetHashCode();
        }

    }
}