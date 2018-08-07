using System;
using System.Collections.Generic;

namespace WebServer
{
    public class Session : MarshalByRefObject
    {
        private readonly Dictionary<string, string> _Content = new Dictionary<string, string>();

        public string this[string key]
        {
            get
            {
                lock (_Content)
                {
                    string result;
                    if (_Content.TryGetValue(key, out result))
                        return result;

                    return null;
                }
            }

            set
            {
                lock (_Content)
                {
                    if (value == null)
                    {
                        if (_Content.ContainsKey(key))
                            _Content.Remove(key);
                    }
                    else
                        _Content[key] = value;
                }
            }
        }

        public void Abandon()
        {
            lock (_Content)
                _Content.Clear();
        }
    }
}