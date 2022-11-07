﻿using NLog;
using NLog.LayoutRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ERP2U.Log.LayoutRenderers;

[System.Diagnostics.DebuggerStepThrough]
[LayoutRenderer("requestBody")]
public class RequestBodyLayoutRenderer : LayoutRenderer
{
    protected override void Append(StringBuilder builder, LogEventInfo logEvent)
    {
        if (logEvent.HasProperties && logEvent.Properties.ContainsKey("RequestBody"))
        {
            builder.Append(logEvent.Properties["RequestBody"]);
        }
    }
}
