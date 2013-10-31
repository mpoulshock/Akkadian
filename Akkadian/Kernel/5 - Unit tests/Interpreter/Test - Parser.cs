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
	public partial class ParserTests : Interpreter
	{
		[Test]
		public void FirstParen_1 ()
		{
			Assert.AreEqual("", FirstParenthetical("x + 3", "(", ")"));                
		}

		[Test]
		public void FirstParen_2 ()
		{
			Assert.AreEqual("(a + 1)", FirstParenthetical("x + (a + 1) + b", "(", ")"));                
		}

		[Test]
		public void FirstParen_3 ()
		{
			Assert.AreEqual("(a + 1)", FirstParenthetical("x + (a + 1) + (b - 2)", "(", ")"));                
		}

		[Test]
		public void FirstParen_4 ()
		{
			Assert.AreEqual("(a + 1b - 2)", FirstParenthetical("x + (a + 1b - 2)", "(", ")"));                
		}

		[Test]
		public void FirstParen_5 ()
		{
			Assert.AreEqual("(a + ( 1 -b ) - 2)", FirstParenthetical("x + (a + ( 1 -b ) - 2)", "(", ")"));                
		}

		[Test]
		public void Parse_1 ()
		{
			string r = ParseFcn("3.14159").ToString();
			Assert.AreEqual("Tnum:3.14159", r);                
		}

		[Test]
		public void Parse_2 ()
		{
			string r = ParseFcn("false").ToString();
			Assert.AreEqual("Tbool:False", r);                
		}

		[Test]
		public void Parse_3 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Tnum:3,Expr:{Op:Mult,Tnum:4,Tnum:2}}", ParseFcn("3 + (4 * 2)"));                
		}

		[Test]
		public void Parse_4 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Tnum:3,Expr:{Op:Mult,Tnum:4,Expr:{Op:Plus,Tnum:2,Tnum:1}}}", 
			                ParseFcn("3 + (4 * ( 2 + 1 ))"));                
		}

		[Test]
		public void Parse_5 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Mult,Tnum:4,Expr:{Op:Plus,Tnum:2,Tnum:1}}", ParseFcn("4 * ( 2 + 1 )"));                
		}

		[Test]
		public void Parse_6 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Plus,Tnum:4,Tnum:6},Expr:{Op:Plus,Tnum:2,Tnum:1}}", 
			                ParseFcn("(4+6) * ( 2 + 1 )"));                
		}

		[Test]
		public void Parse_7 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Tnum:1,Expr:{Op:Div,Tnum:4,Tnum:2}}", ParseFcn("1 + 4 / 2"));                
		}

		[Test]
		public void Parse_8 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Tbool:False}", ParseFcn("true & false"));                
		}

		[Test]
		public void Parse_9 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Expr:{Op:Or,Tbool:False,Tbool:True}}", 
			                ParseFcn("true & ( false | true )"));                
		}

		[Test]
		public void Parse_10 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Expr:{Op:GrTh,Tnum:5,Tnum:99}}", 
			                ParseFcn("true & 5 > 99 "));                
		}

		[Test]
		public void Parse_11 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Expr:{Op:GrTh,Expr:{Op:Plus,Tnum:5,Tnum:31},Tnum:99}}", 
			                ParseFcn("true & 5 + 31 > 99 "));                
		}

		[Test]
		public void Parse_12 ()
		{
			InitializeParseTest(); 
			Assert.AreEqual("Expr:{Op:Mult,Tnum:33,Expr:{Op:Abs,Tnum:9}}", 
			                ParseFcn("33 * Abs[9]"));                
		}

		[Test]
		public void Parse_13 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Mult,Tnum:33,Expr:{Op:Sin,Expr:{Op:Abs,Tnum:9}}}", 
			                ParseFcn("33 * Sin[Abs[9]]"));                
		}

		[Test]
		public void Parse_14 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Cos,Tnum:33},Expr:{Op:Sin,Expr:{Op:Abs,Tnum:9}}}", 
			                ParseFcn("Cos[33] * Sin[Abs[9]]"));                
		}

		[Test]
		public void Parse_14b ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Sin,Expr:{Op:Abs,Tnum:9}},Expr:{Op:Cos,Tnum:33}}", 
			                ParseFcn("Sin[Abs[9]] * Cos[33]"));                
		}

		[Test]
		public void Parse_15 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Cos,Expr:{Op:Plus,Tnum:33,Tnum:9}}", ParseFcn("Cos[33 + 9]"));                
		}

		[Test]
		public void Parse_16 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Cos,Expr:{Op:Plus,Expr:{Op:Abs,Tnum:33},Expr:{Op:Abs,Tnum:9}}}", 
			                ParseFcn("Cos[Abs[33] + Abs[9]]"));                
		}

		[Test]
		public void Parse_17 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Cos,Expr:{Op:Plus,Expr:{Op:Abs,Tnum:33},Expr:{Op:Abs,Tnum:9}}}", 
			                ParseFcn("Cos[ (Abs[33] + Abs[9]) ]"));                
		}

		[Test]
		public void Parse_18 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Mult,Tnum:1,Expr:{Op:Div,Expr:{Op:Cos,Tnum:33},Tnum:2}}", 
			                ParseFcn("1 * (Cos[33] / 2)"));                
		}

		[Test]
		public void Parse_19 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Abs,Expr:{Op:Cos,Tnum:9},Expr:{Op:Sin,Tnum:3}}", 
			                ParseFcn("Abs[Cos[9],Sin[3]]"));                
		}

		[Test]
		public void Parse_20 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Abs,Expr:{Op:Mult,Expr:{Op:Plus,Tnum:4,Tnum:6},Expr:{Op:Plus,Tnum:2,Tnum:1}}}", 
			                ParseFcn("Abs[(4+6) * ( 2 + 1 )]"));                
		}

		[Test]
		public void Parse_21 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Abs,Tnum:9}", ParseFcn("Abs[9]"));                
		}

		[Test]
		public void Parse_22 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:RndUp,Tnum:9,Tnum:2}", ParseFcn("RoundUp[9,2]"));                
		}

		[Test]
		public void Parse_23 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:RndUp,Tnum:9,Expr:{Op:Abs,Tnum:2}}", ParseFcn("RoundUp[9,Abs[2]]"));                
		}

		[Test]
		public void Parse_24 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Abs,Var:0}", ParseFcn("F[x] = Abs[x]"));                
		}

		[Test]
		public void Parse_25 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Expr:{Op:Abs,Var:0},Expr:{Op:Cos,Var:0}}", ParseFcn("F[x] = Abs[x] + Cos[x]"));                
		}

		[Test]
		public void Parse_26 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Expr:{Op:Abs,Var:0},Expr:{Op:Cos,Var:1}}", ParseFcn("F[x,y] = Abs[x] + Cos[y]"));                
		}

		[Test]
		public void Parse_27 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:RndUp,Var:0,Var:1}", ParseFcn("F[x,y] = RoundUp[x,y]"));                
		}

		[Test]
		public void Parse_28 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:RndUp,Var:1,Var:0}", ParseFcn("F[x,y] = RoundUp[y,x]"));                
		}

		[Test]
		public void Parse_29 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Expr:{Op:Abs,Var:0},Var:1}", ParseFcn("F2[x,y] = Abs[x] + y"));                
		}

		[Test]
		public void Parse_30 ()
		{
			Assert.AreEqual("Tdate:2014-10-22", ParseFcn("2014-10-22"));                
		}

		[Test]
		public void Parse_31 ()
		{
			Assert.AreEqual("Tstr:Horsehair", ParseFcn("'Horsehair'"));                
		}

		[Test]
		public void Parse_32 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:GrTh,Expr:{Op:Cos,Var:0},Tnum:65},Expr:{Op:LsTh,Expr:{Op:Sin,Var:0},Tnum:17000}}", 
			                ParseFcn("IsEligible[p]=Cos[p] > 65 | Sin[p] < $17,000"));                
		}

		[Test]
		public void Parse_33 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:GrTh,Expr:{Fcn:Age,Var:0},Tnum:65},Expr:{Op:LsTh,Expr:{Fcn:Income,Var:0},Tnum:17000}}", 
			                ParseFcn("IsEligible[p] = Age[p] > 65 | Income[p] < $17,000"));                
		}

		[Test]
		public void Parse_34 ()
		{
			Assert.AreEqual("Tnum:17000", ParseFcn("$17,000"));                
		}

		[Test]
		public void Parse_35 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:USD,Var:0}", ParseFcn("F[x] = ToUSD[x]"));                
		}

		[Test]
		public void Parse_36 ()
		{
			// Evaluating this parse would cause an infinite loop, but the parser should correctly handle a recusrive function
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Expr:{Fcn:Income,Var:0},Tnum:1}", ParseFcn("Income[p] = Income[p] + 1"));                
		}

		[Test]
		public void Parse_37 ()
		{
			// Another infinite loop...
			InitializeParseTest();
			Assert.AreEqual("Expr:{Fcn:Income,Var:0}", ParseFcn("Income[p] = Income[p]"));                
		}

		[Test]
		public void Parse_38 ()
		{
			// Another infinite loop...
			InitializeParseTest();
			Assert.AreEqual("Expr:{Fcn:Income,Tnum:42}", ParseFcn("Income[p] = Income[42]"));                
		}

		[Test]
		public void Parse_39 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Mult,Tnum:4,Tnum:-12}", ParseFcn("4 * -12"));        
		}

		[Test]
		public void Parse_40 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Expr:{Op:Not,Tbool:False}}", 
			                ParseFcn("true & !false"));                
		}

		[Test]
		public void Parse_41 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Expr:{Op:Not,Expr:{Op:Not,Tbool:False}}}", 
			                ParseFcn("true & !!false"));                
		}

		[Test]
		public void Parse_42 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Not,Expr:{Fcn:Over65,Var:0}}", ParseFcn("IsEligible[p] = !Over65[p]"));                
		}

		[Test]
		public void Parse_43 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:Not,Expr:{Op:And,Tbool:True,Tbool:False}},Tbool:True}", 
			                ParseFcn("!(true & false) | true"));                
		}

		[Test]
		public void Parse_44 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:Not,Tbool:False},Tbool:True}", ParseFcn("!false | true"));                
		}

		[Test]
		public void Parse_45 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Not,Expr:{Op:Or,Tbool:False,Tbool:True}}", ParseFcn("!(false | true)"));                
		}

		[Test]
		public void Parse_46 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Expr:{Op:GrEq,Tnum:5,Tnum:99}}", 
			                ParseFcn("true & 5 >= 99 "));                
		}

		public void Parse_47 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Plus,Tnum:1,Expr:{Op:Div,Tnum:4,Tnum:2}}", ParseFcn("1 + 4 / 2"));                
		}

		[Test]
		public void Parse_48 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Minus,Expr:{Op:Mult,Tnum:11,Tnum:6},Tnum:100}", ParseFcn("(11 * 6) - 100"));                
		}

		[Test]
		public void Parse_49 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Abs,Expr:{Op:Minus,Expr:{Op:Mult,Tnum:11,Tnum:6},Tnum:100}}", ParseFcn("Abs[ (11 * 6) - 100]"));                
		}

		[Test]
		public void Parse_50 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Fcn:Income,Tnum:34}", ParseFcn("Income[34]"));                
		}

		[Test]
		public void StringToNode_1 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:Abs,Var:0}", StringToNode("Expr:{Op:Abs,Var:0}").ToString());                
		}

		[Test]
		public void StringToNode_2 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:LsTh,Tnum:5,Tnum:99}", StringToNode("Expr:{Op:LsTh,Tnum:5,Tnum:99}").ToString());                
		}

		[Test]
		public void StringToNode_3 ()
		{
			InitializeParseTest();
			Assert.AreEqual("Expr:{Op:And,Tbool:True,Expr:{Op:LsTh,Tnum:5,Tnum:99}}", 
			                StringToNode("Expr:{Op:And,Tbool:True,Expr:{Op:LsTh,Tnum:5,Tnum:99}}").ToString());                
		}


		private static void InitializeParseTest()
		{
			// Need this to test parses that reference built-in functions
			InitializeOperatorRegistry();

			FcnTable.ClearFunctionTable();
		}

//		[Test]
//		public void IO_1 ()
//		{
//			string s = ReadFile();
//			Assert.AreEqual("", s);                
//		}
	}
}
