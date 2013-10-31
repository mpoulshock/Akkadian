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
//			Tnum r = (Tnum)sess.ProcessInput("Abs[-66]");
//			Assert.AreEqual(66, r.Out);  

			Expr exp = expr(n(Typ.Op,Op.Abs),nTnum(41));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Abs_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Abs),nTnum(-41));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Addition ()
		{
			// 41 + 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Plus),nTnum(41),nTnum(9));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(50, r.Out);                
		}

		[Test]
		public void Addition_variables_1 ()
		{
			// x + 42, where x = 91
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Plus),n(Typ.Var,"0"),nTnum(42));
			Expr args = expr(nTnum(91),nTnum(92),nTnum(93));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(133, r.Out);                
		}

		[Test]
		public void Addition_variables_2 ()
		{
			// x + 42, where x = 91
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Plus),nTnum(42),n(Typ.Var,"0"));
			Expr args = expr(nTnum(91),nTnum(92),nTnum(93));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(133, r.Out);                
		}

		[Test]
		public void And_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.And),nTbool(true),nTbool(false));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void And_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.And),nTbool(false),nTbool(true));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void ChainedExpressions ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = 1017 - ((x*2) + 1)");
			Tnum r = (Tnum)sess.ProcessInput("F[22]");
			Assert.AreEqual(972, r.Out);          
		}

		[Test]
		public void Concat ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Concat), nTstr("Hello, "), nTstr("World"));
			Tstr r = (Tstr)sess.eval(exp).obj;
			Assert.AreEqual("Hello, World", r.Out);               
		}

		[Test]
		public void Division ()
		{
			// 42 / 7
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Div),nTnum(42),nTnum(7));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(6, r.Out);                
		}

		[Test]
		public void Equality_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTnum(7),nTnum(9));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTnum(9),nTnum(9));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_3 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTstr("ab"),nTstr("ba"));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Equality_4 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTstr("ab"),nTstr("ab"));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_5 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTbool(true),nTbool(true));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Equality_6 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Eq),nTbool(true),nTbool(false));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_1 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Abs,Tnum:-66}");
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(66, r.Out);                
		}

		[Test]
		public void EvalStringExpr_2 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Mult,Tnum:4,Tnum:2}");
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(8, r.Out);                
		}

		[Test]
		public void EvalStringExpr_3 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Not,Tbool:True}");
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_4 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:And,Tbool:True,Tbool:False}");
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_5 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Abs,Var:0}");
			Expr args = expr(nTnum(-109));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(109, r.Out);                
		}

		[Test]
		public void EvalStringExpr_6 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:And,Tbool:True,Tbool:False}");
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalStringExpr_7 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:And,Tbool:True,Expr:{Op:LsTh,Tnum:5,Tnum:99}}");
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void EvalStringExpr_8 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Minus,Expr:{Op:Mult,Tnum:11,Tnum:6},Tnum:100}");
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(-34, r.Out);                
		}

		[Test]
		public void EvalStringExpr_9 ()
		{
			Session sess = new Session();
			Expr exp = StringToExpr("Expr:{Op:Abs,Expr:{Op:Minus,Expr:{Op:Mult,Tnum:11,Tnum:6},Tnum:100}}");
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(34, r.Out);                
		}

		[Test]
		public void EvalStringExpr_10 ()
		{
			Session sess = new Session();
Expr exp = StringToExpr("Expr:{Op:Minus,Expr:{Op:Mult,Tnum:11,Tnum:6},Tnum:100}");
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(-34, r.Out);                
		}

		[Test]
		public void EvalUserString_1 ()
		{
			Session sess = new Session();
			Tnum r = (Tnum)sess.ProcessInput("Abs[-66]");
			Assert.AreEqual(66, r.Out);           
		}

		[Test]
		public void EvalUserString_2 ()
		{
			Session sess = new Session();
			Tnum r = (Tnum)sess.ProcessInput("(11 * 6) - 100");
			Assert.AreEqual(-34, r.Out);               
		}

		[Test]
		public void EvalUserString_3 ()
		{
			Session sess = new Session();
			Tnum r = (Tnum)sess.ProcessInput("Abs[ (11 * 6) - 100]");
			Assert.AreEqual(34, r.Out);               
		}

		[Test]
		public void EvalUserString_4 ()
		{
			Session sess = new Session();
			Tnum r = (Tnum)sess.ProcessInput("(4+6) * ( 2 + 1 )");
			Assert.AreEqual(30, r.Out);               
		}

		[Test]
		public void EvalUserString_5 ()
		{
			Session sess = new Session();
			Tnum r = (Tnum)sess.ProcessInput("1 + 4 / 2");
			Assert.AreEqual(3, r.Out);               
		}

		[Test]
		public void EvalUserString_6 ()
		{
			Session sess = new Session();
			Tbool r = (Tbool)sess.ProcessInput("true & 5 + 31 > 99 ");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void EvalUserString_7 ()
		{
			Session sess = new Session();
			Tnum r = (Tnum)sess.ProcessInput("Abs[(4+6) * ( 2 + 1 )]");
			Assert.AreEqual(30, r.Out);               
		}

		[Test]
		public void EvalUserString_8 ()
		{
			Session sess = new Session();
			Tbool r = (Tbool)sess.ProcessInput("true & !!false");
			Assert.AreEqual(false, r.Out);                
		}

//		[Test]
//		public void ExprToString_1 ()
//		{
//			// Note: Does not start with "Expr:"
//			Assert.AreEqual("{Op:Plus,Var:0,Tnum:42}", LoadedRules()[1].ToString());                
//		}
//
//		[Test]
//		public void ExprToString_2 ()
//		{
//			Assert.AreEqual("{Op:Plus,Expr:{Op:Mult,Var:0,Tnum:2},Tnum:1}", LoadedRules()[0].ToString());                
//		}

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
//			Expr exp = expr(n(Typ.Fcn,"4"));
//			Expr args = expr(nTnum(34),n(Typ.Fcn,"3"));
//			Tnum r = (Tnum)sess.eval(exp,args).obj;
//			Assert.AreEqual(0.5, r.Out);        

			Session sess = new Session();
			sess.ProcessInput("f[x,y] = y[x,17]");
			sess.ProcessInput("f2[a,b] = b/a");
			Tnum r = (Tnum)sess.ProcessInput("f[34,f2]");
			Assert.AreEqual(0.5, r.Out);          
		}

		[Test]
		public void FunctionCall_1 ()
		{
			// (a * 2) + 1, where a = 44
			Session sess = new Session();
			sess.ProcessInput("F[a] = (a*2) + 1");
			Tnum r = (Tnum)sess.ProcessInput("F[44]");
			Assert.AreEqual(89, r.Out);              
		}

		[Test]
		public void FunctionCall_2 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x * 9");
			Tnum r = (Tnum)sess.ProcessInput("F[3]");
			Assert.AreEqual(27, r.Out);              
		}

		[Test]
		public void FunctionWith2Args ()
		{
			// y / x, where y = 99, x = 11
			Session sess = new Session();
			sess.ProcessInput("f3[x,y] = y / x");
			Tnum r = (Tnum)sess.ProcessInput("f3[11,99]");
			Assert.AreEqual(9, r.Out);             
		}

		[Test]
		public void GreaterThan_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.GrTh),nTnum(7),nTnum(9));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void GreaterThan_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.GrTh),nTnum(9),nTnum(7));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Inequality_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Neq),nTstr("ab"),nTstr("ba"));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Inequality_2()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Neq),nTstr("ab"),nTstr("ab"));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Max_1 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Max),nTnum(7));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(7, r.Out);                
		}

		[Test]
		public void Max_2 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Max),nTnum(7),nTnum(9));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(9, r.Out);                
		}

		[Test]
		public void Min ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Min),nTnum(7),nTnum(9));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(7, r.Out);                
		}

		[Test]
		public void Multiplication_1 ()
		{
			// 41 * 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Mult),nTnum(41),nTnum(9));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(369, r.Out);                
		}

		[Test]
		public void Multiplication_2 ()
		{
			// Unstated * 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Mult),n(Typ.Tnum,new Tnum(Hstate.Unstated)),nTnum(9));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Multiplication_3 ()
		{
			// 0 * 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Mult),nTnum(0),nTnum(9));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void NestedExpression ()
		{
			// (a * 2) + 1, where a = 44
			Session sess = new Session();
			Expr sub = expr(n(Typ.Op,Op.Mult),n(Typ.Var,"0"),nTnum(2));
			Expr exp = expr(n(Typ.Op,Op.Plus),n(Typ.Expr,sub),nTnum(1));
			Expr args = expr(nTnum(44));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(89, r.Out);                
		}

		[Test]
		public void Not ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Not),nTbool(true));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Or ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Or),nTbool(true),nTbool(false));
			Tbool r = (Tbool)sess.eval(exp).obj;
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Recursion_1 ()
		{
			// f7(x) = if x = 0 -> 0, else f7(x-1) + 3
			// f7(0) = 0
			Session sess = new Session();
			Expr exp = expr(n(Typ.Fcn,"7"));
			Expr args = expr(nTnum(0));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void Recursion_2 ()
		{
			// f7(x) = if x = 0 -> 0, else f7(x-1) + 3
			// f7(1) = 3
			Session sess = new Session();
			Expr exp = expr(n(Typ.Fcn,"7"));
			Expr args = expr(nTnum(1));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(3, r.Out);                
		}

		[Test]
		public void Recursion_3 ()
		{
			// f7(x) = if x = 0 -> 0, else f7(x-1) + 3
			// f7(2) = 6
			Session sess = new Session();
			Expr exp = expr(n(Typ.Fcn,"7"));
			Expr args = expr(nTnum(2));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(6, r.Out);                
		}

		[Test]
		public void Recursion_4 ()
		{
			// f7(450) = 1350
			Session sess = new Session();
			Expr exp = expr(n(Typ.Fcn,"7"));
			Expr args = expr(nTnum(450));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(1350, r.Out);                
		}

		[Test]
		public void Reverse ()
		{
			Session sess = new Session();
			Thing A = new Thing ("A");
			Thing B = new Thing ("B");
			Expr exp = expr(n(Typ.Op,Op.Rev), n(Typ.Tset,new Tset(A,B)));
			Tset r = (Tset)sess.eval(exp).obj;
			Assert.AreEqual("B, A", r.Out);               
		}

		[Test]
		public void Subtraction_1 ()
		{
			// 41 - 9
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Minus),nTnum(41),nTnum(9));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(32, r.Out);                
		}

		[Test]
		public void Subtraction_2 ()
		{
			// 41 - Unstated
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Minus),nTnum(41),nTnum(new Tnum(Hstate.Unstated)));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Switch_1 ()
		{
			// if cond=true -> 42, 41
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Switch),nTbool(true),nTnum(42),nTnum(41));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(42, r.Out);              

