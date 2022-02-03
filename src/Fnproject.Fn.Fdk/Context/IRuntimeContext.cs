using System;
using System.Collections;

namespace Fnproject.Fn.Fdk.Context
{
    public interface IRuntimeContext
    {
      string AppID();
      string FunctionID();
      string AppName();
      string FunctionName();
      string CallID();
      DateTime Deadline();
      IDictionary Config();
      string ConfigValueByKey(string key);
      ITracingContext TracingContext();
    }
}
