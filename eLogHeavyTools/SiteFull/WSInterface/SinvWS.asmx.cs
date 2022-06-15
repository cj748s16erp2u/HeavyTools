using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Site.WSInterface
{
    [WebService(Name = "Sinv", Description = "Create sales invoice", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class SinvWS : System.Web.Services.WebService
    {
        [WebMethod]
        [return: System.Xml.Serialization.XmlElement("SinvCreateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.SinvCreateResponse Create(
            [System.Xml.Serialization.XmlElement("Sinv")]
            eLog.Base.WSInterface.SinvCreateRequest request)
        {
            var bl = eLog.Base.WSInterface.WSSinvBL.New();
            return bl.Create(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElement("SinvDeleteResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.SinvDeleteResponse Delete(
            [System.Xml.Serialization.XmlElement("Sinv")]
            eLog.Base.WSInterface.SinvDeleteRequest request)
        {
            var bl = eLog.Base.WSInterface.WSSinvDeleteBL.New();
            return bl.Delete(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElement("SinvCloseResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.SinvCloseResponse Close(
            [System.Xml.Serialization.XmlElement("Sinv")]
            eLog.Base.WSInterface.SinvCloseRequest request)
        {
            var bl = eLog.Base.WSInterface.WSSinvCloseBL.New();
            return bl.Close(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("SinvQueryResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.SinvQueryResponse Query(
            [System.Xml.Serialization.XmlElement("Sinv")]
            eLog.Base.WSInterface.SinvQueryRequest request)
        {
            var bl = eLog.Base.WSInterface.WSSinvQueryBL.New();
            return bl.Query(request, this.Context);
        }
    }
}
