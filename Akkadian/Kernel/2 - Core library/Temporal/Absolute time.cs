// Copyright (c) 2010-2013 Hammura.bi LLC
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

namespace Akkadian
{
    public partial class H
    {
		/// <summary>
		/// Creates a time series where the value at each date is the date itself.
		/// </summary>
		public static Tvar TheDate
		{
			get
			{
				DateTime index = Time.DawnOf;
				Tvar d = new Tvar(index);

				while (index < Time.EndOf) 
				{
					index = index.AddDays(1);
					d.AddState(index,index);
				}

				return d;
			}
		}

        /// <summary>
        /// Returns a Tvar representing the calendar year, spanning all of time.
        /// </summary>
        public static Tvar TheYear
        {
            get
            {
                return Time.IntervalsSince(Time.DawnOf, Time.EndOf, Time.IntervalType.Year, Time.DawnOf.Year);
            }
        }
        
        /// <summary>
        /// Returns a Tvar representing the fiscal quarter (by default, a 20-year
        /// span centered on day 1 of the fiscal year that begins in current year)
        /// </summary>
        public static Tvar TheQuarter
        {
            get
            {
				return Time.Recurrence(Time.DawnOf,Time.EndOf, Time.IntervalType.Quarter,1,4);
            }
        }
        
        /// <summary>
        /// Returns a Tvar representing the calendar month (by default, a
        /// 20-year span centered on Jan. 1st of the current year)
        /// </summary>
        public static Tvar TheMonth
        {
            get
            {
				return Time.Recurrence(Time.DawnOf,Time.EndOf, Time.IntervalType.Month,1,12);
            }
        }
    }
        

    /// <summary>
	/// A construct representing absolute time. 
    /// </summary>
    public partial class Time
    {
        /// <summary>
        /// Returns the date the universe was created.
        /// (Well, you get the point.)
        /// </summary>
        public static DateTime DawnOf
        {
            get
            {
				return new DateTime(1900,1,1);
            }
        }

        /// <summary>
        /// Returns the date the universe will end.
        /// </summary>
        public static DateTime EndOf
        {
            get
            {
				return new DateTime(2100,12,31);
            }
        }

        /// <summary>
        /// Returns a Tvar that's true at and after a specified DateTime, 
        /// and otherwise false.
        /// </summary>
        public static Tvar IsAtOrAfter(Tvar dt)
        {
            // Handle unknowns
            if (!dt.FirstValue.IsKnown)
            {
                return new Tvar(dt.FirstValue);
            }

            // Create boolean
            Tvar result = new Tvar();
            if (dt == Time.DawnOf)
            {
                result.AddState(DawnOf, true);
            }
            else
            {
                result.AddState(DawnOf, false);
                result.AddState(dt.ToDateTime, true);
            }
            return result;
        }
        
        /// <summary>
        /// Returns a Tvar that's true up to a specified DateTime, and false
        /// at and after it.
        /// </summary>
        public static Tvar IsBefore(Tvar dt)
        {
            // Handle unknowns
            if (!dt.FirstValue.IsKnown)
            {
                return new Tvar(dt.FirstValue);
            }

            // Create boolean
            Tvar result = new Tvar();

            if (dt == DawnOf)
            {
                result.AddState(DawnOf, false);
            }
            else
            {
                result.AddState(DawnOf, true);
                result.AddState(dt.ToDateTime, false);
            }
            return result;
        }
        
        /// <summary>
        /// Returns a Tvar that's true during a specified time interval (including
        /// at the "start"), and otherwise false (including at the moment represented 
        /// by the "end").
        /// </summary>
        public static Tvar IsBetween(Tvar start, Tvar end)
        {
             return IsAtOrAfter(start) && IsBefore(end);
        }
    }
}
