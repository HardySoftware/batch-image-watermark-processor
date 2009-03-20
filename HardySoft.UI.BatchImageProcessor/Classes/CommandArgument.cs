using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Text.RegularExpressions;

namespace HardySoft.UI.BatchImageProcessor.Classes {
	/// <summary>
	/// Command line argument parser.
	/// </summary>
	/// <remarks>
	/// Original author of this class is http://www.codeproject.com/KB/recipes/command_line.aspx
	/// </remarks>
	class CommandArgument {
		// Variables
		private StringDictionary parameters;

		// Constructor
		public CommandArgument(string[] args) {
			parameters = new StringDictionary();
			Regex spliter = new Regex(@"^-{1,2}|^/|=|:", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			Regex remover = new Regex(@"^['""]?(.*?)['""]?$", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			string parameter = null;
			string[] parts;

			// Valid parameters forms:
			// {-,/,--}param{ ,=,:}((",')value(",'))
			// Examples: -param1 value1 --param2 /param3:"Test-:-work" /param4=happy -param5 '--=nice=--'
			foreach (string text in args) {
				// Look for new parameters (-,/ or --) and a possible enclosed value (=,:)
				parts = spliter.Split(text, 3);
				switch (parts.Length) {
					// Found a value (for the last parameter found (space separator))
					case 1:
						if (parameter != null) {
							if (!parameters.ContainsKey(parameter)) {
								parts[0] = remover.Replace(parts[0], "$1");
								parameters.Add(parameter, parts[0]);
							}
							parameter = null;
						}
						// else Error: no parameter waiting for a value (skipped)
						break;
					// Found just a parameter
					case 2:
						// The last parameter is still waiting. With no value, set it to true.
						if (parameter != null) {
							if (!parameters.ContainsKey(parameter)) {
								parameters.Add(parameter, "true");
							}
						}
						parameter = parts[1];
						break;
					// parameter with enclosed value
					case 3:
						// The last parameter is still waiting. With no value, set it to true.
						if (parameter != null) {
							if (!parameters.ContainsKey(parameter)) {
								parameters.Add(parameter, "true");
							}
						}
						parameter = parts[1];
						// Remove possible enclosing byteArray (",')
						if (!parameters.ContainsKey(parameter)) {
							parts[2] = remover.Replace(parts[2], "$1");
							parameters.Add(parameter, parts[2]);
						}
						parameter = null;
						break;
				}
			}
			// In case a parameter is still waiting
			if (parameter != null) {
				if (!parameters.ContainsKey(parameter))
					parameters.Add(parameter, "true");
			}
		}

		/// <summary>
		/// Retrieve a parameter value if it exists
		/// </summary>
		/// <param name="param">Name of parameter.</param>
		/// <returns>Argument value if the parameter is found, otherwise null.</returns>
		public string this[string param] {
			get {
				return parameters[param];
			}
		}
	}
}