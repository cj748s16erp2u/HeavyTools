using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP2U.Log.LayoutRenderers;

[System.Diagnostics.DebuggerStepThrough]
[LayoutRenderer("category")]
public class CategoryLayoutRenderer : LayoutRenderer
{
    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        if (logEvent.HasProperties && logEvent.Properties.ContainsKey("Category"))
        {
            builder.Append(Helpers.EnumHelpers.GetDescription((Enum)logEvent.Properties["Category"]));
        }
    }
}
