using eLog.Base.Masters.Partner;
using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Masters.Partner
{
    public class OlcPartnCmps : EntityCollection<OlcPartnCmp, OlcPartnCmps>
    {
        public static OlcPartnCmps LoadWithSessionCompanies(int partnId)
        {
            var k = new Key();
            k[OlcPartnCmp.FieldPartnid.Name] = partnId;
            k[OlcPartnCmp.FieldCmpid.Name] = new Key.InAtToSql(Session.CompanyIds);
            return Load(k);
        }

        public static OlcPartnCmps Load(int partnId)
        {
            var k = new Key();
            k[OlcPartnCmp.FieldPartnid.Name] = partnId;
            return Load(k);
        }

        public OlcPartnCmp Find(int cmpid)
        {
            foreach (OlcPartnCmp p in Rows)
            {
                if (p.Cmpid == cmpid)
                    return p;
            }
            return null;
        }
    }
}