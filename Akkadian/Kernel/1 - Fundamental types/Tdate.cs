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
    #pragma warning disable 660, 661

    /// <summary>
    /// An object that represents DateTime values along a timeline.
    /// </summary>
    public partial class Tvar
    {       
        /// <summary>
        /// Constructs a Tvar that is eternally set to a specified DateTime
        /// value.
        /// </summary>    
        public Tvar(DateTime val)
        {
            this.SetEternally(val);
        }

        /// <summary>
        /// Implicitly converts DateTimes to Tvars.
        /// </summary>
        public static implicit operator Tvar(DateTime d) 
        {
            return new Tvar(d);
        }
        
        /// <summary>
        /// Constructs a Tvar that is eternally set to a specified year, 
        /// month, and day.
        /// </summary>
        public Tvar(int year, int month, int day)
        {
            this.SetEternally(new DateTime(year, month, day));
        }
                
        /// <summary>
        /// Converts a Tvar to a nullable DateTime.
        /// Returns null if the Tvar is unknown or if it's value changes over
        /// time (that is, if it's not eternal).
        /// </summary>
        public DateTime? ToNullDateTime
        {
            get
            {
                if (TimeLine.Count > 1) { return null; }

                if (!this.FirstValue.IsKnown) return null;

                return (Convert.ToDateTime(this.FirstValue.Val));
            }
        }
        
        /// <summary>
        /// Converts a Tvar to a DateTime.  Is dangerous because it doesn't account
        /// for unknown or time-varying values.
        /// </summary>
        public DateTime ToDateTime
        {
            get
            {
                return (Convert.ToDateTime(this.FirstValue.Val));
            }
        }
               
        /// <summary>
        /// Returns true in the period during which a given date falls
        /// Example: (2013-04-15).IsInPeriod(TheYear) is true for all of 2013, and otherwise false
        /// </summary>
        public Tvar IsInPeriod(Tvar interval)
        {
            // The event + 1 tick (temporal)
            Tvar theEvent = Time.IsBetween(this, new Tvar(this.ToDateTime.AddTicks(1)));

            // True during the interval when the event occurs
            return theEvent.EverPer(interval);
        }

        /// <summary>
        /// Adds a specified number of days to each DateTime in a Tvar.
        /// Negative values are subtracted.
        /// </summary>    
        public Tvar AddDays(Tvar days)
        {
            return ApplyFcnToTimeline(x => CoreAddDays(x), this, days);
        }    
        private static Hval CoreAddDays(List<Hval> list)
        {
            return Convert.ToDateTime(list[0].Val).AddDays(Convert.ToInt32(list[1].Val));
        }

        /// <summary>
        /// Adds a specified number of months to each DateTime in a Tvar.
        /// Negative values are subtracted.
        /// </summary>
        public Tvar AddMonths(Tvar days)
        {
            return ApplyFcnToTimeline(x => CoreAddMonths(x), this, days);
        }    
        private static Hval CoreAddMonths(List<Hval> list)
        {
            return Convert.ToDateTime(list[0].Val).AddMonths(Convert.ToInt32(list[1].Val));
        }

        /// <summary>
        /// Adds a specified number of years to each DateTime in a Tvar.
        /// Negative values are subtracted.
        /// </summary>
        public Tvar AddYears(Tvar days)
        {
            return ApplyFcnToTimeline(x => CoreAddYears(x), this, days);
        }    
        private static Hval CoreAddYears(List<Hval> list)
        {
            return Convert.ToDateTime(list[0].Val).AddYears(Convert.ToInt32(list[1].Val));
        }

        /// <summary>
        /// Extracts the year from each value in a Tvar.
        /// </summary>
        public Tvar Year
        {       
            get
            {
                return ApplyFcnToTimeline(x => GetYear(x), this);
            }
        }
        private static Hval GetYear(Hval h)
        {
            return Convert.ToDateTime(h.Val).Year;
        }

        /// <summary>
        /// For a given date, determine what calendar quarter it's in.
        /// </summary>
        // TODO: Handle fiscal years that don't start in January.
        public Tvar Quarter
        {       
            get
            {
                return Switch(()=> this.Month <= 3, ()=> 1,
                                    ()=> this.Month <= 6, ()=> 2,
                                    ()=> this.Month <= 9, ()=> 3,
                                    ()=> 4); 
            }
        }

        /// <summary>
        /// Extracts the month from each value in a Tvar.
        /// </summary>
        public Tvar Month
        {       
            get
            {
                return ApplyFcnToTimeline(x => GetMonth(x), this);
            }
        }
        private static Hval GetMonth(Hval h)
        {
            return Convert.ToDateTime(h.Val).Month;
        }

        /// <summary>
        /// Extracts the day from each value in a Tvar.
        /// </summary>
        public Tvar Day
        {       
            get
            {
                return ApplyFcnToTimeline(x => GetDay(x), this);
            }
        }
        private static Hval GetDay(Hval h)
        {
            return Convert.ToDateTime(h.Val).Day;
        }

        /// <summary>
        /// Returns the number of days between the DateTimes in two Tvars.
        /// </summary>
        public static Tvar DayDifference(Tvar td1, Tvar td2)
        {
            return ApplyFcnToTimeline(x => CoreDayDiff(x), td1, td2);
        }    
        private static Hval CoreDayDiff(List<Hval> list)
        {
            DateTime earlierDate = Convert.ToDateTime(list[0].Val);
            DateTime laterDate = Convert.ToDateTime(list[1].Val);
            TimeSpan s = laterDate - earlierDate;
            return s.TotalDays;
        }

        /// <summary>
        /// Returns the number of weeks between the DateTimes in two Tvars.
        /// </summary>
        public static Tvar WeekDifference(Tvar td1, Tvar td2)
        {
            return (DayDifference(td1, td2) / 7).RoundToNearest(0.001);
        }

        // TODO: Add MonthDiff and QuarterDiff functions
        
        /// <summary>
        /// Returns the number of years between the DateTimes in two Tvars.
        /// </summary>
        public static Tvar YearDifference(Tvar td1, Tvar td2)
        {
            return ApplyFcnToTimeline(x => YearDiffRec(x), td1, td2);;
        }

        /// <summary>
        /// Recursively determines the difference in years between two dates, to three decimal places.
        /// </summary>
        private static Hval YearDiffRec(List<Hval> list)
        {
            DateTime date1 = Convert.ToDateTime(list[0].Val);
            DateTime date2 = Convert.ToDateTime(list[1].Val);

            DateTime plus1 = date1.AddYears(1);
            DateTime plus4 = date1.AddYears(4);

            // Count off 4-year periods (in case date1 is on a leap day)
            if (date2 >= plus4)
            {
                return 4 + Convert.ToDecimal(YearDiffRec(new List<Hval>(){plus4, date2}).Val);
            }
            // Full year
            else if (date2 >= plus1)
            {
                return 1 + Convert.ToDecimal(YearDiffRec(new List<Hval>(){plus1, date2}).Val);
            }
            // Partial year
            else
            {
                return Math.Round((date2 - date1).TotalDays / Time.DaysPerYear, 3);
            }
        }
    }
    
    #pragma warning restore 660, 661
}