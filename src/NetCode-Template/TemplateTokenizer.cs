using System;
using System.Collections.Generic;
#pragma warning disable 1591

namespace NetCodeT
{
	public class TemplateTokenizer
	{
		private readonly string _templateContent;
		private bool _inCodeBlock;
		private int _index;
		private int _lineNumber;
		private TemplateToken _next;

		public TemplateTokenizer(string templateContent)
		{
			_templateContent = templateContent ?? throw new ArgumentNullException(nameof(templateContent));
			_index = 0;
			_lineNumber = 1;

			MoveNext();
		}

		public bool More => _next.Type != TemplateTokenType.End;

		public TemplateToken Peek()
		{
			return _next;
		}

		public TemplateToken Next()
		{
			var next = _next;
			MoveNext();
			return next;
		}

		private void MoveNext()
		{
			_next = GetNextToken();
		}

		public TemplateToken[] ToArray()
		{
			var result = new List<TemplateToken>();
			while (true)
			{
				var token = Next();
				result.Add(token);
				if (token.Type == TemplateTokenType.End)
					break;
			}

			return result.ToArray();
		}

		private TemplateToken GetNextToken()
		{
			if (_index >= _templateContent.Length)
				return new TemplateToken(TemplateTokenType.End, _index, _lineNumber, string.Empty);

			switch (_templateContent[_index])
			{
				case '<':
					if (!_inCodeBlock && _index + 1 < _templateContent.Length && _templateContent[_index + 1] == '%')
					{
						var result = new TemplateToken(TemplateTokenType.CodeBlockStart, _index, _lineNumber, "<%");
						_index += 2;
						_inCodeBlock = true;
						return result;
					}
					else
					{
						var result = new TemplateToken(TemplateTokenType.Character, _index, _lineNumber, "<");
						_index += 1;
						return result;
					}

				case '%':
					if (_inCodeBlock && _index + 1 < _templateContent.Length && _templateContent[_index + 1] == '>')
					{
						var result = new TemplateToken(TemplateTokenType.CodeBlockEnd, _index, _lineNumber, "%>");
						_index += 2;
						_inCodeBlock = false;
						return result;
					}
					else
					{
						var result = new TemplateToken(TemplateTokenType.Character, _index, _lineNumber, "%");
						_index += 1;
						return result;
					}

				case '\n':
					if (_index + 1 < _templateContent.Length && _templateContent[_index + 1] == '\r')
					{
						var result = new TemplateToken(TemplateTokenType.LineBreak, _index, _lineNumber, "\n\r");
						_index += 2;
						_lineNumber++;
						return result;
					}
					else
					{
						var result = new TemplateToken(TemplateTokenType.LineBreak, _index, _lineNumber, "\n");
						_index += 1;
						_lineNumber++;
						return result;
					}

				case '\r':
					if (_index + 1 < _templateContent.Length && _templateContent[_index + 1] == '\n')
					{
						var result = new TemplateToken(TemplateTokenType.LineBreak, _index, _lineNumber, "\r\n");
						_index += 2;
						_lineNumber++;
						return result;
					}
					else
					{
						var result = new TemplateToken(TemplateTokenType.LineBreak, _index, _lineNumber, "\r");
						_index += 1;
						_lineNumber++;
						return result;
					}

				default:
					var characterResult = new TemplateToken(TemplateTokenType.Character, _index, _lineNumber, _templateContent[_index].ToString());
					_index++;
					return characterResult;
			}
		}
	}
}