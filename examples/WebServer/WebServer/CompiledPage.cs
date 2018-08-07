using System;
using TextTemplate;

namespace WebServer
{
    public class CompiledPage
    {
        public Template<TemplateParameter> Template;
        public DateTime WhenCompiled;
    }
}