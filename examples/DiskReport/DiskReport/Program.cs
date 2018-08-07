using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using TextTemplate;

namespace DiskReport
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // get the root path of the TextTemplate project
            string rootPath = Path.GetFullPath(Path.Combine(Assembly.GetEntryAssembly().Location, @"..\..\..\..\..\..\"));
            if (rootPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
                rootPath = rootPath.Substring(0, rootPath.Length - 1);

            var report = new Report();
            report.AddPathToReport(rootPath);
            RunReport(report);

            Process.Start("index.html");
        }

        private static void RunReport(Report report)
        {
            var template = new Template<TemplateParameter>();
            try
            {
                template.Content = File.ReadAllText("DirectoryTemplate.txt");
                template.Compile();
            }
            catch (TemplateCompilerException ex)
            {
                foreach (CompilerError error in ex.Errors)
                    Console.Error.WriteLine(error.ErrorNumber + " @ " + error.FileName + "#" + error.Line + "," +
                                            error.Column + " - " + error.ErrorText);
                Environment.Exit(1);
            }

            string path;
            while ((path = report.GetPathToReport()) != null)
            {
                string fileName = report.GetFileForPath(path);
                Console.Out.WriteLine("reporting for " + path);
                string output = template.Execute(new TemplateParameter(report, path, fileName));
                File.WriteAllText(fileName, output, Encoding.UTF8);
            }
        }
    }
}