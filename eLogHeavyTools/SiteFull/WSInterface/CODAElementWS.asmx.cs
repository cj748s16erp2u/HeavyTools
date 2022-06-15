using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Site.WSInterface
{
    [WebService(Name = "CODAElement", Description = "Create or update CODA element", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CODAElementWS : System.Web.Services.WebService
    {
        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("CostCenterAddOrUpdateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public CodaInt.Base.WSInterface.CODAElementAddOrUpdateResponse CostCenterAddOrUpdate(
            [System.Xml.Serialization.XmlElement("CostCenter")]
            CodaInt.Base.WSInterface.CODAElementAddOrUpdateRequest request)
        {
            var bl = CodaInt.Base.WSInterface.WSCODAElementBL.New();

            var args = new CodaInt.Base.WSInterface.WSCODAElementBL.CODAElementAddOrUpdateArgs()
            {
                Request = request,
                Context = this.Context,
                SettingsName = "CostCenter",
                ElmLevelType = CodaInt.Base.WSInterface.WSCODAElementBL.CODAElementAddOrUpdateArgsElmLevelType.CompanyLineRecType,
                ElmLevel = CODALink.Common.CompanyLineRecTypes.CostCenterElmLevelType
            };

            return bl.AddOrUpdate(args);
        }
    }
}
