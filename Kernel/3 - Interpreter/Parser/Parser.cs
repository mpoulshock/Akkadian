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
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace Akkadian
{
	public partial class Interpreter
	{
		public static string Parse2(string s, List<string> subExprs)
		{
			// Handle parentheses
			string fp = FirstParenthetical(s,"(",")");
			if (fp != "")
			{
				string index = Convert.ToString(subExprs.Count);
				string newStrToParse = s.Replace(fp,"#" + index + "#");

				List<string> newSubExprs;
				if (s.Contains("#"))
				{
					newSubExprs = subExprs;
					newSubExprs.Add(Parse2(RemoveParens(fp),subExprs));
				}
				else
				{
					newSubExprs = new List<string>(){Parse2(RemoveParens(fp),subExprs)};
				}

				return Parse2(newStrToParse, newSubExprs);
			}

			// Operators
			if (IsExactMatch(s, parens(wildcard) + @"\+" + parens(wildcard)))
			{
				Match m = Regex.Match(s, parens(wildcard) + @"\+" + parens(wildcard));
				string lhs = Parse2(m.Groups[1].Value.Trim(), subExprs);
				string rhs = Parse2(m.Groups[2].Value.Trim(), subExprs);
				return "{Typ.Op:Op.Plus," + lhs + "," + rhs + "}";
			}

			if (IsExactMatch(s, parens(wildcard) + @"\*" + parens(wildcard)))
			{
				Match m = Regex.Match(s, parens(wildcard) + @"\*" + parens(wildcard));
				string lhs = Parse2(m.Groups[1].Value.Trim(), subExprs);
				string rhs = Parse2(m.Groups[2].Value.Trim(), subExprs);
				return "{Typ.Op:Op.Mult," + lhs + "," + rhs + "}";
			}

			// Literal values
			if (IsExactMatch(s,boolLiteral))
			{
				return "Tbool:" + Convert.ToBoolean(s);
			}
			if (IsExactMatch(s,decimalLiteral))
			{
				return "Tnum:" + Convert.ToDecimal(s);
			}

			// Has to come towards end
//			if (IsExactMatch(s,fcnVariable))
//			{
//				// TODO: Handle multiple variables...
//				return n(Typ.Var,0);
//			}

			return DecompressParse(s, subExprs);
		}

		public static string Parse2(string s)
		{
			return Parse2(s, new List<string>());
		}

		private static string RemoveParens(string s)
		{
			return s.Substring(1,s.Length-2);
		}

		public static string DecompressParse(string s, List<string> subExprs)
		{

			string result = s;
			for (int i=0; i<subExprs.Count; i++)
			{
				result = result.Replace("#" + Convert.ToString(i) + "#", subExprs[i]);
			}

			if (result == s) return result;
			else return DecompressParse(result, subExprs);
		}

		/// <summary>
		/// Parses a string into an expression (Expr).
		/// </summary>
		public static object Parse(string s)
		{
			// Operators
			if (IsExactMatch(s, parens(wildcard) + @"\+" + parens(wildcard)))
			{
				Match m = Regex.Match(s, parens(wildcard) + @"\+" + parens(wildcard));
				Node lhs = (Node)Parse(m.Groups[1].Value.Trim());
				Node rhs = (Node)Parse(m.Groups[2].Value.Trim());
				return expr(n(Typ.Op,Op.Plus), lhs, rhs);
			}

			// Literal values
			if (IsExactMatch(s,boolLiteral))
			{
				return nTbool(Convert.ToBoolean(s));
			}
			if (IsExactMatch(s,decimalLiteral))
			{
				return nTnum(Convert.ToDecimal(s));
			}

			// Has to come towards end
			if (IsExactMatch(s,fcnVariable))
			{
				// TODO: Handle multiple variables...
				return n(Typ.Var,0);
			}

			return new Expr(null);
		}

//		public static Expr ParseInfixOperator(string s, string pattern, string op)
//		{
//			if (IsExactMatch(s, parens(wildcard) + @"\+" + parens(wildcard)))
//			{
//				Match m = Regex.Match(s, parens(wildcard) + @"\+" + parens(wildcard));
//				Node lhs = (Node)Parse(m.Groups[1].Value.Trim());
//				Node rhs = (Node)Parse(m.Groups[2].Value.Trim());
//				return expr(n(Typ.Op,Op.Plus), lhs, rhs);
//			}
//		}

		/// <summary>
		/// Determines if the input string matches the regex exactly.
		/// </summary>
		protected static bool IsExactMatch(string s, string regex)
		{
			return s == Regex.Match(s,regex).Groups[0].Value;
		}

		protected static string parens(string s)
		{
			return "(" + s + ")";
		}

		/// <summary>
		/// Returns the first first-level set of {}s or ()s in the input string.
		/// </summary>
		public static string FirstParenthetical(string clause, string open, string close)
		{
			string substr = "";
			int bracketLevel = 0;
			int start = clause.IndexOf(open);

			if (start == -1) return "";

			// Iterate through clause from first "{" to find its spouse, "}"
			for (int counter = start; counter < clause.Length; counter++)
			{
				if (clause[counter] == Convert.ToChar(open))
					bracketLevel++;
				else if (clause[counter] == Convert.ToChar(close))
					bracketLevel--;
				if (bracketLevel == 0)
				{
					substr = clause.Substring(start,counter-start+1);
					return substr;
				}
			}
			return substr;
		}

		/// <summary>
		/// Reads the source file.
		/// </summary>
		protected static string ReadFile()
		{
			string result = "";
			StreamReader stream = new StreamReader("C:\\Users\\mpoulshock\\Documents\\Test.akk");
			string line;
			while ((line = stream.ReadLine()) != null) 
			{
				result += line;
			}

			return result;
		}
	}
}
