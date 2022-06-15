using System;
using System.Collections.Generic;
using eLog.Base.Masters.Partner;
using eProjectWeb.Framework.Xml;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnAddrSearchProvider3 : PartnAddrSearchProvider, IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(PartnAddrSearchProvider);

        protected override string GetSearchXmlFileName()
        {
            return $"{baseType.Namespace}.{baseType.Name}";
        }

        protected override string GetColorizerID()
        {
            return baseType.Name;
        }

        public override string GetNamespaceName()
        {
            return baseType.Namespace;
        }

        #endregion

        protected override string CreateQueryString(Dictionary<string, object> args, bool fmtonly)
        {
            var query = base.CreateQueryString(args, fmtonly);

            query = query.Replace("--morefields", " , olc.oldcode, olc.glnnum, olc.buildingname --morefields ");

            query = query.Replace("--morejoins", " left join olc_partnaddr olc (nolock) on olc.addrid = a.addrid --morejoins ");

            return query;

        }
    }
}