// Copyright (c) 2013 Hammura.bi LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.IO;

namespace Akkadian
{
	public partial class Interpreter
	{
		public const string lineCommentSymbol = "#";
		public const string blockCommentSymbol = "##";

		/// <summary>
		/// Imports rules from a text file into the session.
		/// </summary>
		public static string ImportRuleFile(Session sess, string loc)
		{
			string result = "";
			int ruleCount = 0;
			bool isCommentBlock = false;

			// Digest the text file, line by line
			StreamReader stream = new StreamReader(loc);
			string line;
			while ((line = stream.ReadLine()) != null) 
			{
				// Handle commented lines, comment blocks, and mid-line comments
				if (!isCommentBlock && IsCommentBlockLine(line)) { isCommentBlock = true; }
				else if (isCommentBlock && IsCommentBlockLine(line)) { isCommentBlock = false; continue; }
				if (isCommentBlock || IsComment(line)) { continue; }
				line = DeComment(line);

				result += line;
			}
			stream.Close();

			// Rules are assumed to end with semicolons
			string[] rules = result.Split(';');

			// Add the rules to the session
			foreach (string r in rules)
			{
				string rule = r.Trim();
				if (rule.Length > 0)
				{
					sess.ProcessInput(rule);
					ruleCount++;
				}
			}

			return Convert.ToString(ruleCount) + " rules imported.";
		}

		/// <summary>
		/// Determines whether a line is a comment line.
		/// </summary>
		private static bool IsComment(string line)
		{
			return line.TrimStart().StartsWith(lineCommentSymbol);
		}

		/// <summary>
		/// Determines whether a line is a line opening or closing a comment block.
		/// </summary>
		private static bool IsCommentBlockLine(string line)
		{
			return line.TrimStart().StartsWith(blockCommentSymbol);
		}

		/// <summary>
		/// Remove the comments from a line.
		/// </summary>
		public static string DeComment(string line)
		{
			if (line.Contains(lineCommentSymbol))
			{
				return line.Substring(0,line.IndexOf(lineCommentSymbol) - 1);
			}
			return line;
		}
	}
}