using System;
using System.IO;
using System.Reflection;

namespace WebServer
{
    public class HttpUtility : MarshalByRefObject
    {
        public string MapUrlToPath(string url)
        {
            if (url.ToLower().StartsWith("http://localhost:8080"))
                url = url.Substring(21);

            if (url.StartsWith("/"))
                url = url.Substring(1);

            if (url == string.Empty)
                url = "index.asp";

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), url);
            if (Path.GetFileName(path) == string.Empty)
                path = Path.Combine(path, "index.asp");
            path = path.Replace("/", Path.DirectorySeparatorChar.ToString());
            if (Directory.Exists(path))
                path = Path.Combine(path, "index.asp");

            return path;
        }
    }
}