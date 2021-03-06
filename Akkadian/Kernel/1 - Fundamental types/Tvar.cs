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
using System.Text;

namespace Akkadian
{    
    /// <summary>
    /// The (abstract) base class of all temporal variables (Tvars).
    /// Ordinarily, the functions in this class should only be called by core
    /// functions, not law-related ones.
    /// </summary>
	public partial class Tvar : H
    {
        /// <summary>
        /// The core Tvar data structure: a timeline of dates and associated values.
        /// </summary>
        protected SortedList<DateTime, Hval> TimeLine = new SortedList<DateTime, Hval>();
        
        /// <summary>
        /// The accessor for TimeLine.
        /// </summary>
        public SortedList<DateTime, Hval> IntervalValues
        {
            get
            {
                return TimeLine;
            }
        }

		/// <summary>
		/// Instantiate a Tvar object
		/// </summary>
		public Tvar()
		{
		}

		public Tvar(Hstate state)
		{
			this.SetEternally(state);
		}

		public Tvar(Hval v)
		{
			this.SetEternally(v);
		}

        /// <summary>
        /// Adds an time-value state to the TimeLine. 
        /// </summary>
        public void AddState(DateTime dt, Hval hval)
        {
            TimeLine.Add(dt, hval);
        }
        
        /// <summary>
        /// Removes redundant intervals from the Tvar. 
        /// </summary>
		public Tvar Lean
		{
			get
			{
				Tvar result = new Tvar();
				result.AddState(TimeLine.Keys[0], TimeLine.Values[0]);

				if (TimeLine.Count > 0)
				{
					for (int i=0; i < TimeLine.Count-1; i++ ) 
					{
						if (!object.Equals(TimeLine.Values[i].Val, TimeLine.Values[i+1].Val))
						{
							result.AddState(TimeLine.Keys[i+1], TimeLine.Values[i+1]);
						}
					}
				}

				return result;
			}
		}  

		/// <summary>
		/// If Tvar is an eternal value that is itself a Tvar, return that value.
		/// Otherwise, return the whole Tvar.
		/// </summary>
		private Tvar DeVerticalize
		{
			get
			{
				if (this.IsEternal && this.FirstValue.Val.GetType() == (new Tvar().GetType()))
				{
					return (Tvar)this.FirstValue.Val;
				}

				return this;
			}
		}

        /// <summary>
        /// Sets a Tvar to an "eternal" value (the same at all points in time). 
        /// </summary>
        public void SetEternally(Hval hval)
        {
            AddState(Time.DawnOf, hval);
        }

        /// <summary>
        /// Displays an output object.
        /// </summary>
        public object Out
        {
            get 
            {  
                if (this.TimeLine.Count == 1)
                {
                    Hval v = this.FirstValue;
                    if (v.IsKnown)
                    {
                        if (v.IsSet())   return v.ToSerializedSet();

                        return v.Obj;
                    }
                    else
                    {
                        return v.ToString;
                    }
                }
                else
                {
                    string result = "{";
                    
                    foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
                    {
						string date = de.Key.ToString("yyyy-MM-dd").Replace("1900-01-01", "Dawn");
                        date = date.Replace(" 12:00:00 AM", "");
						string val = de.Value.ToString.Replace("true","True").Replace("false","False");
						result += date + ": " + val + ", ";
                    }
					return result.TrimEnd(' ', ',') + "}";
                }
            }
        }


		/// <summary>
		/// Represents a Tvar as a string.
		/// </summary>
		public override string ToString()
		{
			if (this.TimeLine.Count == 1)
			{
				Hval v = this.FirstValue;
				if (v.IsKnown)
				{
					return FormatTSValue(v);
				}
				else
				{
					return v.ToString;
				}
			}

			StringBuilder sb = new StringBuilder();
			sb.Append("{");

			bool addLineBreaks = this.TimeLine.Count > 3;

			foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
			{
				string date = Util.FormatDate(de.Key);
				string val = FormatTSValue(de.Value);
				sb.Append(date + ": " + val + ", ");

				if (addLineBreaks) sb.Append("\r\n   ");
			}

			string result = Convert.ToString(sb);
			result = result.TrimEnd(' ', ',') + "}";
			if (addLineBreaks) result = result.Replace(", \r\n}","}");
			return result;
		}

