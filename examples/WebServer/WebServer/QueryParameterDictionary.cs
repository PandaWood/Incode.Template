using System;
using System.Collections.Generic;

namespace WebServer
{
    public class QueryParameterDictionary : MarshalByRefObject
    {
        private readonly Dictionary<string, string> _Parameters;

        public QueryParameterDictionary(Dictionary<string, string> parameters)
        {
            _Parameters = parameters;
        }

        public string this[string key]
        {
            get
            {
                string value;
                if (_Parameters.TryGetValue(key, out value))
                    return value;

                return null;
            }
        }
    }
}