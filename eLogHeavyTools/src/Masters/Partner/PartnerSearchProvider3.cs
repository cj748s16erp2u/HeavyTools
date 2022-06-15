﻿using System;
using System.Collections.Generic;
using CodaInt.Base.Masters.Partner;
using eLog.Base.Masters.Partner;
using eProjectWeb.Framework.Xml;

namespace eLog.HeavyTools.Masters.Partner
{
    public class PartnerSearchProvider3 : PartnerSearchProvider2, IXmlObjectName
    {
        #region IXmlObjectName

        protected static Type baseType = typeof(PartnerSearchProvider);

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

            query = query.Replace("--morefields", " , olc.oldcode --morefields ");

            query = query.Replace("--morejoins", " left join olc_partner olc (nolock) on olc.partnid = a.partnid --morejoins ");

            return query;

        }
    }
}