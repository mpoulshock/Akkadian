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
	public partial class Interpreter
	{
		/// <summary>
		/// Applies an aggregation function to a Tset and an argument function.
		/// </summary>
		private static T ApplyFcnToTset<T>(Tset theSet, 
		                                   Node argumentFcn, 
		                                   Func<List<Tuple<Thing,Hval>>,Hval> aggregationFcn) where T : Tvar
		{
			Dictionary<Thing,Tvar> fcnValues = new Dictionary<Thing,Tvar>();
			List<Tvar> listOfTvars = new List<Tvar>();

			// Get the temporal value of each distinct entity in the set
			foreach(Thing le in Tset.DistinctEntities(theSet))
			{
				// Func<Thing,Tvar> argumentFcn
				Tvar val = (Tvar)eval(argumentFcn, expr(n("Thing",le))).obj;
				fcnValues.Add(le, val);
				listOfTvars.Add(val);
			} 

			// At each breakpoint, for each member of the set,
			// aggregate and analyze the values of the functions
			T result = (T)Akkadian.Util.ReturnProperTvar<T>();
			foreach(DateTime dt in Tset.AggregatedTimePoints(theSet, listOfTvars))
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
					List<Tuple<Thing,Hval>> thingValPairs = new List<Tuple<Thing,Hval>>();

					// Values to check for uncertainty
					List<Hval> values = new List<Hval>();

					foreach(Thing le in (List<Thing>)membersOfSet.Val)
					{
						Tvar funcVal = (Tvar)fcnValues[le];    
						Hval funcValAt = funcVal.ObjectAsOf(dt);
						values.Add(funcValAt);
						thingValPairs.Add(new Tuple<Thing,Hval>(le,funcValAt));
					}

					Hstate top = H.PrecedingState(values);
					if (top != Hstate.Known)
					{
						result.AddState(dt, new Hval(null, top));
					}
					else
					{
						// List<Tuple<Thing,Hval>>,Hval> aggregationFcn
						result.AddState(dt, aggregationFcn(thingValPairs));
					} 
				}
			}

			return result.LeanTvar<T>();
		}
	}
}
