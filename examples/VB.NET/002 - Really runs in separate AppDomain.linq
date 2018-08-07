<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
</Query>

// *****************************************************
// *
// * Shows that the text template really executes in its
// * own AppDomain.
// *
// ***********************

void Main()
{
    AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName).Dump("Loaded assemblies before running template");

    using (var template = new Template())
    {
        template.Content = @"
<%@ language VBv3.5 %>
<%@ import System.Reflection %>
Running domain: <%= AppDomain.CurrentDomain.FriendlyName %>

Loaded assemblies:
<% For Each a As Assembly in AppDomain.CurrentDomain.GetAssemblies() %>
  <%= a.FullName %>
<% Next %>";
        var output = template.Execute();
        
        output.Dump();
    }

    AppDomain.CurrentDomain.GetAssemblies().Select(a => a.FullName).Dump("Loaded assemblies after running template");
}