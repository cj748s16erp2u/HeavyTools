using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eLog.HeavyTools.Services.WhWebShop.BusinessEntities.Dto.Attributes;

public class JsonFieldAttribute : Attribute
{
    public bool Mandotary;
    public bool Mandotarysub;
    public RegexpType? RegexpType = null;


    public JsonFieldAttribute(bool mandotary)
    {
        this.Mandotary = mandotary;
        this.Mandotarysub = mandotary;
    }

    public JsonFieldAttribute(bool mandotary, bool mandotarysub)
    {
        this.Mandotary = mandotary;
        this.Mandotarysub = mandotarysub;
    }

    public JsonFieldAttribute(bool mandotary, RegexpType regexptype)
    {
        this.Mandotary = mandotary;
        this.Mandotarysub = mandotary;
        this.RegexpType = regexptype;
    }
}

public enum RegexpType
{
    Date,
    Email
}
