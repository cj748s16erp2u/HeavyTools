using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Site.WSInterface
{
    /// <summary>
    /// Summary description for CODAQueryWS
    /// </summary>
    [WebService(Name="CODAQuery", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class CODAQueryWS : System.Web.Services.WebService
    {
        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("QueryDocumentResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public CodaInt.Base.WSInterface.CODAQueryDocumentResponse QueryDocument(
            [System.Xml.Serialization.XmlElement("QueryDocumentRequest")]
            CodaInt.Base.WSInterface.CODAQueryDocumentRequest request
            )
        {
            var response = CodaInt.Base.WSInterface.WSCODAQueryBL.New().QueryDocument(request, this.Context);
            return response;
        }
    }
}
