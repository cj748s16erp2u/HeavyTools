using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using eLog.Base.WSInterface;

namespace Site.WSInterface
{
    [WebService(Name = "Partner", Description = "Create or update partner", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class PartnerWS : System.Web.Services.WebService
    {
        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("PartnerAddOrUpdateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public PartnerAddOrUpdateResponse AddOrUpdate(
            [System.Xml.Serialization.XmlElement("Partner")]
            PartnerAddOrUpdateRequest request)
        {
            var bl = WSPartnerBL.New();
            return bl.AddOrUpdate(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("PartnerSetActiveStatusResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public PartnerSetActiveStatusResponse SetActiveStatus(
            [System.Xml.Serialization.XmlElement("PartnerStatus")]
            PartnerSetActiveStatusRequest request)
        {
            var bl = WSPartnerBL.New();
            return bl.SetActiveStatus(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("PartnerResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public PartnerQueryResponse Query(
            [System.Xml.Serialization.XmlElement("Partner")]
            PartnerQueryRequest request)
        {
            var bl = WSPartnerQueryBL.New();
            return bl.Query(request, this.Context);
        }
    }
}
