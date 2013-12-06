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
    #pragma warning disable 660, 661
    
    /// <summary>
    /// A set of legal entities whose membership varies over time.
    /// Example: a family that is composed of different members at various
    /// points in time.
    /// </summary>
    public partial class Tvar
    {
		/// <summary>
		/// Constructs an empty Tset.
		/// </summary>
		public static Tvar MakeTset()
		{
			return MakeTset(new List<object>(){});
		}

		/// <summary>
		/// Constructs a Tvar consisting eternally of a list of members. 
		/// </summary>
		public static Tvar MakeTset(List<object> list)
		{
			Tvar t = new Tvar();
			t.SetEternally(new Hval(list));
			return t;
		}

		/// <summary>
		/// Constructs a Tvar consisting eternally of a list of members. 
		/// </summary>
		public static Tvar MakeTset(params object[] list)
		{
			Tvar t = new Tvar();
			t.SetEternally(list);
			return t;
		}

        /// <summary>
        /// Sets the Tvar to eternally have a list of members. 
        /// </summary>
        public void SetEternally(params object[] list)
        {
			AddState(Time.DawnOf,list);
        }
        
		/// <summary>
		/// Adds a time interval and list of set members to the timeline. 
		/// </summary>
		public void AddState(DateTime dt, params object[] list)
		{
			List<object> entities = new List<object>();

			foreach (object le in list)
				entities.Add(le);

			TimeLine.Add(dt, new Hval(entities));    
		}

		/// <summary>
		/// Constructs a Tvar from an existing Tvar. 
		/// </summary>
		public Tvar(Tvar s)
		{
			for (int i=0; i<s.TimeLine.Count; i++)
			{
				this.AddState(s.TimeLine.Keys[i], s.TimeLine.Values[i]);
			}
		}

        /// <summary>
        /// Determines whether two lists of legal entities are equivalent (ignoring order)
        /// </summary>
        public static bool AreEquivalentSets(List<object> L1, List<object> L2)
        {
            if (L1.Count != L2.Count) return false;
            
            foreach(object i in L1)
            {
                if (!L2.Contains(i)) return false;
            }
            
            return true;
        }                             

        /// <summary>
        /// Counts the number of set members at each time interval. 
        /// </summary>
        public Tvar Count
        {
            get
            {
                return ApplyFcnToTimeline(x => CoreTvarCount(x), this);
            }
        }
        private static Hval CoreTvarCount(Hval h)
        {
            return ((List<object>)h.Val).Count;
        }

        /// <summary>
        /// Returns true when the set has no members.
        /// </summary>
        public Tvar IsEmpty
        {
            get
            {
                return this.Count == 0;
            }
        }    
        
        /// <summary>
        /// Returns true when one Tvar is a subset of another. 
        /// </summary>
        public Tvar IsSubsetOf(Tvar super)
        {
            return ApplyFcnToTimeline(x => CoreSubset(x), this, super);
        }
        private static Hval CoreSubset(List<Hval> list)
        {
            List<object> s1 = (List<object>)list[0].Val;
            List<object> s2 = (List<object>)list[1].Val;
            return new Hval(!s1.Except(s2).Any());
        }

        /// <summary>
        /// Returns true when the Tvar contains a given legal entity. 
        /// </summary>
        public Tvar Contains(object e)
        {
			return ApplyFcnToTimeline(x => CoreSubset(x), MakeTset(e), this);
        }

        /// <summary>
        /// Returns the temporal union of two Tvars.
        /// This is equivalent to a logical OR of two sets.
        /// </summary>
        /// <remarks>
        /// This method provides a second operator denoting
        /// the union of a set.  Though different than standard
        /// programming usage, + reflects the common sense 
        /// notion that you can add two sets together to get the
        /// sum of the parts.
        /// </remarks>            
//        public static Tvar operator + (Tvar set1, Tvar set2)    
//        {
//            return set1 | set2;
//        }
        
        /// <summary>
        /// Returns the temporal union of two Tvars.
        /// This is equivalent to a logical OR of two sets.
        /// </summary>
        public static Tvar Union(Tvar set1, Tvar set2)
        {
            return ApplyFcnToTimeline(x => CoreTvarUnion(x), set1, set2);
        }
        private static Hval CoreTvarUnion(List<Hval> list)
        {
            List<object> s1 = (List<object>)list[0].Val;
            List<object> s2 = (List<object>)list[1].Val;
            return new Hval(s1.Union(s2).ToList());
        }

        /// <summary>
        /// Returns the temporal intersection of two Tvars.
        /// This is equivalent to a logical AND of two sets.
        /// </summary>
        public static Tvar Intersection(Tvar set1, Tvar set2)
        {
            return ApplyFcnToTimeline(x => CoreTvarIntersection(x), set1, set2);
        }
        private static Hval CoreTvarIntersection(List<Hval> list)
        {
            List<object> s1 = (List<object>)list[0].Val;
            List<object> s2 = (List<object>)list[1].Val;
            return new Hval(s1.Intersect(s2).ToList());
        }
        
        /// <summary>
        /// Returns the relative complement (set difference) of two Tvars.
        /// This is equivalent to subtracting the members of the second
        /// Tvar from those of the first (Tvar1 - Tvar2).
        /// Example: theAdults = thePeople - theKids.
        /// </summary>
        public static Tvar RelativeComplement(Tvar set1, Tvar set2)
        {
            return ApplyFcnToTimeline(x => CoreTvarRC(x), set1, set2);
        }
        private static Hval CoreTvarRC(List<Hval> list)
        {
            List<object> s1 = (List<object>)list[0].Val;
            List<object> s2 = (List<object>)list[1].Val;
            return new Hval(s1.Except(s2).ToList());
        }

		/// <summary>
		/// Eliminates redundant intervals in the Tset. 
		/// </summary>
		public Tvar LeanTset
		{
			get
			{
				Tvar n = this;

				// Identify redundant intervals
				List<DateTime> dupes = new List<DateTime>();

				if (TimeLine.Count > 0)
				{
					for (int i=0; i < TimeLine.Count-1; i++ ) 
					{
						if (AreEquivalentSets((List<object>)TimeLine.Values[i+1].Val,(List<object>)TimeLine.Values[i].Val))
						{
							dupes.Add(TimeLine.Keys[i+1]);
						}
					}
				}

				// Remove redundant intervals
				foreach (DateTime d in dupes) TimeLine.Remove(d);

				return n;
			}
		}

        /// <summary>
        /// Returns true when two sets are equal (have the same members). 
        /// </summary>
        public static Tvar AreEquivalentSets(Tvar set1, Tvar set2)    
        {
            return set1.IsSubsetOf(set2) && set2.IsSubsetOf(set1);
        }

        /// <summary>
        /// Reverses the order of the members of a Tvar. 
        /// </summary>
        public Tvar Reverse
        {
            get
            {
                return ApplyFcnToTimeline(x => CoreReverse(x), this);
            }
        }
        private static Hval CoreReverse(Hval h)
        {
            List<object> list = (List<object>)h.Val;

			// Can't use list.Reverse() because it has side effects
			List<object> resultList = new List<object>();
			for (int i=list.Count-1; i>=0; i--) resultList.Add(list[i]);

			return new Hval(resultList);
        }

		/// <summary>
		/// Sums the items in a Tset composed of numbers.
		/// </summary>
		public Tvar SumItems
		{
			get
			{
				return ApplyFcnToTimeline(x => CoreTvarSumItems(x), this);
			}
		}
		private static Hval CoreTvarSumItems(Hval h)
		{
			return ((List<object>)h.Val).Sum(item => Convert.ToDecimal(item));
		}

		/// <summary>
		/// Finds the maximum in a Tset composed of numbers.
		/// </summary>
		public Tvar MaxItem
		{
			get
			{
				return ApplyFcnToTimeline(x => CoreTvarMaxItem(x), this);
			}
		}
		private static Hval CoreTvarMaxItem(Hval h)
		{
			return ((List<object>)h.Val).Max(item => Convert.ToDecimal(item));
		}

		/// <summary>
		/// Finds the minimum in a Tset composed of numbers.
		/// </summary>
		public Tvar MinItem
		{
			get
			{
				return ApplyFcnToTimeline(x => CoreTvarMinItem(x), this);
			}
		}
		private static Hval CoreTvarMinItem(Hval h)
		{
			return ((List<object>)h.Val).Min(item => Convert.ToDecimal(item));
		}

		/// <summary>
		/// Returns the first item in the set, in each time interval. 
		/// </summary>
		public Tvar First
		{
			get
			{
				return ApplyFcnToTimeline(x => CoreTsetFirst(x), this);
			}
		}
		private static Hval CoreTsetFirst(Hval h)
		{
			List<object> theSet = (List<object>)h.Val;
			return new Hval(theSet[0]);
		}

		/// <summary>
		/// Returns every item in the set except the first item, in each time interval. 
		/// </summary>
		public Tvar Rest
		{
			get
			{
				return ApplyFcnToTimeline(x => CoreTsetRest(x), this);
			}
		}
		private static Hval CoreTsetRest(Hval h)
		{
			List<object> theSet = (List<object>)h.Val;
			List<object> resultSet = new List<object>();
			for (int i=0; i<theSet.Count; i++)
			{
				if (i > 0) resultSet.Add(theSet[i]);
			}
			return new Hval(resultSet);
		}

		/// <summary>
		/// Creates a sequential list of integers (a set) from a starting value to an ending value.
		/// </summary>
		public static Tvar Seq(Tvar first, Tvar last)
		{
			return ApplyFcnToTimeline(x => CoreSeq(x), first, last);
		}
		private static Hval CoreSeq(List<Hval> list)
		{
			int fir = Convert.ToInt32(list[0].Val);
			int las = Convert.ToInt32(list[1].Val);

			List<object> resultList = new List<object>();
			for (int i=fir; i<=las; i++)
			{
				resultList.Add(i);
			}

			return new Hval(resultList);
		}
    }

    #pragma warning restore 660, 661
}