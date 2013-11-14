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
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[-66]");
			Assert.AreEqual(66, r.Out);             
		}

		[Test]
		public void Abs_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[-41]");
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Addition ()
		{
			// 41 + 9
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("41 + 9");
			Assert.AreEqual(50, r.Out);               
		}

		[Test]
		public void Addition_variables_1 ()
		{
			// x + 42, where x = 91
			Session sess = new Session();
			sess.ProcessInput("F[x] = x + 42");
			Tvar r = (Tvar)sess.ProcessInput("F[91]");
			Assert.AreEqual(133, r.Out);             
		}

		[Test]
		public void And_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & false");
			Assert.AreEqual(false, r.Out);               
		}

		[Test]
		public void And_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("false & true");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void ChainedExpressions_1 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = 1017 - ((x*2) + 1)");
			Tvar r = (Tvar)sess.ProcessInput("F[22]");
			Assert.AreEqual(972, r.Out);          
		}

		[Test]
		public void ChainedExpressions_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("((22) + 1)");
			Assert.AreEqual(23, r.Out);          
		}

		[Test]
		public void ChainedExpressions_3 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("((22*2))");
			Assert.AreEqual(44, r.Out);          
		}

		[Test]
		public void ChainedExpressions_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("(22*2) + 1");
			Assert.AreEqual(45, r.Out);          
		}

//		[Test]
//		public void Concat ()
//		{
//			Session sess = new Session();
//			Expr exp = expr(n(Typ.Op,Op.Concat), nTvar("Hello, "), nTvar("World"));
//			Tvar r = (Tvar)sess.eval(exp).obj;
//			Assert.AreEqual("Hello, World", r.Out);               
//		}

		[Test]
		public void Division ()
		{
			// 42 / 7
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("42/7");
			Assert.AreEqual(6, r.Out);            
		}

		[Test]
		public void Equality_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("7 == 9");
			Assert.AreEqual(false, r.Out);              
		}

		[Test]
		public void Equality_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("9 == 9");
			Assert.AreEqual(true, r.Out);              
		}

		[Test]
		public void Equality_3 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("'ab' == 'ba'");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("'ab' == 'ab'");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_5 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true == true");
			Assert.AreEqual(true, r.Out);               
		}

		[Test]
		public void Equality_6 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("false == true");
			Assert.AreEqual(false, r.Out);              
		}

		[Test]
		public void EvalUserString_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[-66]");
			Assert.AreEqual(66, r.Out);           
		}

		[Test]
		public void EvalUserString_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("(11 * 6) - 100");
			Assert.AreEqual(-34, r.Out);               
		}

		[Test]
		public void EvalUserString_3 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[ (11 * 6) - 100]");
			Assert.AreEqual(34, r.Out);               
		}

		[Test]
		public void EvalUserString_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("(4+6) * ( 2 + 1 )");
			Assert.AreEqual(30, r.Out);               
		}

		[Test]
		public void EvalUserString_5 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("1 + 4 / 2");
			Assert.AreEqual(3, r.Out);               
		}

		[Test]
		public void EvalUserString_6 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & 5 + 31 > 99 ");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalUserString_7 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[(4+6) * ( 2 + 1 )]");
			Assert.AreEqual(30, r.Out);               
		}

		[Test]
		public void EvalUserString_8 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & !!false");
			Assert.AreEqual(false, r.Out);                
		}

