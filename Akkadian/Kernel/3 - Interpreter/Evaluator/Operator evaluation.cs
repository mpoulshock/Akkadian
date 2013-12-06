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
		/// Evaluates expressions with and/or.
		/// </summary>
		private Node ShortCircuitEval(Expr exp, Expr args, Op op)
		{
			Node n1 = eval(expr(exp.nodes [1]), args);
			Typ tp = n1.objType;
			object ob1 = n1.obj;

			// And, Or
			if (op == Op.And)
			{
				if (((Tvar)ob1).IsFalse) return nTvar(new Tvar(false));

				object ob2 = eval(expr(exp.nodes [2]), args).obj;
				return nTvar((Tvar)ob1 && (Tvar)ob2); 
			}

			if (op == Op.Or)
			{
				if (((Tvar)ob1).IsTrue) return nTvar(new Tvar(true));

				object ob2 = eval(expr(exp.nodes [2]), args).obj;
				return nTvar((Tvar)ob1 || (Tvar)ob2); 
			}

			return n(Typ.Null,null);
		}

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
			if (op == Op.Regularize)				{ return nTvar(((Tvar)ob2).Regularize((Tvar)ob1)); }

			// Date
			if (op == Op.AddDays) 	{ return nTvar(((Tvar)ob2).AddDays((Tvar)ob1)); }
			if (op == Op.AddMonths) { return nTvar(((Tvar)ob2).AddMonths((Tvar)ob1)); }
			if (op == Op.AddYears) 	{ return nTvar(((Tvar)ob2).AddYears((Tvar)ob1)); }
			if (op == Op.DayDiff) 	{ return nTvar(H.DayDiff((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.WeekDiff) 	{ return nTvar(H.WeekDiff((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.YearDiff) 	{ return nTvar(H.YearDiff((Tvar)ob1,(Tvar)ob2)); }

			// Set operators
			if (op == Op.Subset) 	{ return nTvar(((Tvar)ob2).IsSubsetOf((Tvar)ob1)); }
			if (op == Op.Contains) 	{ return n(Typ.Tvar, ((Tvar)ob2).Contains(Tvar.MakeTset(ob1))); }
			if (op == Op.Union) 	{ return nTvar(Tvar.Union((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.Intersect) { return nTvar(Tvar.Intersection((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.Remove) 	{ return nTvar((Tvar.RelativeComplement((Tvar)ob2,(Tvar)ob1))); }
			if (op == Op.Seq) 		{ return nTvar((Tvar.Seq((Tvar)ob1,(Tvar)ob2))); }

			// Math and rounding
			if (op == Op.RndUp) 		{ return nTvar(((Tvar)ob2).RoundUp((Tvar)ob1)); }
			if (op == Op.RndDn) 		{ return nTvar(((Tvar)ob2).RoundDown((Tvar)ob1)); }
			if (op == Op.RoundNearUp) 	{ return nTvar(((Tvar)ob2).RoundToNearest((Tvar)ob1)); }
			if (op == Op.RoundNearDown) { return nTvar(((Tvar)ob2).RoundToNearest((Tvar)ob1, true)); }
			if (op == Op.Concat) 		{ return nTvar(Tvar.Concat((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.Mod) 			{ return nTvar((Tvar)ob1 % (Tvar)ob2); }
			if (op == Op.Pow) 			{ return nTvar(Tvar.Pow((Tvar)ob1,(Tvar)ob2)); }
			if (op == Op.Log) 			{ return nTvar(Tvar.Log((Tvar)ob1,(Tvar)ob2)); }

			return n(Typ.Null,null);
		}

		/// <summary>
		/// Evaluates expressions with one argument.
		/// </summary>
		private Node UnaryFcnEval(Expr exp, Expr args, Op op)
		{
			object ob1 = eval(expr(exp.nodes[1]), args).obj;

			if (op == Op.Not)    			{ return nTvar(!(Tvar)ob1); }
			if (op == Op.ToUSD)   			{ return nTvar(((Tvar)ob1).ToUSD); }
			if (op == Op.Trim)				{ return nTvar(((Tvar)ob1).Lean); }

			if (op == Op.Count)   			{ return nTvar(((Tvar)ob1).Count); }
			if (op == Op.IsEmpty)   		{ return nTvar(((Tvar)ob1).IsEmpty); }
			if (op == Op.Reverse)   		{ return nTvar(((Tvar)ob1).Reverse); }
			if (op == Op.SetSum)   			{ return nTvar(((Tvar)ob1).SumItems); }
			if (op == Op.SetMax)   			{ return nTvar(((Tvar)ob1).MaxItem); }
			if (op == Op.SetMin)   			{ return nTvar(((Tvar)ob1).MinItem); }
			if (op == Op.First)   			{ return nTvar(((Tvar)ob1).First); }
			if (op == Op.Rest)   			{ return nTvar(((Tvar)ob1).Rest); }

			if (op == Op.Day)   			{ return nTvar(((Tvar)ob1).Day); }
			if (op == Op.Month)				{ return nTvar(((Tvar)ob1).Month); }
			if (op == Op.Quarter)   		{ return nTvar(((Tvar)ob1).Quarter); }
			if (op == Op.Year)   			{ return nTvar(((Tvar)ob1).Year); }
			if (op == Op.Year)   			{ return nTvar(((Tvar)ob1).Year); }
			if (op == Op.IsAtOrAfter)   	{ return nTvar( Time.IsAtOrAfter((Tvar)ob1)); }
			if (op == Op.IsBefore)   		{ return nTvar( Time.IsBefore((Tvar)ob1)); }
			if (op == Op.Low)   			{ return nTvar(((Tvar)ob1).Min()); }
			if (op == Op.High)   			{ return nTvar(((Tvar)ob1).Max()); }
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

			if (op == Op.MakeTset) 		
			{ 

				return nTvar(Tvar.MakeTset(list)); 
			}

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

		/// <summary>
		/// Evaluates the unquote operator (~).
		/// </summary>
		private Node EvaluateFunction(Expr exp, Expr args, string fcnName)
		{
			if (ContainsFunction(fcnName))
			{
				// Call the function with the given name
				Expr ex1 = GetFunction(fcnName);
				return MixAndMatch(exp, args, ex1);
			}
			else
			{
				// Make a list of the arguments
				string argString = "";
				for (int i=0; i < exp.nodes.Count; i++)
				{
					if (i>0) 
					{
						Tvar argTv = (Tvar)eval(exp.nodes[i], args).obj;
						argString += argTv.ToString() + ",";
					}
				}

				// Pose the leaf node as a question - temporary
				if (AskQuestions)
				{
					Console.Write("  - " + fcnName + "[" + argString.TrimEnd(',') + "]? ");
					string s = Console.ReadLine();
					return nTvar(s);
				}
				return nTvar(new Tvar(Hstate.Unstated));
			}
		}

		/// <summary>
		/// Applies the higher-order Filter and Map functions to a Tset.
		/// </summary>
		/// <remarks>
		/// Filter - Filters the items in a Tset according to boolean criteria (keeping those that meet the condition).
		/// Map - Maps an expression to the items in a Tset.
		/// </remarks>
		private Node EvalFilterOrMap(Expr exp, Expr args, Op op)
		{
			// E.g., Map[Sq[_],someSet]
			Tvar theSet = (Tvar)eval(expr(exp.nodes[2]),args).obj;

			Tvar result = new Tvar();

			// Foreach interval in theSet
			foreach(KeyValuePair<DateTime,Hval> slice in theSet.IntervalValues)
			{  
				// Handle unknowns
				Hstate top = Tvar.PrecedingState(slice.Value);
				if (top != Hstate.Known)
				{
					result.AddState(slice.Key, new Hval(null, top));
				}
				else
				{
					// Apply the expression to every item in the set
					List<object> sliceSet = (List<object>)slice.Value.Val;
					List<object> resultSet = new List<object>();
					foreach (object ob in sliceSet)
					{
						// The wildcard argument (_) is the last item in the args list
						// Assumption: every item in the set is a Tvar
						Node theOb = nTvar(new Tvar(Convert.ToString(ob)));
						Expr newArgs = AddNodeToExpression(args,theOb);	

						// Evaluate the expression, given the new list of args
						Tvar itemResult = (Tvar)eval(expr(exp.nodes[1]), newArgs).obj;

						// E.g., Map[Sq[_],someSet]
						if (op == Op.Map)
						{
							resultSet.Add(itemResult.FirstValue.Val);  // TODO: Deal with unknowns
						}

						// E.g., Filter[ _ > 25,someSet]
						if (op == Op.Filter)
						{
							if (itemResult.FirstValue.IsTrue)
							{
								resultSet.Add(ob);
							}
						}
					}
					result.AddState(slice.Key, new Hval(resultSet));
				}
			}

			return n(Typ.Tvar, result);
		}

		/// <summary>
		/// Evaluates the unquote operator (~).
		/// </summary>
		private Node EvaluateUnquoteOperator(Expr exp, Expr args)
		{
			// E.g. {Op:Unquote,node2}
			Node node2 = exp.nodes[1];

			if (node2.objType == Typ.Expr)
			{
				Node subnode = ((Expr)node2.obj).nodes[0];
				if ((Op)subnode.obj == Op.Quote)
				{
					// E.g. {Op:Unquote,Expr:{Op.Quote,Tvar:2}}
					Expr quotedExpr = (Expr)exp.nodes[1].obj;			// {Op.Quote,Tvar:2}
					List<Node> nodesToEval = Rest(quotedExpr.nodes);    // {Tvar:2}
					return eval(new Expr(nodesToEval), args);
				}
			}

			// E.g. {Op:Unquote,Var:0}
			// Evaluate the second node, then return an unquoted expression
			Node eval2 = eval(node2,args);
			return eval(new Expr(new List<Node>(){n(Typ.Op,Op.Unquote),eval2}), args);
		}

		/// <summary>
		/// Adds a node to the end of an expression.
		/// </summary>
		private static Expr AddNodeToExpression(Expr args, Node newNode)
		{
			List<Node> resultNodes = new List<Node>();
			foreach (Node n in args.nodes)
			{
				resultNodes.Add(n);
			}
			resultNodes.Add(newNode);
			return new Expr(resultNodes);
		}
	}
}
