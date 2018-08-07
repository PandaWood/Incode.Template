<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\Dev\VS.NET\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

// *****************************************************
// *
// * Shows how to pass information to the template
// * execution.
// *
// ***********************

void Main()
{
    using (var template = new Template<List<string>>("Names"))
    {
        template.Content = @"
<%@ language C#v3.5 %>
<%@ using System.Collections.Generic %>
<%
    foreach (var name in Names)
    {
%>
Hello, <%= name %>.
<%
    }
%>
";
        var output = template.Execute(new List<string>
        {
            "Lasse",
            "Mads",
            "Anders"
        });
        
        output.Dump();
    }
}