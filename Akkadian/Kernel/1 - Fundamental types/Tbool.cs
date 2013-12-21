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
    /// An temporal object that represents boolean values along a timeline.
    /// </summary>
    public partial class Tvar
    {
        
        /// <summary>
        /// Constructs a Tbool 
        /// </summary>
        public Tvar(bool b)
        {
            this.SetEternally(b);
        }

        /// <summary>
        /// Implicitly converts bools to Tvars.
        /// </summary>
        public static implicit operator Tvar(bool b) 
        {
            return new Tvar(b);
        }

        /// <summary>
        /// Indicates whether the Tvar is always true.
        /// </summary>
        public Tvar IsAlwaysTrue()
        {
            // If timeline varies, it cannot always be the given value
            if (this.TimeLine.Count > 1) return false;

            // If val is unknown and base Tvar is eternally unknown,
            // return the typical precedence state
            if (!this.FirstValue.IsKnown)
            {
                return new Tvar(this.FirstValue);
            }

            // Else, test for equality
            if (this.FirstValue.IsEqualTo(true)) return true;

            return false;
        }

        /// <summary>
        /// Determines whether this instance is always true during a specified interval.
        /// </summary>
        public Tvar IsAlwaysTrue(Tvar start, Tvar end)
        {
            Tvar isDuringInterval = Time.IsBetween(start, end);

			Tvar isOverlap = this & isDuringInterval;

            Tvar overlapAndIntervalAreCoterminous = isOverlap == isDuringInterval;

            return !overlapAndIntervalAreCoterminous.IsEver(false);
        }

        /// <summary>
        /// Determines whether this instance is ever true during a specified time period.
        /// </summary>
        public Tvar IsEverTrue(Tvar start, Tvar end)
        {
			Tvar isOverlap = this & Time.IsBetween(start, end);
           
            return isOverlap.IsEverTrue();
        }
        
        /// <summary>
        /// Returns true if the Tvar ever has a specified boolean value. 
        /// </summary>
        public Tvar IsEverTrue()
        {
            return this.IsEver(true);
        }

        /// <summary>
        /// Determines whether the Tvar is ever the specified boolean val.
        /// </summary>
        private Tvar IsEver(Hval val)
        {
            // If val is unknown and base Tvar is eternally unknown,
            // return the typical precedence state
            if (!val.IsKnown && this.TimeLine.Count == 1)
            {
                if (!this.FirstValue.IsKnown)
                {
                    Hstate s = PrecedingState(this.FirstValue, val);
                    return new Tvar(s);
                }
            }

            // If val is unknown, return its state
            if (!val.IsKnown) return new Tvar(val);
           
            // If the base Tvar is ever val, return true
            foreach (Hval h in this.TimeLine.Values)
            {
                if (h.IsEqualTo(val)) return true;
            }

            // If base Tvar has a time period of unknownness, return 
            // the state with the proper precedence
            Hstate returnState = PrecedenceForMissingTimePeriods(this);
            if (returnState != Hstate.Known) return new Tvar(returnState);

            return false;
        }

        /// <summary>
        /// Returns the DateTime when the Tvar is first true.
        /// </summary>
        public Tvar DateFirstTrue
        {
            get
            {
                SortedList<DateTime, Hval> line = this.TimeLine;
                Hval result = new Hval(null,Hstate.Stub);
    
                // If Tval is eternally unknown, return that value
                if (this.IsEternallyUnknown)
                {
                    result = this.FirstValue;
                }
                else
                {
                    // For each time interval...
                    for (int i = 0; i < line.Count; i++) 
                    {
                        // If you encounter an unknown interval, return that value
                        // Warning: Could be problematic b/c initial intervals are likely to be unknown...
                        if (!line.Values[i].IsKnown)
                        {
                            // result = line.Values[i];
                        }
                        // Look for the date when the Tvar has the given value
                        else if (Convert.ToBoolean(line.Values[i].Val))
                        {
                            result = line.Keys[i];
                            return new Tvar(result);
                        }
                        else
                        {
                            // If Tvar never has the given value, return the default value
                        }
                    }
                }
    
                return new Tvar(result);
            }
        }

        /// <summary>
        /// Returns the DateTime when the Tvar is true for the last time
        /// </summary>
        public Tvar DateLastTrue
        {
            get
            {
                SortedList<DateTime, Hval> line = this.TimeLine;
                Hval result = new Hval(null,Hstate.Stub);
    
                // If Tval is eternally unknown, return that value
                if (this.IsEternallyUnknown)
                {
                    result = this.FirstValue;
                }
                else
                {
                    // For each time interval...
                    for (int i = 0; i < line.Count; i++) 
                    {
                        // If you encounter an unknown interval, return that value
                        // Warning: Could be problematic b/c initial intervals are likely to be unknown...
                        if (!line.Values[i].IsKnown)
                        {
                            // result = line.Values[i];
                        }
                        // Look for the date when the Tvar has the given value
                        else if (Convert.ToBoolean(line.Values[i].Val))
                        {
                            if (i >= line.Count-1)
                            {
                                result = Time.EndOf;
                            }
                            else
                            {
                                result = line.Keys[i+1].AddDays(-1);
                            }
                        }
                        else
                        {
                            // If Tvar never has the given value, return the default value
                        }
                    }
                }
    
                return new Tvar(result);
            }
        }

        /// <summary>
        /// Overloaded boolean operator: True.
        /// </summary>
        public static bool operator true (Tvar tb)
        {
            return tb.IsTrue;
        }
        
        /// <summary>
        /// Returns true only if the Tvar is true during the window of concern;
        /// otherwise false. 
        /// Used in symmetrical facts and short-circuit evaluation.
        /// </summary>
        public bool IsTrue
        {
            get
            {
                return this.IsEternal && this.FirstValue.IsTrue;

//                if (Facts.WindowOfConcernIsDefault)
//                {
//                    return this.IntervalValues.Count == 1 && 
//                           Convert.ToBoolean(this.IntervalValues.Values[0].Val) == true; 
//                }
//                if (Facts.WindowOfConcernIsPoint)
//                {
//                    return Convert.ToBoolean(this.AsOf(Facts.WindowOfConcernStart));
//                }
//                
//                return Convert.ToBoolean(this.IsAlways(true, 
//                                                       Facts.WindowOfConcernStart, 
//                                                       Facts.WindowOfConcernEnd
//                                                       ).ToBool);
            }
        }
        
        /// <summary>
        /// Overloaded boolean operator: False.
        /// </summary>
        public static bool operator false (Tvar tb)
        {
            return tb.IsFalse;
        }
        
        /// <summary>
        /// Returns true only if the Tvar is false during the window of concern;
        /// otherwise true. 
        /// </summary>
        public bool IsFalse
        {
            get
            {
                return this.IsEternal && this.FirstValue.IsFalse;

//                if (Facts.WindowOfConcernIsDefault)
//                {
//                    return this.IsEternal && this.FirstValue.IsFalse;
//                }
//                if (Facts.WindowOfConcernIsPoint)
//                {
//                    return Convert.ToBoolean((!this).AsOf(Facts.WindowOfConcernStart));
//                }
//                
//                return Convert.ToBoolean((!this).IsAlwaysTrue( 
//                                                       Facts.WindowOfConcernStart, 
//                                                       Facts.WindowOfConcernEnd
//                                                       ).ToBool);
            }
        }
        
        /// <summary>
        /// Converts a Tvar to a nullable boolean.
        /// Returns null if the Tvar is unknown, if it's value changes over
        /// time (that is, if it's not eternal), and when it's null.
        /// </summary>
        public bool? ToBool
        {
            get
            {
                if (TimeLine.Count > 1) { return null; }

                if (!this.FirstValue.IsKnown) return null;

                return (bool?)this.FirstValue.Val;
            }
        }

        /// <summary>
        /// Equality when the Tvars are known to be Tbools.
        /// </summary>
        public static Tvar TboolEquality(Tvar tb1, Tvar tb2)
        {
            return EqualTo(tb1,tb2);
        }

        /// <summary>
        /// Given a Tvar and an index date, returns the date of the next change date of the Tvar.
        /// </summary>
        public DateTime NextChangeDate(DateTime indexDate)
        {
            for (int j=0; j < this.IntervalValues.Count; j++)
            {
                if (indexDate <= this.IntervalValues.Keys[j])
                {
                    return this.IntervalValues.Keys[j];
                }
            }

            return Time.EndOf;
        }

        /// <summary>
        /// Given a Tvar and an index date, returns the date the Tvar is next true.
        /// </summary>
        public DateTime DateNextTrue(DateTime indexDate)
        {
            for (int j=0; j < this.IntervalValues.Count; j++)
            {
                if (indexDate <= this.IntervalValues.Keys[j] && Convert.ToBoolean(this.IntervalValues.Values[j].Val) == true)
                {
                    return this.IntervalValues.Keys[j];
                }
            }

            return Time.EndOf;
        }
    }
    
    #pragma warning restore 660, 661
}