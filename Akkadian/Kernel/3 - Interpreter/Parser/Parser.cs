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

				// Compensate for one-item functions, such as F[x] = x
//				if (parsedFcn.objType != Typ.Expr) parsedFcn = n(Typ.Expr,parsedFcn);
//				if (parsedFcn.LastIndexOf(":") == parsedFcn.IndexOf(":")) parsedFcn = "Expr:{" + parsedFcn + "}";

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

				// Compensate for one-item functions, such as F[x] = x
//				if (parsedFcn.objType != Typ.Expr) parsedFcn = n(Typ.Expr,parsedFcn);
//				if (parsedFcn.LastIndexOf(":") == parsedFcn.IndexOf(":")) parsedFcn = "Expr:{" + parsedFcn + "}";

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
//			s = s.Replace("[","[(").Replace("]",")]");

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
				string newMainStr = s.Replace(fp, delimiter + index + delimiter);

				// Parse the subexpression and add it to the subexpression list
				Node subExpr = ParseFcn(Util.RemoveParens(fp), subExprs, argNames);
				subExprs.Add(subExpr);

				// Parse the main string
				return ParseFcn(newMainStr, subExprs, argNames);
			}

			// Set and time-series literals
//			if (s.StartsWith("{") && s.EndsWith("}"))
//			{
//				// Sets
//				if (IsExactMatch(s,setLiteral))
//				{
//					// TODO: Handle nested Tsets
//					s = Util.RemoveParens(s).Replace(",","+");
//					if (s == "") s = "*";	// Empty sets represented as *
//					return "Tvar:" + s;
//				}
//				// Time serieses
//				// {Dawn: Stub, 2009-07-24: $7.25}
//				// Series:{Tvar:1800-01-01,Tvar:Stub,Tvar:2009-07-24,Tnum:7.25}
//				return "Series:{" + Util.RemoveParens(s).Replace(":",",") + "}";
//			}

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

//				return n(Typ.Expr,new Expr(fCallNodes));		// toggle out to revert; add in recursion tests

				Node newStr = n(Typ.Expr,new Expr(fCallNodes));

				// Deal with any delimited substitutions
				List<Node> newSubExprs;
				if (s.Contains(delimiter))
				{
					newSubExprs = subExprs;
					newSubExprs.Add(newStr);

//					subExprs.Add(newStr);
				}
				else
				{
					newSubExprs = new List<Node>(){newStr};
				}

				return ParseFcn(newStrToParse, newSubExprs, argNames);
//				return ParseFcn(newStrToParse, subExprs, argNames);
			}

//			// Function calls (innermost functions first)
//			Regex rx1 = new Regex(@"(" + fcnNameRegex + @")\[[a-zA-Z0-9,\(\)\+\-\*/>\.'{} " + delimiter + @"]*\]");
//			var m1 = rx1.Match(s);
//			if (m1.Success)
//			{
//				// Step 1: Deal with nested functions
//				// Replace nested function with delimiter
//				string index = Convert.ToString(subExprs.Count);
//				string newStrToParse = s.Replace(m1.Value,delimiter + index + delimiter);
//
//				// Evaluate nested function and save it to the subExpressions list
//
//
//				// Step 2: Process the main string
//				// To store the result
//				List<Node> fCallNodes = new List<Node>();
//
//				// Function name
//				string fcnRef = m1.Groups[1].Value;
//				fCallNodes.Add(ParseFcn(fcnRef,subExprs,argNames));
//
//				// Bracketed portion - the function's arguments
//				string whatsInTheBrackets = Util.RemoveParens(m1.Value.Replace(fcnRef,""));
//				string[] args = whatsInTheBrackets.Split(',');
//				foreach (string arg in args)
//				{
//					fCallNodes.Add( ParseFcn(arg, subExprs, argNames) );
//				}
//
//				return n(Typ.Expr, new Expr(fCallNodes));
//			}

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
//		protected static Node StringToNode(string s, List<Node> subExprs)
//		{
//			// Determine the node type and value
//			int colon = s.IndexOf(":");
//			string typ = s.Substring(0,colon);
//			string val = s.Substring(colon + 1);
//
//			// Return the node object...
//			if (typ == "Op") 		return n(Typ.Op, (Op)Enum.Parse(typeof(Op),val));
//			if (typ == "Tvar") 		return n(Typ.Tvar, ConvertToBestType(val));
//			if (typ == "Var") 		return n(Typ.Var, Convert.ToInt16(val));
//			if (typ == "Fcn")		return n(Typ.Fcn, val);
//
//			if (typ == "Expr")
//			{
//				// Trim outer brackets
//				val = val.Substring(1,val.Length-2);
//
//				// Identify nested expressions (in brackets)...
//				Match m = Regex.Match(val, "Expr:{" + @"[-0-9a-zA-Z:,'\."+delimiter+"]+" + "}");
//				if (m.Success)
//				{
//					// Replace the nested text with #n#
//					string meat = m.Groups[0].Value;
//					string index = Convert.ToString(subExprs.Count);
//					string newMainStr = s.Replace(meat, delimiter + index + delimiter);
//
//					// Parse the subexpression and add it to the subexpression list
//					Node subExpr = StringToNode(meat, subExprs);
//					subExprs.Add(subExpr);
//
//					// Parse the main string
//					return StringToNode(newMainStr, subExprs);
//				}
//
//				// Process each part of the expression
//				string[] parts = val.Split(',');
//				List<Node> nodes = new List<Node>();
//				foreach (string p in parts)
//				{
//					// See if part is a reference to a parenthetical
//					if (p.Contains(delimiter))
//					{
//						// Get index number within delimiter
//						Match m2 = Regex.Match(p, delimiter + "([0-9]+)" + delimiter);
//						int indx = Convert.ToInt16(m2.Groups[1].Value.Trim());
//
//						// Then, add that subexpression
//						nodes.Add(subExprs[indx]);
//					}
//					else
//					{
//						nodes.Add(StringToNode(p,subExprs));
//					}
//				}
//
//				return new Node(Typ.Expr,new Expr(nodes));
//			}
////			if (typ == "Series")
////			{
////				// Series:{Dawn,Stub,2009-07-24,$7.25}
////				Console.WriteLine(s);
////				string[] intervals = Util.RemoveParens(val).Split(',');
////				Tvar ts = new Tvar();
////				for (int j=0; j<intervals.Length-1; j=j+2)
////				{
////					Console.WriteLine(intervals[j] + " - " + intervals[j+1]);
////					string dt = intervals[j];
////					if (dt.Trim() == "Dawn") dt = "1800-01-01";
////					DateTime date = DateTime.Parse(dt);
////					string intVal = Convert.ToString(intervals[j+1]);
////					ts.AddState(date,intVal);
////				}
////				return nTvar(ts);
////			}
//
//			// Should not get here
//			return n(Typ.Null,null);
//		}
	}
}