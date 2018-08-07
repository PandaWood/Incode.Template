using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;

namespace TextTemplate.Tests
{
	[TestFixture]
	public class TemplateTests
	{
		[TestCase((string)null)]
		[TestCase("")]
		[TestCase(" \r\n\t ")]
		public void Constructor_NullOrEmptyParameterName_ThrowsArgumentNullException(string input)
		{
			Assert.Throws<ArgumentNullException>(() => new Template<string>(input));
		}

		[Serializable]
		private class ParameterClass
		{
			public string Name
			{
				get;
				set;
			}

			public int Age
			{
				get;
				set;
			}
		}

		public void NoParameterName_MakesPropertiesOfParameterTypeAvailable_InCSharp()
		{
			var template = new Template<ParameterClass>
			{
				Content = "Hello, <%= Name %>, happy <%= Age %>th birthday!"
			};

			var output = template.Execute(
				new ParameterClass
				{
					Name = "Dummy",
					Age = 18
				});

			Assert.That(output, Is.EqualTo("Hello, Dummy, happy 18th birthday!"));
		}

		public void NoParameterName_MakesPropertiesOfParameterTypeAvailable_InVB()
		{
			var template = new Template<ParameterClass>
			{
				Content = @"<%@ language VBv3.5 %>
Hello, <%= Name %>, happy <%= Age %>th birthday!"
			};

			var output = template.Execute(
				new ParameterClass
				{
					Name = "Dummy",
					Age = 18
				});

			Assert.That(output, Is.EqualTo("Hello, Dummy, happy 18th birthday!"));
		}

		[Test]
		public void Compile_EmptyTemplate_SetsIsCompiledFlag()
		{
			using (var template = new Template())
			{
				template.Compile();

				Assert.That(template.IsCompiled, Is.True);
			}
		}

		[Test]
		public void Compile_Twice_DoesNotThrowException()
		{
			using (var template = new Template())
			{
				template.Compile();
				template.Compile();
			}
		}

		[Test]
		public void Compile_WithIncompleteTemplateCode_ThrowsTemplateSyntaxException()
		{
			using (var template = new Template())
			{
				template.Content = "<%= xxx %";

				Assert.Throws<TemplateSyntaxException>(() => template.Compile());
			}
		}

		[Test]
		public void Compile_WithInvalidTemplateCode_DoesNotSetIsCompiledFlag()
		{
			using (var template = new Template())
			{
				template.Content = "<%= xxx %>";

				Assert.Throws<TemplateCompilerException>(() => template.Compile());
				Assert.That(template.IsCompiled, Is.False);
			}
		}

		[Test]
		public void Compile_WithInvalidTemplateCode_ThrowsTemplateCompilerException()
		{
			using (var template = new Template())
			{
				template.Content = "<%= xxx %>";

				Assert.Throws<TemplateCompilerException>(() => template.Compile());
			}
		}

		[Test]
		public void Constructor_StartsOutWithEmptyTemplate()
		{
			using (var template = new Template())
			{
				Assert.That(template.Content, Is.EqualTo(string.Empty));
			}
		}

		[Test]
		public void Constructor_StartsOutWithUncompiledTemplate()
		{
			using (var template = new Template())
			{
				Assert.That(template.IsCompiled, Is.False);
			}
		}

		[Test]
		public void CyclicIncludes_ThrowsInvalidOperationException()
		{
			using (var template = new Template())
			{
				template.Content = @"<%@ include first %>";
				template.Include += (s, e) =>
				{
					switch (e.Name)
					{
						case "first":
							e.Content = "<%@ include second %>";
							e.Handled = true;
							break;

						case "second":
							e.Content = "<%@ include first %>";
							e.Handled = true;
							break;
					}
				};

				Assert.Throws<InvalidOperationException>(() => template.Compile());
			}
		}

		[Test]
		public void EnsureTemplateCodeIsNotLoadedIntoMainAppDomain()
		{
			using (var template = new Template())
			{
				template.Content = "<% %>";

				var output = template.Execute();

				Assert.That(output, Is.EqualTo(string.Empty));
			}

			bool wasLoadedHere =
				(from assembly in AppDomain.CurrentDomain.GetAssemblies()
					from type in assembly.GetTypes()
					where (type.FullName ?? string.Empty).StartsWith("GeneratedTextTemplateNamespace.")
					select 1).Any();

			Assert.That(wasLoadedHere, Is.False);
		}

		[Test]
		[TestCaseSource(typeof(TemplateTestSource), nameof(TemplateTestSource.TestCases))]
		public void Execute_WithTestCases_ProducesCorrectOutput(string testName, string input, string expected)
		{
			using (var template = new Template())
			{
				try
				{
					template.Content = input;
					var output = template.Execute();

					Assert.That(output, Is.EqualTo(expected));
				}
				catch (TemplateCompilerException ex)
				{
					foreach (CompilerError err in ex.Errors) Debug.WriteLine((err.IsWarning ? "WARN " : "ERROR ") + err.FileName + "#" + err.Line + "," + err.Column + ": " + err.ErrorText);
					throw;
				}
			}
		}

