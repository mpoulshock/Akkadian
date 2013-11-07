// Copyright (c) 2012-2013 Hammura.bi LLC
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
    /// <summary>
    /// Provides the interface for 
    /// </summary>
    public partial class Tvar
    {
        /// <summary>
        /// Returns true when a condition holds for at least one member of
        /// a given set.
        /// </summary>
        public Tvar Exists(Func<Thing,Tvar> argumentFcn)
        {
            // Analyze set for existence of the condition
            Tvar subset = this.Filter(x => argumentFcn(x));
            return subset.Count > 0;
        }
        
        /// <summary>
        /// Returns true when a condition holds for all members of
        /// a given set.
        /// </summary>
        public Tvar ForAll(Func<Thing,Tvar> argumentFcn)
        {
            // By convention, the universal quantification of an empty set is always true
            // http://en.wikipedia.org/wiki/Universal_quantification#The_empty_set
            if (this.Count == 0) return new Tvar(true);

            // Analyze set for universality of the condition
            Tvar subset = this.Filter(x => argumentFcn(x));
            return this.Count == subset.Count;
        }
        
        /// <summary>
        /// Filter function - for various types of legal entities
        /// </summary>
        public Tvar Filter(Func<Thing,Tvar> argumentFcn)
        {
            return ApplyFcnToTvar(this, x => argumentFcn((Thing)x), y => CoreFilter(y)).LeanTset;
        }
        private static Hval CoreFilter(List<Tuple<Thing,Hval>> list)
        {
            List<Thing> result = new List<Thing>();
            foreach (Tuple<Thing,Hval> tu in list)
            {
                if (tu.Item2.IsTrue)
                {
                    result.Add(tu.Item1);
                }
            }
            return new Hval(result);
        }

        /// <summary>
        /// Totals the values of a given numeric property of the members of
        /// a set.
        /// </summary>
        public Tvar Sum(Func<Thing,Tvar> func)
        {
            return ApplyFcnToTvar(this, x => func((Thing)x), y => CoreSum(y));
        }
        private static Hval CoreSum(List<Tuple<Thing,Hval>> list)
        {
            return SliceHvalsFromCube(list).Sum(item => Convert.ToDecimal(item.Val));
        }

        /// <summary>
        /// Returns the minimum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public Tvar Min(Func<Thing,Tvar> func)
        {
            return ApplyFcnToTvar(this, x => func((Thing)x), y => CoreMin(y));
        }
        private static Hval CoreMin(List<Tuple<Thing,Hval>> list)
        {
            return Util.Minimum(SliceHvalsFromCube(list));
        }

        /// <summary>
        /// Returns the maximum value of a given numeric property of the 
        /// members of a set.
        /// </summary>
        public Tvar Max(Func<Thing,Tvar> func)
        {
            return ApplyFcnToTvar(this, x => func((Thing)x), y => CoreMax(y));
        }
        private static Hval CoreMax(List<Tuple<Thing,Hval>> list)
        {
            return Util.Maximum(SliceHvalsFromCube(list));
        }

        /// <summary>
        /// Sorts the members of a Tvar based on a Tvar function.  Members with lower function values
        /// come first in the sorted list.
        /// </summary>
        public Tvar OrderBy(Func<Thing,Tvar> func)
        {
            return ApplyFcnToTvar(this, x => func((Thing)x), y => CoreOrder(y)).LeanTset;
        }
        private static Hval CoreOrder(List<Tuple<Thing,Hval>> list)
        {
            List<Thing> result = new List<Thing>();

            IEnumerable<Tuple<Thing,Hval>> query = list.OrderBy(pair => pair.Item2.Val);

            foreach (Tuple<Thing,Hval> pair in query)
            {
                result.Add(pair.Item1);
            }

            return new Hval(result);
        }

        /// <summary>
        /// Applies an aggregation function to a Tvar and an argument function.
        /// </summary>
		private static Tvar ApplyFcnToTvar(Tvar theSet, 
		                                   Func<Thing,Tvar> argumentFcn, 
		                                   Func<List<Tuple<Thing,Hval>>,Hval> aggregationFcn)
		{
			Dictionary<Thing,Tvar> fcnValues = new Dictionary<Thing,Tvar>();
			List<Tvar> listOfTvars = new List<Tvar>();

			// Get the temporal value of each distinct entity in the set
			foreach(Thing le in DistinctEntities(theSet))
			{
				Tvar val = argumentFcn(le);
				fcnValues.Add(le, val);
				listOfTvars.Add(val);
			} 

			// At each breakpoint, for each member of the set,
			// aggregate and analyze the values of the functions
			Tvar result = new Tvar();
			foreach(DateTime dt in AggregatedTimePoints(theSet, listOfTvars))
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

					Hstate top = PrecedingState(values);
					if (top != Hstate.Known)
					{
						result.AddState(dt, new Hval(null, top));
					}
					else
					{
						result.AddState(dt, aggregationFcn(thingValPairs));
					} 
				}
			}

			return result.Lean;
		}

        /// <summary>
        /// Private method that aggregates all time points among a Tvar and one
        /// or more Tvars
        /// </summary>
        public static List<DateTime> AggregatedTimePoints(Tvar theSet, List<Tvar> listOfTvars)
        {
            listOfTvars.Add(theSet);
            return TimePoints(listOfTvars);
        }

        /// <summary>
        /// Returns the column of Hvals from the cube of Thing-Hval pairs.
        /// </summary>
        private static List<Hval> SliceHvalsFromCube(List<Tuple<Thing,Hval>> list)
        {
            List<Hval> slice = new List<Hval>();
            foreach (Tuple<Thing,Hval> t in list)
            {
                slice.Add(t.Item2);
            }
            return slice;
        }

        /// <summary>
        /// Returns a list of all legal entities that were ever members of the 
        /// set. 
        /// </summary>
        public static List<Thing> DistinctEntities(Tvar theSet)
        {
            List<Thing> result = new List<Thing>();

            foreach(KeyValuePair<DateTime,Hval> de in theSet.TimeLine)
            {
                if (de.Value.IsKnown)
                {
                    foreach(Thing le in (List<Thing>)de.Value.Val)
                    {
                        if (!result.Contains(le))
                        {
                            result.Add(le);    
                        }
                    }
                }
            }

            return result;
        }
    }
}