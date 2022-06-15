using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Site.WSInterface
{
    [WebService(Name = "Pinv", Description = "Create purchase invoice", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class PinvWS : System.Web.Services.WebService
    {

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("PinvCreateHeadResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.PinvCreateHeadResponse CreateHead(
            [System.Xml.Serialization.XmlElement("PinvHead")]
            eLog.Base.WSInterface.PinvCreateHeadRequest request)
        {
            var bl = eLog.Base.WSInterface.WSPinvBL.New();
            return bl.CreateHead(request, this.Context);
        }
    }
}
