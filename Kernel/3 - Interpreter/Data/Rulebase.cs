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
	public partial class Interpreter
	{
		/// <summary>
		/// List of all functions, accessible by index
		/// </summary>
		//		public static List<Expr> Rules = new List<Expr>();

		public static List<Expr> LoadedRules()
		{
			List<Expr> result = new List<Expr>();

			// Function 0: f0(x) = (x * 2) + 1
			Expr sub1 = expr(n(Typ.Op,"T*"),n(Typ.Var,"0"),nTnum(2));
			Expr exp0 = expr(n(Typ.Op,"T+"),n(Typ.Expr,sub1),nTnum(1));
			result.Add(exp0);

			// Function 1: f1(x) = x + 42
			Expr exp1 = expr(n(Typ.Op,"T+"),n(Typ.Var,"0"),nTnum(42));
			result.Add(exp1);

			// Function 2: f2(x) = 1017 - f0(x)
			Expr exp2 = expr(n(Typ.Op,"T-"),nTnum(1017),n(Typ.Fcn,"0"));
			result.Add(exp2);

			// Function 3: f3(x,y) = y / x
			Expr exp3 = expr(n(Typ.Op,"T/"),n(Typ.Var,"1"),n(Typ.Var,"0"));
			result.Add(exp3);

			// Function 4: f4(x,y) = y(x,17)
			Expr exp4 = expr(n(Typ.Var,"1"),n(Typ.Var,"0"),nTnum(17));
			result.Add(exp4);

			// Function 5: Placeholder
			Expr exp5 = expr(n(Typ.Null,null));
			result.Add(exp5);

			// Function 6: f6(x) = if x = 0 -> 1, else x
			Expr sub6_1 = expr(n(Typ.Op,"T="),n(Typ.Var,"0"),nTnum(0));      // x == 0
			Expr exp6 = expr(n(Typ.Op,"Switch"),n(Typ.Expr,sub6_1),nTnum(1),n(Typ.Var,"0"));
			result.Add(exp6);

			// Function 7 (recursive): f7(x) = if x = 0 -> 0, else f7(x-1) + 3
			Expr sub7_1 = expr(n(Typ.Op,"T="),n(Typ.Var,"0"),nTnum(0));      // x == 0
			Expr sub7_2 = expr(n(Typ.Op,"T-"),n(Typ.Var,"0"),nTnum(1));       // x-1
			Expr sub7_3 = expr(n(Typ.Rec,"7"),n(Typ.Expr,sub7_2));               // f7(x-1)
			Expr sub7_4 = expr(n(Typ.Op,"T+"),n(Typ.Expr,sub7_3),nTnum(3));   // f7(x-1) + 3
			Expr exp7 = expr(n(Typ.Op,"Switch"),n(Typ.Expr,sub7_1),nTnum(0),n(Typ.Expr,sub7_4));
			result.Add(exp7);

			return result;
		}
	}
}