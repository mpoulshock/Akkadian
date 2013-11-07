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
    /// <summary>
    /// An object that represents DateTime values along a timeline.
    /// </summary>
    public partial class Tvar
    {
        /// <summary>
        /// Constructs an unknown Tvar. 
        /// </summary>

        public Tvar(int n)
        {
            this.SetEternally(n);
        }

        public Tvar(decimal n)
        {
            this.SetEternally(n);
        }

        public Tvar(double n)
        {
            this.SetEternally(n);
        }


        /// <summary>
        /// Implicitly converts ints to Tvars.
        /// </summary>
        public static implicit operator Tvar(int i) 
        {
            return new Tvar(i);
        }
        
        /// <summary>
        /// Implicitly converts decimals to Tvars.
        /// </summary>
        public static implicit operator Tvar(decimal d) 
        {
            return new Tvar(d);
        }
        
        /// <summary>
        /// Implicitly converts doubles to Tvars.
        /// </summary>
        public static implicit operator Tvar(double d) 
        {
            return new Tvar(d);
        }
        
        
        /// <summary>
        /// Converts a Tvar to a nullable integer.
        /// Returns null if the Tvar is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        public int? ToInt
        {
            get
            {
                if (TimeLine.Count > 1) return null;

                if (!this.FirstValue.IsKnown) return null;

                return (Convert.ToInt32(this.FirstValue.Val));
            }
        }

        /// <summary>
        /// Converts a Tvar to an integer.  Should only be used when it is
        /// not possible for the value to be unknown.
        /// </summary>
        public int ToHardInt
        {
            get
            {
                return Convert.ToInt32(this.ToInt);
            }
        }

        /// <summary>
        /// Converts a Tvar to a nullable decimal.
        /// Returns null if the Tvar is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        public decimal? ToDecimal
        {
            get
            {
                if (TimeLine.Count > 1) { return null; }

                if (!this.FirstValue.IsKnown) return null;

                return (Convert.ToDecimal(this.FirstValue.Val));
            }
        }

        /// <summary>
        /// Converts a Tvar to an decimal.  Should only be used when it is
        /// not possible for the value to be unknown.
        /// </summary>
        public decimal ToHardDecimal
        {
            get
            {
                return Convert.ToDecimal(this.ToDecimal);
            }
        }

        /// <summary>
        /// Converts a Tvar value in days to the equivalent (fractional) years.
        /// </summary>
        public Tvar DaysToYears
        {
            get
            {
                return this / Time.DaysPerYear;
            }
        }

        /// <summary>
        /// Converts a Tvar value in days to the equivalent (fractional) months.
        /// </summary>
        public Tvar DaysToMonths
        {
            get
            {
                return this / Time.DaysPerMonth;
            }
        }

        /// <summary>
        /// Converts a Tvar value in days to the equivalent (fractional) weeks.
        /// </summary>
        public Tvar DaysToWeeks
        {
            get
            {
                return this / 7;
            }
        }

        // TODO: Max(startDate,endDate), Min(startDate,endDate)
        
        /// <summary>
        /// Returns the all-time maximum value of the Tvar. 
        /// </summary>
        public Tvar Max()
        {
            // Deal with unknowns
            Hstate state = PrecedenceForMissingTimePeriods(this);
            if (state != Hstate.Known) { return new Tvar(state); }

            // Determine the maximum value
            decimal max = Convert.ToDecimal(this.FirstValue.Val);
            foreach(Hval s in TimeLine.Values)
            {
                if (Convert.ToDecimal(s.Val) > max)
                {
                    max = Convert.ToDecimal(s.Val);
                }
            }
            return new Tvar(max);
        }
		
        /// <summary>
        /// Returns the all-time minimum value of the Tvar. 
        /// </summary>
        public Tvar Min() 
        {     
            // Deal with unknowns
            Hstate state = PrecedenceForMissingTimePeriods(this);
            if (state != Hstate.Known) { return new Tvar(state); }

            // Determine the maximum value
            decimal min = Convert.ToDecimal(this.FirstValue.Val);
            foreach(Hval s in TimeLine.Values)
            {
                if (Convert.ToDecimal(s.Val) < min)
                {
                    min = Convert.ToDecimal(s.Val);
                }
            }
            return new Tvar(min);
        }

        /// <summary>
        /// Converts a Tvar to a Tvar formatted as U.S. dollars.
        /// </summary>
        public Tvar ToUSD
        {
            get
            {
                return ApplyFcnToTimeline(x => CoreToUSD(x), this);
            }
        }
        private static Hval CoreToUSD(Hval h)
        {
            return String.Format("{0:C}" ,Convert.ToDecimal(h.Val));
        }

        /// <summary>
        /// Converts Uncertain and Stub time periods to a given value.
        /// </summary>
        /// <remarks>
        /// Used to rid a Tvar of uncertainty.  Note that it does not convert Unstated 
        /// periods because doing so would break the backward chaining interview.
        /// </remarks>
        public Tvar NormalizedTo(decimal val)
        {
            Tvar result = new Tvar();

            foreach (KeyValuePair<DateTime,Hval> slice in this.IntervalValues)
            {
                Hval theVal = slice.Value;
                
                if (theVal.IsUncertain || theVal.IsStub)
                {
                    result.AddState(slice.Key, val);
                }
                else
                {
                    result.AddState(slice.Key, slice.Value);
                }

            }
            
            return result.Lean;
        }
    }
}