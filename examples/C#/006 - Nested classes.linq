<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

// *****************************************************
// *
// * Shows how to declare and use nested classes in
// * template.
// *
// ***********************

void Main()
{
    using (var template = new Template())
    {
        template.Content = @"
<%@ language C#v3.5 %>
<%@ using System.Collections.Generic %>
<%
    var instance = new NestedClass();
    foreach (var entry in instance.Entries)
    {
%>
<%= entry %>
<%
    }
%>
<%+  // <-- notice the + sign, this means extra code outside of template method

    public class NestedClass
    {
        public IEnumerable<string> Entries
        {
            get
            {
                yield return ""First entry"";
                yield return ""Second entry"";
                yield return ""Third and last entry"";
            }
        }
    }
%>
";
        var output = template.Execute();
        
        output.Dump();
    }
}