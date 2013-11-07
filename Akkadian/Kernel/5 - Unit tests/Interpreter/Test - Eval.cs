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
//			Tvar r = (Tvar)sess.ProcessInput("Abs[-66]");
//			Assert.AreEqual(66, r.Out);  

			Expr exp = expr(n(Typ.Op,Op.Abs),nTvar(41));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Abs_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Abs),nTvar(-41));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Addition ()
		{
			// 41 + 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Plus),nTvar(41),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(50, r.Out);                
		}

		[Test]
		public void Addition_variables_1 ()
		{
			// x + 42, where x = 91
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Plus),n(Typ.Var,"0"),nTvar(42));
			Expr args = expr(nTvar(91),nTvar(92),nTvar(93));
			Tvar r = (Tvar)sess.eval(exp,args).obj;
			Assert.AreEqual(133, r.Out);                
		}

		[Test]
		public void Addition_variables_2 ()
		{
			// x + 42, where x = 91
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Plus),nTvar(42),n(Typ.Var,"0"));
			Expr args = expr(nTvar(91),nTvar(92),nTvar(93));
			Tvar r = (Tvar)sess.eval(exp,args).obj;
			Assert.AreEqual(133, r.Out);                
		}

		[Test]
		public void And_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.And),nTvar(true),nTvar(false));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void And_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.And),nTvar(false),nTvar(true));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void ChainedExpressions ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = 1017 - ((x*2) + 1)");
			Tvar r = (Tvar)sess.ProcessInput("F[22]");
			Assert.AreEqual(972, r.Out);          
		}

		[Test]
		public void Concat ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Concat), nTvar("Hello, "), nTvar("World"));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("Hello, World", r.Out);               
		}

		[Test]
		public void Division ()
		{
			// 42 / 7
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Div),nTvar(42),nTvar(7));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(6, r.Out);                
		}

		[Test]
		public void Equality_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTvar(7),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTvar(9),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_3 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTvar("ab"),nTvar("ba"));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_4 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTvar("ab"),nTvar("ab"));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_5 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTvar(true),nTvar(true));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_6 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTvar(true),nTvar(false));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_1 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Abs,Tvar:-66}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(66, r.Out);                
		}

		[Test]
		public void EvalStringExpr_2 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Mult,Tvar:4,Tvar:2}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(8, r.Out);                
		}

		[Test]
		public void EvalStringExpr_3 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Not,Tvar:True}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_4 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:And,Tvar:True,Tvar:False}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_5 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Abs,Var:0}");
			Expr args = expr(nTvar(-109));
			Tvar r = (Tvar)sess.eval(exp,args).obj;
			Assert.AreEqual(109, r.Out);                
		}

		[Test]
		public void EvalStringExpr_6 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:And,Tvar:True,Tvar:False}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_7 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:And,Tvar:True,Expr:{Op:LsTh,Tvar:5,Tvar:99}}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void EvalStringExpr_8 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Minus,Expr:{Op:Mult,Tvar:11,Tvar:6},Tvar:100}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(-34, r.Out);                
		}

		[Test]
		public void EvalStringExpr_9 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Abs,Expr:{Op:Minus,Expr:{Op:Mult,Tvar:11,Tvar:6},Tvar:100}}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(34, r.Out);                
		}

		[Test]
		public void EvalStringExpr_10 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Minus,Expr:{Op:Mult,Tvar:11,Tvar:6},Tvar:100}");
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(-34, r.Out);                
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
			Expr exp = expr(n(Typ.Op,Op.GrTh),nTvar(7),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void GreaterThan_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.GrTh),nTvar(9),nTvar(7));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Inequality_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Neq),nTvar("ab"),nTvar("ba"));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Inequality_2()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Neq),nTvar("ab"),nTvar("ab"));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Max_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Max),nTvar(7));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(7, r.Out);                
		}

		[Test]
		public void Max_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Max),nTvar(7),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(9, r.Out);                
		}

		[Test]
		public void Min ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Min),nTvar(7),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(7, r.Out);                
		}

		[Test]
		public void Multiplication_1 ()
		{
			// 41 * 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Mult),nTvar(41),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(369, r.Out);                
		}

		[Test]
		public void Multiplication_2 ()
		{
			// Unstated * 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Mult),n(Typ.Tvar,new Tvar(Hstate.Unstated)),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Multiplication_3 ()
		{
			// 0 * 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Mult),nTvar(0),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void NestedExpression ()
		{
			// (a * 2) + 1, where a = 44
			Session sess = new Session();
			Expr sub = expr(n(Typ.Op,Op.Mult),n(Typ.Var,"0"),nTvar(2));
			Expr exp = expr(n(Typ.Op,Op.Plus),n(Typ.Expr,sub),nTvar(1));
			Expr args = expr(nTvar(44));
			Tvar r = (Tvar)sess.eval(exp,args).obj;
			Assert.AreEqual(89, r.Out);                
		}

		[Test]
		public void Not ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Not),nTvar(true));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Or ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Or),nTvar(true),nTvar(false));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Recursion_1 ()
		{
			// f(x) = if x = 0 -> 0, else f(x-1) + 3
			// f(0) = 0
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[0]");
			Assert.AreEqual(0, r.Out);               
		}

		[Test]
		public void Recursion_2 ()
		{
			// f7(x) = if x = 0 -> 0, else f7(x-1) + 3
			// f7(1) = 3
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[1]");
			Assert.AreEqual(3, r.Out);                 
		}

		[Test]
		public void Recursion_3 ()
		{
			// f7(x) = if x = 0 -> 0, else f7(x-1) + 3
			// f7(2) = 6
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[2]");
			Assert.AreEqual(6, r.Out);               
		}

		[Test]
		public void Recursion_4 ()
		{
			// f7(450) = 1350
			Session sess = new Session();
			sess.ProcessInput("F[x] = x==0 -> 0, F[x-1]+3");
			Tvar r = (Tvar)sess.ProcessInput("F[450]");
			Assert.AreEqual(1350, r.Out);                 
		}

		[Test]
		public void Reverse ()
		{
			Session sess = new Session();
			Thing A = new Thing ("A");
			Thing B = new Thing ("B");
			Expr exp = expr(n(Typ.Op,Op.Rev), n(Typ.Tvar,new Tvar(A,B)));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("B, A", r.Out);               
		}

		[Test]
		public void Subtraction_1 ()
		{
			// 41 - 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Minus),nTvar(41),nTvar(9));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(32, r.Out);                
		}

		[Test]
		public void Subtraction_2 ()
		{
			// 41 - Unstated
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Minus),nTvar(41),nTvar(new Tvar(Hstate.Unstated)));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Switch_1 ()
		{
			// if cond=true -> 42, 41
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Switch),nTvar(true),nTvar(42),nTvar(41));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(42, r.Out);              

