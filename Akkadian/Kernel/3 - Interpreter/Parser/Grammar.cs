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
using System.Text.RegularExpressions;

namespace Akkadian
{
	public partial class Interpreter
	{
		// Letters, digits, dates
		protected const string white = "[ ]*";
		protected const string wildcard = ".*";
		protected const string letter = "[a-zA-Z]";
		protected const string letters = "["+letter+"]+";
		protected const string digit = "[0-9]";
		protected const string integer = "[0-9]+";
		protected const string alphadecimals = "["+letter+"|"+digit+"]+";

		// Literal values
		protected const string boolLiteral = "(true|false)";
		protected const string decimalLiteral = "-?" + integer+@"(\."+integer+")?";
		protected const string currencyLiteral = @"\$[0-9\.,]+";
		protected const string stringLiteral = @"'" + wildcard + "'";	// TODO: Change to double quotes
		protected const string dateLiteral = @"(1|2)[0-9]{3}\-(0[1-9]|1[0-2])\-(0[1-9]|[1-2][0-9]|30|31)";		// yyyy-mm-dd
		protected const string setLiteral = "{ }";
		protected const string stubLiteral = "Stub";

		// Time-varying literal values
		// {Dawn: 42; 2011-01-01: 43}

		// Function parts
		protected const string akkType = "(Tvar|Tvar|Tvar|Tvar|Tvar|Thing)";
		protected const string fcnVariable = letter + "[a-zA-Z0-9_]*";
		protected const string fcnName = letter + "[a-zA-Z0-9_]*";
		protected const string bracketInnards = "[a-zA-Z0-9_, ]*";
		protected const string fcnSignature = fcnName + white + @"\["+ white + bracketInnards + white + @"\]";
		
		// Switch
		protected const string switchStatement = wildcard + "->" + wildcard + "," + wildcard;

		// Binary operators
		protected const string binaryOp = @"(&|\|)";

		/// <summary>
		/// Determines if the input string matches the regex exactly.
		/// </summary>
		public static bool IsExactMatch(string s, string regex)
		{
			return s == Regex.Match(s,regex).Groups[0].Value;
		}

		/// <summary>
		/// Puts parentheses around a string.
		/// </summary>
		private static string parens(string s)
		{
			return "(" + s + ")";
		}
	}
}