//		[Test]
//		public void Expression_1 ()
//		{
//			Session sess = new Session();
//			sess.ProcessInput("Pi[] = 3.14159");  // Illegal expression b/c no params
//			Tvar r = (Tvar)sess.ProcessInput("Pi[]");
//			Assert.AreEqual(3.14159, r.Out);                
//		}

		[Test]
		public void Expression_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Pi = 3.14159");
			Tvar r = (Tvar)sess.ProcessInput("Pi");
			Assert.AreEqual(3.14159, r.Out);                
		}

		//		[Test]
		//		public void Filter_1 ()
		//		{
		//			Thing T1 = new Thing("T1");
		//			Thing T2 = new Thing("T2");
		//
		//			Expr exp = expr(n(Typ.Op,"Filter"));
		//			Func<int> r = (Func<int>)sess.eval(exp).obj;
		//			Assert.AreEqual(7, r.Invoke());                
		//		}
		//
		//		private Tvar IncomeOf(Thing t)
		//		{
		//			if (t.Id == "T1") return new Tvar(500);
		//			else return new Tvar(1000);
		//		}

		[Test]
		public void FunctionsAsArguments ()
		{
			// f(x,y) = y(x,17); x = 34; y = f(a,b) = b/a
			//        = y(34,17) = 17/34 = 0.5
//			Expr exp = expr(n(Typ.Fcn,"4"));
//			Expr args = expr(nTvar(34),n(Typ.Fcn,"3"));
//			Tvar r = (Tvar)sess.eval(exp,args).obj;
//			Assert.AreEqual(0.5, r.Out);        

			Session sess = new Session();
			sess.ProcessInput("f[x,y] = y[x,17]");
			sess.ProcessInput("f2[a,b] = b/a");
			Tvar r = (Tvar)sess.ProcessInput("f[34,f2]");
			Assert.AreEqual(0.5, r.Out);          
		}

		[Test]
		public void FunctionCall_1 ()
		{
			// (a * 2) + 1, where a = 44
			Session sess = new Session();
			sess.ProcessInput("F[a] = (a*2) + 1");
			Tvar r = (Tvar)sess.ProcessInput("F[44]");
			Assert.AreEqual(89, r.Out);              
		}

		[Test]
		public void FunctionCall_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x * 9");
			Tvar r = (Tvar)sess.ProcessInput("F[3]");
			Assert.AreEqual(27, r.Out);              
		}

		[Test]
		public void FunctionWith2Args ()
		{
			// y / x, where y = 99, x = 11
			Session sess = new Session();
			sess.ProcessInput("f3[x,y] = y / x");
			Tvar r = (Tvar)sess.ProcessInput("f3[11,99]");
			Assert.AreEqual(9, r.Out);             
		}

		[Test]
		public void GreaterThan_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("7 > 9");
			Assert.AreEqual(false, r.Out);            
		}

		[Test]
		public void GreaterThan_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("9 > 7");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void GreaterThan_3 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("2015-01-01 > 2013-02-02");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void GreaterThan_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("2011-01-01 > 2013-02-02");
			Assert.AreEqual(false, r.Out);                
		}

