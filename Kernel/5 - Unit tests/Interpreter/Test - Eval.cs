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
using NUnit.Framework;

namespace Akkadian.UnitTests
{
	[TestFixture]
	public partial class EvalTests : Interpreter
	{
		[Test]
		public void Abs_1 ()
		{
			Expr exp = expr(n("op","Abs"),n("Tnum",new Tnum(41)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Abs_2 ()
		{
			Expr exp = expr(n("op","Abs"),n("Tnum",new Tnum(-41)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Addition_straight ()
		{
			// 41 + 9
			Expr exp = expr(n("op","+"),n("dec","41"),n("dec","9"));
			Expr args = expr(n("dec","1"),n("dec","2"),n("dec","3"));
			Assert.AreEqual(50, eval(exp,args).obj);                
		}

		[Test]
		public void Addition ()
		{
			// 41 + 9
			Expr exp = expr(n("op","T+"),n("Tnum",new Tnum(41)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(50, r.Out);                
		}

		[Test]
		public void Addition_variables_1 ()
		{
			// x + 42, where x = 91
			Expr exp = expr(n("op","+"),n("var","0"),n("dec","42"));
			Expr args = expr(n("dec","91"),n("dec","92"),n("dec","93"));
			Assert.AreEqual(133, eval(exp,args).obj);                
		}

		[Test]
		public void Addition_variables_2 ()
		{
			// 42 + x, where x = 91
			Expr exp = expr(n("op","+"),n("dec","42"),n("var","0"));
			Expr args = expr(n("dec","91"),n("dec","92"),n("dec","93"));
			Assert.AreEqual(133, eval(exp,args).obj);                
		}

		[Test]
		public void And ()
		{
			Expr exp = expr(n("op","T&"),n("Tbool",new Tbool(true)),n("Tnum",new Tbool(false)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void ChainedFunctions ()
		{
			// 1017 - [(x * 2) + 1], where x = 22.3
			Expr exp = expr(n("fcn","2"));
			Expr args = expr(n("dec","22.3"));
			Assert.AreEqual(971.4, eval(exp,args).obj);                
		}

		[Test]
		public void Decimals ()
		{
			Expr exp = expr(n("dec","123"));
			Expr args = expr(n("dec","1"));
			Assert.AreEqual("123", eval(exp,args).obj);                
		}

		[Test]
		public void Division ()
		{
			// 42 / 7
			Expr exp = expr(n("op","T/"),n("Tnum",new Tnum(42)),n("Tnum",new Tnum(7)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(6, r.Out);                
		}

		[Test]
		public void Equality_1 ()
		{
			Expr exp = expr(n("op","T="),n("Tnum",new Tnum(7)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_2 ()
		{
			Expr exp = expr(n("op","T="),n("Tnum",new Tnum(9)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_3 ()
		{
			Expr exp = expr(n("op","T="),n("Tstr",new Tstr("ab")),n("Tstr",new Tstr("ba")));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_4 ()
		{
			Expr exp = expr(n("op","T="),n("Tstr",new Tstr("ab")),n("Tstr",new Tstr("ab")));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_5 ()
		{
			Expr exp = expr(n("op","T="),n("Tbool",new Tbool(true)),n("Tbool",new Tbool(true)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_6 ()
		{
			Expr exp = expr(n("op","T="),n("Tbool",new Tbool(true)),n("Tbool",new Tbool(false)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(false, r.Out);                
		}

//		[Test]
//		public void Filter_1 ()
//		{
//			Thing T1 = new Thing("T1");
//			Thing T2 = new Thing("T2");
//
//			Expr exp = expr(n("Op","Filter"));
//			Expr args = expr(n(null,null)); 
//			Func<int> r = (Func<int>)eval(exp,args).obj;
//			Assert.AreEqual(7, r.Invoke());                
//		}
//
//		private Tnum IncomeOf(Thing t)
//		{
//			if (t.Id == "T1") return new Tnum(500);
//			else return new Tnum(1000);
//		}

		[Test]
		public void FunctionsAsArguments ()
		{
			// f(x,y) = y(x,17); x = 34; y = f(a,b) = b/a
			//        = y(34,17) = 17/34 = 0.5
			Expr exp = expr(n("fcn","4"));
			Expr args = expr(n("dec","34"),n("fcn","3"));
			Assert.AreEqual(0.5, eval(exp,args).obj);                
		}

		[Test]
		public void FunctionCall ()
		{
			// (a * 2) + 1, where a = 44
			Expr exp = expr(n("fcn","0"));
			Expr args = expr(n("dec","44"));
			Assert.AreEqual(89, eval(exp,args).obj);                
		}

		[Test]
		public void FunctionWith2Args ()
		{
			// y / x, where y = 99, x = 11
			Expr exp = expr(n("fcn","3"));
			Expr args = expr(n("dec","11"),n("dec","99"));
			Assert.AreEqual(9, eval(exp,args).obj);                
		}

		[Test]
		public void GreaterThan_1 ()
		{
			Expr exp = expr(n("op","T>"),n("Tnum",new Tnum(7)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void GreaterThan_2 ()
		{
			Expr exp = expr(n("op","T>"),n("Tnum",new Tnum(9)),n("Tnum",new Tnum(7)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Switch_1 ()
		{
			// if cond=true -> 42, 41
			Expr exp = expr(n("op","Switch"),n("Tbool",new Tbool(true)),n("Tnum",new Tnum(42)),n("Tnum",new Tnum(41)));
			Expr args = expr(n(null,null));
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(42, r.Out);                
		}

		[Test]
		public void Switch_2 ()
		{
			// if cond=true -> 42, 41
			Expr exp = expr(n("op","Switch"),n("Tbool",new Tbool(false)),n("Tnum",new Tnum(42)),n("Tnum",new Tnum(41)));
			Expr args = expr(n(null,null));
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Switch_3 ()
		{
			Tbool cond = new Tbool(true);
			cond.AddState(new DateTime(2015,1,1), false);

			Expr exp = expr(n("op","Switch"),n("Tbool",cond),n("Tnum",new Tnum(42)),n("Tnum",new Tnum(41)));
			Expr args = expr(n(null,null));
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual("{Dawn: 42; 1/1/2015: 41}", r.Out);                
		}

		[Test]
		public void IfThen_1 ()
		{
			// if cond=true -> 42, 41
			Expr exp = expr(n("op","if"),n("bool",true),n("dec","42"),n("dec","41"));
			Expr args = expr(n(null,null));
			Assert.AreEqual("42", eval(exp,args).obj);                
		}

		[Test]
		public void IfThen_2 ()
		{
			Expr exp = expr(n("op","if"),n("bool",false),n("dec","42"),n("dec","41"));
			Expr args = expr(n(null,null));
			Assert.AreEqual("41", eval(exp,args).obj);                
		}

		[Test]
		public void IfThen_3 ()
		{
			Expr exp = expr(n("fcn","6"));
			Expr args = expr(n("dec","0"));
			Assert.AreEqual("1", eval(exp,args).obj);                
		}

		[Test]
		public void IfThen_4 ()
		{
			Expr exp = expr(n("fcn","6"));
			Expr args = expr(n("dec","45"));
			Assert.AreEqual("45", eval(exp,args).obj);                
		}

		[Test]
		public void Inequality_1 ()
		{
			Expr exp = expr(n("op","T<>"),n("Tstr",new Tstr("ab")),n("Tstr",new Tstr("ba")));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Inequality_2()
		{
			Expr exp = expr(n("op","T<>"),n("Tstr",new Tstr("ab")),n("Tstr",new Tstr("ab")));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Max_1 ()
		{
			Expr exp = expr(n("op","Tmax"),n("Tnum",new Tnum(7)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(7, r.Out);                
		}

		[Test]
		public void Max_2 ()
		{
			Expr exp = expr(n("op","Tmax"),n("Tnum",new Tnum(7)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(9, r.Out);                
		}

		[Test]
		public void Min ()
		{
			Expr exp = expr(n("op","Tmin"),n("Tnum",new Tnum(7)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(7, r.Out);                
		}

		[Test]
		public void Multiplication_straight ()
		{
			// 41 * 9
			Expr exp = expr(n("op","*"),n("dec","41"),n("dec","9"));
			Expr args = expr(n("dec","1"),n("dec","2"),n("dec","3"));
			Assert.AreEqual(369, eval(exp,args).obj);                
		}

		[Test]
		public void Multiplication ()
		{
			// 41 - 9
			Expr exp = expr(n("op","T*"),n("Tnum",new Tnum(41)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(369, r.Out);                
		}

		[Test]
		public void NestedExpression ()
		{
			// (a * 2) + 1, where a = 44
			Expr sub = expr(n("op","*"),n("var","0"),n("dec","2"));
			Expr exp = expr(n("op","+"),n("expr",sub),n("dec","1"));
			Expr args = expr(n("dec","44"));
			Assert.AreEqual(89, eval(exp,args).obj);                
		}

		[Test]
		public void Not ()
		{
			Expr exp = expr(n("op","T!"),n("Tbool",new Tbool(true)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Or ()
		{
			Expr exp = expr(n("op","T|"),n("Tbool",new Tbool(true)),n("Tnum",new Tbool(false)));
			Expr args = expr(n(null,null)); 
			Tbool r = (Tbool)eval(exp,args).obj;
			Assert.AreEqual(true, r.Out);                
		}

        [Test]
		public void Recursion_1 ()
		{
			// f5(x) = if x = 0 -> 0, else f5(x-1) + 3
			// f5(0) = 0
			Expr exp = expr(n("fcn","5"));
			Expr args = expr(n("dec","0"));
			Assert.AreEqual("0", eval(exp,args).obj);                
		}

		[Test]
		public void Recursion_2 ()
		{
			// f5(x) = if x = 0 -> 0, else f5(x-1) + 3
			// f5(1) = 3
			Expr exp = expr(n("fcn","5"));
			Expr args = expr(n("dec","1"));
			Assert.AreEqual(3, eval(exp,args).obj);                
		}

		[Test]
		public void Recursion_3 ()
		{
			// f5(x) = if x = 0 -> 0, else f5(x-1) + 3
			// f5(2) = 6
			Expr exp = expr(n("fcn","5"));
			Expr args = expr(n("dec","2"));
			Assert.AreEqual(6, eval(exp,args).obj);                
		}

		[Test]
		public void Recursion_4 ()
		{
			Expr exp = expr(n("fcn","5"));
			Expr args = expr(n("dec","300"));
			Assert.AreEqual(900, eval(exp,args).obj);                
		}

		[Test]
		public void Recursion_PreliminaryStep ()
		{
			// f1(56) = 98
			Expr exp = expr(n("rec","1"),n("dec","56"));
			Expr args = expr(n(null,null));
			Assert.AreEqual(98, eval(exp,args).obj);                
		}

		[Test]
		public void Subtraction_1 ()
		{
			// 41 - 9
			Expr exp = expr(n("op","T-"),n("Tnum",new Tnum(41)),n("Tnum",new Tnum(9)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual(32, r.Out);                
		}

		[Test]
		public void Subtraction_2 ()
		{
			// 41 - Unstated
			Expr exp = expr(n("op","T-"),n("Tnum",new Tnum(41)),n("Tnum",new Tnum(Hstate.Unstated)));
			Expr args = expr(n(null,null)); 
			Tnum r = (Tnum)eval(exp,args).obj;
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void VariableValues ()
		{
			Expr exp = expr(n("var","2"));
			Expr args = expr(n("dec","91"),n("dec","92"),n("dec","93"));
			Assert.AreEqual("93", eval(exp,args).obj);                
		}
	}
}
