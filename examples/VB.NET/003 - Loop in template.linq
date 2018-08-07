<Query Kind="Program">
  <Reference Relative="..\TextTemplate\bin\Debug\TextTemplate.dll">C:\Dev\VS.NET\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

// *****************************************************
// *
// * Shows how to use VB.NET as the template language
// *
// ***********************

void Main()
{
	var template = new Template();
    template.Content = @"<%@ language VBv3.5 %>
<%
    Dim Index As Integer
    For Index = 1 To 20
%>
Index: <%= Index %>
<%
    Next
%>";
    template.Execute().Dump();
}
