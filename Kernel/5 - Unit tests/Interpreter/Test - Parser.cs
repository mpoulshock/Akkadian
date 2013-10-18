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
			string r = Parse("3.14159").ToString();
			Assert.AreEqual("Tnum:3.14159", r);                
		}

		[Test]
		public void Parse_2 ()
		{
			string r = Parse("false").ToString();
			Assert.AreEqual("Tbool:False", r);                
		}

		[Test]
		public void Parse_3 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:3,{Typ.Op:Op.Mult,Tnum:4,Tnum:2}}", Parse("3 + (4 * 2)"));                
		}

		[Test]
		public void Parse_4 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:3,{Typ.Op:Op.Mult,Tnum:4,{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}}", 
			                Parse("3 + (4 * ( 2 + 1 ))"));                
		}

		[Test]
		public void Parse_5 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Mult,Tnum:4,{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}", Parse("4 * ( 2 + 1 )"));                
		}

		[Test]
		public void Parse_6 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Mult,{Typ.Op:Op.Plus,Tnum:4,Tnum:6},{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}", 
			                Parse("(4+6) * ( 2 + 1 )"));                
		}

		[Test]
		public void Parse_7 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:1,{Typ.Op:Op.Div,Tnum:4,Tnum:2}}", Parse("1 + 4 / 2"));                
		}

		[Test]
		public void Parse_8 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,Tbool:False}", Parse("true & false"));                
		}

		[Test]
		public void Parse_9 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.Or,Tbool:False,Tbool:True}}", 
			                Parse("true & ( false | true )"));                
		}

		[Test]
		public void Parse_10 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.GrTh,Tnum:5,Tnum:99}}", 
			                Parse("true & 5 > 99 "));                
		}

		[Test]
		public void Parse_11 ()
		{
			Assert.AreEqual("{Typ.Op:Op.And,Tbool:True,{Typ.Op:Op.GrTh,{Typ.Op:Op.Plus,Tnum:5,Tnum:31},Tnum:99}}", 
			                Parse("true & 5 + 31 > 99 "));                
		}

//		[Test]
//		public void IO_1 ()
//		{
//			string s = ReadFile();
//			Assert.AreEqual("", s);                
//		}

	}
}
