// Copyright (c) 2013 Hammura.bi LLC
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
	public partial class Session 
	{
		/// <summary>
		/// Loads the Akkadian standard library functions into the session.
		/// </summary>
		public void LoadStandardLibrary()
		{
			// Logic
			ProcessInput("IfThen[a,b] = !a | b;");
			ProcessInput("BoolToBinary[b] = If[b == True, 1, 0];");
			ProcessInput("BoolCount[set] = set |> Map[BoolToBinary[_]] |> SetSum;");

			// Set - basic
			ProcessInput("IsEmpty[set] = (set |> Count) == 0;");
			ProcessInput("Contains[thing,set] = (set |> Filter[_ == thing] |> Count) > 0;");

			// Higher-order set
			ProcessInput("Exists[fcn,set] = (Filter[~fcn,set] |> Count) > 0;");
			ProcessInput("ForAll[fcn,set] = (Filter[~fcn,set] |> Count) == (set |> Count);");

			// Time
			ProcessInput("TheWeek = WeeksSince[AddDays[6,Dawn]];");   // Starts on a Sunday (see en.wikipedia.org/wiki/Seven-day_week#Week_numbering)
			ProcessInput("DayOfWeek = Mod[DaysSince[1900-01-07],7] + 1;");
			ProcessInput("DaysInMonth = If[TheMonth ==  2 & IsLeapYear, 29, TheMonth ==  2, 28, " +
				"TheMonth ==  4, 30, TheMonth ==  6, 30, TheMonth ==  9, 30, TheMonth == 11, 30, 31];");
			ProcessInput("IsLeapYear = If[TheYear == 2100, false, Mod[TheYear,4] == 0, true, false];");
			ProcessInput("DaysInYear = If[IsLeapYear, 366, 365];");
			ProcessInput("DaysInQuarter = If[TheQuarter == 1 & IsLeapYear, 91, TheQuarter == 1, 90, TheQuarter == 2, 91, " +
				"TheQuarter == 3, 92, TheQuarter == 4, 92, 0];");

			// Uncertainty
			ProcessInput("Open[t] = If[!t, Uncertain, t];");
		}
	}
}
