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
<%@ language VBv3.5 %>
<%@ reference System.Core %>
<%@ import System.Linq %>
TextTemplate <--> <%= Reverse(""TextTemplate"") %>
<%+  ' <-- notice the + sign, this means extra code outside of template method

    Public Function Reverse(ByVal s As String) As String
        Return New String(s.Reverse().ToArray())
    End Function
%>
";
        var output = template.Execute();
        
        output.Dump();
    }
}