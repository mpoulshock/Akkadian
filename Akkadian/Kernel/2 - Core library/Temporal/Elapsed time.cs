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

namespace Akkadian
{    
    public partial class Tvar
    {
        /// <summary>
        /// Returns the total number of elapsed intervals between two dates.
        /// </summary>
        public Tvar TotalElapsedIntervals(Tvar interval, Tvar start, Tvar end)
        {
            Tvar rei = RunningElapsedIntervals(interval);

            return rei.AsOf(end) - rei.AsOf(start);
        }

        /// <summary>
        /// Provides a running count of the number of whole intervals 
        /// that a Tvar has been true.
        /// </summary>
        /// <remarks>
        /// Example:
        ///         tb = <--FTFTTFF-->
        ///     tb.REI = <--0011233-->
        /// 
        /// Note: An elapsed interval is counted in the subsequent interval.
        /// </remarks>
        public Tvar RunningElapsedIntervals(Tvar interval)  
        {
            // If base Tvar is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            if (baseState != Hstate.Known) return new Tvar(baseState);

            int intervalCount = 0;
            DateTime dateNextTrue = this.DateNextTrue(Time.DawnOf);
            DateTime dateNextTrueIntervalEnds = this.NextChangeDate(dateNextTrue.AddTicks(1));

            Tvar result = new Tvar(0);

            // Iterate through the time intervals in the input Tvar
            for (int i=0; i < interval.IntervalValues.Count-1; i++)
            {
                DateTime start = interval.IntervalValues.Keys[i];
                DateTime end = interval.IntervalValues.Keys[i+1];

                // If base Tvar is always true during the interval, increment the count
                if (end <= dateNextTrueIntervalEnds)
                {
                    if (start >= dateNextTrue)
                    {
                        intervalCount++;
                        result.AddState(end, intervalCount);
                        continue;
                    }
                }
                else
                {
                    // Otherwise, skip to next true interval
                    dateNextTrue = this.DateNextTrue(end);
                    dateNextTrueIntervalEnds = this.NextChangeDate(dateNextTrue.AddTicks(1));
                }
            }

            return result;
        }

        /// <summary>
        /// Provides a running count of how many whole intervals a Tvar 
        /// has been continuously true.
        /// </summary>
        /// <remarks>
        /// Example:
        ///         tb = <--FTFTTF-->
        ///     tb.ICT = <--010120-->
        /// 
        /// Use judiciously with TheDay and TheCalendarWeek, as they have thousands of time intervals.
        /// </remarks>
        public Tvar ContinuousElapsedIntervals(Tvar interval)  
        {
            // If base Tvar is ever unknown during the time period, return 
            // the state with the proper precedence
            Hstate baseState = PrecedenceForMissingTimePeriods(this);
            if (baseState != Hstate.Known) return new Tvar(baseState);

            int intervalCount = 0;
            DateTime dateNextTrue = this.DateNextTrue(Time.DawnOf);
            DateTime dateNextTrueIntervalEnds = this.NextChangeDate(dateNextTrue.AddTicks(1));

            Tvar result = new Tvar(0);

            // Iterate through the time intervals in the input Tvar
            for (int i=0; i < interval.IntervalValues.Count-1; i++)
            {
                DateTime start = interval.IntervalValues.Keys[i];
                DateTime end = interval.IntervalValues.Keys[i+1];

                // If base Tvar is always true during the interval, increment the count
                if (end <= dateNextTrueIntervalEnds)
                {
                    if (start >= dateNextTrue)
                    {
                        intervalCount++;
                        result.AddState(end, intervalCount);
                        continue;
                    }
                }
                else
                {
                    // Otherwise, skip to next true interval
                    intervalCount = 0;
                    result.AddState(end, intervalCount);
                    dateNextTrue = this.DateNextTrue(end);
                    dateNextTrueIntervalEnds = this.NextChangeDate(dateNextTrue.AddTicks(1));
                }
            }

            return result;
        }

        /// <summary>
        /// Provides a running count of how many intervals (years, days, etc.) a Tvar 
        /// has been true within some sliding window of time.  At the end of that sliding 
        /// window, this function returns the amount of time that has elapsed within the 
        /// window during which the Tvar was true.
        /// </summary>
        /// <remarks>
        /// Example: For a given Tvar, at any given point in time, for how many days during the
        /// previous 3 days is the Tvar true?
        /// 
        ///                     tb = <FTTTFTTTFFFF>
        ///      tb.SEI(3, TheDay) = <001232223210>
        /// </remarks>
        public Tvar SlidingElapsedIntervals(Tvar interval, Tvar windowSize)
        {
            // If a Tvar is eternally true, return windowSize
            if (this.IsTrue)
            {
                return windowSize;
            }

            // The number of true intervals in a sliding window of time equals
            // the running count as of time1 minus the running count as of time0.
            Tvar r = this.RunningElapsedIntervals(interval);

            int size = Convert.ToInt32(windowSize.FirstValue.Val) * -1;

            // Counts the current inerval
            return r - r.Shift(size, interval);  
        }
    }    
}