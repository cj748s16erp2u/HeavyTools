using eLog.Base.Warehouse.StockTran;
using eProjectWeb.Framework;
using eProjectWeb.Framework.BL;
using eProjectWeb.Framework.UI.Controls;
using eProjectWeb.Framework.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Warehouse.StockTran
{
    internal class TransferingHeadEditTab3 : TransferingHeadEditTab, IXmlObjectName
    {
        Combo towhid;
        Combo fromwhid;

        public override string GetNamespaceName()
        {
            return typeof(TransferingHeadEditTab).Namespace;
        }
        protected override string GetPageXmlFileName()
        {
            return GetNamespaceName() + "." + typeof(TransferingHeadEditTab).Name;
        }
        #region IXmlObjectName Members

        private string m_xmlObjectName = null;

        public string XmlObjectName
        {
            get { if (string.IsNullOrEmpty(m_xmlObjectName)) m_xmlObjectName = typeof(TransferingHeadEditTab).Name; return m_xmlObjectName; }
        }
        #endregion

        protected override void CreateBase()
        {
            base.CreateBase();
            towhid = FindRenderable<Combo>("towhid");
            fromwhid = FindRenderable<Combo>("fromwhid");
        }

        protected override StHead DefaultPageLoad_LoadEntity(PageUpdateArgs args)
        {
            var sth=base.DefaultPageLoad_LoadEntity(args);

            var ch = OlcStHead.Load(sth.PK);
            if (ch != null)
            {
                // RKUT esetén
                if (!string.IsNullOrEmpty(ch.Onroadtowhid))
                {
                    towhid.Value = ch.Onroadtowhid;
                }


                //RKAT esetén
                if (sth.Stdocid == TransferingHeadBL3.OnRoadStdocIdFrom)
                {
                    var osth = StHead.Load(ch.Onroadfromstid);
                    fromwhid.Value = osth.Fromwhid;
                }
            }
           
            return sth;
        }

        protected override BLObjectMap SaveControlsToBLObjectMap(PageUpdateArgs args, StHead e)
        { 
            var map = base.SaveControlsToBLObjectMap(args, e);

             
            var ch = OlcStHead.Load(e.PK);

            if (ch == null)
            {
                ch = OlcStHead.CreateNew();
                ch.SetKey(e.PK);
            }

            // RKUT esetén
            if (e.Stdocid== TransferingHeadBL3.OnRoadStdocIdTo)
            {
                ch.Onroadtowhid = ConvertUtils.ToString(towhid.Value);
                e.Towhid = TransferingHeadBL3.OnRoadWarehouse;
            }

            //RKAT esetén
            if (e.Stdocid == TransferingHeadBL3.OnRoadStdocIdFrom)
            {
                fromwhid.Value = TransferingHeadBL3.OnRoadWarehouse;
            } 

            map.Add(ch);
            
            return map;
        }

    }
}