//			Session sess = new Session();
//			sess.ProcessInput("f3[x,y] = y / x");
//			Tvar r = (Tvar)sess.ProcessInput("f3[11,99]");
//			Assert.AreEqual(9, r.Out);  
		}

		[Test]
		public void Switch_2 ()
		{
			// if cond=true -> 42, 41
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Switch),nTvar(false),nTvar(42),nTvar(41));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Switch_3 ()
		{
			Session sess = new Session();
			Tvar cond = new Tvar(true);
			cond.AddState(new DateTime(2015,1,1), false);

			Expr exp = expr(n(Typ.Op,Op.Switch),nTvar(cond),nTvar(42),nTvar(41));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("{Dawn: 42; 1/1/2015: 41}", r.Out);                
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
		public void ToUSD ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.USD), nTvar(42.224));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("$42.22", r.Out);               
		}

		[Test]
		public void Union ()
		{
			Session sess = new Session();
			Thing A = new Thing ("A");
			Thing B = new Thing ("B");
			Expr exp = expr(n(Typ.Op,Op.Union), nTvar(A), nTvar(B));
			Tvar r = (Tvar)sess.eval(exp).obj;
			Assert.AreEqual("A, B", r.Out);               
		}

		[Test]
		public void VariableValues ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Var,"2"));
			Expr args = expr(nTvar(91),nTvar(92),nTvar(93));
			Tvar r = (Tvar)sess.eval(exp,args).obj;
			Assert.AreEqual(93, r.Out);                
		}
	}
}