using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Site.WSInterface
{
    [WebService(Name = "OtherTransation", Description = "Create other transactions", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OtherTranWS : System.Web.Services.WebService
    {
        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("OtherTranCreateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.OtherTranCreateResponse Create(
            [System.Xml.Serialization.XmlElement("OtherTran")]
            eLog.Base.WSInterface.OtherTranCreateRequest<eLog.Base.WSInterface.OtherTranCreateRequestItem> request)
        {
            var bl = eLog.Base.WSInterface.WSOtherTranBL.New();
            return bl.Create(request, this.Context);
        }
    }
}
