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
using System.Linq;

namespace Akkadian
{
	public partial class Session
	{

		// Filter2[IsFemale[_], thePeople]
		public Tvar Filter2(Expr argumentFcn, Expr args, Tvar mainSet)
		{
			return ApplyFcnToTvar2(mainSet, argumentFcn, args, y => CoreFilter2(y)).LeanTset;
		}
		private static Hval CoreFilter2(List<Tuple<object,Hval>> list)
		{
			List<object> result = new List<object>();
			foreach (Tuple<object,Hval> tu in list)
			{
				if (tu.Item2.IsTrue)
				{
					result.Add(tu.Item1);
				}
			}
			return new Hval(result);
		}

		/// <summary>
		/// Applies an aggregation function to a Tvar and an argument function.
		/// </summary>
		private Tvar ApplyFcnToTvar2(Tvar theSet, Expr argumentFcn, Expr args,
									 Func<List<Tuple<object,Hval>>,Hval> aggregationFcn)
		{
			Dictionary<object,Tvar> fcnValues = new Dictionary<object,Tvar>();
			List<Tvar> listOfTvars = new List<Tvar>();

			// Get the temporal value of each distinct entity in the set
			foreach(object le in Tvar.DistinctEntities(theSet))
			{
				// set * in args to le
				List<Node> argNodes = args.nodes;
				List<Node> argNodesWithSubstitution = new List<Node>();
				foreach (Node node in argNodes)
				{
					argNodesWithSubstitution.Add(node);
				}
				Expr newArgs = new Expr(argNodesWithSubstitution);

				Tvar val = (Tvar)this.eval(argumentFcn, newArgs).obj;
				fcnValues.Add(le, val);
				listOfTvars.Add(val);
			} 

			// At each breakpoint, for each member of the set,
			// aggregate and analyze the values of the functions
			Tvar result = new Tvar();
			foreach(DateTime dt in Tvar.AggregatedTimePoints(theSet, listOfTvars))
			{
				Hval membersOfSet = theSet.ObjectAsOf(dt);

				// If theSet is unknown...
				if (!membersOfSet.IsKnown)
				{
					result.AddState(dt, membersOfSet);
				}
				else
				{
					// Cube that gets sent to the aggregation function
					List<Tuple<object,Hval>> thingValPairs = new List<Tuple<object,Hval>>();

					// Values to check for uncertainty
					List<Hval> values = new List<Hval>();

					foreach(object le in (List<object>)membersOfSet.Val)
					{
						Tvar funcVal = (Tvar)fcnValues[le];    
						Hval funcValAt = funcVal.ObjectAsOf(dt);
						values.Add(funcValAt);
						thingValPairs.Add(new Tuple<object,Hval>(le,funcValAt));
					}

					Hstate top = H.PrecedingState(values);
					if (top != Hstate.Known)
					{
						result.AddState(dt, new Hval(null, top));
					}
					else
					{
						// List<Tuple<object,Hval>>,Hval> aggregationFcn
						result.AddState(dt, aggregationFcn(thingValPairs));
					} 
				}
			}

			return result.LeanTset;
		}
	}
}
