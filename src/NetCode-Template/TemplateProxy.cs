using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
// ReSharper disable UnusedMember.Global

namespace NetCodeT
{
	/// <summary>
	///   This class is loaded into its own <see cref="AppDomain" />
	///   and hosts the compiled code from a template.
	/// </summary>
	[Serializable]
	internal class TemplateProxy : MarshalByRefObject, ITemplateProxy
	{
		private Assembly _assembly;
		private string _code;

		#region ITemplateProxy Members

		/// <summary>
		///   Compiles the template content.
		/// </summary>
		/// <param name="content">
		///   The template content to compile.
		/// </param>
		/// <param name="handlers">
		///   The set of known languages and their handler types.
		/// </param>
		/// <param name="parameterType">
		///   The <see cref="Type" /> of the parameter to the template.
		/// </param>
		/// <param name="parameterName">
		///   The name of the parameter to the template. If left as <see cref="String.Empty" />, the public readable
		///   properties of the <paramref name="parameterType" /> will be exposed as their own parameters instead.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///   <para><paramref name="content" /> is <c>null</c>.</para>
		///   <para>- or -</para>
		///   <para><paramref name="handlers" /> is <c>null</c>.</para>
		///   <para>- or -</para>
		///   <para><paramref name="parameterType" /> is <c>null</c>.</para>
		///   <para>- or -</para>
		///   <para><paramref name="parameterName" /> is <c>null</c>.</para>
		/// </exception>
		public void Compile(string content, Dictionary<string, Type> handlers, Type parameterType, string parameterName)
		{
			if (content == null)
				throw new ArgumentNullException(nameof(content));
			if (handlers == null)
				throw new ArgumentNullException(nameof(handlers));
			if (parameterType == null)
				throw new ArgumentNullException(nameof(parameterType));
			if (parameterName == null)
				throw new ArgumentNullException(nameof(parameterName));

			var parser = new TemplateParser(content);
			parser.Parse();

			if (!handlers.TryGetValue(parser.Language, out var handlerType))
				throw new NotSupportedException(string.Format(CultureInfo.InvariantCulture, 
					"Language '{0}' is not supported as a template language", parser.Language));

			var handler = (ITemplateLanguageHandler)Activator.CreateInstance(handlerType);
			_assembly = handler.RewriteAndCompile(parser.Language, parser.NamespaceImports, parser.AssemblyReferences, parser.CodeParts, parameterType, parameterName, out _code);
		}

		/// <summary>
		///   Executes the template and returns the generated output.
		/// </summary>
		/// <param name="templateState">
		///   The state for the template to process.
		/// </param>
		/// <returns>
		///   The generated output.
		/// </returns>
		public string Execute(object templateState)
		{
			var templateInstance = _assembly.CreateInstance("GeneratedNetCodeTemplateNamespace.GeneratedNetCodeTemplateClass");
			var result = (string)templateInstance.GetType().GetMethod("ExecuteTemplateCode").Invoke(templateInstance, new[] { templateState });
			return result ?? string.Empty;
		}

		/// <summary>
		///   Returns the code that was compiled, for debugging purposes.
		/// </summary>
		/// <returns>The code that was compiled.</returns>
		public string GetCode()
		{
			return _code;
		}

		#endregion
	}
}