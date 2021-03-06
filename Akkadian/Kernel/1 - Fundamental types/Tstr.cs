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
    /// A string object whose value changes at various  points in time. 
    /// </summary>
    public partial class Tvar
    {
        /// <summary>
        /// Constructs a Tvar that is eternally set to a given value. 
        /// </summary>
        public Tvar(string val)
        {
            this.SetEternally(val);
        }

        /// <summary>
        /// Implicitly converts strings to Tvars.
        /// </summary>
        public static implicit operator Tvar(string s) 
        {
            return new Tvar(s);
        }
      
        /// <summary>
        /// Concatenates two or more Tvars. 
        /// </summary>
        public static Tvar Concat(Tvar ts1, Tvar ts2)    
        {
            return ApplyFcnToTimeline(x => Concat(x), ts1, ts2);
        }
        private static Hval Concat(List<Hval> list)
        {
            return Convert.ToString(list[0].Val) + Convert.ToString(list[1].Val);
        }

    }
    
    #pragma warning restore 660, 661
}
