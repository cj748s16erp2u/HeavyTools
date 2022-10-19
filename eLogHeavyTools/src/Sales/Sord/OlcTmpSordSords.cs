using eProjectWeb.Framework.Data;

namespace eLog.HeavyTools.Sales.Sord
{
    internal class OlcTmpSordSords : EntityCollection<OlcTmpSordSord, OlcTmpSordSords>
    {
        public void Save()
        {
            SqlDataAdapter.SaveDataSet(DB.Main, this);
        }
    }
}
