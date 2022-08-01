using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP4U.Log.LayoutRenderers;

[System.Diagnostics.DebuggerStepThrough]
[LayoutRenderer("action")]
public class ActionLayoutRenderer : LayoutRenderer
{
    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        if (logEvent.HasProperties && logEvent.Properties.ContainsKey("Action"))
        {
            builder.Append(logEvent.Properties["Action"]);
        }
    }
}
