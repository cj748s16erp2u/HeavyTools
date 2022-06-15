using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;

namespace Site.WSInterface
{
    /// <summary>
    /// Post transaction to Unit4 Finance (dochead/docline)
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class CODATranWS : System.Web.Services.WebService
    {

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("Response")]
        public CodaInt.Base.WSInterface.CODATranPostResponse Post(
            [System.Xml.Serialization.XmlElement("Request")]
            CodaInt.Base.WSInterface.CODATranPostRequest req)
        {
            var bl = CodaInt.Base.WSInterface.WSCODATranBL.New();
            return bl.Post(req, Context);
        }
    }
}