		[Test]
		public void ExtraCodeBlocks_AllowsInlineNonCodeBlocks_InCSharp()
		{
			using (var template = new Template())
			{
				template.Content = @"123<% More(); %>789<%+
public void More()
{
%>
456<%+
}
%>
";
				var output = template.Execute();

				Assert.That(output, Is.EqualTo("123456789"));
			}
		}

		[Test]
		public void ExtraCodeBlocks_AllowsInlineNonCodeBlocks_InVisualBasic()
		{
			using (var template = new Template())
			{
				template.Content = @"<%@ language VBv3.5 %>123<% More() %>789<%+
Public Sub More()
%>
456<%+
End Sub
%>
";
				var output = template.Execute();

				Assert.That(output, Is.EqualTo("123456789"));
			}
		}

		[Test]
		public void Includes_AreHandledCorrectly()
		{
			using (var template = new Template())
			{
				template.Content = @"<%@ include more %>";
				template.Include += (s, e) =>
				{
					e.Content = "<%= 1 + 2 %>";
					e.Handled = true;
				};

				var output = template.Execute();

				Assert.That(output, Is.EqualTo("3"));
			}
		}

		[Test]
		public void InvalidReference_ThrowsTemplateCompilerException()
		{
			using (var template = new Template())
			{
				template.Content = "<%@ reference XYZ %>";

				Assert.Throws<TemplateCompilerException>(() => template.Compile());
			}
		}

		[Test]
		public void LessThanSignOnItsOwn_DoesNotCrashTheTokenizer()
		{
			using (var template = new Template())
			{
				template.Content = @"<<%= 1 %>";
				template.Compile();
			}
		}

		[Test]
		public void LessThanSignOnItsOwn_InsideCodeBlock_DoesNotEndCodeBlockTokenizing()
		{
			using (var template = new Template())
			{
				template.Content = @"<% //< %>";
				template.Compile();
			}
		}

		[Test]
		public void MultiLevelIncludes_AreHandledCorrectly()
		{
			using (var template = new Template())
			{
				template.Content = @"<%@ include first %>";
				template.Include += (s, e) =>
				{
					switch (e.Name)
					{
						case "first":
							e.Content = "<%@ include second %>";
							e.Handled = true;
							break;

						case "second":
							e.Content = "<%@ include third %>";
							e.Handled = true;
							break;

						case "third":
							e.Content = "<%= 1 + 2 %>";
							e.Handled = true;
							break;
					}
				};

				var output = template.Execute();

				Assert.That(output, Is.EqualTo("3"));
			}
		}

		[Test]
		public void ParameterNameSpecified_MakesParameterAvailableUnderThatName()
		{
			var template = new Template<string>("Name");
			template.Content = "Hello, <%= Name %>!";

			var output = template.Execute("World");

			Assert.That(output, Is.EqualTo("Hello, World!"));
		}

		[Test]
		public void PercentageThanSignOnItsOwn_OutsideCodeBlock_DoesNotStartCodeBlockTokenizing()
		{
			using (var template = new Template())
			{
				template.Content = @"% <% %>";
				template.Compile();
			}
		}

		[Test]
		public void SettingTemplate_AfterCompiling_ClearsIsCompiledFlag()
		{
			using (var template = new Template())
			{
				template.Compile();
				template.Content = "test";

				Assert.That(template.IsCompiled, Is.False);
			}
		}

		[Test]
		public void TemplateWithNestedClass_CompilesAndRuns_InCSharp()
		{
			using (var template = new Template())
			{
				template.Content =
					@"
<%
  var x = new NestedClass();
%>
<%= x.Report %><%+
public class NestedClass
{
    public string Report
    {
        get
        {
            return ""success"";
        }
    }
}
%>
";
				template.Compile();
				var output = template.Execute();

				Assert.That(output, Is.EqualTo("success"));
			}
		}

		[Test]
		public void TemplateWithNestedClass_CompilesAndRuns_InVisualBasic()
		{
			using (var template = new Template())
			{
				template.Content =
					@"<%@ language VBv3.5 %>
<%
  Dim x as New NestedClass
%>
<%= x.Report %><%+
Public Class NestedClass
    Public ReadOnly Property Report() As String
        Get
            Return ""success""
        End Get
    End Property
End Class
%>
";
				template.Compile();
				var output = template.Execute();

				Assert.That(output, Is.EqualTo("success"));
			}
		}

		[Test]
		public void UnknownDirective_ThrowsException()
		{
			using (var template = new Template())
			{
				template.Content = "<%@ XYZ KLM %>";
				Assert.Throws<TemplateSyntaxException>(() => template.Compile());
			}
		}
	}
}