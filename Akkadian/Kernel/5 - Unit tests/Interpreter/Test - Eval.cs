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
			Tvar r = (Tvar)sess.ProcessInput("\"ab\" == \"ba\"");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("\"ab\" == \"ab\"");
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

		[Test]
		public void Filter_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7} |> Filter[_ >= 4]");
			Assert.AreEqual("{5,7}", r.Out);                
		}

		[Test]
		public void Filter_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{True,False,False} |> Filter[_ == False]");
			Assert.AreEqual("{False,False}", r.Out);                
		}

		[Test]
		public void Filter_3 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[x] = x * x");
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7} |> Filter[Sq[_] >= _ + 3]");
			Assert.AreEqual("{3,5,7}", r.Out);                
		}

		[Test]
		public void FunctionsAsArguments ()
		{
			// f(x,y) = y(x,17); x = 34; y = f(a,b) = b/a
			//        = y(34,17) = 17/34 = 0.5
//			Session sess = new Session();
//			sess.ProcessInput("f[x,y] = y[x,17]");
//			sess.ProcessInput("f2[a,b] = b/a");
//			Tvar r = (Tvar)sess.ProcessInput("f[34,f2]");
//			Assert.AreEqual(0.5, r.Out);          
		}

		[Test]
		public void FunctionCall_1 ()
		{
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

		[Test]
		public void Inequality_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("\"ab\" <> \"ab\"");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Inequality_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("\"ab\" <> \"ba\"");
			Assert.AreEqual(true, r.Out);                
		}

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
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("0 * 9");
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void NestedExpression ()
		{
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
		public void FT_AlwaysPerInterval_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{Dawn: false, 2012-11-08: true} |> AlwaysPer[TheYear]");
			Assert.AreEqual("{Dawn: False, 2013-01-01: True}", r.Out);                
		}

		[Test]
		public void FT_AlwaysPerInterval_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Stub |> AlwaysPer[TheYear]");
			Assert.AreEqual("Stub", r.Out);                
		}

		[Test]
		public void FT_EverPerInterval_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{Dawn: false, 2012-11-08: true} |> EverPer[TheYear]");
			Assert.AreEqual("{Dawn: False, 2012-01-01: True}", r.Out);                
		}

		[Test]
		public void FT_EverPerInterval_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Unstated |> EverPer[TheYear]");
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Map_1 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[x] = x * x");
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7} |> Map[Sq[_]]");
			Assert.AreEqual("{4,9,25,49}", r.Out);                
		}

		[Test]
		public void Map_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("MultBy[x,y] = x * y");
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7} |> Map[MultBy[_,2]]");
			Assert.AreEqual("{4,6,10,14}", r.Out);                
		}

		[Test]
		public void Map_3 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x -> 1, 0");
			Tvar r = (Tvar)sess.ProcessInput("{True,False,True,True,True} |> Map[F[_]]");
			Assert.AreEqual("{1,0,1,1,1}", r.Out);                
		}

		[Test]
		public void Map_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7} |> Map[_ >= 4]");
			Assert.AreEqual("{False,False,True,True}", r.Out);                
		}

		[Test]
		public void Map_5 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[x] = x * x");
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7} |> Map[Sq[_] >= _ + 3]");
			Assert.AreEqual("{False,True,True,True}", r.Out);                
		}

		[Test]
		public void Map_6_unknown_list_members_treated_as_num ()
		{
			Session sess = new Session();
			// sess.ProcessInput("Sq[x] = x * x");  // When Sq is undefined, treats enum value as number
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3,4} |> Map[Sq[_]] |> SetSum");
			Assert.AreEqual(30, r.Out);                
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

		[Test]
		public void Pipes_5 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[x] = x*x");
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3,4,5,6,7} |> Map[Sq[_]] |> SetSum");
			Assert.AreEqual(140, r.Out);                
		}

		[Test]
		public void Quote_Unquote_1 ()
		{
			Session sess = new Session(); 
			Tvar r = (Tvar)sess.ProcessInput("~'2");
			Assert.AreEqual(2, r.Out);                
		}

		[Test]
		public void Quote_Unquote_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Exists[fcn,set] = (Filter[~fcn,set] |> Count) > 0");
			sess.ProcessInput("GrTh4[n] = n > 4"); 
			Tvar r = (Tvar)sess.ProcessInput("{2,3,4,5,6} |> Exists['GrTh4[_]]");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Quote_Unquote_3 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Exists[fcn,set] = (Filter[~fcn,set] |> Count) > 0");
			sess.ProcessInput("GrTh4[n] = n > 4"); 
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3} |> Exists['GrTh4[_]]");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Quote_Unquote_4 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Exists[fcn,set] = (Filter[~fcn,set] |> Count) > 0");
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3,4,5} |> Exists[ '( _ > 4 )]");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Quote_Unquote_5 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Exists[fcn,set] = (Filter[~fcn,set] |> Count) > 0");
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3} |> Exists['(_ > 4)]");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Quote_Unquote_6 ()
		{
			Session sess = new Session();
			sess.ProcessInput("SumOver[fcn,set] = set |> Map[~fcn] |> SetSum");
			sess.ProcessInput("Sq[n] = n^2"); 
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3,4} |> SumOver['Sq[_]]");
			Assert.AreEqual(30, r.Out);                
		}

		[Test]
		public void Quote_Unquote_7 ()
		{
			Session sess = new Session();
			sess.ProcessInput("SumOver[fcn,set] = set |> Map[~fcn] |> SetSum");
			sess.ProcessInput("MultBy[x,y] = x*y"); 
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3,4} |> SumOver['MultBy[_,7]]");
			Assert.AreEqual(70, r.Out);                
		}

		[Test]
		public void Recursion_1 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[0]");
			Assert.AreEqual(0, r.Out);               
		}

		[Test]
		public void Recursion_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[1]");
			Assert.AreEqual(3, r.Out);                 
		}

		[Test]
		public void Recursion_3 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[2]");
			Assert.AreEqual(6, r.Out);               
		}

		[Test]
		public void Recursion_4 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[350]");  // 415
			Assert.AreEqual(1050, r.Out);                 
		}

		[Test]
		public void Reverse ()
		{
			Session sess = new Session();
			Thing A = new Thing ("A");
			Thing B = new Thing ("B");
			Expr exp = expr(n(Typ.Op,Op.Reverse), n(Typ.Tvar,Tvar.MakeTset(A,B)));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("{B,A}", r.Out);               
		}

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
			sess.ProcessInput("FedMinWage = {Dawn:Stub,2009-07-24:$7.25}");
			Tvar r = (Tvar)sess.ProcessInput("FedMinWage");
			Assert.AreEqual("{Dawn: Stub, 2009-07-24: 7.25}", r.Out);                
		}

		[Test]
		public void TimeSeries_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {Dawn: $1.95,2009-07-24: $7.25}");
			Tvar r = (Tvar)sess.ProcessInput("FedMinWage");
			Assert.AreEqual("{Dawn: 1.95, 2009-07-24: 7.25}", r.Out);                
		}

		[Test]
		public void TimeSeries_3 ()
		{
			Session sess = new Session();
			sess.ProcessInput("hh = {Dawn: {\"a\"}, 2009-07-24:{\"a\",\"b\"}}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: {a}, 2009-07-24: {a,b}}", r.Out);                
		}

		[Test]
		public void TimeSeries_4 ()
		{
			Session sess = new Session();
			sess.ProcessInput("hh = {Dawn: {1}, 2009-07-24: {1, 2}}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: {1}, 2009-07-24: {1,2}}", r.Out);                
		}

		[Test]
		public void TimeSeries_5 ()
		{
			Session sess = new Session();
			sess.ProcessInput("hh = {Dawn: Abs[-9], 2009-07-24: Abs[-42] + 1}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: 9, 2009-07-24: 43}", r.Out);                
		}

		[Test]
		public void TimeSeries_6 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[x] = x^2");
			sess.ProcessInput("hh = {Dawn: Sq[3], 2009-07-24: Sq[Sq[2]]}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: 9, 2009-07-24: 16}", r.Out);                
		}

		[Test]
		public void TimeSeries_7 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = x*y");
			sess.ProcessInput("hh = {Dawn: F[4,5], 2009-07-24: F[2,3]}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: 20, 2009-07-24: 6}", r.Out);                
		}

		[Test]
		public void TimeSeries_8 ()
		{
			Session sess = new Session();
			sess.ProcessInput("hh = {Dawn: (9*7), 2009-07-24: false|true}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: 63, 2009-07-24: True}", r.Out);                
		}

		[Test]
		public void TimeSeries_9 ()
		{
			Session sess = new Session();
			sess.ProcessInput("hh = {Dawn: -8 |> Abs, 2009-07-24: 42}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: 8, 2009-07-24: 42}", r.Out);                
		}

		[Test]
		public void TimeSeries_10 ()
		{
			Session sess = new Session();
			sess.ProcessInput("hh = {Dawn: {1,3,5} |> Count, 2009-07-24: 42}");
			Tvar r = (Tvar)sess.ProcessInput("hh");
			Assert.AreEqual("{Dawn: 3, 2009-07-24: 42}", r.Out);                
		}

		[Test]
		public void Tset_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{A,B,C}");
			Assert.AreEqual("{Unstated,Unstated,Unstated}", r.Out);                
		}

		[Test]
		public void Tset_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Union[{1,2,3},{4,5}]");
			Assert.AreEqual("{1,2,3,4,5}", r.Out);                
		}

		[Test]
		public void Tset_3 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{1,2, {3,4}}");
			Assert.AreEqual("{1,2,{3,4}}", r.Out);                
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