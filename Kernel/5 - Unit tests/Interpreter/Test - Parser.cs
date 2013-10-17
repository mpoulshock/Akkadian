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
			string r = Parse("x + 3").ToString();
			Assert.AreEqual("{Op:Plus, Var:0, Tnum:3}", r);                
		}

		[Test]
		public void Parse_2 ()
		{
			string r = Parse("3.14159").ToString();
			Assert.AreEqual("Tnum:3.14159", r);                
		}

		[Test]
		public void Parse_3 ()
		{
			string r = Parse("false").ToString();
			Assert.AreEqual("Tbool:False", r);                
		}

		[Test]
		public void Parse_4 ()
		{
			string r = Parse("x").ToString();
			Assert.AreEqual("Var:0", r);                
		}

		[Test]
		public void Parse2_1 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:3,{Typ.Op:Op.Mult,Tnum:4,Tnum:2}}", Parse2("3 + (4 * 2)"));                
		}

		[Test]
		public void Parse2_2 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Plus,Tnum:3,{Typ.Op:Op.Mult,Tnum:4,{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}}", 
			                Parse2("3 + (4 * ( 2 + 1 ))"));                
		}

		[Test]
		public void Parse2_3 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Mult,Tnum:4,{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}", Parse2("4 * ( 2 + 1 )"));                
		}

		[Test]
		public void Parse2_4 ()
		{
			Assert.AreEqual("{Typ.Op:Op.Mult,{Typ.Op:Op.Plus,Tnum:4,Tnum:6},{Typ.Op:Op.Plus,Tnum:2,Tnum:1}}", 
			                Parse2("(4+6) * ( 2 + 1 )"));                
		}

//		[Test]
//		public void IO_1 ()
//		{
//			string s = ReadFile();
//			Assert.AreEqual("", s);                
//		}

	}
}
