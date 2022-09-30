using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP2U.Log.LayoutRenderers;

[System.Diagnostics.DebuggerStepThrough]
[LayoutRenderer("clientIpAddress")]
public class ClientIpAddressLayoutRenderer : LayoutRenderer
{
    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        if (logEvent.HasProperties && logEvent.Properties.ContainsKey("ClientIpAddress"))
        {
            builder.Append(logEvent.Properties["ClientIpAddress"]);
        }
    }
}
