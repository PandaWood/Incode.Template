using System;
using System.Collections.Generic;

namespace DiskReport
{
    public class Report : MarshalByRefObject
    {
        private readonly Dictionary<string, string> _PathsReported = new Dictionary<string, string>();
        private readonly Queue<string> _PathsToReport = new Queue<string>();

        public string AssignFileToPath(string path)
        {
            string fileName;
            if (_PathsReported.Count == 0)
                fileName = "index.html";
            else
                fileName = "path" + _PathsReported.Count + ".html";

            _PathsReported[path] = fileName;
            return fileName;
        }

        public string GetFileForPath(string path)
        {
            if (_PathsReported.ContainsKey(path))
                return _PathsReported[path];

            return null;
        }

        public string GetPathToReport()
        {
            if (_PathsToReport.Count == 0)
                return null;
            else
                return _PathsToReport.Dequeue();
        }

        public string AddPathToReport(string path)
        {
            string fileName = AssignFileToPath(path);
            _PathsToReport.Enqueue(path);
            return fileName;
        }
    }
}