using System;
using NetCodeT;
using NUnit.Framework;

namespace Tests.NetCodeT
{
	[TestFixture]
	public class TemplateParserTests
	{
		[Test]
		public void Constructor_NullTemplateContent_ThrowsArgumentNullException()
		{
			Assert.Throws<ArgumentNullException>(() => new TemplateTokenizer(null));
		}

		[Test]
		public void Parse_EmptyString_ReturnsEndToken()
		{
			var parser = new TemplateTokenizer(string.Empty);
			var tokens = parser.ToArray();
			CollectionAssert.AreEqual(
				tokens, new[]
				{
					new TemplateToken(TemplateTokenType.End, 0, 1, string.Empty)
				});
		}

		[Test]
		public void Parse_LineNumbers_AreCorrect()
		{
			var parser = new TemplateTokenizer("1\n2\r3\n\r4\r\n5");
			var tokens = parser.ToArray();
			CollectionAssert.AreEqual(
				tokens,
				new[]
				{
					new TemplateToken(TemplateTokenType.Character, 0, 1, "1"),
					new TemplateToken(TemplateTokenType.LineBreak, 1, 1, "\n"),
					new TemplateToken(TemplateTokenType.Character, 2, 2, "2"),
					new TemplateToken(TemplateTokenType.LineBreak, 3, 2, "\r"),
					new TemplateToken(TemplateTokenType.Character, 4, 3, "3"),
					new TemplateToken(TemplateTokenType.LineBreak, 5, 3, "\n\r"),
					new TemplateToken(TemplateTokenType.Character, 7, 4, "4"),
					new TemplateToken(TemplateTokenType.LineBreak, 8, 4, "\r\n"),
					new TemplateToken(TemplateTokenType.Character, 10, 5, "5"),
					new TemplateToken(TemplateTokenType.End, 11, 5, string.Empty)
				});
		}

		[Test]
		public void Parse_StringLiteral_ReturnsCorrectTokens()
		{
			var parser = new TemplateTokenizer("Test123");
			var tokens = parser.ToArray();
			CollectionAssert.AreEqual(
				tokens,
				new[]
				{
					new TemplateToken(TemplateTokenType.Character, 0, 1, "T"),
					new TemplateToken(TemplateTokenType.Character, 1, 1, "e"),
					new TemplateToken(TemplateTokenType.Character, 2, 1, "s"),
					new TemplateToken(TemplateTokenType.Character, 3, 1, "t"),
					new TemplateToken(TemplateTokenType.Character, 4, 1, "1"),
					new TemplateToken(TemplateTokenType.Character, 5, 1, "2"),
					new TemplateToken(TemplateTokenType.Character, 6, 1, "3"),
					new TemplateToken(TemplateTokenType.End, 7, 1, string.Empty)
				});
		}
	}
}