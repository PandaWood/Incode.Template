<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

// *****************************************************
// *
// * Shows how to declare and use extra methods in
// * the template
// *
// ***********************

void Main()
{
    using (var template = new Template())
    {
        template.Content = @"
<%@ language C#v3.5 %>
<%@ reference System.Core %>
<%@ using System.Linq %>
TextTemplate <--> <%= Reverse(""TextTemplate"") %>
<%+  // <-- notice the + sign, this means extra code outside of template method

    public string Reverse(string s)
    {
        return new String(s.Reverse().ToArray());
    }
%>
";
        var output = template.Execute();
        
        output.Dump();
    }
}