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
			Assert.AreEqual("", Util.FirstParenthetical("x + 3", "(", ")"));                
		}

		[Test]
		public void FirstParen_2 ()
		{
			Assert.AreEqual("(a + 1)", Util.FirstParenthetical("x + (a + 1) + b", "(", ")"));                
		}

		[Test]
		public void FirstParen_3 ()
		{
			Assert.AreEqual("(a + 1)", Util.FirstParenthetical("x + (a + 1) + (b - 2)", "(", ")"));                
		}

		[Test]
		public void FirstParen_4 ()
		{
			Assert.AreEqual("(a + 1b - 2)", Util.FirstParenthetical("x + (a + 1b - 2)", "(", ")"));                
		}

		[Test]
		public void FirstParen_5 ()
		{
			Assert.AreEqual("(a + ( 1 -b ) - 2)", Util.FirstParenthetical("x + (a + ( 1 -b ) - 2)", "(", ")"));                
		}

		[Test]
		public void FirstParen_6 ()
		{
			Assert.AreEqual("((22*2) + 1)", Util.FirstParenthetical("((22*2) + 1)", "(", ")"));                
		}

//		[Test]
//		public void Parse_1 ()
//		{
//			string r = ParseFcn("3.14159").ToString();
//			Assert.AreEqual("Tvar:3.14159", r);                
//		}
//
//		[Test]
//		public void Parse_2 ()
//		{
//			string r = ParseFcn("false").ToString();
//			Assert.AreEqual("Tvar:False", r);                
//		}
//
//		[Test]
//		public void Parse_3 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Plus,Tvar:3,Expr:{Op:Mult,Tvar:4,Tvar:2}}", ParseFcn("3 + (4 * 2)"));                
//		}
//
//		[Test]
//		public void Parse_4 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Plus,Tvar:3,Expr:{Op:Mult,Tvar:4,Expr:{Op:Plus,Tvar:2,Tvar:1}}}", 
//			                ParseFcn("3 + (4 * ( 2 + 1 ))"));                
//		}
//
//		[Test]
//		public void Parse_5 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Mult,Tvar:4,Expr:{Op:Plus,Tvar:2,Tvar:1}}", ParseFcn("4 * ( 2 + 1 )"));                
//		}
//
//		[Test]
//		public void Parse_6 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Plus,Tvar:4,Tvar:6},Expr:{Op:Plus,Tvar:2,Tvar:1}}", 
//			                ParseFcn("(4+6) * ( 2 + 1 )"));                
//		}
//
//		[Test]
//		public void Parse_7 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Plus,Tvar:1,Expr:{Op:Div,Tvar:4,Tvar:2}}", ParseFcn("1 + 4 / 2"));                
//		}
//
//		[Test]
//		public void Parse_8 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Tvar:False}", ParseFcn("true & false"));                
//		}
//
//		[Test]
//		public void Parse_9 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Expr:{Op:Or,Tvar:False,Tvar:True}}", 
//			                ParseFcn("true & ( false | true )"));                
//		}
//
//		[Test]
//		public void Parse_10 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Expr:{Op:GrTh,Tvar:5,Tvar:99}}", 
//			                ParseFcn("true & 5 > 99 "));                
//		}
//
//		[Test]
//		public void Parse_11 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Expr:{Op:GrTh,Expr:{Op:Plus,Tvar:5,Tvar:31},Tvar:99}}", 
//			                ParseFcn("true & 5 + 31 > 99 "));                
//		}
//
//		[Test]
//		public void Parse_12 ()
//		{
//			InitializeParseTest(); 
//			Assert.AreEqual("Expr:{Op:Mult,Tvar:33,Expr:{Op:Abs,Tvar:9}}", 
//			                ParseFcn("33 * Abs[9]"));                
//		}
//
//		[Test]
//		public void Parse_13 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Mult,Tvar:33,Expr:{Op:Sin,Expr:{Op:Abs,Tvar:9}}}", 
//			                ParseFcn("33 * Sin[Abs[9]]"));                
//		}
//
//		[Test]
//		public void Parse_14 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Cos,Tvar:33},Expr:{Op:Sin,Expr:{Op:Abs,Tvar:9}}}", 
//			                ParseFcn("Cos[33] * Sin[Abs[9]]"));                
//		}
//
//		[Test]
//		public void Parse_14b ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Sin,Expr:{Op:Abs,Tvar:9}},Expr:{Op:Cos,Tvar:33}}", 
//			                ParseFcn("Sin[Abs[9]] * Cos[33]"));                
//		}
//
//		[Test]
//		public void Parse_15 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Cos,Expr:{Op:Plus,Tvar:33,Tvar:9}}", ParseFcn("Cos[33 + 9]"));                
//		}
//
//		[Test]
//		public void Parse_16 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Cos,Expr:{Op:Plus,Expr:{Op:Abs,Tvar:33},Expr:{Op:Abs,Tvar:9}}}", 
//			                ParseFcn("Cos[Abs[33] + Abs[9]]"));                
//		}
//
//		[Test]
//		public void Parse_17 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Cos,Expr:{Op:Plus,Expr:{Op:Abs,Tvar:33},Expr:{Op:Abs,Tvar:9}}}", 
//			                ParseFcn("Cos[ (Abs[33] + Abs[9]) ]"));                
//		}
//
//		[Test]
//		public void Parse_18 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Mult,Tvar:1,Expr:{Op:Div,Expr:{Op:Cos,Tvar:33},Tvar:2}}", 
//			                ParseFcn("1 * (Cos[33] / 2)"));                
//		}
//
//		[Test]
//		public void Parse_19 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Abs,Expr:{Op:Cos,Tvar:9},Expr:{Op:Sin,Tvar:3}}", 
//			                ParseFcn("Abs[Cos[9],Sin[3]]"));                
//		}
//
//		[Test]
//		public void Parse_20 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Abs,Expr:{Op:Mult,Expr:{Op:Plus,Tvar:4,Tvar:6},Expr:{Op:Plus,Tvar:2,Tvar:1}}}", 
//			                ParseFcn("Abs[(4+6) * ( 2 + 1 )]"));                
//		}
//
//		[Test]
//		public void Parse_21 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Abs,Tvar:9}", ParseFcn("Abs[9]"));                
//		}
//
//		[Test]
//		public void Parse_22 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:RndUp,Tvar:9,Tvar:2}", ParseFcn("RoundUp[9,2]"));                
//		}
//
//		[Test]
//		public void Parse_23 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:RndUp,Tvar:9,Expr:{Op:Abs,Tvar:2}}", ParseFcn("RoundUp[9,Abs[2]]"));                
//		}
//
//		[Test]
//		public void Parse_24 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x] = Abs[x]");
//			Assert.AreEqual("Expr:{Op:Abs,Var:0}", pr.ParserString);                
//		}
//
//		[Test]
//		public void Parse_25 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x] = Abs[x] + Cos[x]");
//			Assert.AreEqual("Expr:{Op:Plus,Expr:{Op:Abs,Var:0},Expr:{Op:Cos,Var:0}}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_26 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y] = Abs[x] + Cos[y]");
//			Assert.AreEqual("Expr:{Op:Plus,Expr:{Op:Abs,Var:0},Expr:{Op:Cos,Var:1}}", pr.ParserString);                
//		}
//
//		[Test]
//		public void Parse_27 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y] = RoundUp[x,y]");
//			Assert.AreEqual("Expr:{Op:RndUp,Var:0,Var:1}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_28 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y] = RoundUp[y,x]");
//			Assert.AreEqual("Expr:{Op:RndUp,Var:1,Var:0}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_29 ()
//		{
//			ParserResponse pr = ParseInputLine("F2[x,y] = Abs[x] + y");
//			Assert.AreEqual("Expr:{Op:Plus,Expr:{Op:Abs,Var:0},Var:1}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_30 ()
//		{
//			Assert.AreEqual("Tvar:2014-10-22", ParseFcn("2014-10-22"));                
//		}
//
//		[Test]
//		public void Parse_31 ()
//		{
//			Assert.AreEqual("Tvar:Horsehair", ParseFcn("'Horsehair'"));                
//		}
//
//		[Test]
//		public void Parse_32 ()
//		{
//			ParserResponse pr = ParseInputLine("IsEligible[p]=Cos[p] > 65 | Sin[p] < $17,000");
//			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:GrTh,Expr:{Op:Cos,Var:0},Tvar:65},Expr:{Op:LsTh,Expr:{Op:Sin,Var:0},Tvar:17000}}", pr.ParserString);           
//		}
//
//		[Test]
//		public void Parse_33 ()
//		{
//			ParserResponse pr = ParseInputLine("IsEligible[p] = Age[p] > 65 | Income[p] < $17,000");
//			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:GrTh,Expr:{Fcn:Age,Var:0},Tvar:65},Expr:{Op:LsTh,Expr:{Fcn:Income,Var:0},Tvar:17000}}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_34 ()
//		{
//			Assert.AreEqual("Tvar:17000", ParseFcn("$17,000"));                
//		}
//
//		[Test]
//		public void Parse_35 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x] = ToUSD[x]");
//			Assert.AreEqual("Expr:{Op:ToUSD,Var:0}", pr.ParserString);                             
//		}
//
//		[Test]
//		public void Parse_36 ()
//		{
//			// Evaluating this parse would cause an infinite loop, but the parser should correctly handle a recusrive function
//			ParserResponse pr = ParseInputLine("Income[p] = Income[p] + 1");
//			Assert.AreEqual("Expr:{Op:Plus,Expr:{Fcn:Income,Var:0},Tvar:1}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_37 ()
//		{
//			// Another infinite loop...
//			ParserResponse pr = ParseInputLine("Income[p] = Income[p]");
//			Assert.AreEqual("Expr:{Fcn:Income,Var:0}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_38 ()
//		{
//			// Another infinite loop...
//			ParserResponse pr = ParseInputLine("Income[p] = Income[42]");
//			Assert.AreEqual("Expr:{Fcn:Income,Tvar:42}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_39_mult_by_neg ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Mult,Tvar:4,Tvar:-12}", ParseFcn("4 * -12"));        
//		}
//
//		[Test]
//		public void Parse_40 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Expr:{Op:Not,Tvar:False}}", 
//			                ParseFcn("true & !false"));                
//		}
//
//		[Test]
//		public void Parse_41 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Expr:{Op:Not,Expr:{Op:Not,Tvar:False}}}", 
//			                ParseFcn("true & !!false"));                
//		}
//
//		[Test]
//		public void Parse_42 ()
//		{
//			ParserResponse pr = ParseInputLine("IsEligible[p] = !Over65[p]");
//			Assert.AreEqual("Expr:{Op:Not,Expr:{Fcn:Over65,Var:0}}", pr.ParserString);                
//		}
//
//		[Test]
//		public void Parse_43 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:Not,Expr:{Op:And,Tvar:True,Tvar:False}},Tvar:True}", 
//			                ParseFcn("!(true & false) | true"));                
//		}
//
//		[Test]
//		public void Parse_44 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Or,Expr:{Op:Not,Tvar:False},Tvar:True}", ParseFcn("!false | true"));                
//		}
//
//		[Test]
//		public void Parse_45 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Not,Expr:{Op:Or,Tvar:False,Tvar:True}}", ParseFcn("!(false | true)"));                
//		}
//
//		[Test]
//		public void Parse_46 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Expr:{Op:GrEq,Tvar:5,Tvar:99}}", 
//			                ParseFcn("true & 5 >= 99 "));                
//		}
//
//		public void Parse_47 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Plus,Tvar:1,Expr:{Op:Div,Tvar:4,Tvar:2}}", ParseFcn("1 + 4 / 2"));                
//		}
//
//		[Test]
//		public void Parse_48 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Minus,Expr:{Op:Mult,Tvar:11,Tvar:6},Tvar:100}", ParseFcn("(11 * 6) - 100"));                
//		}
//
//		[Test]
//		public void Parse_49 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Abs,Expr:{Op:Minus,Expr:{Op:Mult,Tvar:11,Tvar:6},Tvar:100}}", ParseFcn("Abs[ (11 * 6) - 100]"));                
//		}
//
//		[Test]
//		public void Parse_50 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Fcn:Income,Tvar:34}", ParseFcn("Income[34]"));                
//		}
//
//		[Test]
//		public void Parse_51 ()
//		{
//			ParserResponse pr = ParseInputLine("true -> 42, 0");
//			Assert.AreEqual("Expr:{Op:Switch,Tvar:True,Tvar:42,Tvar:0}", pr.ParserString);                 
//		}
//
//		[Test]
//		public void Parse_52 ()
//		{
//			ParserResponse pr = ParseInputLine("false -> 42, true -> 41, 0");
//			Assert.AreEqual("Expr:{Op:Switch,Tvar:False,Tvar:42,Tvar:True,Tvar:41,Tvar:0}", pr.ParserString);                
//		}
//
//		[Test]
//		public void Parse_53 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y] = x -> 42, y -> 41, 0");
//			Assert.AreEqual("Expr:{Op:Switch,Var:0,Tvar:42,Var:1,Tvar:41,Tvar:0}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_54 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x] = x==3 -> 42, x>0  -> 41, 0");
//			Assert.AreEqual("Expr:{Op:Switch,Expr:{Op:Eq,Var:0,Tvar:3},Tvar:42,Expr:{Op:GrTh,Var:0,Tvar:0},Tvar:41,Tvar:0}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_55 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y] = (x -> 42, y -> 41, 0) * 3");
//			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Switch,Var:0,Tvar:42,Var:1,Tvar:41,Tvar:0},Tvar:3}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_56 ()
//		{
//			// Without the parentheses, the parser things the commas are separating arguments to the Abs function
//			ParserResponse pr = ParseInputLine("F[x,y] = Abs[(x -> -42, y -> -41, 0)]");
//			Assert.AreEqual("Expr:{Op:Abs,Expr:{Op:Switch,Var:0,Tvar:-42,Var:1,Tvar:-41,Tvar:0}}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_58 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x] = x==0 -> 0, F[x-1]+3");
//			Assert.AreEqual("Expr:{Op:Switch,Expr:{Op:Eq,Var:0,Tvar:0},Tvar:0,Expr:{Op:Plus,Expr:{Fcn:F,Expr:{Op:Minus,Var:0,Tvar:1}},Tvar:3}}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_59 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x] = x");
//			Assert.AreEqual("Expr:{Var:0}", pr.ParserString);        
//		}
//
//		[Test]
//		public void Parse_60 ()
//		{
//			ParserResponse pr = ParseInputLine("Pi = 3.14159");
//			Assert.AreEqual("Expr:{Tvar:3.14159}", pr.ParserString);               
//		}
//
//		[Test]
//		public void Parse_61 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Abs,Tvar:9.1}", ParseFcn("Abs[9.1]"));                
//		}
//
//		[Test]
//		public void Parse_62 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Fcn:Age,Fcn:martha}", ParseFcn("Age[martha]"));                
//		}
//
//		[Test]
//		public void Parse_63 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Fcn:Age,Tvar:martha}", ParseFcn("Age['martha']"));                
//		}
//
//		[Test]
//		public void Parse_64 ()
//		{
//			ParserResponse pr = ParseInputLine("Test[x] = Over9Employees[x] |> EverPer[TheYear]");
//			Assert.AreEqual("Expr:{Op:Pipe,Expr:{Fcn:Over9Employees,Var:0},Expr:{Op:EverPer,Op:TheYear}}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_65 ()
//		{
//			ParserResponse pr = ParseInputLine("-9.1 |> Abs");
//			Assert.AreEqual("Expr:{Op:Pipe,Tvar:-9.1,Op:Abs}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_66 ()
//		{
//			ParserResponse pr = ParseInputLine("33 |> F[2]");
//			Assert.AreEqual("Expr:{Op:Pipe,Tvar:33,Expr:{Fcn:F,Tvar:2}}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_67 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y] = x * y");
//			Assert.AreEqual("Expr:{Op:Mult,Var:0,Var:1}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_68 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y,z] = x * y + z");
//			Assert.AreEqual("Expr:{Op:Plus,Expr:{Op:Mult,Var:0,Var:1},Var:2}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_69 ()
//		{
//			ParserResponse pr = ParseInputLine("F[x,y,z] = x * y * z");
//			Assert.AreEqual("Expr:{Op:Mult,Expr:{Op:Mult,Var:0,Var:1},Var:2}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_70 ()
//		{
//			ParserResponse pr = ParseInputLine("{A,B,C}");
//			Assert.AreEqual("Tvar:A+B+C", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_71 ()
//		{
//			ParserResponse pr = ParseInputLine("{A}");
//			Assert.AreEqual("Tvar:A", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_72 ()
//		{
//			ParserResponse pr = ParseInputLine("{}");
//			Assert.AreEqual("Tvar:*", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_73 ()
//		{
//			ParserResponse pr = ParseInputLine("SomeSet = {A,B,C}");
//			Assert.AreEqual("Expr:{Tvar:A+B+C}", pr.ParserString);              
//		}
//
//		[Test]
//		public void Parse_74 ()
//		{
//			ParserResponse pr = ParseInputLine("FedMinWage = {Dawn:Stub,2009-07-24:$7.25}");
//			Assert.AreEqual("Expr:{Series:,Dawn,Stub,2009-07-24,$7.25}", pr.ParserString);              
//		}

