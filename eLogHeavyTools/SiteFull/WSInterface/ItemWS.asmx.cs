using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Services;
using eLog.Base.WSInterface;

namespace Site.WSInterface
{
    [WebService(Name = "Item", Description = "Create or update item", Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class ItemWS : System.Web.Services.WebService
    {
        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("ItemAddOrUpdateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public ItemAddOrUpdateResponse AddOrUpdate(
            [System.Xml.Serialization.XmlElement("Item")]
            ItemAddOrUpdateRequest request)
        {
            var bl = WSItemBL.New();
            return bl.AddOrUpdate(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("ItemSetActiveStatusResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public ItemSetActiveStatusResponse SetActiveStatus(
            [System.Xml.Serialization.XmlElement("ItemStatus")]
            ItemSetActiveStatusRequest request)
        {
            var bl = WSItemBL.New();
            return bl.SetActiveStatus(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("ItemUnitAddOrUpdateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.ItemUnitAddOrUpdateResponse UnitAddOrUpdate(
            [System.Xml.Serialization.XmlElement("Item")]
            eLog.Base.WSInterface.ItemUnitAddOrUpdateRequest request)
        {
            var bl = eLog.Base.WSInterface.WSItemUnitBL.New();
            return bl.AddOrUpdate(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("ItemExtAddOrUpdateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.ItemExtAddOrUpdateResponse ExtAddOrUpdate(
            [System.Xml.Serialization.XmlElement("Item")]
            eLog.Base.WSInterface.ItemExtAddOrUpdateRequest request)
        {
            var bl = eLog.Base.WSInterface.WSItemExtBL.New();
            return bl.AddOrUpdate(request, this.Context);
        }

        [WebMethod]
        [return: System.Xml.Serialization.XmlElementAttribute("ItemChangeAddOrUpdateResponse")] // ha ezt kihagyjuk, akkor az XML tag neve <fuggvenynev>Result lesz
        public eLog.Base.WSInterface.ItemChangeAddOrUpdateResponse UnitChangeAddOrUpdate(
            [System.Xml.Serialization.XmlElement("Item")]
            eLog.Base.WSInterface.ItemChangeAddOrUpdateRequest request)
        {
            var bl = eLog.Base.WSInterface.WSItemChangeBL.New();
            return bl.AddOrUpdate(request, this.Context);
        }
    }
}
