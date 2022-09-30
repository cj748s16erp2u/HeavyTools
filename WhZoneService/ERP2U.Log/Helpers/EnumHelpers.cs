using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ERP2U.Log.Helpers;

[System.Diagnostics.DebuggerStepThrough]
public static class EnumHelpers
{
    public static string GetDescription(Enum value)
    {
        return value
            .GetType()
            .GetMember(value.ToString())
            .FirstOrDefault()
            ?.GetCustomAttribute<DescriptionAttribute>()
            ?.Description ?? string.Empty;
    }
}