//		[Test]
//		public void StringToNode_1 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Abs,Var:0}", StringToNode("Expr:{Op:Abs,Var:0}").ToString());                
//		}
//
//		[Test]
//		public void StringToNode_2 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:LsTh,Tvar:5,Tvar:99}", StringToNode("Expr:{Op:LsTh,Tvar:5,Tvar:99}").ToString());                
//		}
//
//		[Test]
//		public void StringToNode_3 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:And,Tvar:True,Expr:{Op:LsTh,Tvar:5,Tvar:99}}", 
//			                StringToNode("Expr:{Op:And,Tvar:True,Expr:{Op:LsTh,Tvar:5,Tvar:99}}").ToString());                
//		}
//
//		[Test]
//		public void StringToNode_4 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Switch,Tvar:False,Tvar:42,Tvar:True,Tvar:41,Tvar:0}", 
//			                StringToNode("Expr:{Op:Switch,Tvar:False,Tvar:42,Tvar:True,Tvar:41,Tvar:0}").ToString());                
//		}
//
//		[Test]
//		public void StringToNode_5 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Op:Switch,Expr:{Op:Eq,Var:0,Tvar:0},Tvar:0,Expr:{Op:Plus,Expr:{Fcn:F,Expr:{Op:Minus,Var:0,Tvar:1}},Tvar:3}}", 
//			                StringToNode("Expr:{Op:Switch,Expr:{Op:Eq,Var:0,Tvar:0},Tvar:0,Expr:{Op:Plus,Expr:{Fcn:F,Expr:{Op:Minus,Var:0,Tvar:1}},Tvar:3}}").ToString());                
//		}
//
//		[Test]
//		public void StringToNode_6 ()
//		{
//			InitializeParseTest();
//			Assert.AreEqual("Expr:{Tvar:3.14159}", StringToNode("Expr:{Tvar:3.14159}").ToString());                
//		}

		private static void InitializeParseTest()
		{
			// Need this to test parses that reference built-in functions
			InitializeOperatorRegistry();
		}
	}
}
