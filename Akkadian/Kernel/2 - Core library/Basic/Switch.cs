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

namespace Akkadian
{
    public partial class H
    {		
        /// <summary>
        /// Returns a Tvar when its associated Tvar is true.  
        /// </summary>
        /// <remarks>
        /// Similar in principle to a C# switch statement, just temporal.
        /// Sample usage: Switch(Tvar1, Tvar1, Tvar2, Tvar2, ..., defaultTvar).  
        /// Returns Tvar1 if Tvar2 is true, else Tvar2 if Tvar2 is true, etc., else defaultTvar. 
        /// </remarks>
        public static Tvar Switch(params Func<Tvar>[] arguments) 
        {
            // Default result
            Hval h = new Hval(null, Hstate.Null);
            Tvar result = new Tvar(h);

            // Analyze each condition-value pair...and keep going
            // until all intervals of the result Tvar are defined...
            int len = (int)arguments.Length;
            for (int arg=0; arg < len-1; arg+=2)
            {
                // Get value of the condition
				Tvar newCondition = new Tvar(arguments[arg].Invoke());

                // Identify the intervals when the new condition is neither false nor true
                // Falsehood causes it to fall through to next condition. Truth causes the
                // result to assume the value during that interval.
                Tvar newConditionIsUnknown = Util.HasUnknownState(newCondition);

                // Merge these 'unknown' intervals in new condition into the result.
                result = Util.MergeTvars(result, Util.ConditionalAssignment(newConditionIsUnknown, newCondition));

                // Identify the intervals when the new condition is true.
                // Ignore irrelevant periods when result is already determined.
                // During these intervals, "result" takes on the value of its conclusion.
                Tvar newConditionIsTrueAndResultIsNull = newCondition && Util.IsNull(result);

                // If new true segments are found, accumulate the values during those intervals
                if (newConditionIsTrueAndResultIsNull.IsEverTrue())
                {
                    Tvar val = new Tvar(arguments[arg+1].Invoke());
                    result = Util.MergeTvars(result, Util.ConditionalAssignment(newConditionIsTrueAndResultIsNull, val)); 
                }

                if (!Util.HasUndefinedIntervals(result))
                {
                    return result.Lean;
                }

            }

            Tvar defaultVal = new Tvar(arguments[len-1].Invoke());
            result = Util.MergeTvars(result, defaultVal);

            return result.Lean;
        }
    }

    /// <summary>
    /// Class of utility functions for analyzing and manipulating Tvars.
    /// </summary>
    public partial class Util
    {
        /// <summary>
        /// When a Tvar (tb) is true, get the value of one Tvar (val) and assign it to 
        /// a second Tvar (result).
        /// </summary>
        /// <remarks>
        /// Example:        tb = <--F--|--T--|--F--|--T--|--F--> 
        ///                val = <--------------4--------------> 
        ///         CA(tb,val) = <--n--|--4--|--n--|--4--|--n-->  where n = Hstate.Null
        /// </remarks>
        public static Tvar ConditionalAssignment(Tvar tb, Tvar val) 
        {
            Tvar result = new Tvar();

            foreach (KeyValuePair<DateTime,List<Hval>> pair in H.TimePointValues(tb,val))
            {
                if (pair.Value[0].IsTrue)
                {
                    result.AddState(pair.Key, pair.Value[1]);
                }
                else
                {
                    result.AddState(pair.Key, new Hval(null,Hstate.Null));
                }
            }

            return result.Lean;
        }

        /// <summary>
        /// Merges the values from two Tvars. If intervals are in conflict, 
        /// the first Tvar takes priority.
        /// </summary>
        /// <remarks>
        /// Example:       tv1 = <--0--|--1--|--n--|--3--|--n--> 
        ///                tv2 = <--9--|--n--|--2--|--n--|--n--> 
        ///     Merge(tv1,tv2) = <--0--|--1--|--2--|--3--|--n-->  where n = Hstate.Null
        /// </remarks>
        public static Tvar MergeTvars(Tvar tv1, Tvar tv2)
        {
            Tvar result = new Tvar();

            foreach (KeyValuePair<DateTime,List<Hval>> pair in H.TimePointValues(tv1,tv2))
            {
                if (pair.Value[0].State != Hstate.Null)
                {
                    result.AddState(pair.Key, pair.Value[0]);
                }
                else if (pair.Value[1].State != Hstate.Null)
                {
                    result.AddState(pair.Key, pair.Value[1]);
                }
                else
                {
                    result.AddState(pair.Key, new Hval(null,Hstate.Null));
                }
            }

            return result.Lean;
        }

        /// <summary>
        /// Identifies intervals where a Tvar is Stub, Uncertain, or Unstated.
        /// </summary>
        public static Tvar HasUnknownState(Tvar tvar) 
        {
            Tvar result = new Tvar();
            
            foreach (KeyValuePair<DateTime,Hval> slice in tvar.IntervalValues)
            {
                result.AddState(slice.Key, slice.Value.IsStub || slice.Value.IsUncertain || slice.Value.IsUnstated);
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Identifies intervals where a Tvar is Hstate.Null.
        /// </summary>
        public static Tvar IsNull(Tvar tvar) 
        {
            Tvar result = new Tvar();
            
            foreach (KeyValuePair<DateTime,Hval> slice in tvar.IntervalValues)
            {
                result.AddState(slice.Key, slice.Value.State == Hstate.Null);
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Determines whether a Tvar ever has any undefined intervals (where Hstate = Null).
        /// </summary>
        public static bool HasUndefinedIntervals(Tvar tvar) 
        {
            foreach (Hval h in tvar.IntervalValues.Values)
            {
                if (h.State == Hstate.Null) return true;
            }
            return false;
        }
    }
}