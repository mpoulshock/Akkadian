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
		// Used to delineate parethetical expressions
		private const string delimiter = "#";

		/// <summary>
		/// Object returned when an input string is parsed.
		/// </summary> 
		public class ParserResponse
		{
			public string ParserString = "";
			public Node ThatWhichHasBeenParsed;
			public bool IsNewFunction = false;
			public string FunctionName;

			public ParserResponse(Node parseStr, bool isFcn, string fcnName)
			{
				ThatWhichHasBeenParsed = parseStr;
				IsNewFunction = isFcn;
				FunctionName = fcnName;
			}

			public ParserResponse(Node parseStr)
			{
				ThatWhichHasBeenParsed = parseStr;
			}
		}

		/// <summary>
		/// Parses a user-input string.
		/// </summary>
		public static ParserResponse ParseInputLine(string s)
		{
			s = s.Trim();

			// This could be much cleaner with the proper regex...

			// Matches variable declarations (e.g. Pi = 3.14159)
			if (IsExactMatch(s, fcnNameRegex + white + "=" + white + parens(wildcard)))
			{
				// Identify the function name
				int eq = s.IndexOf("=");
				string fcnSig = s.Substring(0,eq).Trim();

				// Parse the expression to the right of the = sign
				string fcnText = s.Substring(eq + 1).Trim();
				Node parsedFcn = ParseFcn(fcnText, new List<Node>(), new string[]{}); 

				if (!OperatorRegistry.ContainsKey(fcnSig))
				{
					return new ParserResponse(parsedFcn, true, fcnSig);
				}
				else 
				{
					// Error: Function name is reserved
				}
			}

			// Matches function declarations (e.g. F[x] = x + 1)
			if (IsExactMatch(s, fcnSignature + white + "=" + white + parens(wildcard)))
			{
				// Identify the function name
				int eq = s.IndexOf("=");
				string fcnSig = s.Substring(0,eq).Trim();
				int firstBrack = fcnSig.IndexOf("[");
				string fName = fcnSig.Substring(0,firstBrack);

				// Identify variable names in the function signature
				string args = fcnSig.Substring(firstBrack + 1).Trim(']');
				string[] argArray = args.Split(',');

				// Parse the expression to the right of the = sign
				string fcnText = s.Substring(eq + 1).Trim();
				Node parsedFcn = ParseFcn(fcnText, new List<Node>(), argArray); 

				if (!OperatorRegistry.ContainsKey(fName))
				{
					return new ParserResponse(parsedFcn, true, fName);
				}
				else 
				{
					// Error: Function name is reserved
				}
			}

			// Special maintenance risk: Put set literals in parentheses  :)
			s = s.Replace("{","({").Replace("}","})");

			return new ParserResponse(ParseFcn(s));
		}

		/// <summary>
		/// Parses a function into a string of nested tokens (representing Exprs).
		/// </summary>
		public static Node ParseFcn(string s, List<Node> subExprs, string[] argNames)
		{
			s = s.Trim();

			// Parentheses
			string fp = Util.FirstParenthetical(s,"(",")");
			if (fp != "")
			{
				// Replace the nested text with #n#
				string index = Convert.ToString(subExprs.Count);
				string newStrToParse = s.Replace(fp, delimiter + index + delimiter);
				Node newStr = ParseFcn(Util.RemoveParens(fp), subExprs, argNames);
				return AddToSubExprListAndParse(s, newStrToParse, newStr, subExprs, argNames);
			}

			// Set and time-series literals
			if (s.StartsWith("{") && s.EndsWith("}"))
			{
				string[] members = Util.RemoveParens(s).Split(',');

				// Sets
				if (IsExactMatch(s,setLiteral))
				{
					// TODO: Handle nested Tsets
					List<object> mems = new List<object>();
					foreach (string mem in members) mems.Add(mem);
					return nTvar(Tvar.MakeTset(mems));
				}

				// Time series literal, such as {Dawn: Stub, 2009-07-24: $7.25}
				Tvar tv = new Tvar();
				foreach (string mem in members)
				{
					// 2009-07-24: $7.25
					int colon = mem.IndexOf(":");
					string datePart = mem.Substring(0,colon);
					if (datePart == "Dawn") datePart = "1800-01-01";

					string valPart = mem.Substring(colon+1);
					valPart = valPart.Trim().Replace("$","");

					tv.AddState(DateTime.Parse(datePart), Convert.ToDecimal(valPart));
				}
				return nTvar(tv);
			}

			// Pipelined functions |>
			Regex pipeRegex = new Regex(parens(wildcard) + white + @"\|>" + white + parens(@"EverPer\[" + wildcard + @"\]"));
			var pipeMatch = pipeRegex.Match(s);
			if (pipeMatch.Success)
			{
				Node lhs = ParseFcn(pipeMatch.Groups[1].Value, subExprs, argNames);
				Node rhs = ParseFcn(pipeMatch.Groups[2].Value, subExprs, argNames);

				List<Node> pipeNodes = new List<Node>();
				pipeNodes.Add(n(Typ.Op,Op.Pipe));
				pipeNodes.Add(lhs);
				pipeNodes.Add(rhs);
				Expr pipeExpr = new Expr(pipeNodes);
				return n(Typ.Expr,pipeExpr);
			}

			// Function calls (innermost functions first)
			Regex rx1 = new Regex(@"(" + fcnNameRegex + @")\[[a-zA-Z0-9,\(\)\+\-\*/>\.'{} " + delimiter + @"]*\]");
			var m1 = rx1.Match(s);
			if (m1.Success)
			{
				// Entire function expression
				string firstFcn = m1.Value;

				// Bracketed portion
				string index = Convert.ToString(subExprs.Count);
				string newStrToParse = s.Replace(firstFcn,delimiter + index + delimiter);
				string whatsInTheBrackets = Util.RemoveParens(m1.Value.Replace(m1.Groups[1].Value,""));

				// Function name
				string fcnRef = m1.Groups[1].Value;

				// If function name is in operator registry, reference it;
				// otherwise, assume this is a user-defined function (or a leaf node)
				List<Node> fCallNodes = new List<Node>();
				fCallNodes.Add(ParseFcn(fcnRef,subExprs,argNames));

				// Add the function's arguments
				string[] args = whatsInTheBrackets.Split(',');
				foreach (string arg in args)
				{
					fCallNodes.Add( ParseFcn(arg, subExprs, argNames) );
				}

				Node newStr = n(Typ.Expr,new Expr(fCallNodes));

				return AddToSubExprListAndParse(s, newStrToParse, newStr ,subExprs, argNames);
			}

			// Switch
			Regex switchRegex = new Regex(switchStatement);
			var switchMatch = switchRegex.Match(s);
			if (switchMatch.Success)
			{
				string switchParts = switchMatch.Value.Replace("->",",");
				string[] switchArgs = switchParts.Split(',');

				List<Node> switchNodes = new List<Node>();
				switchNodes.Add(n(Typ.Op,Op.Switch));

				foreach (string arg in switchArgs)
				{
					switchNodes.Add( ParseFcn(arg, subExprs, argNames) );
				}

				Expr switcExpr = new Expr(switchNodes);
				return n(Typ.Expr,switcExpr);
			}

			// Date literals - must be parsed before subtraction
			if (IsExactMatch(s,dateLiteral))
			{
				DateTime d = Convert.ToDateTime(s);
				return nTvar(new Tvar(d));
			}

			// Numeric literals - must be before subtraction due to negative numbers
			if (IsExactMatch(s,decimalLiteral) || IsExactMatch(s,currencyLiteral))
			{
				decimal d = Convert.ToDecimal(s.Replace("$",""));
				return nTvar(new Tvar(d));
			}

			// Infix operators
			foreach (string oprtr in infixOps)
			{
				string pattern = parens(wildcard) + oprtr + parens(wildcard);

				if (IsExactMatch(s, pattern))
				{
					Match m = Regex.Match(s, pattern);
					Node lhs = ParseFcn(m.Groups[1].Value.Trim(), subExprs, argNames);
					Node rhs = ParseFcn(m.Groups[2].Value.Trim(), subExprs, argNames);

					List<Node> infixNodes = new List<Node>();
					Op theOp = OperatorRegistry[oprtr]; 
					infixNodes.Add(n(Typ.Op,theOp));
					infixNodes.Add(lhs);
					infixNodes.Add(rhs);
					Expr infixExpr = new Expr(infixNodes);
					return n(Typ.Expr,infixExpr);
				}
			}

			// Not - must go after infix ops
			if (s.StartsWith("!"))
			{
				List<Node> notNodes = new List<Node>();
				notNodes.Add(n(Typ.Op,Op.Not));
				notNodes.Add(ParseFcn(s.Substring(1), subExprs, argNames));
				Expr notExpr = new Expr(notNodes);
				return n(Typ.Expr,notExpr);
			}

			// Variable references
			if (argNames != null)
			{
				for (int i=0; i<argNames.Length; i++)
				{
					if (s == argNames[i]) return n(Typ.Var,i);
				}
			}

			// Boolean and string literal values
			if (IsExactMatch(s.ToLower(),boolLiteral))
			{
				return nTvar(new Tvar(Convert.ToBoolean(s)));
			}
			if (IsExactMatch(s,stringLiteral))
			{
				return nTvar(new Tvar(Convert.ToString(s.Trim('\''))));
			}

			// References to operator names (such as "Abs")
			if (OperatorRegistry.ContainsKey(s))
			{
				Op theOp = OperatorRegistry[s];
				return n(Typ.Op,theOp);
			}

			// Constants (i.e. constant functions - those with no arguments)
			if (IsExactMatch(s,fcnNameRegex))
			{
				return n(Typ.Fcn,s);
			}

			// See if part is a reference to a parenthetical
			if (s.StartsWith(delimiter) && s.Length <= 4)
			{
				// Get index number within delimiter
				Match m2 = Regex.Match(s, delimiter + "([0-9]+)" + delimiter);
				int indx = Convert.ToInt16(m2.Groups[1].Value.Trim());

				// Then, add that subexpression
				return subExprs[indx];
			}

			// Should never get here
			return n(Typ.Null,-42);
		}

		public static Node ParseFcn(string s)
		{
			return ParseFcn(s, new List<Node>(), null);
		}

		/// <summary>
		/// Adds a string to the sub-expression list and parses the main string
		/// </summary>
		private static Node AddToSubExprListAndParse(string s, string newStrToParse, Node newStr, List<Node> subExprs, string[] argNames)
		{
			List<Node> newSubExprs;
			if (s.Contains(delimiter))
			{
				newSubExprs = subExprs;
				newSubExprs.Add(newStr);
			}
			else
			{
				newSubExprs = new List<Node>(){newStr};
			}

			return ParseFcn(newStrToParse, newSubExprs, argNames);
		}
	}
}