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
	public partial class Session
	{
		/// <summary>
		/// Returns a Tvar when its associated Tvar is true.  
		/// </summary>
		/// <remarks>
		/// Similar in principle to a C# switch statement, just temporal.
		/// Sample usage: Switch(Tvar1, Tvar1, Tvar2, Tvar2, ..., defaultTvar).  
		/// Returns Tvar1 if Tvar2 is true, else Tvar2 if Tvar2 is true, etc., else defaultTvar. 
		/// </remarks>
		public Tvar Switch2(Expr arguments, Expr args)
		{
			// Default result
			Hval h = new Hval(null, Hstate.Null);
			Tvar result = new Tvar(h);

			// Analyze each condition-value pair...and keep going
			// until all intervals of the result Tvar are defined...
			int len = (int)arguments.nodes.Count;
			for (int arg=0; arg < len-1; arg+=2)
			{
				// Get value of the condition
				Tvar newCondition = (Tvar)eval(arguments.nodes[arg],args).obj;

				// Identify the intervals when the new condition is neither false nor true
				// Falsehood causes it to fall through to next condition. Truth causes the
				// result to assume the value during that interval.
				Tvar newConditionIsUnknown = Util.HasUnknownState(newCondition);

				// Merge these 'unknown' intervals in new condition into the result.
				result = Util.MergeTvars(result, Util.ConditionalAssignment(newConditionIsUnknown, newCondition));

				// Identify the intervals when the new condition is true.
				// Ignore irrelevant periods when result is already determined.
				// During these intervals, "result" takes on the value of its conclusion.
				Tvar newConditionIsTrueAndResultIsNull = newCondition && Util.IsNull(result);

				// If new true segments are found, accumulate the values during those intervals
				if (newConditionIsTrueAndResultIsNull.IsEverTrue())
				{
					Tvar val = (Tvar)eval(arguments.nodes[arg+1],args).obj;
					result = Util.MergeTvars(result, Util.ConditionalAssignment(newConditionIsTrueAndResultIsNull, val)); 
				}

				if (!Util.HasUndefinedIntervals(result))
				{
					return result.Lean;
				}
			}

			Tvar defaultVal = (Tvar)eval(arguments.nodes[len-1],args).obj;
			result = Util.MergeTvars(result, defaultVal);

			return result.Lean;
		}
	}
}