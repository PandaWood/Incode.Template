using System;
using System.Linq;
using System.Text;

namespace NetCodeT
{
	/// <summary>
	///   Internal helper routines for <see cref="Type" /> objects.
	/// </summary>
	public static class TypeHelper
	{
		/// <summary>
		/// convenience method to avoid hard-coding strings with the namespace
		/// </summary>
		/// <returns>the namespace</returns>
		public static string GetNamespace()
		{
			return typeof(TypeHelper).Namespace;
		}
		
		/// <summary>
		///   Returns the fully formatted name of the specified type, in C# or VB.NET syntax.
		/// </summary>
		/// <returns>
		///   The fully formatted type name.
		/// </returns>
		/// <param name="type">
		///   The <see cref="Type" /> to get the fully formatted name for.
		/// </param>
		/// <param name="csharpSyntax">
		///   If <c>true</c>, return C# syntax for the fully formatted name;
		///   otherwise, return VB.NET syntax.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///   <para><paramref name="type" /> is <c>null</c>.</para>
		/// </exception>
		public static string TypeToString(Type type, bool csharpSyntax)
		{
			if (type == null)
			{
				throw new ArgumentNullException(nameof(type));
			}

			var sb = new StringBuilder();

			if (type.IsNested)
			{
				sb.Append(TypeToString(type.DeclaringType, csharpSyntax) + ".");
			}
			else
			{
				sb.Append(csharpSyntax ? "global::" : "Global.");
				sb.Append(type.Namespace + ".");
			}

			if (type.IsGenericType)
			{
				var name = type.Name;
				var quoteIndex = name.IndexOf('`');
				if (quoteIndex > 0)
					name = name.Substring(0, quoteIndex);
				sb.Append(name);

				if (csharpSyntax)
					sb.Append('<');
				else
					sb.Append("(Of ");

				sb.Append(string.Join(", ", type.GetGenericArguments().Select(t => TypeToString(t, csharpSyntax)).ToArray()));
				sb.Append(csharpSyntax ? '>' : ')');
			}
			else
			{
				sb.Append(type.Name);
			}

			return sb.ToString();
		}
	}
}