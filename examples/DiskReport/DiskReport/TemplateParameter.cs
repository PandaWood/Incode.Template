using System;

namespace DiskReport
{
    [Serializable]
    public class TemplateParameter : MarshalByRefObject
    {
        public TemplateParameter(Report report, string path, string fileName)
        {
            Report = report;
            Path = path;
            FileName = fileName;
        }

        public Report Report { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}