using eProjectWeb.Framework;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Sales.Retail.Cart
{
    internal class BarcodeSelectionProvider : DefaultSelectionProvider
    {
        public static readonly string ID = typeof(BarcodeSelectionProvider).FullName;

        static string m_queryString = @"select '' barcode";

        private static QueryArg[] m_argsTemplate2 = new QueryArg[] {
            new QueryArg("barcode", FieldType.String),
        };

        public BarcodeSelectionProvider()
            : base(m_queryString, m_argsTemplate2, BarcodeSelectPage.ID, "itemid")
        {
        }

       
        protected override void GetQuery(Dictionary<string, object> args, out string sql, out string argsStr)
        {
            if (args == null)
                args = new Dictionary<string, object>();

            var b = "";
            if (args.ContainsKey("barcode"))
            {
                b = args["barcode"].ToString();
            }
            sql=$"select {Utils.SqlToString(b)} barcode";
            argsStr = "";
        }
    }
}
