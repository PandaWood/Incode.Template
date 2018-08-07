using System;
using System.Net;

namespace WebServer
{
    public class TemplateResponse : MarshalByRefObject
    {
        private readonly HttpListenerResponse _Response;

        public TemplateResponse(HttpListenerResponse response)
        {
            _Response = response;
        }

        public void Redirect(string url)
        {
            _Response.Redirect(url);
        }
    }
}