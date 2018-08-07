<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

// *****************************************************
// *
// * Simple loop that runs from 1 to 20.
// *
// ***********************

void Main()
{
	var template = new Template();
    template.Content = @"<%@ language C#v3.5 %>
<%
    for (int index = 1; index <= 20; index++)
    {
%>
Index: <%= index %>
<%
    }
%>";
    template.Execute().Dump();
}