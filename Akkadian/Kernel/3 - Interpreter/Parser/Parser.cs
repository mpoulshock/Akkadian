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
			public string ParserString;
			public bool IsNewFunction = false;
			public string FunctionName;

			public ParserResponse(string parseStr, bool isFcn, string fcnName)
			{
				ParserString = parseStr;
				IsNewFunction = isFcn;
				FunctionName = fcnName;
			}

			public ParserResponse(string parseStr)
			{
				ParserString = parseStr;
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
			if (IsExactMatch(s, fcnName + white + "=" + white + parens(wildcard)))
			{
				// Identify the function name
				int eq = s.IndexOf("=");
				string fcnSig = s.Substring(0,eq).Trim();

				// Parse the expression to the right of the = sign
				string fcnText = s.Substring(eq + 1).Trim();
				string parsedFcn = ParseFcn(fcnText, new List<string>(), new string[]{}); 

				// Compensate for one-item functions, such as F[x] = x
				if (parsedFcn.LastIndexOf(":") == parsedFcn.IndexOf(":")) parsedFcn = "Expr:{" + parsedFcn + "}";

				return new ParserResponse(parsedFcn, true, fcnSig);
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
				string parsedFcn = ParseFcn(fcnText, new List<string>(), argArray); 

				// Compensate for one-item functions, such as F[x] = x
				if (parsedFcn.LastIndexOf(":") == parsedFcn.IndexOf(":")) parsedFcn = "Expr:{" + parsedFcn + "}";

				return new ParserResponse(parsedFcn, true, fName);
			}

			return new ParserResponse(ParseFcn(s));
		}

		/// <summary>
		/// Parses a function into a string of nested tokens (representing Exprs).
		/// </summary>
		public static string ParseFcn(string s, List<string> subExprs, string[] argNames)
		{
			s = s.Trim();

			// Parentheses
			string fp = Util.FirstParenthetical(s,"(",")");
			if (fp != "")
			{
				string index = Convert.ToString(subExprs.Count);
				string newStrToParse = s.Replace(fp, delimiter + index + delimiter);
				string newStr = ParseFcn(Util.RemoveParens(fp), subExprs, argNames);
				return AddToSubExprListAndParse(s, newStrToParse, newStr,subExprs, fcnName, argNames);
			}

			// Pipelined functions |>
			Regex pipeRegex = new Regex(parens(wildcard) + white + @"\|>" + white + parens(@"EverPer\[" + wildcard + @"\]"));
			var pipeMatch = pipeRegex.Match(s);
			if (pipeMatch.Success)
			{
				string stuffBeforePipe = pipeMatch.Groups[1].Value;
				string stuffAfterPipe = pipeMatch.Groups[2].Value;
				return "Expr:{Op:Pipe," + ParseFcn(stuffBeforePipe,subExprs,argNames) + "," + ParseFcn(stuffAfterPipe,subExprs,argNames) + "}";
			}

			// Function calls (innermost functions first)
			Regex rx1 = new Regex(@"([a-zA-Z_][a-zA-Z0-9_]*)\[[a-zA-Z0-9,\(\)\+\-\*/>\.' " + delimiter + @"]*\]");
			var m1 = rx1.Match(s);
			if (m1.Success)
			{
				string firstFcn = m1.Value;
				string index = Convert.ToString(subExprs.Count);
				string newStrToParse = s.Replace(firstFcn,delimiter + index + delimiter);
				string whatsInTheBrackets = Util.RemoveParens(m1.Value.Replace(m1.Groups[1].Value,""));

				// Functions with multiple parameters
				string[] args = whatsInTheBrackets.Split(',');
				string fcnRef = m1.Groups[1].Value;

				// If function name is in operator registry, reference it;
				// otherwise, assume this is a user-defined function (or a leaf node)
				string newStr = ""; 
				if (OperatorRegistry.ContainsKey(fcnRef))
				{
					newStr = "Expr:{Op:" + Convert.ToString(OperatorRegistry[fcnRef]);
				}
				else
				{
					newStr = "Expr:{Fcn:" + fcnRef;  	// Should this be an int?
				}

				// Add the function's arguments
				foreach (string arg in args)
				{
					newStr += "," + ParseFcn(arg, subExprs, argNames);
				}
				newStr += "}";

				return AddToSubExprListAndParse(s, newStrToParse, newStr ,subExprs, fcnName, argNames);
			}

			// Switch
			Regex switchRegex = new Regex(switchStatement);
			var switchMatch = switchRegex.Match(s);
			if (switchMatch.Success)
			{
				string switchParts = switchMatch.Value.Replace("->",",");
				string[] switchArgs = switchParts.Split(',');

				string switchResult = "Expr:{Op:Switch"; 
				foreach (string arg in switchArgs)
				{
					switchResult += "," + ParseFcn(arg, subExprs, argNames);
				}
				switchResult += "}";

				return switchResult;
			}

			// Date literals - must be parsed before subtraction
			if (IsExactMatch(s,dateLiteral))
			{
				return "Tvar:" + s;
			}

			// Numeric literals - must be before subtraction due to negative numbers
			if (IsExactMatch(s,decimalLiteral) || IsExactMatch(s,currencyLiteral))
			{
				return "Tvar:" + Convert.ToDecimal(s.Replace("$",""));
			}

			// Infix operators
			foreach (string oprtr in infixOps)
			{
				string infixResult = TestForInfixOp(s, oprtr, subExprs, fcnName, argNames);
				if (s != infixResult && infixResult != "") return infixResult;
			}

			// Not - must go after infix ops
			if (s.StartsWith("!"))
			{
				return "Expr:{Op:Not," + ParseFcn(s.Substring(1), subExprs, argNames) + "}";
			}

			// Variable references
			if (argNames != null)
			{
				for (int i=0; i<argNames.Length; i++)
				{
					if (s == argNames[i]) return "Var:" + i;
				}
			}

			// More literal values
			if (IsExactMatch(s.ToLower(),boolLiteral))
			{
				return "Tvar:" + Convert.ToBoolean(s);
			}
			if (IsExactMatch(s,stringLiteral))
			{
				return "Tvar:" + s.Trim('\'');
			}

			// Constants (i.e. constant functions - those with no arguments)
			if (IsExactMatch(s,fcnName))
			{
				return "Fcn:" + s;
			}

			return DecompressParse(s, subExprs);
		}

		public static string ParseFcn(string s)
		{
			return ParseFcn(s, new List<string>(), null);
		}

		/// <summary>
		/// Decomposes expressions defining new functions.
		/// </summary>
		private static string TestForFcnExpression(string s, List<string> subExprs)
		{
			string pattern = fcnSignature + white + "=" + white + parens(wildcard);

			if (IsExactMatch(s, pattern))
			{
				int eq = s.IndexOf("=");

				// Identify the function name
				string fcnSig = s.Substring(0,eq).Trim();
				int firstBrack = fcnSig.IndexOf("[");
				string fcnName = fcnSig.Substring(0,firstBrack);

				// Identify variable names in the function signature
				string args = fcnSig.Substring(firstBrack + 1).Trim(']');
				string[] argArray = args.Split(',');

				// Parse the expression to the right of the = sign
				string fcnText = s.Substring(eq + 1).Trim();
				string parsedFcn = ParseFcn(fcnText, subExprs, argArray);

				return parsedFcn;
			}

			return s;
		}

		/// <summary>
		/// Adds a string to the sub-expression list and parses the main string
		/// </summary>
		private static string AddToSubExprListAndParse(string s, string newStrToParse, string newStr, List<string> subExprs, string fcnName, string[] argNames)
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

			return ParseFcn(newStrToParse, newSubExprs, argNames);
		}
		
		/// <summary>
		/// Puts extracted sub-parses back into the main parse string.
		/// </summary>
		private static string DecompressParse(string s, List<string> subExprs)
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
		private static string TestForInfixOp(string s, string op, List<string> subExprs, string fcnName, string[] argNames)
		{
			string pattern = parens(wildcard) + op + parens(wildcard);

			if (IsExactMatch(s, pattern))
			{
				Match m = Regex.Match(s, pattern);
				string lhs = ParseFcn(m.Groups[1].Value.Trim(), subExprs, argNames);
				string rhs = ParseFcn(m.Groups[2].Value.Trim(), subExprs, argNames);
				return "Expr:{Op:" + Convert.ToString(OperatorRegistry[op]) + "," + lhs + "," + rhs + "}";
			}

			return "";
		}

		/*
		 * The following method converts parse stings into Nodes.
		 * 
		 * To deal with nested expressions, the function first looks for the
		 * innermost {} first, replacing them with a delimiter.  For example:
		 * 
		 * {Op:Mult,{Op:Cos,Tvar:33},{Op:Sin,{Op:Abs,Tvar:9}}}
		 * {Op:Mult,#0#,{Op:Sin,{Op:Abs,Tvar:9}}}
		 * {Op:Mult,#0#,{Op:Sin,#1#}}
		 * {Op:Mult,#0#,#2#}
		 * 
		 * Later, the placeholders are replaced with their Node objects, which
		 * are passed around through the recursive process in the subExprs list.
		 */

		/// <summary>
		/// Converts a parse string into a Node object.
		/// </summary>
		/// <remarks>
		/// This is kludgy. One day, we'll have to change ParseRule to build 
		/// the Expr object directly, rather than generating a string and then
		/// having this function turn it into an Expr.
		/// </remarks>
		protected static Node StringToNode(string s, List<Node> subExprs)
		{
			// Determine the node type and value
			int colon = s.IndexOf(":");
			string typ = s.Substring(0,colon);
			string val = s.Substring(colon + 1);

			// Return the node object...
			if (typ == "Op") 		return n(Typ.Op, (Op)Enum.Parse(typeof(Op),val));
			if (typ == "Tvar") 		return n(Typ.Tvar, ConvertToBestType(val));
			if (typ == "Var") 		return n(Typ.Var, Convert.ToInt16(val));
			if (typ == "Fcn")		return n(Typ.Fcn, val);

			if (typ == "Expr")
			{
				// Trim outer brackets
				val = val.Substring(1,val.Length-2);

				// Identify nested expressions (in brackets)...
				Match m = Regex.Match(val, "Expr:{" + @"[-0-9a-zA-Z:,'\."+delimiter+"]+" + "}");
				if (m.Success)
				{
					// Replace the nested text with #n#
					string meat = m.Groups[0].Value;
					string index = Convert.ToString(subExprs.Count);
					string newMainStr = s.Replace(meat, delimiter + index + delimiter);

					// Parse the subexpression and add it to the subexpression list
					Node subExpr = StringToNode(meat, subExprs);
					subExprs.Add(subExpr);

					// Parse the main string
					return StringToNode(newMainStr, subExprs);
				}

				// Process each part of the expression
				string[] parts = val.Split(',');
				List<Node> nodes = new List<Node>();
				foreach (string p in parts)
				{
					// See if part is a reference to a parenthetical
					if (p.Contains(delimiter))
					{
						// Get index number within delimiter
						Match m2 = Regex.Match(p, delimiter + "([0-9]+)" + delimiter);
						int indx = Convert.ToInt16(m2.Groups[1].Value.Trim());

						// Then, add that subexpression
						nodes.Add(subExprs[indx]);
					}
					else
					{
						nodes.Add(StringToNode(p,subExprs));
					}
				}

				return new Node(Typ.Expr,new Expr(nodes));
			}

			// Should not get here
			return n(Typ.Null,null);
		}

		public static Node StringToNode(string p)
		{
			return StringToNode(p, new List<Node>());
		}

		/// <summary>
		/// Converts a string representing a node into an expression (Expr)
		/// </summary>
		public static Expr StringToExpr(string s)
		{
			Node n = StringToNode(s);
			return new Expr(new List<Node>(){n});
		}

		/// <summary>
		/// Converts the string to the most appropriate type of Tvar object.
		/// </summary>
		private static Tvar ConvertToBestType(string s)
		{
			if (IsExactMatch(s,decimalLiteral)) 		return new Tvar(Convert.ToDecimal(s));
			if (IsExactMatch(s,dateLiteral)) 			return new Tvar(Convert.ToDateTime(s));
			if (IsExactMatch(s.ToLower(),boolLiteral))	return new Tvar(Convert.ToBoolean(s));
			return new Tvar(s);
		}
	}
}