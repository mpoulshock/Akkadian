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
			Assert.AreEqual("", Util.InnermostParenthetical("x + 3"));                
		}

		[Test]
		public void FirstParen_2 ()
		{
			Assert.AreEqual("(a + 1)", Util.InnermostParenthetical("x + (a + 1) + b"));                
		}

		[Test]
		public void FirstParen_3 ()
		{
			Assert.AreEqual("(a + 1)", Util.InnermostParenthetical("x + (a + 1) + (b - 2)"));                
		}

		[Test]
		public void FirstParen_4 ()
		{
			Assert.AreEqual("(a + 1b - 2)", Util.InnermostParenthetical("x + (a + 1b - 2)"));                
		}

		[Test]
		public void FirstParen_5 ()
		{
			Assert.AreEqual("( 1 -b )", Util.InnermostParenthetical("x + (a + ( 1 -b ) - 2)"));                
		}

		[Test]
		public void FirstParen_6 ()
		{
			Assert.AreEqual("(22*2)", Util.InnermostParenthetical("((22*2) + 1)"));                
		}
	}
}
