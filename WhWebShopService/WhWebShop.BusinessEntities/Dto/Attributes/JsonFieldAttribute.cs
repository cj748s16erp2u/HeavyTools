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
    public string Condition = null!;
    /// <summary>
    /// A bejövő adat változhat, ezért vissza kell állítani a kosár cache-hez, pl: itemdid esetén töltődik a itemcode
    /// </summary>
    public bool DeleteAnotherfield = false;

    public JsonFieldAttribute(params MandotaryType[] mandotary)
    {
        AddRange2(mandotary);
    }
    public JsonFieldAttribute(string condition, bool deleteanotherfield ,params MandotaryType[] mandotary)
    {
        Condition = condition;
        AddRange2(mandotary);
        DeleteAnotherfield = deleteanotherfield;
    }


    private void AddRange2(MandotaryType[] mandotary)
    {
        foreach (var item in mandotary)
        {
            if (item==MandotaryType.Yes)
            {
                MandotaryDeep.Add(true);
            }
            else
            {
                MandotaryDeep.Add(false);
            }
        }
    }

    public JsonFieldAttribute(RegexpType regexptype, params MandotaryType[] mandotary)
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

public enum MandotaryType
{ 
    No=0,
    Yes=1,
    Pointless=2
}