//			Session sess = new Session();
//			sess.ProcessInput("f3[x,y] = y / x");
//			Tnum r = (Tnum)sess.ProcessInput("f3[11,99]");
//			Assert.AreEqual(9, r.Out);  
		}

		[Test]
		public void Switch_2 ()
		{
			// if cond=true -> 42, 41
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.Switch),nTbool(false),nTnum(42),nTnum(41));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual(41, r.Out);                
		}

		[Test]
		public void Switch_3 ()
		{
			Session sess = new Session();
			Tbool cond = new Tbool(true);
			cond.AddState(new DateTime(2015,1,1), false);

			Expr exp = expr(n(Typ.Op,Op.Switch),nTbool(cond),nTnum(42),nTnum(41));
			Tnum r = (Tnum)sess.eval(exp).obj;
			Assert.AreEqual("{Dawn: 42; 1/1/2015: 41}", r.Out);                
		}

		[Test]
		public void Switch_4 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Fcn,"6"));
			Expr args = expr(nTnum(0));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(1, r.Out);                
		}

		[Test]
		public void Switch_5 ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Fcn,"6"));
			Expr args = expr(nTnum(45));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(45, r.Out);                
		}

		[Test]
		public void ToUSD ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Op,Op.USD), nTnum(42.224));
			Tstr r = (Tstr)sess.eval(exp).obj;
			Assert.AreEqual("$42.22", r.Out);               
		}

		[Test]
		public void Union ()
		{
			Session sess = new Session();
			Thing A = new Thing ("A");
			Thing B = new Thing ("B");
			Expr exp = expr(n(Typ.Op,Op.Union), nTset(A), nTset(B));
			Tset r = (Tset)sess.eval(exp).obj;
			Assert.AreEqual("A, B", r.Out);               
		}

		[Test]
		public void VariableValues ()
		{
			Session sess = new Session();
			Expr exp = expr(n(Typ.Var,"2"));
			Expr args = expr(nTnum(91),nTnum(92),nTnum(93));
			Tnum r = (Tnum)sess.eval(exp,args).obj;
			Assert.AreEqual(93, r.Out);                
		}
	}
}