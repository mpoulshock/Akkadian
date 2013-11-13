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
using System.Collections.Generic;

namespace Akkadian
{
	/// <summary>
	/// Types of nodes used in interpreter expressions (Expr)
	/// </summary>
	public enum Typ
	{
		//  Akkadian types
		Tvar,
		Thing,
		Tline,

		// Types used by the parser
		Expr,		// Expression
		Fcn,		// Function call
		Op,			// Built-in operator
		Var,		// Variable
		Series,		// Time series literal
		Null		// Meaningless
	}

	/// <summary>
	/// Types of operators used in interpreter expressions
	/// </summary>
	public enum Op
	{
		// Short-circuting operators
		And = 1,
		Or = 2,
		Mult = 3,

		// Binary operators
		Plus = 10,
		Minus = 11,
		Div = 12,
		Eq = 13, 
		Neq = 14,
		GrTh = 15, 
		GrEq = 16,
		LsTh = 17, 
		LsEq = 18,
		Mod = 19,
		Nlog = 20, 
		Pow = 21,
		RndUp = 22, 
		RndDn = 23, 
		RndNrUp = 24, 
		RndNrDn = 25,
		Concat = 26,
		Subset = 27, 
		Contains = 28, 
		Union = 29, 
		Intersect = 30, 
		Remove = 31,
		AddDays = 32, 
		AddMos = 33, 
		AddYrs = 34, 
		DayDiff = 35, 
		WeekDiff = 36,
		YearDiff = 37,
		AsOf = 38,
		EverPer = 39,
		AlwaysPer = 40,
		CountPer = 41,
		RunningCountPer = 42,
		TotalElapsedDaysPer = 43,
		PeriodEndVal = 44,
		IsInPeriod = 45,
		IsBetween = 46,
		RunningElapsedIntervals = 47,
		ContinuousElapsedIntervals = 48,
		RunningSummedIntervals = 49,

		// Unary operators
		Not = 100,
		ToUSD = 101,
		Abs = 102,
		Sqrt = 103,
		Log = 104,
		Sin = 105,
		Cos = 106,
		Tan = 107,
		Asin = 108,
		Acos = 109,
		Atan = 110,
		Count = 111,
		IsEmpty = 112,
		Reverse = 113,
		ToThing = 114,
		Day = 115,
		Month = 116,
		Quarter = 117,
		Year = 118,
		DateFirstTrue = 119,
		DateLastTrue = 120,
		DaysToYears = 121,
		DaysToMonths = 122,
		DaysToWeeks = 123,
		IsAtOrAfter = 124,
		IsBefore = 125,
		Low = 126,
		High = 127,

		// Three arguments
		Shift = 200,
		ComposeDate = 201,
		SlidingElapsedIntervals = 202,
		SlidingSummedIntervals = 203,
		IsAlwaysTrue = 204,
		IsEverTrue = 205,

		// Four arguments
		TotalSummedIntervals = 250,
		TotalElapsedIntervals = 251,

		// Special
		Switch = 300,
		Max = 301,
		Min = 302,
		BoolCount = 303,
		Pipe = 304,		// Pipeline |>

		// Constants
		Unstated = 400,
		Uncertain = 401,
		Stub = 402,
		TheYear = 403,
		TheQuarter = 404,
		TheMonth = 405,
		TheWeek = 406,
		TheDay = 407,
		DaysInYear = 408,
		DaysInQuarter = 409,
		DaysInMonth = 410,
		IsLeapYear = 411,
		ConstPi = 420,
		ConstE = 421,
		DawnOfTime = 430,
		EndOfTime = 431,
		Dawn = 432,
		End = 433,
		Now = 434,
		True = 435,
		False = 436,

		// Meaningless
		Null = 999
	}

	public partial class Interpreter
	{
		// Infix operators - order is important here (boolean, comparison, arithmetic) for parsing
		public static string[] infixOps = {@"\|>","&",@"\|","==","<>",">=","<=",">","<",@"\+","-",@"\*","/",@"\^"};