//		[Test]
//		public void Inequality_1 ()
//		{
//			Session sess = new Session();
//			Expr exp = expr(n(Typ.Op,Op.Neq),nTvar("ab"),nTvar("ba"));
//			Tvar r = (Tvar)sess.eval(exp).obj;
//			Assert.AreEqual(true, r.Out);                
//		}
//
//		[Test]
//		public void Inequality_2()
//		{
//			Session sess = new Session();
//			Expr exp = expr(n(Typ.Op,Op.Neq),nTvar("ab"),nTvar("ab"));
//			Tvar r = (Tvar)sess.eval(exp).obj;
//			Assert.AreEqual(false, r.Out);                
//		}

		[Test]
		public void Max_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Max[7]");
			Assert.AreEqual(7, r.Out);                 
		}

		[Test]
		public void Max_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Max[7,9]");
			Assert.AreEqual(9, r.Out);              
		}

		[Test]
		public void Min ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Min[7,9]");
			Assert.AreEqual(7, r.Out);                
		}

		[Test]
		public void Multiplication_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("41 * 9");
			Assert.AreEqual(369, r.Out);                 
		}

		[Test]
		public void Multiplication_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Unstated * 9");
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Multiplication_3 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Stub * 9");
			Assert.AreEqual("Stub", r.Out);                
		}

		[Test]
		public void Multiplication_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Stub * 0");
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void Multiplication_5 ()
		{
			// 0 * 9
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("0 * 9");
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void NestedExpression ()
		{
			// (a * 2) + 1, where a = 44
			Session sess = new Session();
			sess.ProcessInput("Fcn[a] = (a * 2) + 1");
			Tvar r = (Tvar)sess.ProcessInput("Fcn[44]");
			Assert.AreEqual(89, r.Out);               
		}

		[Test]
		public void Not_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("!true");
			Assert.AreEqual(false, r.Out);            
		}

		[Test]
		public void Not_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("!false");
			Assert.AreEqual(true, r.Out);               
		}

		[Test]
		public void Not_3 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("!!false");
			Assert.AreEqual(false, r.Out);               
		}

		[Test]
		public void Or ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true | false");
			Assert.AreEqual(true, r.Out);             
		}

		[Test]
		public void Exponents_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("2^4");
			Assert.AreEqual(16, r.Out);                
		}

		[Test]
		public void Exponents_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("10 * 2^4");
			Assert.AreEqual(160, r.Out);                
		}

		[Test]
		public void Misc_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Misc_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 + (4 * 2)");
			Assert.AreEqual(11, r.Out);                
		}

		[Test]
		public void Misc_3_Parens ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 + (4 * ( 2 + 1 ))");
			Assert.AreEqual(15, r.Out);                
		}

		[Test]
		public void Misc_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("(4+6) * ( 2 + 1 )");
			Assert.AreEqual(30, r.Out);                
		}

		[Test]
		public void Misc_5 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("1 + 4 / 2");
			Assert.AreEqual(3, r.Out);                
		}

		[Test]
		public void Misc_6 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & ( false | true )");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Misc_7 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & 5 > 99");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Misc_8 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & 5 + 31 > 99");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Misc_9 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 * Abs[-9]");
			Assert.AreEqual(27, r.Out);                
		}

		[Test]
		public void Misc_10 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("RoundUp[0.01,Sin[Abs[9]] * Cos[33]]");
			Assert.AreEqual(0.01, r.Out);                
		}

		[Test]
		public void Misc_11 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[9 + -11]");
			Assert.AreEqual(2, r.Out);                
		}

		[Test]
		public void Misc_12 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 * (Abs[-32] / 2)");
			Assert.AreEqual(48, r.Out);                
		}

		[Test]
		public void Misc_13 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[(4+6) * ( 2 + 1 )]");
			Assert.AreEqual(30, r.Out);                
		}

		[Test]
		public void Misc_14 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("RoundUp[10,Abs[2]]");
			Assert.AreEqual(10, r.Out);                
		}

		[Test]
		public void Misc_15 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = Abs[x]");
			Tvar r = (Tvar)sess.ProcessInput("F[-3]");
			Assert.AreEqual(3, r.Out);                
		}

		[Test]
		public void Misc_16 ()
		{
			Session sess = new Session();
			sess.ProcessInput("IsEligible[p] = Age[p] > 65 | Income[p] < $17,000");
			Tvar r = (Tvar)sess.ProcessInput("IsEligible['jim']");
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Misc_17 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = Abs[x] + Cos[y]");
			Tvar r = (Tvar)sess.ProcessInput("F[5,1] |> RoundUp[0.001]");
			Assert.AreEqual(5.541, r.Out);                
		}

		[Test]
		public void Misc_18 ()
		{
//			Session sess = new Session();
//			sess.ProcessInput("Factorial[n] = n == 1 -> 1, n * Factorial[n-1]");
//			Tvar r = (Tvar)sess.ProcessInput("Factorial[4]");
//			Assert.AreEqual(24, r.Out);                
		}

		[Test]
		public void Misc_19_MultByNegativeNum ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("4 * -12");
			Assert.AreEqual(-48, r.Out);                
		}

		[Test]
		public void Misc_20_MultByNegativeNum ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("-4 * 12");
			Assert.AreEqual(-48, r.Out);                
		}

