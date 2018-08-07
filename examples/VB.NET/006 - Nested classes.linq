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
<%@ language VBv3.5 %>
<%@ import System.Collections.Generic %>
<%
    Dim instance As new NestedClass
    For Each entry As String In instance.Entries
%>
<%= entry %>
<%
    Next
%>
<%+  ' <-- notice the + sign, this means extra code outside of template method

    Public Class NestedClass
        Public ReadOnly Property Entries As IEnumerable(Of String)
            Get
                Dim result As New List(Of String)
                result.Add(""First entry"")
                result.Add(""Second entry"")
                result.Add(""Third and last entry"")
                Return result
            End Get
        End Property
    End Class
%>
";
        var output = template.Execute();
        
        output.Dump();
    }
}