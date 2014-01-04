using System;
using System.Collections.Generic;
using System.Linq;

namespace HardySoft.CC {
	public class Parser {
		/// <summary>
		/// To find out all tag strings from a give text.
		/// </summary>
		/// <param name="textToParse"></param>
		/// <param name="tagPrefix"></param>
		/// <param name="tagSuffix"></param>
		/// <returns></returns>
		public static string[] TagParser(string textToParse, string tagPrefix, string tagSuffix) {
			if (string.IsNullOrEmpty(tagPrefix) || string.IsNullOrEmpty(tagSuffix)) {
				throw new ArgumentException("Tag prefix or suffix cannot be empty.");
			}

			tagPrefix = tagPrefix.Trim();
			tagSuffix = tagSuffix.Trim();

			if (tagPrefix.Length != tagSuffix.Length) {
				throw new ArgumentException("Tag prefix and suffix must have same length.");
			}

			Stack<char> stack = new Stack<char>();
			List<string> tags = new List<string>();

			bool prefixFound = false, suffixFound = false;
			for (int i = 0; i < textToParse.Length; i++) {

				if (i < textToParse.Length - 1) {
					string tagDelimiter = textToParse.Substring(i, tagPrefix.Length);

					if (string.Compare(tagDelimiter, tagPrefix, false) == 0) {
						// found a opening tag
						prefixFound = true;
						suffixFound = false;
					}

					if (string.Compare(tagDelimiter, tagSuffix, false) == 0) {
						// found a closing tag
						suffixFound = true;
					}
				}

				if (prefixFound && !suffixFound) {
					stack.Push(textToParse[i]);
				}

				if (prefixFound && suffixFound) {
					// prepare to find next tag
					prefixFound = false;
					suffixFound = false;

					string tag = new string(stack.Reverse<char>().ToArray());
					// remove tag prefix
					tag = tag.Substring(tagPrefix.Length).Trim();
					tags.Add(tag);

					stack.Clear();
				}
			}

			return tags.ToArray();
		}
	}
}
