using Microsoft.AspNetCore.Http;
using System.Collections;

namespace Fnproject.Fn.Fdk
{
    public interface IContext
    {
        string AppID();
        string FunctionID();
        string AppName();
        string FunctionName();
        string CallID();
        string RequestURL();
        string RequestMethod();
        IHeaderDictionary Headers();
        IDictionary Config();
        void AddHeader(string key, string value);
        void SetStatus(int statusCode);

        ITracingContext TracingContext();
    }
}
