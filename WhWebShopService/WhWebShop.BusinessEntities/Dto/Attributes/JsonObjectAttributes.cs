using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;

public class JsonObjectAttributes : Attribute
{
    public JsonObjectAttributes(string objectname)
    {
        ObjectName = objectname;
    }

    public string ObjectName { get; }
}
