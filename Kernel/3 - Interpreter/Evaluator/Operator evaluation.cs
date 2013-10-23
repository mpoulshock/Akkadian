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
	public partial class Interpreter
	{
		/// <summary>
		/// Evaluates expressions in which a short-circuit may be required.
		/// </summary>
		private static Node EvalShortCircuitFcns(Expr exp, Expr args, Op op)
		{
			// TODO: Forget short-circuits and instead paralellize?
			Node n1 = eval(expr(exp.nodes [1]), args);

			if (op == Op.And) 	// And
			{
				// See if first argument is eternally false
				if (((Tbool)n1.obj).IsFalse) return nTbool(false);

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return nTbool((Tbool)n1.obj && (Tbool)n2.obj); 
			}

			if (op == Op.Or) 	// Or
			{
				// See if first argument is eternally true
				if (((Tbool)n1.obj).IsTrue) return nTbool(true);

				// Else, eval the second argument
				Node n2 = eval(expr(exp.nodes [2]), args);
				return nTbool((Tbool)n1.obj || (Tbool)n2.obj); 
			}

			if (op == Op.Mult) 	// Multiplication
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
		private static Node BinaryFcnEval(Expr exp, Expr args, Op op)
		{
			// TODO: Paralellize
			Node n1 = eval(expr(exp.nodes [1]), args);
			Typ tp = n1.objType;
			object ob1 = n1.obj;
			object ob2 = eval(expr(exp.nodes [2]), args).obj;

			if (op == Op.Plus) { return nTnum((Tnum)ob1 + (Tnum)ob2); }
			if (op == Op.Minus) { return nTnum((Tnum)ob1 - (Tnum)ob2); }
			if (op == Op.Div) { return nTnum((Tnum)ob1 / (Tnum)ob2); }

			if (op == Op.Eq) 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 == (Tnum)ob2); 
				if (tp == Typ.Tstr)  return nTbool((Tstr)ob1 == (Tstr)ob2);
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 == (Tdate)ob2);
				if (tp == Typ.Tset)  return nTbool((Tset)ob1 == (Tset)ob2);
				if (tp == Typ.Tbool) return nTbool((Tbool)ob1 == (Tbool)ob2);
			}

			if (op == Op.Neq) 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 != (Tnum)ob2); 
				if (tp == Typ.Tstr)  return nTbool((Tstr)ob1 != (Tstr)ob2);
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 != (Tdate)ob2);
				if (tp == Typ.Tset)  return nTbool((Tset)ob1 != (Tset)ob2);
				if (tp == Typ.Tbool) return nTbool((Tbool)ob1 != (Tbool)ob2);
			}

			if (op == Op.GrTh) 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 > (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 > (Tdate)ob2);
			}
			if (op == Op.GrEq) 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 >= (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 >= (Tdate)ob2);
			}
			if (op == Op.LsTh) 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 < (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 < (Tdate)ob2);
			}
			if (op == Op.LsEq) 
			{ 
				if (tp == Typ.Tnum)  return nTbool((Tnum)ob1 <= (Tnum)ob2); 
				if (tp == Typ.Tdate) return nTbool((Tdate)ob1 <= (Tdate)ob2);
			}

			// Set operators
			if (op == Op.Subset) 	{ return nTbool(((Tset)ob1).IsSubsetOf((Tset)ob2)); }
			if (op == Op.Contains) 	{ return n(Typ.Tbool, ((Tset)ob1).Contains((Thing)ob2)); }
			if (op == Op.Union) 	{ return nTset((Tset)ob1 | (Tset)ob2); }
			if (op == Op.Intersect) { return nTset((Tset)ob1 & (Tset)ob2); }
			if (op == Op.RelComp) 	{ return nTset((Tset)ob1 - (Tset)ob2); }

			// Date
			if (op == Op.AddDays) 	{ return nTdate(((Tdate)ob1).AddDays((Tnum)ob2)); }
			if (op == Op.AddMos) 	{ return nTdate(((Tdate)ob1).AddMonths((Tnum)ob2)); }
			if (op == Op.AddYrs) 	{ return nTdate(((Tdate)ob1).AddYears((Tnum)ob2)); }
			if (op == Op.DayDiff) 	{ return nTnum(H.DayDiff((Tdate)ob1, (Tdate)ob2)); }
			if (op == Op.WeekDiff) 	{ return nTnum(H.WeekDiff((Tdate)ob1, (Tdate)ob2)); }
			if (op == Op.YearDiff) 	{ return nTnum(H.YearDiff((Tdate)ob1, (Tdate)ob2)); }

			// Math and rounding
			if (op == Op.RndUp) 	{ return nTnum(((Tnum)ob1).RoundUp((Tnum)ob2)); }
			if (op == Op.RndDn) 	{ return nTnum(((Tnum)ob1).RoundDown((Tnum)ob2)); }
			if (op == Op.RndNrUp) 	{ return nTnum(((Tnum)ob1).RoundToNearest((Tnum)ob2)); }
			if (op == Op.RndNrDn) 	{ return nTnum(((Tnum)ob1).RoundToNearest((Tnum)ob2, true)); }
			if (op == Op.Concat) 	{ return nTstr((Tstr)ob1 + (Tstr)ob2); }
			if (op == Op.Mod) 		{ return nTnum((Tnum)ob1 % (Tnum)ob2); }
			if (op == Op.Pow) 		{ return nTnum(Tnum.Pow((Tnum)ob1, (Tnum)ob2)); }
			if (op == Op.Log) 		{ return nTnum(Tnum.Log((Tnum)ob1, (Tnum)ob2)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with one argument.
		/// </summary>
		private static Node UnaryFcnEval(Expr exp, Expr args, Op op)
		{
			object ob1 = eval(expr(exp.nodes [1]), args).obj;

			if (op == Op.Not)    	{ return nTbool(!(Tbool)ob1); }
			if (op == Op.USD)   	{ return nTstr(((Tnum)ob1).ToUSD); }

			if (op == Op.Count)   	{ return nTnum(((Tset)ob1).Count); }
			if (op == Op.Empty)   	{ return nTbool(((Tset)ob1).IsEmpty); }
			if (op == Op.Rev)   	{ return nTset(((Tset)ob1).Reverse); }
			if (op == Op.ToThing)   { return n(Typ.Thing, ((Tset)ob1).ToThing); }

			if (op == Op.Day)   	{ return nTnum(((Tdate)ob1).Day); }
			if (op == Op.Month)		{ return nTnum(((Tdate)ob1).Month); }
			if (op == Op.Quarter)   { return nTnum(((Tdate)ob1).Quarter); }
			if (op == Op.Year)   	{ return nTnum(((Tdate)ob1).Year); }

			if (op == Op.Abs)   	{ return nTnum(Tnum.Abs((Tnum)ob1)); }
			if (op == Op.Sqrt)  	{ return nTnum(Tnum.Sqrt((Tnum)ob1)); }
			if (op == Op.Nlog)   	{ return nTnum(Tnum.Log((Tnum)ob1)); }
			if (op == Op.Sin)   	{ return nTnum(Tnum.Sin((Tnum)ob1)); }
			if (op == Op.Cos)   	{ return nTnum(Tnum.Cos((Tnum)ob1)); }
			if (op == Op.Tan)   	{ return nTnum(Tnum.Tan((Tnum)ob1)); }
			if (op == Op.Asin)  	{ return nTnum(Tnum.ArcSin((Tnum)ob1)); }
			if (op == Op.Acos) 		{ return nTnum(Tnum.ArcCos((Tnum)ob1)); }
			if (op == Op.Atan)  	{ return nTnum(Tnum.ArcTan((Tnum)ob1)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with three or more arguments.
		/// </summary>
		private static Node MultiTnumFcnEval(Expr exp, Expr args, Op op)
		{
			// TODO: Parallelize
			Tnum[] list = new Tnum[exp.nodes.Count-1];
			for (int i=1; i<exp.nodes.Count; i++)
			{
				list[i-1] = (Tnum)eval(expr(exp.nodes[i]), args).obj;
			}

			if (op == Op.Max) { return nTnum(Tnum.Max(list)); }
			if (op == Op.Min) { return nTnum(Tnum.Min(list)); }

			return n(Typ.Null,null);
		}

		//		private static Node EvalExists(Expr exp, Expr args, string op)
		//		{
		//			Node argFcnNode = n(Typ.Null,null);
		//			Tset theSet  = (Tset)eval(expr(exp.nodes [1]), args).obj;
		//			Tset result = ApplyFcnToTset<Tset>(theSet, argFcnNode, y => CoreFilter(y));
		//
		//			//			Tset theSet  = (Tset)eval(expr(exp.nodes [1]), args).obj;
		//			//			Func<Thing,Tbool> theFunc = (Func<Thing,Tbool>)eval(expr(exp.nodes [2]), args).obj;
		//
		//			return n("Tset", result);  
		//		}
	}
}
