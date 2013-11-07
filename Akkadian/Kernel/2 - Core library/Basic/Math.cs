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
    
    public partial class Tvar
    {        
        /// <summary>
        /// Adds two Tvars together.
        /// </summary>
        public static Tvar operator + (Tvar tn1, Tvar tn2)    
        {
            return ApplyFcnToTimeline(x => CoreSum(x), tn1, tn2);
        }
        private static Hval CoreSum(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) + Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Subtracts one Tvar from another.
        /// </summary>
        public static Tvar operator - (Tvar tn1, Tvar tn2)    
        {
            return ApplyFcnToTimeline(x => Minus(x), tn1, tn2);
        }
        private static Hval Minus(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) - Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Multiplies two Tvars together.
        /// </summary>
        public static Tvar operator * (Tvar tn1, Tvar tn2)    
        {
            Tvar result = new Tvar();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {    
                Hstate top = PrecedingState(slice.Value);
                decimal val1 = Convert.ToDecimal(slice.Value [0].Val);
                decimal val2 = Convert.ToDecimal(slice.Value [1].Val);

                // Short circuit 1
                if (val1 == 0 || val2 == 0)
                {
                    result.AddState(slice.Key, new Hval(0));
                }
                else if (top != Hstate.Known)      // Short circuit 2
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else                               // Do the math
                {
                    decimal prod = val1 * val2;
                    result.AddState(slice.Key, new Hval(prod));
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Divides one Tvar by another. 
        /// </summary>
        public static Tvar operator / (Tvar tn1, Tvar tn2)    
        {
            Tvar result = new Tvar();
            
            foreach(KeyValuePair<DateTime,List<Hval>> slice in TimePointValues(tn1,tn2))
            {    
                Hstate top = PrecedingState(slice.Value);
                decimal denominator = Convert.ToDecimal(slice.Value[1].Val);

                if (denominator == 0)   // Short circuit 1: Div-by-zero
                {
                    result.AddState(slice.Key, new Hval(null));
                }
                else if (top != Hstate.Known)                     // Short circuit 2: Hstates
                {
                    result.AddState(slice.Key, new Hval(null,top));
                }
                else                                              // Do the math
                {
                    decimal r = Convert.ToDecimal(slice.Value[0].Val) / denominator;
                    result.AddState(slice.Key, new Hval(r));
                }
            }
            
            return result.Lean;
        }

        /// <summary>
        /// Temporal modulo function. 
        /// </summary>
        public static Tvar operator % (Tvar tn1, Tvar tn2)    
        {
            return ApplyFcnToTimeline(x => Mod(x), tn1, tn2);
        }
        private static Hval Mod(List<Hval> list)
        {
            return Convert.ToDecimal(list[0].Val) % Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Rounds a value to the nearest multiple of a given number.  By default, 
        /// it rounds up in case of a tie.
        /// </summary>
        public Tvar RoundToNearest(Tvar multiple)
        {
            return RoundToNearest(multiple,false);
        }

        public Tvar RoundToNearest(Tvar multiple, Tvar breakTieByRoundingDown)
        {
            Tvar diff = this % multiple;
            return Switch(() => diff > multiple / 2, () => this - diff + multiple,
                                ()=> diff < multiple / 2 || breakTieByRoundingDown, ()=> this - diff,
                                () => true, () => this - diff + multiple);
        }

        /// <summary>
        /// Rounds a value up to the next multiple of a given number.
        /// </summary>
        public Tvar RoundUp(Tvar multiple)
        {
            return Switch(() => this % multiple != 0, () => this - (this % multiple) + multiple,
                                () => true, () => this);
        }        
        
        /// <summary>
        /// Rounds a value down to the next multiple of a given number.
        /// </summary>
        public Tvar RoundDown(Tvar multiple)
        {
            return Switch(() => this % multiple != 0, () => this - (this % multiple),
                                () => true, () => this);
        }
    }

    public partial class H
    {   
        /// <summary>
        /// Temporal absolute value function
        /// </summary>
        public static Tvar Abs(Tvar tn)
        {
            return ApplyFcnToTimeline(x => CoreAbs(x), tn);
        }
        private static Hval CoreAbs(Hval h)
        {
            return Math.Abs(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// A raised to the B power
        /// </summary>
        public static Tvar Pow(Tvar tnA, Tvar tnB)    
        {
            return ApplyFcnToTimeline(x => CorePow(x), tnA, tnB);
        }
        private static Hval CorePow(List<Hval> list)
        {
            return Math.Pow(Convert.ToDouble(list[0].Val), Convert.ToDouble(list[1].Val));
        }

        /// <summary>
        /// Square root
        /// </summary>
        public static Tvar Sqrt(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreSqrt(x), tn);
        }
        private static Hval CoreSqrt(Hval h)
        {
            return Math.Sqrt(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// Natural logarithm
        /// </summary>
        public static Tvar Log(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreNatLog(x), tn);
        }
        private static Hval CoreNatLog(Hval h)
        {
            return Math.Log(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// Logarithm of n to base b
        /// </summary>
        public static Tvar Log(Tvar b, Tvar n)    
        {
            return ApplyFcnToTimeline(x => CoreLog(x), b, n);
        }
        private static Hval CoreLog(List<Hval> list)
        {
            return Math.Log(Convert.ToDouble(list[1].Val), Convert.ToDouble(list[0].Val));
        }

        /// <summary>
        /// Sine
        /// </summary>
        public static Tvar Sin(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreSin(x), tn);
        }
        private static Hval CoreSin(Hval h)
        {
            return Math.Sin(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// Cosine
        /// </summary>
        public static Tvar Cos(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreCos(x), tn);
        }
        private static Hval CoreCos(Hval h)
        {
            return Math.Cos(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// Tangent
        /// </summary>
        public static Tvar Tan(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreTan(x), tn);
        }
        private static Hval CoreTan(Hval h)
        {
            return Math.Tan(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// ArcSin
        /// </summary>
        public static Tvar ArcSin(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreArcSin(x), tn);
        }
        private static Hval CoreArcSin(Hval h)
        {
            return Math.Asin(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// ArcCos
        /// </summary>
        public static Tvar ArcCos(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreArcCos(x), tn);
        }
        private static Hval CoreArcCos(Hval h)
        {
            return Math.Acos(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// ArcTan
        /// </summary>
        public static Tvar ArcTan(Tvar tn)    
        {
            return ApplyFcnToTimeline(x => CoreArcTan(x), tn);
        }
        private static Hval CoreArcTan(Hval h)
        {
            return Math.Atan(Convert.ToDouble(h.Val));
        }

        /// <summary>
        /// Constant Pi
        /// </summary>
        public static Tvar ConstPi = new Tvar(Math.PI);

        /// <summary>
        /// Constant e
        /// </summary>
        public static Tvar ConstE = new Tvar(Math.E);
    }
    
    #pragma warning restore 660, 661
}