		/// <summary>
		/// Formats the value component of the date-value pair in each time series interval.
		/// </summary>
		private static string FormatTSValue(Hval v)
		{
			if (v.IsSet())   return v.ToSerializedSet();

			string d = Convert.ToString(v.Val);

			// Numbers
			Decimal temp2;
			if (decimal.TryParse(d, out temp2)) return temp2.ToString("G29");  // Removes trailing zeros from decimal

			// Dates
			DateTime temp;
			if (DateTime.TryParse(d, out temp)) return Util.FormatDate(Convert.ToDateTime(d));

			// Uncertain states
			if (v.State != Hstate.Known) return v.ToString;

			return v.Obj.ToString().Replace("true","True").Replace("false","False");
		}

        /// <summary>
        /// Returns the value of the Tvar at the first time interval.
        /// </summary>
        public Hval FirstValue
        {
            get
            {
                return this.TimeLine.Values[0];
            }
        }

        /// <summary>
        /// Indicates whether the Tvar has the same value at all time intervals.
        /// </summary>
        public bool IsEternal
        {
            get
            {
                return this.TimeLine.Count == 1;
            }
        }

        /// <summary>
        /// Indicates whether the Tvar is eternally unknown.
        /// </summary>
        public bool IsEternallyUnknown
        {
            get
            {
                return this.IsEternal &&
                        (this.FirstValue.IsStub ||
                         this.FirstValue.IsUncertain ||
                         this.FirstValue.IsUnstated);
            }
        }

        /// <summary>
        /// Indicates whether the Tvar is eternally uncertain.
        /// </summary>
        public bool IsEternallyUncertain
        {
            get
            {
                return this.IsEternal && this.FirstValue.IsUncertain;
            }
        }

        /// <summary>
        /// Indicates whether the Tvar is eternally unstated.
        /// </summary>
        public bool IsEternallyUnstated
        {
            get
            {
                return this.IsEternal && this.FirstValue.IsUnstated;
            }
        }

        /// <summary>
        /// Returns the value of a Tvar at a specified point in time. 
        /// </summary>
        /// <remarks>
        /// If the Tvar varies over time, only the first value is used.
        /// </remarks>
		public Tvar AsOf(Tvar date)
		{
			Hval result;

			// If base Tvar has eternal, known value, return that.
			// (In these cases, the Tvar is irrelevant.)
			if (this.IsEternal && !this.IsEternallyUnknown)
			{
				result = this.FirstValue;
			}
			// If either the base Tvar or Tvar are unknown...
			else if (!date.FirstValue.IsKnown || this.IsEternallyUnknown) 
			{
				Hstate top = PrecedingState(this.FirstValue, date.FirstValue);
				result = new Hval(null,top);
			}
			else
			{
				result = this.ObjectAsOf(date.ToDateTime);
			}

			return new Tvar(result);
		}

        /// <summary>
        /// Returns an object value of the Tvar at a specified point in time.
        /// </summary>
        public Hval ObjectAsOf(DateTime dt)
        {
            for (int i = 0; i < TimeLine.Count-1; i++ ) 
            {
                // If value is between two adjacent points on the timeline...
                if (dt >= TimeLine.Keys[i])
                {
                    if (dt < TimeLine.Keys[i+1])
                    {
                        return TimeLine.Values[i];
                    }
                }
            }

            return TimeLine.Values[TimeLine.Count-1];
        }

