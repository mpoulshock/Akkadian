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

			// Temporal
			if (op == Op.AsOf) 						{ return nTvar(((Tvar)ob2).AsOf((Tvar)ob1)); }
			if (op == Op.EverPer) 					{ return nTvar(((Tvar)ob2).EverPer((Tvar)ob1)); }
			if (op == Op.AlwaysPer) 				{ return nTvar(((Tvar)ob2).AlwaysPer((Tvar)ob1)); }
			if (op == Op.CountPer) 					{ return nTvar(((Tvar)ob2).CountPer((Tvar)ob1)); }
			if (op == Op.RunningCountPer) 			{ return nTvar(((Tvar)ob2).RunningCountPer((Tvar)ob1)); }
			if (op == Op.TotalElapsedDaysPer) 		{ return nTvar(((Tvar)ob2).TotalElapsedDaysPer((Tvar)ob1)); }
			if (op == Op.PeriodEndVal) 				{ return nTvar(((Tvar)ob2).PeriodEndVal((Tvar)ob1)); }
			if (op == Op.IsInPeriod) 				{ return nTvar(((Tvar)ob2).IsInPeriod((Tvar)ob1)); }
			if (op == Op.IsBetween) 				{ return nTvar(Time.IsBetween((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.RunningElapsedIntervals)	{ return nTvar(((Tvar)ob2).RunningElapsedIntervals((Tvar)ob1)); }
			if (op == Op.ContinuousElapsedIntervals){ return nTvar(((Tvar)ob2).ContinuousElapsedIntervals((Tvar)ob1)); }
			if (op == Op.RunningSummedIntervals)	{ return nTvar(((Tvar)ob2).RunningSummedIntervals((Tvar)ob1)); }

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

			if (op == Op.Not)    			{ return nTvar(!(Tvar)ob1); }
			if (op == Op.USD)   			{ return nTvar(((Tvar)ob1).ToUSD); }

			if (op == Op.Count)   			{ return nTvar(((Tvar)ob1).Count); }
			if (op == Op.Empty)   			{ return nTvar(((Tvar)ob1).IsEmpty); }
			if (op == Op.Rev)   			{ return nTvar(((Tvar)ob1).Reverse); }
			if (op == Op.ToThing)   		{ return n(Typ.Thing, ((Tvar)ob1).ToThing); }

			if (op == Op.Day)   			{ return nTvar(((Tvar)ob1).Day); }
			if (op == Op.Month)				{ return nTvar(((Tvar)ob1).Month); }
			if (op == Op.Quarter)   		{ return nTvar(((Tvar)ob1).Quarter); }
			if (op == Op.Year)   			{ return nTvar(((Tvar)ob1).Year); }
			if (op == Op.Year)   			{ return nTvar(((Tvar)ob1).Year); }
			if (op == Op.IsAtOrAfter)   	{ return nTvar( Time.IsAtOrAfter((Tvar)ob1)); }
			if (op == Op.IsBefore)   		{ return nTvar( Time.IsBefore((Tvar)ob1)); }
			if (op == Op.TemporalMin)   	{ return nTvar(((Tvar)ob1).Min()); }
			if (op == Op.TemporalMax)   	{ return nTvar(((Tvar)ob1).Max()); }
			if (op == Op.DateFirstTrue)   	{ return nTvar(((Tvar)ob1).DateFirstTrue); }
			if (op == Op.DateLastTrue)   	{ return nTvar(((Tvar)ob1).DateLastTrue); }
			if (op == Op.DaysToYears)   	{ return nTvar(((Tvar)ob1).DaysToYears); }
			if (op == Op.DaysToMonths)   	{ return nTvar(((Tvar)ob1).DaysToMonths); }
			if (op == Op.DaysToWeeks)   	{ return nTvar(((Tvar)ob1).DaysToWeeks); }

			if (op == Op.Abs)   			{ return nTvar(Tvar.Abs((Tvar)ob1)); }
			if (op == Op.Sqrt)  			{ return nTvar(Tvar.Sqrt((Tvar)ob1)); }
			if (op == Op.Nlog)   			{ return nTvar(Tvar.Log((Tvar)ob1)); }
			if (op == Op.Sin)   			{ return nTvar(Tvar.Sin((Tvar)ob1)); }
			if (op == Op.Cos)   			{ return nTvar(Tvar.Cos((Tvar)ob1)); }
			if (op == Op.Tan)   			{ return nTvar(Tvar.Tan((Tvar)ob1)); }
			if (op == Op.Asin)  			{ return nTvar(Tvar.ArcSin((Tvar)ob1)); }
			if (op == Op.Acos) 				{ return nTvar(Tvar.ArcCos((Tvar)ob1)); }
			if (op == Op.Atan)  			{ return nTvar(Tvar.ArcTan((Tvar)ob1)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with three arguments.
		/// </summary>
		private Node ThreeArgFcnEval(Expr exp, Expr args, Op op)
		{
			object ob1 = eval(expr(exp.nodes [1]), args).obj;
			object ob2 = eval(expr(exp.nodes [2]), args).obj;
			object ob3 = eval(expr(exp.nodes [3]), args).obj;

			if (op == Op.Shift) 					{ return nTvar(((Tvar)ob3).Shift((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.ComposeDate) 				{ return nTvar( Tvar.ComposeDate((Tvar)ob1,(Tvar)ob2,(Tvar)ob3) ); }
			if (op == Op.SlidingElapsedIntervals) 	{ return nTvar(((Tvar)ob3).SlidingElapsedIntervals((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.SlidingSummedIntervals) 	{ return nTvar(((Tvar)ob3).SlidingSummedIntervals((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.IsAlwaysTrue) 				{ return nTvar(((Tvar)ob3).IsAlwaysTrue((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.IsEverTrue) 				{ return nTvar(((Tvar)ob3).IsEverTrue((Tvar)ob1,(Tvar)ob2)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with four arguments.
		/// </summary>
		private Node FourArgFcnEval(Expr exp, Expr args, Op op)
		{
			object ob1 = eval(expr(exp.nodes [1]), args).obj;
			object ob2 = eval(expr(exp.nodes [2]), args).obj;
			object ob3 = eval(expr(exp.nodes [3]), args).obj;
			object ob4 = eval(expr(exp.nodes [4]), args).obj;

			if (op == Op.TotalSummedIntervals) 	{ return nTvar(((Tvar)ob4).TotalSummedIntervals((Tvar)ob1,(Tvar)ob2,(Tvar)ob3)); }
			if (op == Op.TotalElapsedIntervals) { return nTvar(((Tvar)ob4).TotalElapsedIntervals((Tvar)ob1,(Tvar)ob2,(Tvar)ob3)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with a dynamic number of arguments.
		/// </summary>
		private Node MultiTvarFcnEval(Expr exp, Expr args, Op op)
		{
			// TODO: Parallelize
			Tvar[] list = new Tvar[exp.nodes.Count-1];
			for (int i=1; i<exp.nodes.Count; i++)
			{
				list[i-1] = (Tvar)eval(expr(exp.nodes[i]), args).obj;
			}

			if (op == Op.Max) 			{ return nTvar(Tvar.Max(list)); }
			if (op == Op.Min) 			{ return nTvar(Tvar.Min(list)); }
			if (op == Op.BoolCount) 	{ return nTvar(H.BoolCount(list)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates built-in constants.
		/// </summary>
		private Node ConstantEval(Op op)
		{
			if (op == Op.Unstated) 			{ return nTvar(new Tvar(Hstate.Unstated)); }
			if (op == Op.Uncertain) 		{ return nTvar(new Tvar(Hstate.Uncertain)); }
			if (op == Op.Stub) 				{ return nTvar(new Tvar(Hstate.Stub)); }

			if (op == Op.DawnOfTime) 		{ return nTvar(Time.DawnOf); }
			if (op == Op.Dawn) 				{ return nTvar(Time.DawnOf); }
			if (op == Op.EndOfTime) 		{ return nTvar(Time.EndOf); }
			if (op == Op.End) 				{ return nTvar(Time.EndOf); }
			if (op == Op.Now) 				{ return nTvar(new Tvar(DateTime.Now)); }

			if (op == Op.TheYear) 			{ return nTvar(H.TheYear); }
			if (op == Op.TheQuarter) 		{ return nTvar(H.TheQuarter); }
			if (op == Op.TheMonth) 			{ return nTvar(H.TheMonth); }
			if (op == Op.TheWeek) 			{ return nTvar(H.TheCalendarWeek); }
			if (op == Op.TheDay) 			{ return nTvar(H.TheDay); }

			if (op == Op.DaysInYear) 		{ return nTvar(Time.DaysInYear()); }
			if (op == Op.DaysInQuarter) 	{ return nTvar(Time.DaysInQuarter()); }
			if (op == Op.DaysInMonth) 		{ return nTvar(Time.DaysInMonth()); }
			if (op == Op.IsLeapYear) 		{ return nTvar(Time.IsLeapYear()); }

			if (op == Op.ConstPi) 			{ return nTvar(Tvar.ConstPi); }
			if (op == Op.ConstE) 			{ return nTvar(Tvar.ConstE); }

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
