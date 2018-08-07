using System;
using System.Collections.Generic;
using System.Net;

namespace WebServer
{
    public class SessionManager
    {
        private static readonly Dictionary<string, Session> _Sessions = new Dictionary<string, Session>();

        public static Session GetSession(HttpListenerContext context)
        {
            Cookie cookie = context.Request.Cookies["sessionid"];
            string id;
            Session session;
            lock (_Sessions)
            {
                if (cookie != null)
                {
                    id = cookie.Value;
                    if (_Sessions.TryGetValue(id, out session))
                        return session;
                }

                id = Guid.NewGuid().ToString().Replace("-", "").Replace("{", "").Replace("}", "");
                context.Response.AppendCookie(new Cookie("sessionid", id));
                session = new Session();
                _Sessions[id] = session;
                return session;
            }
        }
    }
}