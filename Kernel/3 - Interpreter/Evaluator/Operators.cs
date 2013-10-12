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

namespace Akkadian
{
	/// <summary>
	/// Types of nodes used in interpreter expressions (Expr)
	/// </summary>
	public enum Typ
	{
		//  Akkadian types
		Tbool,
		Tdate,
		Thing,
		Tline,
		Tnum,
		Tset,
		Tstr,

		// Types used by the parser
		Expr,
		Fcn,
		Op,
		Rec,
		Var
	}

	public partial class Interpreter
	{


		public enum Op
		{
			// Short-circuting operators
			And = 1,
			Or = 2,
			Mult = 3,

			// Binary operators
			Plus = 10,
			Minus = 11,
			Div = 12,
			Eq, 
			Neq,
			Gr, 
			GrEq,
			LsTh, 
			LsEq,
			Nlog, 
			Pow,
			RndUp, 
			RndDn, 
			RndNrUp, 
			RndNrDn,
			Concat,
			Subset, 
			Contains, 
			Union, 
			Intersect, 
			RelComp,
			AddDays, 
			AddMos, 
			AddYrs, 
			DayDiff, 
			WeekDiff,
			YearDiff,

			// Unary operators
			Not,
			USD,
			Abs,
			Sqrt,
			Log,
			Sin,
			Cos,
			Tan,
			Asin,
			Acos,
			Atan,
			Count,
			Empty,
			Rev,
			ToThing,
			Day,
			Month,
			Quarter,
			Year
		}

		private static string[] shortCircuits = {"T&","T|","T*"}; 

		private static string[] binaryOps = {"T+","T-","T/",
			"T=","T<>","T>","T>=","T<","T<=",
			"Pow","Log2","RndUp","RndDn","RndNrUp","RndNrDn","Concat",
			"Subset","Contains","Union","Intersect","RelComp",
			"AddDays","AddMos","AddYrs","DayDiff","WeekDiff","YearDiff"}; 

		private static string[] unaryOps = {"T!","USD","Abs","Sqrt","Log","Sin","Cos","Tan","Asin","Acos","Atan",
			"Count","Empty","Rev","ToThing",
			"TdateDay","TdateMonth","TdateQtr","TdateYear"}; 


		/// <summary>
		/// Evaluates expressions in which a short-circuit may be required.
		/// </summary>
		private static Node EvalShortCircuitFcns(Expr exp, Expr args, string op)
		{
			Node n1 = eval(expr(exp.nodes [1]), args);

			if (op == "T&") 	// And
			{
				// See if first argument is eternally false
				if (((Tbool)n1.obj).IsFalse) return n("Tbool",new Tbool(false));

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return n("Tbool", (Tbool)n1.obj && (Tbool)n2.obj); 
			}

			if (op == "T|") 	// Or
			{
				// See if first argument is eternally true
				if (((Tbool)n1.obj).IsTrue) return n("Tbool",new Tbool(true));

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return n("Tbool", (Tbool)n1.obj || (Tbool)n2.obj); 
			}

			if (op == "T*") 	// Multiplication
			{
				// See if first argument is zero
				Tnum tn1 = ((Tnum)n1.obj);
//				if (tn1.IsEternal && (int)tn1.FirstValue.Val == 0) 
//				{
//					return n ("Tnum", new Tnum (0));
//				}

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return n("Tnum", (Tnum)n1.obj * (Tnum)n2.obj); 
			}

			return n(null,null);
		}