//		[Test]
//		public void Misc_21 ()
//		{
//			Session sess = new Session();
//			sess.ProcessInput("IsEligible[p] = !Over65[p]");
//			sess.ProcessInput("Over65['jon'] = false");
//			Tvar r = (Tvar)sess.ProcessInput("IsEligible['jon']");
//			Assert.AreEqual(true, r.Out);                
//		}

		[Test]
		public void Misc_22 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("!(true & false) | true");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Misc_23 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x");
			Tvar r = (Tvar)sess.ProcessInput("F[4]");
			Assert.AreEqual(4, r.Out);                
		}

		[Test]
		public void Misc_24 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Pi = 4");
			Tvar r = (Tvar)sess.ProcessInput("Pi");
			Assert.AreEqual(4, r.Out);                
		}

		[Test]
		public void Misc_25 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y,z] = x * y + z");
			Tvar r = (Tvar)sess.ProcessInput("F[3,4,5]");
			Assert.AreEqual(17, r.Out);                
		}

		[Test]
		public void Misc_26_SetLiteral ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{A,B,C}");
			Assert.AreEqual("A,B,C", r.Out);                
		}

		[Test]
		public void Misc_27_EmptySet ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{}");
			Assert.AreEqual("", r.Out);                
		}

		[Test]
		public void Misc_28_TimeSeriesLiteral ()
		{
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {Dawn:Stub,2009-07-24:$7.25}");
			Tvar r = (Tvar)sess.ProcessInput("FedMinWage");
			Assert.AreEqual("{Dawn:Stub,2009-07-24:$7.25}", r.Out);                
		}

		[Test]
		public void NestedFcns_1 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[n] = n^2");
			sess.ProcessInput("AddOne[n] = n+1");
			Tvar r = (Tvar)sess.ProcessInput("AddOne[Sq[4]]");
			Assert.AreEqual(17, r.Out);                
		}

		[Test]
		public void NestedFcns_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[n] = n^2");
			sess.ProcessInput("AddOne[n] = n+1");
			Tvar r = (Tvar)sess.ProcessInput("AddOne[Sq[(5+2)-3]]");
			Assert.AreEqual(17, r.Out);                
		}

		[Test]
		public void Num_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("9");
			Assert.AreEqual(9, r.Out);                
		}

		[Test]
		public void Parens_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("(2+3) * (15-6)");
			Assert.AreEqual(45, r.Out);                
		}

		[Test]
		public void Pipes_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("-9.1 |> Abs");
			Assert.AreEqual(9.1, r.Out);                
		}

		[Test]
		public void Pipes_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = x * y");
			Tvar r = (Tvar)sess.ProcessInput("33 |> F[2]");
			Assert.AreEqual(66, r.Out);                
		}

		[Test]
		public void Pipes_3 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y,z] = x * y * z");
			Tvar r = (Tvar)sess.ProcessInput("30 |> F[2,2]");
			Assert.AreEqual(120, r.Out);                
		}

		[Test]
		public void Pipes_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("8.455 |> RoundUp[0.01]");
			Assert.AreEqual(8.46, r.Out);                
		}

//		[Test]
//		public void Recursion_1 ()
//		{
//			// f(x) = if x = 0 -> 0, else f(x-1) + 3
//			// f(0) = 0
//			Session sess = new Session();
//			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
//			Tvar r = (Tvar)sess.ProcessInput("F[0]");
//			Assert.AreEqual(0, r.Out);               
//		}
//
//		[Test]
//		public void Recursion_2 ()
//		{
//			// f7(x) = if x = 0 -> 0, else f7(x-1) + 3
//			// f7(1) = 3
//			Session sess = new Session();
//			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
//			Tvar r = (Tvar)sess.ProcessInput("F[1]");
//			Assert.AreEqual(3, r.Out);                 
//		}
//
//		[Test]
//		public void Recursion_3 ()
//		{
//			// f7(x) = if x = 0 -> 0, else f7(x-1) + 3
//			// f7(2) = 6
//			Session sess = new Session();
//			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
//			Tvar r = (Tvar)sess.ProcessInput("F[2]");
//			Assert.AreEqual(6, r.Out);               
//		}
//
//		[Test]
//		public void Recursion_4 ()
//		{
//			// f7(450) = 1350
//			Session sess = new Session();
//			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
//			Tvar r = (Tvar)sess.ProcessInput("F[375]");
//			Assert.AreEqual(1125, r.Out);                 
//		}

