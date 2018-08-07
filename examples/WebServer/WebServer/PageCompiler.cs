using System;
using System.Collections.Generic;
using System.IO;
using TextTemplate;

namespace WebServer
{
    public static class PageCompiler
    {
        private static readonly Dictionary<string, CompiledPage> _CompiledPages = new Dictionary<string, CompiledPage>();

        public static Template<TemplateParameter> GetPage(string fileName)
        {
            CompiledPage result;

            lock (_CompiledPages)
            {
                if (_CompiledPages.TryGetValue(fileName, out result))
                {
                    if (File.GetLastWriteTimeUtc(fileName) <= result.WhenCompiled)
                        return result.Template;

                    result.Template.Dispose();
                }
                else
                    result = new CompiledPage();

                result.Template = new Template<TemplateParameter>();
                result.WhenCompiled = DateTime.UtcNow;
                result.Template.Content = File.ReadAllText(fileName);
                try
                {
                    Console.Out.WriteLine(DateTime.Now.ToString("g") + " - compiling " + fileName);
                    result.Template.Compile();
                }
                catch
                {
                    if (_CompiledPages.ContainsKey(fileName))
                        _CompiledPages.Remove(fileName);
                    throw;
                }
                _CompiledPages[fileName] = result;
                return result.Template;
            }
        }
    }
}