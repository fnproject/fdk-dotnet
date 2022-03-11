using Microsoft.AspNetCore.Http;

namespace Fnproject.Fn.Fdk.Context
{
    /// <summary>
    /// IHTTPContext provides access to the HTTP Context passed by the service. 
    /// </summary>
    public interface IHTTPContext
    {
        /// <summary>
        /// RuntimeContext returns the IRuntimeContext provided by the service
        /// </summary>
        /// <returns><see cref="Fnproject.Fn.Fdk.Context.IRuntimeContext"/></returns>
        IRuntimeContext RuntimeContext();
        
        /// <summary>
        /// RequestURL returns the HTTP Request URL
        /// </summary>
        /// <returns><see cref="System.String"/> containing the HTTP Request URL</returns>
        string RequestURL();

        /// <summary>
        /// RequestMethod returns the HTTP Request Method
        /// </summary>
        /// <returns><see cref="System.String"/> containing the HTTP Request Method</returns>
        string RequestMethod();

        /// <summary>
        /// Headers returns the HTTP Request Headers
        /// </summary>
        /// <returns>IHeaderDictionary containing the HTTP Request Headers</returns>
        IHeaderDictionary Headers();

        /// <summary>
        /// GetHeaderByKey returns the HTTP header corresponding to the provided key
        /// </summary>
        /// <param name="key">HTTP header name</param>
        /// <returns><see cref="System.String"/> containing the header value corresponding to the header name</returns>
        string GetHeaderByKey(string key);

        /// <summary>
        /// Query returns the HTTP Request Query Params
        /// </summary>
        /// <returns>IQueryCollection containing the HTTP Request Query Params</returns>
        IQueryCollection Query();

        /// <summary>
        /// GetQueryByKey returns the HTTP query values corresponding to the provided key
        /// </summary>
        /// <param name="key">HTTP query name</param>
        /// <returns><see cref="System.String"/> array containing the query values corresponding to the query name</returns>
        string[] GetQueryByKey(string key);

        /// <summary>
        /// AddHeader sets a header in the HTTP Response
        /// </summary>
        /// <param name="key"><see cref="System.String"/> containing the HTTP header name</param>
        /// <param name="value"><see cref="System.String"/> containing the HTTP header value</param>
        void AddHeader(string key, string value);

        /// <summary>
        /// SetStatus sets a HTTP Response Status Code
        /// </summary>
        /// <param name="statusCode">HTTP Response Status Code</param>
        /// <exception cref="System.Net.ProtocolViolationException">
        /// <param name="statusCode"/> is not a valid status code
        /// </exception>
        void SetStatus(int statusCode);
    }
}
