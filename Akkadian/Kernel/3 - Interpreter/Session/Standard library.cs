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
			ProcessInput("BoolToBinary[b] = b == True -> 1, 0;");
			ProcessInput("BoolCount[set] = set |> Map[BoolToBinary[_]] |> SetSum;");

//			ProcessInput("BoolCount[set] = set |> Map[(_ == True -> 1, 0)] |> SetSum;");

			// Set - basic
			ProcessInput("IsEmpty2[set] = (set |> Count) == 0;");

			// Set aggregations
//			ProcessInput("SetSum2[set] = Count[set] == 0 -> 0, Count[set] == 1 -> First[set], First[set] + SetSum2[Rest[set]];");
			ProcessInput("SetSum2[set] = Count[set] == 0 -> 0, First[set] + SetSum2[Rest[set]];");

			// Higher-order set
			ProcessInput("Exists[fcn,set] = (Filter[~fcn,set] |> Count) > 0;");
			ProcessInput("ForAll[fcn,set] = (Filter[~fcn,set] |> Count) == (set |> Count);");
		}
	}
}
