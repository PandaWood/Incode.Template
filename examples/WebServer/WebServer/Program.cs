using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using TextTemplate;
using WebServer.Properties;

namespace WebServer
{
    internal class Program
    {
        private static readonly Dictionary<string, string> _MimeTypes = new Dictionary<string, string>
                                                                            {
                                                                                {".png", "image/png"},
                                                                                {".gif", "image/gif"},
                                                                                {".jpg", "image/jpeg"},
                                                                                {".jpeg", "image/jpeg"},
                                                                                {".txt", "text/plain"},
                                                                                {".css", "text/css"},
                                                                                {".js", "application/x-javascript"},
                                                                                {",ico", "image/vnd.microsoft.icon"},
                                                                            };

        private static void Main(string[] args)
        {
            var server = new HttpListener();
            server.Prefixes.Add("http://localhost:8080/");
            server.Start();

            Process.Start("http://localhost:8080");

            while (true)
            {
                HttpListenerContext context = server.GetContext();
                ThreadPool.QueueUserWorkItem(ServeRequest, context);
            }
        }

        private static void ServeRequest(object state)
        {
            var context = (HttpListenerContext) state;
            try
            {
                try
                {
                    string url = context.Request.Url.ToString();
                    var parameters = new Dictionary<string, string>();
                    int paramIndex = url.IndexOf('?');
                    if (paramIndex > 0)
                    {
                        LoadParameters(url.Substring(paramIndex + 1), parameters);
                        url = url.Substring(0, paramIndex);
                    }
                    string fileName = new HttpUtility().MapUrlToPath(url);

                    if (!File.Exists(fileName))
                    {
                        context.Response.StatusCode = 404;
                        return;
                    }

                    if (Path.GetExtension(fileName).ToLower() == ".asp")
                    {
                        Session session = SessionManager.GetSession(context);
                        Template<TemplateParameter> template = PageCompiler.GetPage(fileName);
                        string output =
                            template.Execute(new TemplateParameter(context.Request, context.Response, parameters,
                                                                   session));

                        context.Response.ContentType = "text/html";
                        byte[] bytes = Encoding.UTF8.GetBytes(output);
                        context.Response.ContentEncoding = Encoding.UTF8;
                        context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                    }
                    else
                    {
                        string mimeType;
                        if (_MimeTypes.TryGetValue(Path.GetExtension(fileName).ToLower(), out mimeType))
                            context.Response.ContentType = mimeType;
                        else
                            context.Response.ContentType = "application/octet-stream";
                        using (var stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            var buffer = new byte[4096];
                            int inBuffer;
                            context.Response.ContentLength64 = stream.Length;
                            while ((inBuffer = stream.Read(buffer, 0, buffer.Length)) > 0)
                                context.Response.OutputStream.Write(buffer, 0, inBuffer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    var errorTemplate = new Template<Exception>("Exception");
                    errorTemplate.Content = Resources.ExceptionErrorPage;
                    string output = errorTemplate.Execute(ex);

                    context.Response.ContentType = "text/html";
                    byte[] bytes = Encoding.UTF8.GetBytes(output);
                    context.Response.ContentEncoding = Encoding.UTF8;
                    context.Response.OutputStream.Write(bytes, 0, bytes.Length);
                }
            }
            finally
            {
                Console.Out.WriteLine(context.Response.StatusCode + " - " + DateTime.Now.ToString("g") + " - " +
                                      context.Request.Url);
                context.Response.Close();
            }
        }

        private static void LoadParameters(string parameterString, Dictionary<string, string> parameters)
        {
            foreach (string keyValue in parameterString.Split('&'))
            {
                int equalIndex = keyValue.IndexOf('=');
                if (equalIndex < 0)
                {
                    parameters[keyValue] = "";
                }
                else
                {
                    string key = keyValue.Substring(0, equalIndex);
                    string value = System.Web.HttpUtility.UrlDecode(keyValue.Substring(equalIndex + 1));
                    parameters[key] = value;
                }
            }
        }
    }
}