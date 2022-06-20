using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using eLog.HeavyTools.ImportBase;

namespace eLog.HeavyTools.Masters.Item.Import
{
    class ItemImportConditional : ImportConditional
    {
        public new ItemImportConditionalType? Type
        {
            get
            {
                var type = base.Type;
                if (type.HasValue)
                {
                    return (ItemImportConditionalType)type.Value;
                }

                return null;
            }
            set
            {
                if (value.HasValue)
                {
                    base.Type = (ImportConditionalType)value.Value;
                }
                else
                {
                    base.Type = null;
                }
            }
        }

        public string RefColumn { get; set; }
        public string RefColumnName { get; set; }
        [Newtonsoft.Json.JsonIgnore]
        public int? RefColumnIndex { get; set; }
    }

    public enum ItemImportConditionalType
    {
        Unknown = ImportConditionalType.Unknown,
        IsNotEmpty = ImportConditionalType.IsNotEmpty,
        Expression = ImportConditionalType.Expression,

        CheckCompanyHierarchy
    }
}