		/// <summary>
		/// Maps operators, as used in Akkadian, to their op code (above).
		/// </summary>
		public static Dictionary<string,Op> OperatorRegistry = new Dictionary<string,Op>();

		/// <summary>
		/// Initializes the operator registry by adding string-Op pairs to it.
		/// </summary>
		public static void InitializeOperatorRegistry()
		{
			OperatorRegistry.Clear();

			// Short-circuit operators
			OperatorRegistry.Add("&",Op.And);
			OperatorRegistry.Add(@"\|",Op.Or);
			OperatorRegistry.Add(@"\*",Op.Mult);

			// Binary operators
			OperatorRegistry.Add(@"\|>",Op.Pipe);
			OperatorRegistry.Add(@"\+",Op.Plus);
			OperatorRegistry.Add("-",Op.Minus);
			OperatorRegistry.Add("/",Op.Div);
			OperatorRegistry.Add("==",Op.Eq);
			OperatorRegistry.Add("<>",Op.Neq);
			OperatorRegistry.Add(">=",Op.GrEq);
			OperatorRegistry.Add(">",Op.GrTh);
			OperatorRegistry.Add("<=",Op.LsEq);
			OperatorRegistry.Add("<",Op.LsTh);
			OperatorRegistry.Add("Mod",Op.Mod);
			OperatorRegistry.Add("Ln",Op.Nlog);
			OperatorRegistry.Add(@"\^",Op.Pow);
			OperatorRegistry.Add("RoundUp",Op.RndUp);
			OperatorRegistry.Add("RoundDown",Op.RndDn);
			OperatorRegistry.Add("RoundNearUp",Op.RndNrUp);
			OperatorRegistry.Add("RoundNearDown",Op.RndNrDn);
			OperatorRegistry.Add("?????",Op.Concat);
			OperatorRegistry.Add("IsSubsetOf",Op.Subset);
			OperatorRegistry.Add("Contains",Op.Contains);
			OperatorRegistry.Add("Union",Op.Union);
			OperatorRegistry.Add("Intersection",Op.Intersect);
			OperatorRegistry.Add("Remove",Op.Remove);
			OperatorRegistry.Add("AddDays",Op.AddDays);
			OperatorRegistry.Add("AddMonths",Op.AddMos);
			OperatorRegistry.Add("AddYears",Op.AddYrs);
			OperatorRegistry.Add("DayDiff",Op.DayDiff);
			OperatorRegistry.Add("WeekDiff",Op.WeekDiff);
			OperatorRegistry.Add("YearDiff",Op.YearDiff);
			OperatorRegistry.Add("AsOf",Op.AsOf);
			OperatorRegistry.Add("EverPer",Op.EverPer);
			OperatorRegistry.Add("AlwaysPer",Op.AlwaysPer);
			OperatorRegistry.Add("CountPer",Op.CountPer);
			OperatorRegistry.Add("RunningCountPer",Op.RunningCountPer);
			OperatorRegistry.Add("TotalElapsedDaysPer",Op.TotalElapsedDaysPer);
			OperatorRegistry.Add("PeriodEndVal",Op.PeriodEndVal);
			OperatorRegistry.Add("IsInPeriod",Op.IsInPeriod);
			OperatorRegistry.Add("IsBetween",Op.IsBetween);
			OperatorRegistry.Add("RunningElapsedIntervals",Op.RunningElapsedIntervals);
			OperatorRegistry.Add("ContinuousElapsedIntervals",Op.ContinuousElapsedIntervals);
			OperatorRegistry.Add("RunningSummedIntervals",Op.RunningSummedIntervals);

			// Unary operators
			OperatorRegistry.Add("!",Op.Not);
			OperatorRegistry.Add("ToUSD",Op.ToUSD);
			OperatorRegistry.Add("Abs",Op.Abs);
			OperatorRegistry.Add("Sqrt",Op.Sqrt);
			OperatorRegistry.Add("Log",Op.Log);
			OperatorRegistry.Add("Sin",Op.Sin);
			OperatorRegistry.Add("Cos",Op.Cos);
			OperatorRegistry.Add("Tan",Op.Tan);
			OperatorRegistry.Add("Asin",Op.Asin);
			OperatorRegistry.Add("Acos",Op.Acos);
			OperatorRegistry.Add("Atan",Op.Atan);
			OperatorRegistry.Add("Count",Op.Count);
			OperatorRegistry.Add("IsEmpty",Op.IsEmpty);
			OperatorRegistry.Add("Reverse",Op.Reverse);
			OperatorRegistry.Add("ToThing",Op.ToThing);
			OperatorRegistry.Add("Day",Op.Day);
			OperatorRegistry.Add("Month",Op.Month);
			OperatorRegistry.Add("Quarter",Op.Quarter);
			OperatorRegistry.Add("Year",Op.Year);
			OperatorRegistry.Add("DateFirstTrue",Op.DateFirstTrue);
			OperatorRegistry.Add("DateLastTrue",Op.DateLastTrue);
			OperatorRegistry.Add("DaysToYears",Op.DaysToYears);
			OperatorRegistry.Add("DaysToMonths",Op.DaysToMonths);
			OperatorRegistry.Add("DaysToWeeks",Op.DaysToWeeks);
			OperatorRegistry.Add("IsAtOrAfter",Op.IsAtOrAfter);
			OperatorRegistry.Add("IsBefore",Op.IsBefore);
			OperatorRegistry.Add("Low",Op.Low);
			OperatorRegistry.Add("High",Op.High);

			// Three arguments
			OperatorRegistry.Add("Shift",Op.Shift);
			OperatorRegistry.Add("Date",Op.ComposeDate);
			OperatorRegistry.Add("SlidingElapsedIntervals",Op.SlidingElapsedIntervals);
			OperatorRegistry.Add("SlidingSummedIntervals",Op.SlidingSummedIntervals);
			OperatorRegistry.Add("IsAlwaysTrue",Op.IsAlwaysTrue);
			OperatorRegistry.Add("IsEverTrue",Op.IsEverTrue);

			// Four arguments
			OperatorRegistry.Add("TotalSummedIntervals",Op.TotalSummedIntervals);
			OperatorRegistry.Add("TotalElapsedIntervals",Op.TotalElapsedIntervals);

			// Special
			OperatorRegistry.Add("Switch",Op.Switch);
			OperatorRegistry.Add("Max",Op.Max);
			OperatorRegistry.Add("Min",Op.Min);
			OperatorRegistry.Add("BoolCount",Op.BoolCount);

			// Constants
			OperatorRegistry.Add("Unstated",Op.Unstated);
			OperatorRegistry.Add("Uncertain",Op.Uncertain);
			OperatorRegistry.Add("Stub",Op.Stub);
			OperatorRegistry.Add("TheYear",Op.TheYear);
			OperatorRegistry.Add("TheQuarter",Op.TheQuarter);
			OperatorRegistry.Add("TheMonth",Op.TheMonth);
			OperatorRegistry.Add("TheWeek",Op.TheWeek);
			OperatorRegistry.Add("TheDay",Op.TheDay);
			OperatorRegistry.Add("DaysInYear",Op.DaysInYear);
			OperatorRegistry.Add("DaysInQuarter",Op.DaysInQuarter);
			OperatorRegistry.Add("DaysInMonth",Op.DaysInMonth);
			OperatorRegistry.Add("IsLeapYear",Op.IsLeapYear);
			OperatorRegistry.Add("ConstPi",Op.ConstPi);
			OperatorRegistry.Add("ConstE",Op.ConstE);
			OperatorRegistry.Add("DawnOfTime",Op.DawnOfTime);
			OperatorRegistry.Add("EndOfTime",Op.EndOfTime);
			OperatorRegistry.Add("Dawn",Op.Dawn);
			OperatorRegistry.Add("End",Op.End);
			OperatorRegistry.Add("Now",Op.Now);
			OperatorRegistry.Add("True",Op.True);
			OperatorRegistry.Add("False",Op.False);
			OperatorRegistry.Add("true",Op.True);
			OperatorRegistry.Add("false",Op.False);
		}
	}
}