//		[Test]
//		public void Reverse ()
//		{
//			Session sess = new Session();
//			Thing A = new Thing ("A");
//			Thing B = new Thing ("B");
//			Expr exp = expr(n(Typ.Op,Op.Reverse), n(Typ.Tvar,Tvar.MakeTset(A,B)));
//			Tvar r = (Tvar)sess.eval(exp).obj;
//			Assert.AreEqual("B,A", r.Out);               
//		}

		[Test]
		public void Subtraction_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("41 - 9");
			Assert.AreEqual(32, r.Out);              
		}

		[Test]
		public void Subtraction_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("41 - Unstated");
			Assert.AreEqual("Unstated", r.Out);               
		}

		[Test]
		public void Switch_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x -> 42, 41");
			Tvar r = (Tvar)sess.ProcessInput("F[true]");
			Assert.AreEqual(42, r.Out);                
		}

		[Test]
		public void Switch_3 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x -> 42, 41");
			Tvar r = (Tvar)sess.ProcessInput("F[false]");
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Switch_4 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x -> 42, 0");
			Tvar r = (Tvar)sess.ProcessInput("F[True]");
			Assert.AreEqual(42, r.Out);              
		}

		[Test]
		public void Switch_5 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x -> 42, 0");
			Tvar r = (Tvar)sess.ProcessInput("F[false]");
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void Switch_6 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = x -> 42, y -> 41, 0");
			Tvar r = (Tvar)sess.ProcessInput("F[true,false]");
			Assert.AreEqual(42, r.Out);                
		}

		[Test]
		public void Switch_7 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = x -> 42, y -> 41, 0");
			Tvar r = (Tvar)sess.ProcessInput("F[false,true]");
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Switch_8 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = x -> 42, y -> 41, 0");
			Tvar r = (Tvar)sess.ProcessInput("F[false,false]");
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void Switch_9 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = (x -> 42, y -> 41, 0) * 3");
			Tvar r = (Tvar)sess.ProcessInput("F[false,true]");
			Assert.AreEqual(123, r.Out);                
		}

		[Test]
		public void Switch_10 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = x -> 42, y -> 41*3, 0");
			Tvar r = (Tvar)sess.ProcessInput("F[false,true]");
			Assert.AreEqual(123, r.Out);                
		}

		[Test]
		public void Switch_11 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = x -> 42, y -> Abs[-9], 0");
			Tvar r = (Tvar)sess.ProcessInput("F[false,true]");
			Assert.AreEqual(9, r.Out);                
		}

		[Test]
		public void Switch_12 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = Abs[(x -> -42, y -> -41, 0)]");
			Tvar r = (Tvar)sess.ProcessInput("F[false,true]");
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Switch_13 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x -> false, true");
			Tvar r = (Tvar)sess.ProcessInput("F[true]");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void TimeSeries_1 ()
		{
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {1800-01-01:3.00,2009-07-24:7.25}");
			Tvar r = (Tvar)sess.ProcessInput("FedMinWage |> Max");
			Assert.AreEqual(7.25, r.Out);                
		}

		[Test]
		public void Tset_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{A,B,C}");
			Assert.AreEqual("A,B,C", r.Out);                
		}

		[Test]
		public void Tset_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Union[{A,B,C},{D,E}]");
			Assert.AreEqual("A,B,C,D,E", r.Out);                
		}

		[Test]
		public void ToUSD ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("ToUSD[42.224]");
			Assert.AreEqual("$42.22", r.Out);               
		}
	}
}