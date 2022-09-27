using CodaInt.Base;
using eProjectWeb.Framework.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Reports.Financials.StatementOfAccount
{
    [CODATable("")]
    public class StatementOfAccountItem : VirtualEntity<StatementOfAccountItem>
    {
        [Field("xcvid", DataType = FieldType.Integer, IsMandatory = true, IsPrimaryKey = true)]
        public static Field FieldXcvid { get; protected set; }
        public int? Xcvid
        {
            get { return (int?)this[FieldXcvid]; }
            set { this[FieldXcvid] = value; }
        }

        [Field("seqno", DataType = FieldType.Integer, IsMandatory = true, IsPrimaryKey = true)]
        public static Field FieldSeqno { get; protected set; }
        public int? Seqno
        {
            get { return (int?)this[FieldSeqno]; }
            set { this[FieldSeqno] = value; }
        }

        [Field("company", DataType = FieldType.String, IsMandatory = true)]
        public static Field FieldCompany { get; protected set; }
        public StringN Company
        {
            get { return new StringN(this[FieldCompany]); }
            set { this[FieldCompany] = value; }
        }

        [Field("template", DataType = FieldType.String)]
        public static Field FieldTemplate { get; protected set; }
        public StringN Template
        {
            get { return new StringN(this[FieldTemplate]); }
            set { this[FieldTemplate] = value; }
        }

        public override void Save()
        {
            var bl = StatementOfAccountBL.New();
            bl.SaveXml(this);
        }
    }
}
