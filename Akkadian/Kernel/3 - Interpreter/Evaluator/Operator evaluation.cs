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
	public partial class Session
	{
		/// <summary>
		/// Evaluates expressions with two arguments.
		/// </summary>
		private Node BinaryFcnEval(Expr exp, Expr args, Op op)
		{
			// TODO: Paralellize
			Node n1 = eval(expr(exp.nodes [1]), args);
			Typ tp = n1.objType;
			object ob1 = n1.obj;
			object ob2 = eval(expr(exp.nodes [2]), args).obj;

			// And, Or
			if (op == Op.And) 		{ return nTvar((Tvar)ob1 && (Tvar)ob2); }
			if (op == Op.Or) 		{ return nTvar((Tvar)ob1 || (Tvar)ob2); }

			// Arithmetic
			if (op == Op.Plus) 		{ return nTvar((Tvar)ob1 + (Tvar)ob2); }
			if (op == Op.Minus) 	{ return nTvar((Tvar)ob1 - (Tvar)ob2); }
			if (op == Op.Mult) 		{ return nTvar((Tvar)ob1 * (Tvar)ob2); }
			if (op == Op.Div) 		{ return nTvar((Tvar)ob1 / (Tvar)ob2); }

			// Comparison
			if (op == Op.Eq) 		{ return nTvar(Tvar.EqualTo((Tvar)ob1, (Tvar)ob2)); }
			if (op == Op.Neq) 		{ return nTvar(Tvar.NotEqualTo((Tvar)ob1, (Tvar)ob2)); }
			if (op == Op.GrTh) 		{ return nTvar((Tvar)ob1 > (Tvar)ob2); }
			if (op == Op.GrEq) 		{ return nTvar((Tvar)ob1 >= (Tvar)ob2); }
			if (op == Op.LsTh) 		{ return nTvar((Tvar)ob1 < (Tvar)ob2); }
			if (op == Op.LsEq) 		{ return nTvar((Tvar)ob1 <= (Tvar)ob2); }

			// Pipeline operators
//			if (op == Op.Pipe) 		{ return nTvar(Tvar.EqualTo((Tvar)ob1, (Tvar)ob2)); }

			// Date
			if (op == Op.AddDays) 	{ return nTvar(((Tvar)ob1).AddDays((Tvar)ob2)); }
			if (op == Op.AddMos) 	{ return nTvar(((Tvar)ob1).AddMonths((Tvar)ob2)); }
			if (op == Op.AddYrs) 	{ return nTvar(((Tvar)ob1).AddYears((Tvar)ob2)); }
			if (op == Op.DayDiff) 	{ return nTvar(H.DayDiff((Tvar)ob1, (Tvar)ob2)); }
			if (op == Op.WeekDiff) 	{ return nTvar(H.WeekDiff((Tvar)ob1, (Tvar)ob2)); }
			if (op == Op.YearDiff) 	{ return nTvar(H.YearDiff((Tvar)ob1, (Tvar)ob2)); }

			// Set operators
			if (op == Op.Subset) 	{ return nTvar(((Tvar)ob1).IsSubsetOf((Tvar)ob2)); }
			if (op == Op.Contains) 	{ return n(Typ.Tvar, ((Tvar)ob1).Contains((Thing)ob2)); }
			if (op == Op.Union) 	{ return nTvar(Tvar.Union((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.Intersect) { return nTvar(Tvar.Intersection((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.RelComp) 	{ return nTvar((Tvar.RelativeComplement((Tvar)ob1,(Tvar)ob2))); }

			// Math and rounding
			if (op == Op.RndUp) 	{ return nTvar(((Tvar)ob1).RoundUp((Tvar)ob2)); }
			if (op == Op.RndDn) 	{ return nTvar(((Tvar)ob1).RoundDown((Tvar)ob2)); }
			if (op == Op.RndNrUp) 	{ return nTvar(((Tvar)ob1).RoundToNearest((Tvar)ob2)); }
			if (op == Op.RndNrDn) 	{ return nTvar(((Tvar)ob1).RoundToNearest((Tvar)ob2, true)); }
			if (op == Op.Concat) 	{ return nTvar(Tvar.Concat((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.Mod) 		{ return nTvar((Tvar)ob1 % (Tvar)ob2); }
			if (op == Op.Pow) 		{ return nTvar(Tvar.Pow((Tvar)ob1, (Tvar)ob2)); }
			if (op == Op.Log) 		{ return nTvar(Tvar.Log((Tvar)ob1, (Tvar)ob2)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with one argument.
		/// </summary>
		private Node UnaryFcnEval(Expr exp, Expr args, Op op)
		{
			object ob1 = eval(expr(exp.nodes [1]), args).obj;

			if (op == Op.Not)    	{ return nTvar(!(Tvar)ob1); }
			if (op == Op.USD)   	{ return nTvar(((Tvar)ob1).ToUSD); }

			if (op == Op.Count)   	{ return nTvar(((Tvar)ob1).Count); }
			if (op == Op.Empty)   	{ return nTvar(((Tvar)ob1).IsEmpty); }
			if (op == Op.Rev)   	{ return nTvar(((Tvar)ob1).Reverse); }
			if (op == Op.ToThing)   { return n(Typ.Thing, ((Tvar)ob1).ToThing); }

			if (op == Op.Day)   	{ return nTvar(((Tvar)ob1).Day); }
			if (op == Op.Month)		{ return nTvar(((Tvar)ob1).Month); }
			if (op == Op.Quarter)   { return nTvar(((Tvar)ob1).Quarter); }
			if (op == Op.Year)   	{ return nTvar(((Tvar)ob1).Year); }

			if (op == Op.Abs)   	{ return nTvar(Tvar.Abs((Tvar)ob1)); }
			if (op == Op.Sqrt)  	{ return nTvar(Tvar.Sqrt((Tvar)ob1)); }
			if (op == Op.Nlog)   	{ return nTvar(Tvar.Log((Tvar)ob1)); }
			if (op == Op.Sin)   	{ return nTvar(Tvar.Sin((Tvar)ob1)); }
			if (op == Op.Cos)   	{ return nTvar(Tvar.Cos((Tvar)ob1)); }
			if (op == Op.Tan)   	{ return nTvar(Tvar.Tan((Tvar)ob1)); }
			if (op == Op.Asin)  	{ return nTvar(Tvar.ArcSin((Tvar)ob1)); }
			if (op == Op.Acos) 		{ return nTvar(Tvar.ArcCos((Tvar)ob1)); }
			if (op == Op.Atan)  	{ return nTvar(Tvar.ArcTan((Tvar)ob1)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with three or more arguments.
		/// </summary>
		private Node MultiTvarFcnEval(Expr exp, Expr args, Op op)
		{
			// TODO: Parallelize
			Tvar[] list = new Tvar[exp.nodes.Count-1];
			for (int i=1; i<exp.nodes.Count; i++)
			{
				list[i-1] = (Tvar)eval(expr(exp.nodes[i]), args).obj;
			}

			if (op == Op.Max) { return nTvar(Tvar.Max(list)); }
			if (op == Op.Min) { return nTvar(Tvar.Min(list)); }

			return n(Typ.Null,null);
		}

		//		private static Node EvalExists(Expr exp, Expr args, string op)
		//		{
		//			Node argFcnNode = n(Typ.Null,null);
		//			Tvar theSet  = (Tvar)eval(expr(exp.nodes [1]), args).obj;
		//			Tvar result = ApplyFcnToTvar<Tvar>(theSet, argFcnNode, y => CoreFilter(y));
		//
		//			//			Tvar theSet  = (Tvar)eval(expr(exp.nodes [1]), args).obj;
		//			//			Func<Thing,Tvar> theFunc = (Func<Thing,Tvar>)eval(expr(exp.nodes [2]), args).obj;
		//
		//			return n("Tvar", result);  
		//		}
	}
}
