<Query Kind="Program">
  <Reference Relative="..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

// *****************************************************
// *
// * Shows how to reference additional assemblies
// * from the template.
// *
// ***********************

void Main()
{
    using (var template = new Template())
    {
        template.Content = @"
<%@ language C#v3.5 %>
<%@ reference System.Drawing %>
<%@ using System.Drawing %>
<%@ using System.Reflection %>
<%
    foreach (var prop in typeof(Color).GetProperties(BindingFlags.Static | BindingFlags.Public))
    {
        var color = (Color)prop.GetValue(null, null);
%>
<%= prop.Name %> = <%= color.R %>, <%= color.G %>, <%= color.B %>
<%
    }
%>
";
        var output = template.Execute();
        output.Dump();
    }
}