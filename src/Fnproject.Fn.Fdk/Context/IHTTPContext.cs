using Microsoft.AspNetCore.Http;

namespace Fnproject.Fn.Fdk.Context
{
    public interface IHTTPContext
    {
        IRuntimeContext RuntimeContext();
        string RequestURL();
        string RequestMethod();
        IHeaderDictionary Headers();
        string GetHeaderByKey(string key);
        IQueryCollection Query();
        string[] GetQueryByKey(string key);
        void AddHeader(string key, string value);
        void SetStatus(int statusCode);
    }
}