		/// <summary>
		/// Evaluates expressions with two arguments.
		/// </summary>
		private static Node BinaryFcnEval(Expr exp, Expr args, string op)
		{
			Node n1 = eval(expr(exp.nodes [1]), args);
			Node n2 = eval(expr(exp.nodes [2]), args);

			if (op == "T!") { return n("Tbool", !(Tbool)n1.obj); }
			if (op == "T+") { return n("Tnum", (Tnum)n1.obj + (Tnum)n2.obj); }
			if (op == "T-") { return n("Tnum", (Tnum)n1.obj - (Tnum)n2.obj); }
			if (op == "T/") { return n("Tnum", (Tnum)n1.obj / (Tnum)n2.obj); }

			if (op == "T=") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj == (Tnum)n2.obj); 
				if (n1.objType == "Tstr")  return n("Tbool", (Tstr)n1.obj == (Tstr)n2.obj);
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj == (Tdate)n2.obj);
				if (n1.objType == "Tset")  return n("Tbool", (Tset)n1.obj == (Tset)n2.obj);
				if (n1.objType == "Tbool") return n("Tbool", (Tbool)n1.obj == (Tbool)n2.obj);
			}

			if (op == "T<>") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj != (Tnum)n2.obj); 
				if (n1.objType == "Tstr")  return n("Tbool", (Tstr)n1.obj != (Tstr)n2.obj);
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj != (Tdate)n2.obj);
				if (n1.objType == "Tset")  return n("Tbool", (Tset)n1.obj != (Tset)n2.obj);
				if (n1.objType == "Tbool") return n("Tbool", (Tbool)n1.obj != (Tbool)n2.obj);
			}

			if (op == "T>") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj > (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj > (Tdate)n2.obj);
			}
			if (op == "T>=") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj >= (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj >= (Tdate)n2.obj);
			}
			if (op == "T<") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj < (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj < (Tdate)n2.obj);
			}
			if (op == "T<=") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj <= (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj <= (Tdate)n2.obj);
			}

			// Set operators
			if (op == "Subset") { return n("Tbool", ((Tset)n1.obj).IsSubsetOf((Tset)n2.obj)); }
			if (op == "Contains") { return n("Tbool", ((Tset)n1.obj).Contains((Thing)n2.obj)); }
			if (op == "Union") { return n("Tbool", (Tset)n1.obj | (Tset)n2.obj); }
			if (op == "Intersect") { return n("Tbool", (Tset)n1.obj & (Tset)n2.obj); }
			if (op == "RelComp") { return n("Tbool", (Tset)n1.obj - (Tset)n2.obj); }

			// Date
			if (op == "AddDays") { return n("Tdate", ((Tdate)n1.obj).AddDays((Tnum)n2.obj)); }
			if (op == "AddMos") { return n("Tdate", ((Tdate)n1.obj).AddMonths((Tnum)n2.obj)); }
			if (op == "AddYrs") { return n("Tdate", ((Tdate)n1.obj).AddYears((Tnum)n2.obj)); }
			if (op == "DayDiff") { return n("Tnum", H.DayDiff((Tdate)n1.obj, (Tdate)n2.obj)); }
			if (op == "WeekDiff") { return n("Tnum", H.WeekDiff((Tdate)n1.obj, (Tdate)n2.obj)); }
			if (op == "YearDiff") { return n("Tnum", H.YearDiff((Tdate)n1.obj, (Tdate)n2.obj)); }

			// Math and rounding
			if (op == "RndUp") { return n("Tnum", ((Tnum)n1.obj).RoundUp((Tnum)n2.obj)); }
			if (op == "RndDn") { return n("Tnum", ((Tnum)n1.obj).RoundDown((Tnum)n2.obj)); }
			if (op == "RndNrUp") { return n("Tnum", ((Tnum)n1.obj).RoundToNearest((Tnum)n2.obj)); }
			if (op == "RndNrDn") { return n("Tnum", ((Tnum)n1.obj).RoundToNearest((Tnum)n2.obj, true)); }
			if (op == "Concat") { return n("Tstr", (Tstr)n1.obj + (Tstr)n2.obj); }
			if (op == "Mod") { return n("Tnum", (Tnum)n1.obj % (Tnum)n2.obj); }
			if (op == "Pow") { return n("Tnum", Tnum.Pow((Tnum)n1.obj, (Tnum)n2.obj)); }
			if (op == "Log2") { return n("Tnum", Tnum.Log((Tnum)n1.obj, (Tnum)n2.obj)); }

			return n(null,null);
		}

		/// <summary>
		/// Evaluates expressions with one argument.
		/// </summary>
		private static Node UnaryFcnEval(Expr exp, Expr args, string op)
		{
			object ob1 = eval(expr(exp.nodes [1]), args).obj;

			if (op == "T!")    { return n("Tbool", !(Tbool)ob1); }
			if (op == "USD")   { return n("Tstr",  ((Tnum)ob1).ToUSD); }

			if (op == "Count")   { return n("Tnum",  ((Tset)ob1).Count); }
			if (op == "Empty")   { return n("Tbool",  ((Tset)ob1).IsEmpty); }
			if (op == "Rev")   { return n("Tstr",  ((Tset)ob1).Reverse); }
			if (op == "ToThing")   { return n("Thing",  ((Tset)ob1).ToThing); }

			if (op == "TdateDay")   { return n("Tnum", ((Tdate)ob1).Day); }
			if (op == "TdateMonth")   { return n("Tnum", ((Tdate)ob1).Month); }
			if (op == "TdateQtr")   { return n("Tnum", ((Tdate)ob1).Quarter); }
			if (op == "TdateYear")   { return n("Tnum", ((Tdate)ob1).Year); }

			if (op == "Abs")   { return n("Tnum", Tnum.Abs((Tnum)ob1)); }
			if (op == "Sqrt")  { return n("Tnum", Tnum.Sqrt((Tnum)ob1)); }
			if (op == "Log")   { return n("Tnum", Tnum.Log((Tnum)ob1)); }
			if (op == "Sin")   { return n("Tnum", Tnum.Sin((Tnum)ob1)); }
			if (op == "Cos")   { return n("Tnum", Tnum.Cos((Tnum)ob1)); }
			if (op == "Tan")   { return n("Tnum", Tnum.Tan((Tnum)ob1)); }
			if (op == "Asin")  { return n("Tnum", Tnum.ArcSin((Tnum)ob1)); }
			if (op == "Acos")  { return n("Tnum", Tnum.ArcCos((Tnum)ob1)); }
			if (op == "Atan")  { return n("Tnum", Tnum.ArcTan((Tnum)ob1)); }

			return n(null,null);
		}

		/// <summary>
		/// Evaluates expressions with three or more arguments.
		/// </summary>
		private static Node MultiTnumFcnEval(Expr exp, Expr args, string op)
		{
			Tnum[] list = new Tnum[exp.nodes.Count-1];
			for (int i=1; i<exp.nodes.Count; i++)
			{
				list[i-1] = (Tnum)eval(expr(exp.nodes[i]), args).obj;
			}

			if (op == "Tmax") { return n("Tbool", Tnum.Max(list)); }
			if (op == "Tmin") { return n("Tbool", Tnum.Min(list)); }

			return n(null,null);
		}
	}
}
