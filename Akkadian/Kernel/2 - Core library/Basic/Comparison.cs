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
    public partial class Tvar
    {       
		/// <summary>
		/// Returns true when two Tvar values are equal. 
		/// </summary>
		public static Tvar operator == (Tvar tv1, Tvar tv2)
		{
			return EqualTo(tv1,tv2);
		}
		public static Tvar EqualTo(Tvar tv1, Tvar tv2)
		{
			return ApplyFcnToTimeline(x => Eq(x), tv1, tv2);
		}
		private static Hval Eq(List<Hval> list)
		{
			string type = list[0].Val.GetType().ToString();

			if (type == "System.Boolean") return Convert.ToBoolean(list[0].Val) == Convert.ToBoolean(list[1].Val);
			if (type == "System.DateTime") return Convert.ToDateTime(list[0].Val) == Convert.ToDateTime(list[1].Val);
			if (type == "System.String") return Convert.ToString(list[0].Val) == Convert.ToString(list[1].Val);
			return Convert.ToDecimal(list[0].Val) == Convert.ToDecimal(list[1].Val);
		}

		/// <summary>
		/// Returns true when two Tvar values are not equal. 
		/// </summary>
		public static Tvar operator != (Tvar tv1, Tvar tv2)
		{
			return NotEqualTo(tv1,tv2);
		}
		public static Tvar NotEqualTo(Tvar tv1, Tvar tv2)
		{
			return ApplyFcnToTimeline(x => NotEq(x), tv1, tv2);
		}
		private static Hval NotEq(List<Hval> list)
		{
			string type = list[0].Val.GetType().ToString();

			if (type == "System.Boolean") return Convert.ToBoolean(list[0].Val) != Convert.ToBoolean(list[1].Val);
			if (type == "System.DateTime") return Convert.ToDateTime(list[0].Val) != Convert.ToDateTime(list[1].Val);
			if (type == "System.String") return Convert.ToString(list[0].Val) != Convert.ToString(list[1].Val);
			return Convert.ToDecimal(list[0].Val) != Convert.ToDecimal(list[1].Val);
		}

        /// <summary>
        /// Returns true when one Tvar is greather than another
        /// </summary>
        public static Tvar operator > (Tvar tn1, Tvar tn2)
        {
            return ApplyFcnToTimeline(x => GrTh(x), tn1, tn2);
        }
        private static Hval GrTh(List<Hval> list)
        {
			string type = list[0].Val.GetType().ToString();
			if (type == "System.DateTime") return Convert.ToDateTime(list[0].Val) > Convert.ToDateTime(list[1].Val);
			return Convert.ToDecimal(list[0].Val) > Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Returns true when one Tvar is greather than or equal to another.
        /// </summary>
        public static Tvar operator >= (Tvar tn1, Tvar tn2)
        {
            return ApplyFcnToTimeline(x => GrEq(x), tn1, tn2);
        }
        private static Hval GrEq(List<Hval> list)
        {
			string type = list[0].Val.GetType().ToString();
			if (type == "System.DateTime") return Convert.ToDateTime(list[0].Val) >= Convert.ToDateTime(list[1].Val);
			return Convert.ToDecimal(list[0].Val) >= Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Returns true when one Tvar is less than another.
        /// </summary>
        public static Tvar operator < (Tvar tn1, Tvar tn2)
        {
            return ApplyFcnToTimeline(x => LsTh(x), tn1, tn2);
        }
        private static Hval LsTh(List<Hval> list)
        {
			string type = list[0].Val.GetType().ToString();
			if (type == "System.DateTime") return Convert.ToDateTime(list[0].Val) < Convert.ToDateTime(list[1].Val);
			return Convert.ToDecimal(list[0].Val) < Convert.ToDecimal(list[1].Val);
        }

        /// <summary>
        /// Returns true when one Tvar is less than or equal to another.
        /// </summary>
        public static Tvar operator <= (Tvar tn1, Tvar tn2)
        {
            return ApplyFcnToTimeline(x => LsEq(x), tn1, tn2);
        }
        private static Hval LsEq(List<Hval> list)
        {
			string type = list[0].Val.GetType().ToString();
			if (type == "System.DateTime") return Convert.ToDateTime(list[0].Val) <= Convert.ToDateTime(list[1].Val);
			return Convert.ToDecimal(list[0].Val) <= Convert.ToDecimal(list[1].Val);
        }
    }
}