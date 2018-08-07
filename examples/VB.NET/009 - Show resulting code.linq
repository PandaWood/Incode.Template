<Query Kind="Program">
  <Reference Relative="..\..\TextTemplate\bin\Debug\TextTemplate.dll">C:\dev\vs.net\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
  <Namespace>System.CodeDom.Compiler</Namespace>
</Query>

// *****************************************************
// *
// * Shows how to get hold of the transformed code.
// *
// ***********************

void Main()
{
    using (var template = new Template())
    {
        template.Content = @"<%@ language VBv3.5 %>
<%@ include more %>";
        template.Include += (s, e) =>
        {
            if (e.Name == "more")
            {
                e.Content = "<%= 1 & 2 %>"; // used & to distinguish this from C#
                e.Handled = true;
            }
        };
        
        template.Compile();
        template.Code.Dump();
    }
}