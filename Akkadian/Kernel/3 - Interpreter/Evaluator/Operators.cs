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
		Tbool,
		Tdate,
		Thing,
		Tline,
		Tnum,
		Tset,
		Tstr,

		// Types used by the parser
		Expr,		// Expression
		Fcn,		// Function call
		Op,			// Built-in operator
		Var,		// Variable
		Ask,		// Leaf node
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
		RelComp = 31,
		AddDays = 32, 
		AddMos = 33, 
		AddYrs = 34, 
		DayDiff = 35, 
		WeekDiff = 36,
		YearDiff = 37,

		// Unary operators
		Not = 100,
		USD = 101,
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
		Empty = 112,
		Rev = 113,
		ToThing = 114,
		Day = 115,
		Month = 116,
		Quarter = 117,
		Year = 118,

		// Ternary functions

		// Other
		Switch = 300,
		Max = 301,
		Min = 302,

		// Meaningless
		Null = 999
	}

	public partial class Interpreter
	{
		// Infix operators - order is important here (boolean, comparison, arithmetic) for parsing
		public static string[] infixOps = {"&",@"\|","==","<>",">=","<=",">","<",@"\+","-",@"\*","/"};

		// Dot operators - needed for parsing
		public static string[] dotOps = {""};

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
			OperatorRegistry.Add("Pow",Op.Pow);
			OperatorRegistry.Add("RoundUp",Op.RndUp);
			OperatorRegistry.Add("RoundDown",Op.RndDn);
			OperatorRegistry.Add("RoundNearUp",Op.RndNrUp);
			OperatorRegistry.Add("RoundNearDown",Op.RndNrDn);
			OperatorRegistry.Add("?????",Op.Concat);
			OperatorRegistry.Add("IsSubsetOf",Op.Subset);
			OperatorRegistry.Add("Contains",Op.Contains);
			OperatorRegistry.Add("Union",Op.Union);
			OperatorRegistry.Add("Intersection",Op.Intersect);
			OperatorRegistry.Add("RelativeComplement",Op.RelComp);
			OperatorRegistry.Add("AddDays",Op.AddDays);
			OperatorRegistry.Add("AddMonths",Op.AddMos);
			OperatorRegistry.Add("AddYears",Op.AddYrs);
			OperatorRegistry.Add("DayDiff",Op.DayDiff);
			OperatorRegistry.Add("WeekDiff",Op.WeekDiff);
			OperatorRegistry.Add("YearDiff",Op.YearDiff);

			// Unary operators
			OperatorRegistry.Add("!",Op.Not);
			OperatorRegistry.Add("ToUSD",Op.USD);
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
			OperatorRegistry.Add("IsEmpty",Op.Empty);
			OperatorRegistry.Add("Reverse",Op.Rev);
			OperatorRegistry.Add("ToThing",Op.ToThing);
			OperatorRegistry.Add("Day",Op.Day);
			OperatorRegistry.Add("Month",Op.Month);
			OperatorRegistry.Add("Quarter",Op.Quarter);
			OperatorRegistry.Add("Year",Op.Year);

			// Other
			OperatorRegistry.Add("Switch",Op.Switch);
			OperatorRegistry.Add("Max",Op.Max);
			OperatorRegistry.Add("Min",Op.Min);
		}

//		private static Op GetOpFromString(string s)
//		{
//			OperatorRegistry.TryGetValue(s, out Op.Null);
//		}
	}
}
