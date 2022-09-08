using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;

public class JsonFieldAttribute : Attribute
{
    List<bool> MandotaryDeep = new List<bool>(); 
    public RegexpType? RegexpType = null;


    public JsonFieldAttribute(params object[] mandotary)
    {
        AddRange2(mandotary);
    }

    private void AddRange2(object[] mandotary)
    {
        foreach (var item in mandotary)
        {
            var b = item as bool?;
            if (b.HasValue)
            {
                MandotaryDeep.Add(b.Value);
            }
            else
            {
                MandotaryDeep.Add(false);
            }
        }
    }

    public JsonFieldAttribute(RegexpType regexptype, params object[] mandotary)
    {
        AddRange2(mandotary);
        this.RegexpType = regexptype;
    }

    public bool IsMandotary(int deep)
    {
        try
        {
            return MandotaryDeep[deep];
        }
        catch (Exception)
        {
            return false;
        }
    }
}

public enum RegexpType
{
    Date,
    Email
}
