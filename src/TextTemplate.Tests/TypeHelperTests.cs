using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace TextTemplate.Tests
{
	[TestFixture]
	public class TypeHelperTests
	{
		private class InnerClass
		{
		}

		private IEnumerable<object[]> TestCases()
		{
			yield return new object[] { typeof(string), "global::System.String", "Global.System.String" };
			yield return new object[] { typeof(bool), "global::System.Boolean", "Global.System.Boolean" };
			yield return new object[] { typeof(int), "global::System.Int32", "Global.System.Int32" };
			yield return
				new object[]
				{
					typeof(List<string>), "global::System.Collections.Generic.List<global::System.String>",
					"Global.System.Collections.Generic.List(Of Global.System.String)"
				};
			yield return
				new object[]
				{
					typeof(Dictionary<string, int>), "global::System.Collections.Generic.Dictionary<global::System.String, global::System.Int32>",
					"Global.System.Collections.Generic.Dictionary(Of Global.System.String, Global.System.Int32)"
				};
			yield return
				new object[]
					{ typeof(InnerClass), "global::TextTemplate.Tests.TypeHelperTests.InnerClass", "Global.TextTemplate.Tests.TypeHelperTests.InnerClass" };
		}

		[TestCaseSource(nameof(TestCases))]
		public void TestCases_ProducesCorrectOutputForCSharp(Type type, string expectedCSharpOutput, string expectedVisualBasicOutput)
		{
			Assert.That(TypeHelper.TypeToString(type, true), Is.EqualTo(expectedCSharpOutput));
		}

		[TestCaseSource(nameof(TestCases))]
		public void TestCases_ProducesCorrectOutputForVisualBasic(Type type, string expectedCSharpOutput, string expectedVisualBasicOutput)
		{
			Assert.That(TypeHelper.TypeToString(type, false), Is.EqualTo(expectedVisualBasicOutput));
		}
	}
}