		/// <summary>
		/// Extracts a window from the time series between two given dates.
		/// </summary>
		public Tvar Window(Tvar start, Tvar end)
		{
			Hstate top = H.PrecedingState(start.FirstValue, end.FirstValue);
			if (top != Hstate.Known)
			{
				return new Tvar(new Hval(null, top));
			}

			DateTime startDT = Convert.ToDateTime(start.FirstValue.Val);

			Tvar result = new Tvar();
			result.AddState(startDT,this.ObjectAsOf(startDT));

			// Add the states within the time window
			for (int i = 0; i < this.TimeLine.Count-1; i++ ) 
			{
				// If value is between two adjacent points on the timeline...
				if (TimeLine.Keys[i] > start && TimeLine.Keys[i] <= end)
				{
					result.AddState(TimeLine.Keys[i], TimeLine.Values[i]);
				}
			}

			return result;
		}

        /// <summary>
        /// Gets all points in time at which value of the Tvar changes.
        /// </summary>
        public IList<DateTime> TimePoints()
        {
            return (IList<DateTime>)TimeLine.Keys;
        }

		/// <summary>
		/// Returns true whenever the Tvar has a value of Stub.
		/// </summary>
		public Tvar IsStub()
		{
			Tvar result = new Tvar();

			for (int i = 0; i < TimeLine.Count; i++ ) 
			{
				result.AddState(TimeLine.Keys[i], TimeLine.Values[i].IsStub);
			}

			return result.Lean;
		}

		/// <summary>
		/// Returns true whenever the Tvar has a value of Uncertain.
		/// </summary>
		public Tvar IsUncertain()
		{
			Tvar result = new Tvar();

			for (int i = 0; i < TimeLine.Count; i++ ) 
			{
				result.AddState(TimeLine.Keys[i], TimeLine.Values[i].IsUncertain);
			}

			return result.Lean;
		}

		/// <summary>
		/// Returns true whenever the Tvar has a value of Unstated.
		/// </summary>
		public Tvar IsUnstated()
		{
			Tvar result = new Tvar();

			for (int i = 0; i < TimeLine.Count; i++ ) 
			{
				result.AddState(TimeLine.Keys[i], TimeLine.Values[i].IsUnstated);
			}

			return result.Lean;
		}

        /// <summary>
        /// Hstate precedences for missing time periods.
        /// </summary>
        public static Hstate PrecedenceForMissingTimePeriods(Tvar t)
        {
            // If the base Tvar is ever unstated, return Unstated
            // b/c user could provide answer that resolves the question
            foreach (Hval h in t.TimeLine.Values)
            {
                if (h.IsUnstated) return Hstate.Unstated;
            }

            // If the base Tvar is ever uncertain, return uncertain
            // b/c if user changes answer, this function might 
            // resolve the question
            foreach (Hval h in t.TimeLine.Values)
            {
                if (h.IsUncertain) return Hstate.Uncertain;
            }

            // If the base Tvar is ever uncertain, return uncertain
            // b/c if the rule logic were complete it might resolve
            // the question.
            foreach (Hval h in t.TimeLine.Values)
            {
                if (h.IsStub) return Hstate.Stub;
            }

            return Hstate.Known;
        }

