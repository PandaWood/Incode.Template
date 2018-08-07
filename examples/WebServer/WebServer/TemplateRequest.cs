using System;
using System.Net;

namespace WebServer
{
    public class TemplateRequest : MarshalByRefObject
    {
        private readonly HttpListenerRequest _Request;

        public TemplateRequest(HttpListenerRequest request)
        {
            _Request = request;
        }

        public Uri Url
        {
            get
            {
                return _Request.Url;
            }
        }
    }
}