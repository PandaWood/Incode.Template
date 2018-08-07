using System;
using System.Collections.Generic;
using System.Net;

namespace WebServer
{
    public class TemplateParameter : MarshalByRefObject
    {
        private readonly HttpUtility _HttpUtility = new HttpUtility();
        private readonly QueryParameterDictionary _Parameters;
        private readonly TemplateRequest _Request;
        private readonly TemplateResponse _Response;
        private readonly Session _Session;

        public TemplateParameter(HttpListenerRequest request, HttpListenerResponse response,
                                 Dictionary<string, string> parameters, Session session)
        {
            _Parameters = new QueryParameterDictionary(parameters);
            _Request = new TemplateRequest(request);
            _Response = new TemplateResponse(response);
            _Session = session;
        }

        public TemplateRequest Request
        {
            get
            {
                return _Request;
            }
        }

        public TemplateResponse Response
        {
            get
            {
                return _Response;
            }
        }

        public HttpUtility HttpUtility
        {
            get
            {
                return _HttpUtility;
            }
        }

        public QueryParameterDictionary QueryString
        {
            get
            {
                return _Parameters;
            }
        }

        public Session Session
        {
            get
            {
                return _Session;
            }
        }
    }
}