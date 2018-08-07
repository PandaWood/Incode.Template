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
        template.Content = @"<%@ include more %>";
        template.Include += (s, e) =>
        {
            if (e.Name == "more")
            {
                e.Content = "<%= 1 + 2 %>";
                e.Handled = true;
            }
        };
        
        template.Compile();
        template.Code.Dump();
    }
}