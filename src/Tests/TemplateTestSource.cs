using System.Collections.Generic;

namespace Tests.NetCodeT
{
	public class TemplateTestSource
	{
		// ReSharper disable once UnusedMember.Global
		public static IEnumerable<object[]> TestCases()
		{
			/*
			[0] = name of test case, for debugging purposes
			[1] = template content
			[2] = expected output of execution
			*/

			yield return new object[]
			{
				"empty template",
				string.Empty,
				string.Empty
			};
			yield return new object[]
			{
				"basic template",
				"test",
				"test"
			};
			yield return new object[]
			{
				"basic code",
				"<%= 1+2 %>",
				"3"
			};
			yield return new object[]
			{
				"line-wraps #1",
				"1\n2",
				"1\n2"
			};
			yield return new object[]
			{
				"line-wraps #2",
				"1\r2",
				"1\r2"
			};
			yield return new object[]
			{
				"line-wraps #3",
				"<% _GeneratedTemplateOutput.Append(\"1\"); %>\n<%= 2 %>",
				"12"
			};
			yield return new object[]
			{
				"line-wraps #4",
				"<% _GeneratedTemplateOutput.Append(\"1\"); %>\r<%= 2 %>",
				"12"
			};
			yield return new object[]
			{
				"line-wraps #5",
				"<% _GeneratedTemplateOutput.Append(\"1\"); %>\n\r<%= 2 %>",
				"12"
			};
			yield return new object[]
			{
				"line-wraps #6",
				"<% _GeneratedTemplateOutput.Append(\"1\"); %>\r\n<%= 2 %>",
				"12"
			};
			yield return new object[]
			{
				"line-wraps #7",
				"<%= 1 %>\n<%= 2 %>",
				"1\n2"
			};
			yield return new object[]
			{
				"line-wraps #8",
				"<%= 1 %>\r<%= 2 %>",
				"1\r2"
			};
			yield return new object[]
			{
				"line-wraps #9",
				"<%= 1 %>\n\r<%= 2 %>",
				"1\n\r2"
			};
			yield return new object[]
			{
				"line-wraps #10",
				"<%= 1 %>\r\n<%= 2 %>",
				"1\r\n2"
			};
			yield return new object[]
			{
				"using-directives #1",
				@"<%@ use System.IO %><%= Path.GetFileName(@""C:/Temp/test.txt"") %>",
				"test.txt"
			};
			yield return new object[]
			{
				"using-directives #2",
				@"<%@ using System.IO %><%= Path.GetFileName(@""C:/Temp/test.txt"") %>",
				"test.txt"
			};
			yield return new object[]
			{
				"using-directives #3",
				@"<%@ import System.IO %><%= Path.GetFileName(@""C:/Temp/test.txt"") %>",
				"test.txt"
			};
			yield return
				new object[]
				{
					"reference-directive #1",
					@"<%@ ref System.Drawing %><%@ use System.Drawing %><%= Color.FromArgb(255, 192, 192).ToString() %>",
					"Color [A=255, R=255, G=192, B=192]"
				};
			yield return
				new object[]
				{
					"reference-directive #1",
					@"<%@ reference System.Drawing %><%@ use System.Drawing %><%= Color.FromArgb(255, 192, 192).ToString() %>",
					"Color [A=255, R=255, G=192, B=192]"
				};
			yield return
				new object[]
				{
					"reference-directive #1",
					@"<%@ references System.Drawing %><%@ use System.Drawing %><%= Color.FromArgb(255, 192, 192).ToString() %>",
					"Color [A=255, R=255, G=192, B=192]"
				};
			yield return new object[]
			{
				"language-support #1",
				@"<%@ language VBv3.5 %><%= ""1"" & 2 %>",
				"12"
			};
		}
	}
}