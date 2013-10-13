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
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Akkadian.UnitTests
{
	[TestFixture]
	public partial class LexTests : Interpreter
	{
		[Test]
		public void Regex_BinaryBooleanOp_1 ()
		{
			Assert.IsTrue(IsExactMatch("|", binaryOp));
		}

		[Test]
		public void Regex_BoolLiteral_1 ()
		{
			Assert.IsTrue(IsExactMatch("true", boolLiteral));
		}

		[Test]
		public void Regex_Date_1 ()
		{
			Assert.IsTrue(IsExactMatch("2014-10-31", dateLiteral));
		}

		[Test]
		public void Regex_Date_2 ()
		{
			Assert.IsFalse(IsExactMatch("2014-13-31", dateLiteral));
		}

		[Test]
		public void Regex_Date_3 ()
		{
			Assert.IsFalse(IsExactMatch("2014-10-32", dateLiteral));
		}

		[Test]
		public void Regex_DecimalLiteral_1 ()
		{
			Assert.IsTrue(IsExactMatch("2.01", decimalLiteral));
		}

		[Test]
		public void Regex_DecimalLiteral_2 ()
		{
			Assert.IsTrue(IsExactMatch("2", decimalLiteral));
		}

		[Test]
		public void Regex_DecimalLiteral_3 ()
		{
			Assert.IsTrue(IsExactMatch("0.00000001", decimalLiteral));
		}

		[Test]
		public void Regex_DecimalLiteral_4 ()
		{
			Assert.IsTrue(IsExactMatch("123413", decimalLiteral));
		}

		[Test]
		public void Regex_FcnName_1 ()
		{
			Assert.IsTrue(IsExactMatch("IsEligible",fcnName));
		}

		[Test]
		public void Regex_FcnName_2 ()
		{
			Assert.IsFalse(IsExactMatch("2IsEligible",fcnName));
		}

		[Test]
		public void Regex_FcnName_3 ()
		{
			Assert.IsFalse(IsExactMatch("Is Eligible",fcnName));
		}

		[Test]
		public void Regex_FcnName_4 ()
		{
			Assert.IsTrue(IsExactMatch("Is_Eligible",fcnName));
		}

		[Test]
		public void Regex_FcnName_5 ()
		{
			Assert.IsTrue(IsExactMatch("A",fcnName));
		}

		[Test]
		public void Regex_FcnSig_1 ()
		{

			Assert.IsTrue(IsExactMatch("AreRelated(A)",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_2 ()
		{
			Assert.IsTrue(IsExactMatch("AreRelated  (A)",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_3 ()
		{
			Assert.IsTrue(IsExactMatch("Are_Related( A )",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_4 ()
		{
			Assert.IsTrue(IsExactMatch("Are_Related(    A  )",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_5 ()
		{
			Assert.IsTrue(IsExactMatch("AreRelated(A,B)",fcnSignature));
		}


		/// <summary>
		/// Determines if the input string matches the regex exactly.
		/// </summary>
		private static bool IsExactMatch(string s, string regex)
		{
			return s == Regex.Match(s,regex).Groups[0].Value;
		}

		// To test match results:
		// Assert.AreEqual("(A)",Regex.Match("(A)",fcnSignature).Groups[0].Value);
	}
}
