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
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:3,{Typ.Op:Op.Mult,Tnum:4,Tnum:2}}", ParseFcn("3 + (4 * 2)"));                
		}

		[Test]
		public void Parse_4 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:3,{Typ.Op:Op.Mult,Tnum:4,{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}}", 
			                ParseFcn("3 + (4 * ( 2 + 1 ))"));                
		}

		[Test]
		public void Parse_5 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Mult,Tnum:4,{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}", ParseFcn("4 * ( 2 + 1 )"));                
		}

		[Test]
		public void Parse_6 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Mult,{Typ.Op:Op.Plus,Tnum:4,Tnum:6},{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}", 
			                ParseFcn("(4+6) * ( 2 + 1 )"));                
		}

		[Test]
		public void Parse_7 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:1,{Typ.Op:Op.Div,Tnum:4,Tnum:2}}", ParseFcn("1 + 4 / 2"));                
		}

		[Test]
		public void Parse_8 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,Tbool:False}", ParseFcn("true & false"));                
		}

		[Test]
		public void Parse_9 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.Or,Tbool:False,Tbool:True}}", 
			                ParseFcn("true & ( false | true )"));                
		}

		[Test]
		public void Parse_10 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.GrTh,Tnum:5,Tnum:99}}", 
			                ParseFcn("true & 5 > 99 "));                
		}

		[Test]
		public void Parse_11 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.GrTh,{Typ.Op:Op.Plus,Tnum:5,Tnum:31},Tnum:99}}", 
			                ParseFcn("true & 5 + 31 > 99 "));                
		}

		[Test]
		public void Parse_12 ()
		{
			InitializeParseTest();  // Need this to test parses that reference built-in functions
			Assert.AreEqual("{Typ.Op:Op.Mult,Tnum:33,{Typ.Op:Op.Abs,Tnum:9}}", 
			                ParseFcn("33 * Abs[9]"));                
		}

		[Test]
		public void Parse_13 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Mult,Tnum:33,{Typ.Op:Op.Sin,{Typ.Op:Op.Abs,Tnum:9}}}", 
			                ParseFcn("33 * Sin[Abs[9]]"));                
		}

		[Test]
		public void Parse_14 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Mult,{Typ.Op:Op.Cos,Tnum:33},{Typ.Op:Op.Sin,{Typ.Op:Op.Abs,Tnum:9}}}", 
			                ParseFcn("Cos[33] * Sin[Abs[9]]"));                
		}

		[Test]
		public void Parse_14b ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Mult,{Typ.Op:Op.Sin,{Typ.Op:Op.Abs,Tnum:9}},{Typ.Op:Op.Cos,Tnum:33}}", 
			                ParseFcn("Sin[Abs[9]] * Cos[33]"));                
		}

		[Test]
		public void Parse_15 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Cos,{Typ.Op:Op.Plus,Tnum:33,Tnum:9}}", ParseFcn("Cos[33 + 9]"));                
		}

		[Test]
		public void Parse_16 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Cos,{Typ.Op:Op.Plus,{Typ.Op:Op.Abs,Tnum:33},{Typ.Op:Op.Abs,Tnum:9}}}", 
			                ParseFcn("Cos[Abs[33] + Abs[9]]"));                
		}

		[Test]
		public void Parse_17 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Cos,{Typ.Op:Op.Plus,{Typ.Op:Op.Abs,Tnum:33},{Typ.Op:Op.Abs,Tnum:9}}}", 
			                ParseFcn("Cos[ (Abs[33] + Abs[9]) ]"));                
		}

		[Test]
		public void Parse_18 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Mult,Tnum:1,{Typ.Op:Op.Div,{Typ.Op:Op.Cos,Tnum:33},Tnum:2}}", 
			                ParseFcn("1 * (Cos[33] / 2)"));                
		}

		[Test]
		public void Parse_19 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Abs,{Typ.Op:Op.Cos,Tnum:9},{Typ.Op:Op.Sin,Tnum:3}}", 
			                ParseFcn("Abs[Cos[9],Sin[3]]"));                
		}

		[Test]
		public void Parse_20 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Abs,{Typ.Op:Op.Mult,{Typ.Op:Op.Plus,Tnum:4,Tnum:6},{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}}", 
			                ParseFcn("Abs[(4+6) * ( 2 + 1 )]"));                
		}

		[Test]
		public void Parse_21 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Abs,Tnum:9}", ParseFcn("Abs[9]"));                
		}

		[Test]
		public void Parse_22 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.RndUp,Tnum:9,Tnum:2}", ParseFcn("RoundUp[9,2]"));                
		}

		[Test]
		public void Parse_23 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.RndUp,Tnum:9,{Typ.Op:Op.Abs,Tnum:2}}", ParseFcn("RoundUp[9,Abs[2]]"));                
		}

		[Test]
		public void Parse_24 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Abs,Var:0}", ParseFcn("F[x] = Abs[x]"));                
		}

		[Test]
		public void Parse_25 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Plus,{Typ.Op:Op.Abs,Var:0},{Typ.Op:Op.Cos,Var:0}}", ParseFcn("F[x] = Abs[x] + Cos[x]"));                
		}

		[Test]
		public void Parse_26 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Plus,{Typ.Op:Op.Abs,Var:0},{Typ.Op:Op.Cos,Var:1}}", ParseFcn("F[x,y] = Abs[x] + Cos[y]"));                
		}

		[Test]
		public void Parse_27 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.RndUp,Var:0,Var:1}", ParseFcn("F[x,y] = RoundUp[x,y]"));                
		}

		[Test]
		public void Parse_28 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.RndUp,Var:1,Var:0}", ParseFcn("F[x,y] = RoundUp[y,x]"));                
		}

		[Test]
		public void Parse_29 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Plus,{Typ.Op:Op.Abs,Var:0},Var:1}", ParseFcn("F2[x,y] = Abs[x] + y"));                
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
			Assert.AreEqual("{Typ.Op:Op.Or,{Typ.Op:Op.GrTh,{Typ.Op:Op.Cos,Var:0},Tnum:65},{Typ.Op:Op.LsTh,{Typ.Op:Op.Sin,Var:0},Tnum:17000}}", 
			                ParseFcn("IsEligible[p]=Cos[p] > 65 | Sin[p] < $17,000"));                
		}

		[Test]
		public void Parse_33 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Or,{Typ.Op:Op.GrTh,{Fcn:Age,Var:0},Tnum:65},{Typ.Op:Op.LsTh,{Fcn:Income,Var:0},Tnum:17000}}", 
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
			Assert.AreEqual("{Typ.Op:Op.USD,Var:0}", ParseFcn("F[x] = ToUSD[x]"));                
		}

		[Test]
		public void Parse_36 ()
		{
			// Evaluating this parse would cause an infinite loop, but the parser should correctly handle a recusrive function
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Plus,{Rec:Income,Var:0},Tnum:1}", ParseFcn("Income[p] = Income[p] + 1"));                
		}

		[Test]
		public void Parse_37 ()
		{
			// Another infinite loop...
			InitializeParseTest();
			Assert.AreEqual("{Rec:Income,Var:0}", ParseFcn("Income[p] = Income[p]"));                
		}

		[Test]
		public void Parse_38 ()
		{
			// Another infinite loop...
			InitializeParseTest();
			Assert.AreEqual("{Rec:Income,Tnum:42}", ParseFcn("Income[p] = Income[42]"));                
		}

		[Test]
		public void Parse_39 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Mult,Tnum:4,Tnum:-12}", ParseFcn("4 * -12"));        
		}

		[Test]
		public void Parse_40 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.Not,Tbool:False}}", 
			                ParseFcn("true & !false"));                
		}

		[Test]
		public void Parse_41 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.Not,{Typ.Op:Op.Not,Tbool:False}}}", 
			                ParseFcn("true & !!false"));                
		}

		[Test]
		public void Parse_42 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Not,{Fcn:Over65,Var:0}}", ParseFcn("IsEligible[p] = !Over65[p]"));                
		}

		[Test]
		public void Parse_43 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Or,{Typ.Op:Op.Not,{Typ.Op:Op.And,Tbool:True,Tbool:False}},Tbool:True}", 
			                ParseFcn("!(true & false) | true"));                
		}

		[Test]
		public void Parse_44 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Or,{Typ.Op:Op.Not,Tbool:False},Tbool:True}", ParseFcn("!false | true"));                
		}

		[Test]
		public void Parse_45 ()
		{
			InitializeParseTest();
			Assert.AreEqual("{Typ.Op:Op.Not,{Typ.Op:Op.Or,Tbool:False,Tbool:True}}", ParseFcn("!(false | true)"));                
		}

		private static void InitializeParseTest()
		{
			// Need this to test parses that reference built-in functions
			InitializeOperatorRegistry();
		}

//		[Test]
//		public void IO_1 ()
//		{
//			string s = ReadFile();
//			Assert.AreEqual("", s);                
//		}

	}
}
