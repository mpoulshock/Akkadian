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
		private const string delimiter = "#";

		/// <summary>
		/// Parses a function into a string of nested tokens (representing Exprs).
		/// </summary>
		public static string Parse(string s, List<string> subExprs)
		{
			s = s.Trim();

			// Parentheses
			string fp = FirstParenthetical(s,"(",")");
			if (fp != "")
			{
				string index = Convert.ToString(subExprs.Count);
				string newStrToParse = s.Replace(fp,delimiter + index + delimiter);
				string newStr = Parse(RemoveParens(fp),subExprs);

				return AddToSubExprListAndParse(s, newStrToParse, newStr,subExprs);
			}

			// Function calls (innermost functions first)
			Regex rx1 = new Regex(@"([a-zA-Z_][a-zA-Z0-9_]*)\[[a-zA-Z0-9,\(\)\+\-\*/ " + delimiter + @"]+\]");
			var m1 = rx1.Match(s);
			if (m1.Success)
			{
				string firstFcn = m1.Value;
				string index = Convert.ToString(subExprs.Count);
				string newStrToParse = s.Replace(firstFcn,delimiter + index + delimiter);
				string whatsInTheBrackets = RemoveParens(m1.Value.Replace(m1.Groups[1].Value,""));

				// Functions with multiple parameters
				string[] args = whatsInTheBrackets.Split(',');
				string newStr = "{Typ.Op:Op." + m1.Groups[1].Value;  // TODO: Identify function
				foreach (string arg in args)
				{
					newStr += "," + Parse(arg,subExprs);
				}
				newStr += "}";

				return AddToSubExprListAndParse(s, newStrToParse, newStr,subExprs);
			}

			// Infix operators - order is important here (boolean, comparison, arithmetic)
			string[] infixOps = {"&",@"\|","==","<>",">=","<=",">","<",@"\+","-",@"\*","/"};
			foreach (string oprtr in infixOps)
			{
				string infixResult = TestForInfixOp(s, oprtr, subExprs);
				if (s != infixResult && infixResult != "") return infixResult;
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

		public static string Parse(string s)
		{
			return Parse(s, new List<string>());
		}

		/// <summary>
		/// Adds a string to the sub-expression list and parses the main string
		/// </summary>
		private static string AddToSubExprListAndParse(string s, string newStrToParse, string newStr, List<string> subExprs)
		{
			List<string> newSubExprs;
			if (s.Contains(delimiter))
			{
				newSubExprs = subExprs;
				newSubExprs.Add(newStr);
			}
			else
			{
				newSubExprs = new List<string>(){newStr};
			}

			return Parse(newStrToParse, newSubExprs);
		}

		/// <summary>
		/// Removes parentheses from a string.
		/// </summary>
		private static string RemoveParens(string s)
		{
			return s.Substring(1,s.Length-2);
		}

		/// <summary>
		/// Puts extracted sub-parses back into the main parse string.
		/// </summary>
		public static string DecompressParse(string s, List<string> subExprs)
		{

			string result = s;
			for (int i=0; i<subExprs.Count; i++)
			{
				result = result.Replace(delimiter + Convert.ToString(i) + delimiter, subExprs[i]);
			}

			if (result == s) return result;
			else return DecompressParse(result, subExprs);
		}

		/// <summary>
		/// Decomposes expressions with infix operators.
		/// </summary>
		private static string TestForInfixOp(string s, string op, List<string> subExprs)
		{
			string w = parens(wildcard);

			if (IsExactMatch(s, w + op + w))
			{
				Match m = Regex.Match(s, w + op + w);
				string lhs = Parse(m.Groups[1].Value.Trim(), subExprs);
				string rhs = Parse(m.Groups[2].Value.Trim(), subExprs);
				return "{Typ.Op:Op." + StringToOp(op) + "," + lhs + "," + rhs + "}";
			}

			return "";
		}

		/// <summary>
		/// Converts the Akkadian symbol to the Op equivalent (as a string).
		/// </summary>
		private static string StringToOp(string op)
		{
			if (op == "&") return "And";
			if (op == @"\|") return "Or";
			if (op == @"\+") return "Plus";
			if (op == "/") return "Div";
			if (op == "-") return "Minus";
			if (op == "==") return "Eq";
			if (op == "<>") return "Neq";
			if (op == ">=") return "GrEq";
			if (op == "<=") return "LsEq";
			if (op == ">") return "GrTh";
			if (op == "<") return "LsTh";
			if (op == @"\*") return "Mult";
			return "should%not$happen";
		}

		/// <summary>
		/// Determines if the input string matches the regex exactly.
		/// </summary>
		protected static bool IsExactMatch(string s, string regex)
		{
			return s == Regex.Match(s,regex).Groups[0].Value;
		}

		/// <summary>
		/// Puts parentheses around a string.
		/// </summary>
		protected static string parens(string s)
		{
			return "(" + s + ")";
		}

		/// <summary>
		/// Returns the first first-level set of {}s or ()s in the input string.
		/// </summary>
		public static string FirstParenthetical(string clause, string open, string close)
		{
			int start = clause.IndexOf(open);
			if (start == -1) return "";

			string substr = "";
			int bracketLevel = 0;

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
