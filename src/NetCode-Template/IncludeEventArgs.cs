using System;

namespace NetCodeT
{
	/// <summary>
	///   This <see cref="EventArgs" /> descendant is used by <see cref="Template{T}.Include" />.
	/// </summary>
	public sealed class IncludeEventArgs : EventArgs
	{
		private string _Content;

		/// <summary>
		///   Initializes a new instance of the <see cref="IncludeEventArgs" /> class.
		/// </summary>
		/// <param name="name">
		///   Name of the template content to include.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///   <para><paramref name="name" /> is <c>null</c> or empty.</para>
		/// </exception>
		public IncludeEventArgs(string name)
		{
			if (string.IsNullOrWhiteSpace(name))
				throw new ArgumentNullException(nameof(name));

			Name = name;
		}

		/// <summary>
		///   Gets the name of the template content to include.
		/// </summary>
		public string Name
		{
			get;
		}

		/// <summary>
		///   Gets or sets the content to include.
		/// </summary>
		public string Content
		{
			get => _Content;

			set => _Content = (value ?? string.Empty).Trim();
		}

		/// <summary>
		///   Gets or sets a value indicating whether the include directive was handled.
		///   If left at its default, <c>false</c>, the inclusion attempt will fail with an exception.
		/// </summary>
		public bool Handled
		{
			get;
			set;
		}
	}
}