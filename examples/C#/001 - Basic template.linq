<Query Kind="Program">
  <Reference Relative="..\TextTemplate\bin\Debug\TextTemplate.dll">C:\Dev\VS.NET\TextTemplate\TextTemplate\bin\Debug\TextTemplate.dll</Reference>
  <Namespace>TextTemplate</Namespace>
</Query>

// *****************************************************
// *
// * Shows a basic template that contains the current
// * date and time.
// *
// ***********************

void Main()
{
	using (var template = new Template())
    {
        template.Content = "The current date: <%= DateTime.Now %>.";
        var output = template.Execute();
        
        output.Dump();
    }
}