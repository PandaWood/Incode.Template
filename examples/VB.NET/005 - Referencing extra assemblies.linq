<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
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
        template.Content = @"<%@ language VBv3.5 %>
<%@ reference System.Drawing %>
<%@ import System.Drawing %>
<%@ import System.Reflection %>
<%
    For Each prop As PropertyInfo In GetType(Color).GetProperties(BindingFlags.Static Or BindingFlags.Public)
        Dim color As Color = CType(prop.GetValue(Nothing, Nothing), Color)
%>
<%= prop.Name %> = <%= color.R %>, <%= color.G %>, <%= color.B %>
<%
    Next
%>
";
        var output = template.Execute();
        output.Dump();
    }
}