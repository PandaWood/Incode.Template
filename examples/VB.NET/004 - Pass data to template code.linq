<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
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
<%@ language VBv3.5 %>
<%@ import System.Collections.Generic %>
<%
    For Each name As String In Names
%>
Hello, <%= name %>.
<%
    Next
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