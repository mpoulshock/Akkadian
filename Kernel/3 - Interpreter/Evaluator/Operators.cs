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
		Var,
		Ask,
		Null
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
				if (((Tbool)n1.obj).IsFalse) return nTbool(false);

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return nTbool((Tbool)n1.obj && (Tbool)n2.obj); 
			}

			if (op == "T|") 	// Or
			{
				// See if first argument is eternally true
				if (((Tbool)n1.obj).IsTrue) return nTbool(true);

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return nTbool((Tbool)n1.obj || (Tbool)n2.obj); 
			}

			if (op == "T*") 	// Multiplication
			{
				// See if first argument is zero
				Tnum tn1 = ((Tnum)n1.obj);
//				if (tn1.IsEternal && (int)tn1.FirstValue.Val == 0) 
//				{
//					return nTnum(0);
//				}

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return nTnum((Tnum)n1.obj * (Tnum)n2.obj); 
			}

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with two arguments.
		/// </summary>
		private static Node BinaryFcnEval(Expr exp, Expr args, string op)
		{
			Node n1 = eval(expr(exp.nodes [1]), args);
			Typ tp = n1.objType;
			object ob1 = n1.obj;
			object ob2 = eval(expr(exp.nodes [2]), args).obj;

			if (op == "T!") { return nTbool(!(Tbool)ob1); }
			if (op == "T+") { return nTnum((Tnum)ob1 + (Tnum)ob2); }
			if (op == "T-") { return nTnum((Tnum)ob1 - (Tnum)ob2); }
			if (op == "T/") { return nTnum((Tnum)ob1 / (Tnum)ob2); }

			if (op == "T=") 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 == (Tnum)ob2); 
				if (tp == Typ.Tstr)  return nTbool((Tstr)ob1 == (Tstr)ob2);
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 == (Tdate)ob2);
				if (tp == Typ.Tset)  return nTbool((Tset)ob1 == (Tset)ob2);
				if (tp == Typ.Tbool) return nTbool((Tbool)ob1 == (Tbool)ob2);
			}

			if (op == "T<>") 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 != (Tnum)ob2); 
				if (tp == Typ.Tstr)  return nTbool((Tstr)ob1 != (Tstr)ob2);
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 != (Tdate)ob2);
				if (tp == Typ.Tset)  return nTbool((Tset)ob1 != (Tset)ob2);
				if (tp == Typ.Tbool) return nTbool((Tbool)ob1 != (Tbool)ob2);
			}

			if (op == "T>") 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 > (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 > (Tdate)ob2);
			}
			if (op == "T>=") 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 >= (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 >= (Tdate)ob2);
			}
			if (op == "T<") 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 < (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 < (Tdate)ob2);
			}
			if (op == "T<=") 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 <= (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 <= (Tdate)ob2);
			}

			// Set operators
			if (op == "Subset") 	{ return nTbool(((Tset)ob1).IsSubsetOf((Tset)ob2)); }
			if (op == "Contains") 	{ return n(Typ.Tbool, ((Tset)ob1).Contains((Thing)ob2)); }
			if (op == "Union") 		{ return nTset((Tset)ob1 | (Tset)ob2); }
			if (op == "Intersect") 	{ return nTset((Tset)ob1 & (Tset)ob2); }
			if (op == "RelComp") 	{ return nTset((Tset)ob1 - (Tset)ob2); }

			// Date
			if (op == "AddDays") 	{ return nTdate(((Tdate)ob1).AddDays((Tnum)ob2)); }
			if (op == "AddMos") 	{ return nTdate(((Tdate)ob1).AddMonths((Tnum)ob2)); }
			if (op == "AddYrs") 	{ return nTdate(((Tdate)ob1).AddYears((Tnum)ob2)); }
			if (op == "DayDiff") 	{ return nTnum(H.DayDiff((Tdate)ob1, (Tdate)ob2)); }
			if (op == "WeekDiff") 	{ return nTnum(H.WeekDiff((Tdate)ob1, (Tdate)ob2)); }
			if (op == "YearDiff") 	{ return nTnum(H.YearDiff((Tdate)ob1, (Tdate)ob2)); }

			// Math and rounding
			if (op == "RndUp") 		{ return nTnum(((Tnum)ob1).RoundUp((Tnum)ob2)); }
			if (op == "RndDn") 		{ return nTnum(((Tnum)ob1).RoundDown((Tnum)ob2)); }
			if (op == "RndNrUp") 	{ return nTnum(((Tnum)ob1).RoundToNearest((Tnum)ob2)); }
			if (op == "RndNrDn") 	{ return nTnum(((Tnum)ob1).RoundToNearest((Tnum)ob2, true)); }
			if (op == "Concat") 	{ return nTstr((Tstr)ob1 + (Tstr)ob2); }
			if (op == "Mod") 		{ return nTnum((Tnum)ob1 % (Tnum)ob2); }
			if (op == "Pow") 		{ return nTnum(Tnum.Pow((Tnum)ob1, (Tnum)ob2)); }
			if (op == "Log2") 		{ return nTnum(Tnum.Log((Tnum)ob1, (Tnum)ob2)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with one argument.
		/// </summary>
		private static Node UnaryFcnEval(Expr exp, Expr args, string op)
		{
			object ob1 = eval(expr(exp.nodes [1]), args).obj;

			if (op == "T!")    			{ return nTbool(!(Tbool)ob1); }
			if (op == "USD")   			{ return nTstr(((Tnum)ob1).ToUSD); }

			if (op == "Count")   		{ return nTnum(((Tset)ob1).Count); }
			if (op == "Empty")   		{ return nTbool(((Tset)ob1).IsEmpty); }
			if (op == "Rev")   			{ return nTset(((Tset)ob1).Reverse); }
			if (op == "ToThing")   		{ return n(Typ.Thing, ((Tset)ob1).ToThing); }

			if (op == "TdateDay")   	{ return nTnum(((Tdate)ob1).Day); }
			if (op == "TdateMonth")		{ return nTnum(((Tdate)ob1).Month); }
			if (op == "TdateQtr")   	{ return nTnum(((Tdate)ob1).Quarter); }
			if (op == "TdateYear")   	{ return nTnum(((Tdate)ob1).Year); }

			if (op == "Abs")   			{ return nTnum(Tnum.Abs((Tnum)ob1)); }
			if (op == "Sqrt")  			{ return nTnum(Tnum.Sqrt((Tnum)ob1)); }
			if (op == "Log")   			{ return nTnum(Tnum.Log((Tnum)ob1)); }
			if (op == "Sin")   			{ return nTnum(Tnum.Sin((Tnum)ob1)); }
			if (op == "Cos")   			{ return nTnum(Tnum.Cos((Tnum)ob1)); }
			if (op == "Tan")   			{ return nTnum(Tnum.Tan((Tnum)ob1)); }
			if (op == "Asin")  			{ return nTnum(Tnum.ArcSin((Tnum)ob1)); }
			if (op == "Acos") 			{ return nTnum(Tnum.ArcCos((Tnum)ob1)); }
			if (op == "Atan")  			{ return nTnum(Tnum.ArcTan((Tnum)ob1)); }

			return n(Typ.Null,null);
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

			if (op == "Tmax") { return nTnum(Tnum.Max(list)); }
			if (op == "Tmin") { return nTnum(Tnum.Min(list)); }

			return n(Typ.Null,null);
		}
	}
}
