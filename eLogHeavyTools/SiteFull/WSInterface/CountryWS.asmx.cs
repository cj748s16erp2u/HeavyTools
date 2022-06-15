using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using eLog.Base.WSInterface;

namespace Site.WSInterface
{
    [WebService(Name = "Country", Description = "Create or update country", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CountryWS : System.Web.Services.WebService
    {
        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("CountryAddOrUpdateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public CountryAddOrUpdateResponse AddOrUpdate(
            [System.Xml.Serialization.XmlElement("Country")]
            CountryAddOrUpdateRequest request)
        {
            var bl = WSCountryBL.New();
            return bl.AddOrUpdate(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("CountrySetActiveStatusResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public CountrySetActiveStatusResponse SetActiveStatus(
            [System.Xml.Serialization.XmlElement("CountryStatus")]
            CountrySetActiveStatusRequest request)
        {
            var bl = WSCountryBL.New();
            return bl.SetActiveStatus(request, this.Context);
        }
    }
}
