using System.Collections.Generic;
using Jd.Api.Request;

namespace Jd.Api.Jos
{
    /// <summary>
    /// JOS专用Jd客户端。
    /// </summary>
    public class JosJdClient
    {
        private const string SYNC_CENTER_URL = "http://eai.jd.com/api";

        private DefaultJdClient JdClient;

        public JosJdClient(string serverUrl, string appKey, string appSecret)
        {
            this.JdClient = new DefaultJdClient(serverUrl, appKey, appSecret);
            this.JdClient.SetDisableParser(true);
        }

        public JosJdClient(string appKey, string appSecret)
            : this(SYNC_CENTER_URL, appKey, appSecret)
        {
        }

        public JosJdClient(string serverUrl, string appKey, string appSecret, int timeout)
            : this(serverUrl, appKey, appSecret)
        {
            this.JdClient.SetTimeout(timeout);
        }

        public  string execute(string apiName, IDictionary<string, string> parameters, string session)
        {
            JosJDRequest request = new JosJDRequest();
            request.ApiName = apiName;
            request.Parameters = parameters;
            JosJDResponse response = JdClient.Execute(request, session);
            return response.Body;
        }
    }

    public class JosJDRequest : IJdRequest<JosJDResponse>
    {
        public string ApiName { get; set; }
        public IDictionary<string, string> Parameters { get; set; }

        public string GetApiName()
        {
            return this.ApiName;
        }

        public IDictionary<string, string> GetParameters()
        {
            return this.Parameters;
        }

        public void Validate()
        {
        }
    }

    public class JosJDResponse : JdResponse
    {
    }
}
