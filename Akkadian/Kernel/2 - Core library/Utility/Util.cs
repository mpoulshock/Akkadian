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
using System.Text.RegularExpressions;

namespace Akkadian
{
    public static partial class Util
    {
        /// <summary>
        /// Returns the maximum value of a list of input values
        /// </summary>
        public static Hval Maximum(List<Hval> list)
        {
            decimal max = Convert.ToDecimal(list[0].Val);
            foreach (Hval v in list) 
            {
                if (Convert.ToDecimal(v.Val) > max)
                {
                    max = Convert.ToDecimal(v.Val); 
                }
            }
            
            return new Hval(max);
        }

        /// <summary>
        /// Returns the minimum value of a list of input values
        /// </summary>
        public static Hval Minimum(List<Hval> list)
        {
            decimal min = Convert.ToDecimal(list[0].Val);
            foreach (Hval v in list) 
            {
                if (Convert.ToDecimal(v.Val) < min)
                {
                    min = Convert.ToDecimal(v.Val); 
                }
            }
            
            return new Hval(min);
        }

        /// <summary>
        /// Converts a generic list into a generic array.
        /// </summary>
        public static T[] ListToArray<T>(List<T> list)
        {
            T[] result = new T[list.Count];
            for (int i=0; i<list.Count; i++)
            {
                result[i] = list[i];
            }
            return result;
        }

		/// <summary>
		/// Removes parentheses from a string.
		/// </summary>
		/// <remarks>
		/// This function assumes that there are parentheses around the input.
		/// </remarks>
		public static string RemoveParens(string s)
		{
			return s.Substring(1,s.Length-2);
		}

		/// <summary>
		/// Returns the substring that's the innermost parenthetical expression.
		/// </summary>
		public static string InnermostParenthetical(string clause)
		{
			Regex rex = new Regex(@"\([-a-zA-Z0-9+/\.\|><=!*,&\{\}#_"" ]*\)");
			var m = rex.Match(clause);
			if (m.Success) 	return m.Value;
			return "";
		}

		/// <summary>
		/// Returns the substring that's the first set literal in the expression.
		/// </summary>
		public static string FirstSetLiteral(string clause)
		{
			Regex rex = new Regex(@"\{[-a-zA-Z0-9\.',#'"" ]*\}");
			var m = rex.Match(clause);
			if (m.Success) 	return m.Value;
			return "";
		}

		/// <summary>
		/// Returns the substring that's the first string literal in the expression.
		/// </summary>
		public static string FirstStringLiteral(string clause)
		{
			Regex rex = new Regex("\"[^\"]*\"");
			var m = rex.Match(clause);
			if (m.Success) 	return m.Value;
			return "";
		}

		/// <summary>
		/// Formats a date as yyyy-mm-dd.
		/// </summary>
		public static string FormatDate(DateTime d)
		{
			string date = d.ToString("yyyy-MM-dd").Replace("1900-01-01", "Dawn");
			return date.Replace(" 12:00:00 AM", "");
		}
    }
}