        /// <summary>
        /// Returns a Tvar in which the values are shifted in time relative to
        /// the dates. A negative offset gets values from the past; a positive one
        /// gets them from the future.
        /// </summary>
        /// <remarks>
        /// Used, for example, to get the value of a Tvar during a prior or future
        /// time period.
        /// Note: Time points on both the base Tvar and the temporalPeriod Tvar 
        /// must line up in order for the method to work properly.
        /// </remarks>
        /// <example>
        ///                 N =  <--33--|--44--|--55--|--66--|--77-->
        ///              Year =  <-2010-|-2011-|-2012-|-2013-|-2014->
        ///  N.Shift(-2,Year) =  <---------33---------|--44--|--55--|--66--|--77-->
        /// </example>
		public Tvar Shift(Tvar offset, Tvar temporalPeriod)
		{
			Tvar result = new Tvar();

			// Handle uncertainty in offset
			// No need to handle uncertainty b/c this method just reuses the values in
			// the base Tvar.
			Hstate top1 = PrecedingState(offset.FirstValue);
			if (top1 != Hstate.Known)
			{
				result.AddState(Time.DawnOf, new Hval(null, top1));
				return result;
			}

			// Set initial state of Tvar
			result.AddState(this.TimeLine.Keys[0], this.TimeLine.Values[0]);

			// Iterate through pairs in the base Tvar
			foreach(KeyValuePair<DateTime,Hval> de in this.TimeLine)
			{
				// Extract parts of the date-value pair
				DateTime origDate = Convert.ToDateTime(de.Key);
				Hval val = de.Value;
				DateTime offsetDate = Time.EndOf;

				// Leave the value at Time.DawnOf alone
				if (origDate != Time.DawnOf)
				{
					// Get the time point with the appropriate offset from the current time point
					// First, look up the original date in temporalPeriod
					for (int i=0; i<temporalPeriod.TimeLine.Values.Count; i++)
					{
						DateTime testDate = Convert.ToDateTime(temporalPeriod.TimeLine.Keys[i]);
						if (testDate == origDate)
						{
							// Then get the date offset from the original date
							int offsetInt = Convert.ToInt16(offset.FirstValue.Val);
							int offsetIndex = i + (offsetInt * -1);

							// Don't overrun the temporalPeriod list
							if (offsetIndex < temporalPeriod.TimeLine.Count &&
							    offsetIndex >= 0)
							{
								offsetDate = temporalPeriod.TimeLine.Keys[offsetIndex];

								// Prevent overflowing the bounds of Time
								if (offsetDate < Time.EndOf) 
								{
									result.AddState(offsetDate, val);
								}
								break;
							}
						}
					}
				}
			}

			return result.Lean;		// TODO: Implement LeanTset for Tsets
		}

        /// <summary>
        /// Given a base Tvar and a period, returns a Tvar where the value of each period
        /// is the value of the base Tvar at the end of each period.
        /// </summary>
        /// <remarks>
        /// Example: marital status for tax purposes is the status as of the last day
        /// of the tax year.
        /// </remarks>
        /// <example>
        ///              Tvar =  <------1---|-----------2----------->
        ///              Year =  <-2010-|-2011-|-2012-|-2013-|-2014->
        ///    Tvar.PEV(Year) =  <--1---|----------2---------------->
        /// </example>            
		public Tvar PeriodEndVal(Tvar temporalPeriod)
		{
			Tvar result = new Tvar();
			result.AddState(this.TimeLine.Keys[0], this.TimeLine.Values[0]);

			// No need to handle uncertainty b/c this method just reuses the values in
			// the base Tvar.

			// Iterate backwards through the timeline of the base Tvar
			for(int i=this.TimeLine.Count-1; i>0; i--)
			{
				// If the date is not lined up with a time point in temporalPeriod
				DateTime theDate = Convert.ToDateTime(this.TimeLine.Keys[i]);
				if (temporalPeriod.TimeLine.Keys.Contains(theDate))
				{
					// If result does not already contain the date, add it to result
					if (!result.TimeLine.Keys.Contains(theDate))
					{
						result.AddState(theDate, this.TimeLine.Values[i]);
					}
				}
				else
				{
					// Get the date in temporalPeriod that is immediately prior to theDate
					// (i.e. the beginning of that period)
					for (int j=temporalPeriod.TimeLine.Count-1; j>0; j--)
					{
						DateTime periodDate = temporalPeriod.TimeLine.Keys[j];
						if (periodDate < theDate)
						{
							// If result does not already contain the new change date, add it to result
							if (!result.TimeLine.Keys.Contains(periodDate))
							{
								result.AddState(periodDate, this.TimeLine.Values[i]);
							}

							break;
						}
					}
				}
			}

			return result;
		}
    }    
}