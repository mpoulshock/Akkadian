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
	public partial class GrammarTests : Interpreter
	{
		[Test]
		public void Regex_BoolLiteral_1 ()
		{
			Assert.IsTrue(IsExactMatch("true", boolLiteral));
		}

		[Test]
		public void Regex_Currency_1 ()
		{
			Assert.IsTrue(IsExactMatch("$99", currencyLiteral));
		}

		[Test]
		public void Regex_Currency_2 ()
		{
			Assert.IsTrue(IsExactMatch("$1.07", currencyLiteral));
		}

		[Test]
		public void Regex_Currency_3 ()
		{
			Assert.IsTrue(IsExactMatch("$13,304.07", currencyLiteral));
		}

		[Test]
		public void Regex_Currency_4 ()
		{
			Assert.IsTrue(IsExactMatch("$0.007", currencyLiteral));
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
		public void Regex_DecimalLiteral_5 ()
		{
			Assert.IsTrue(IsExactMatch("7.0", decimalLiteral));
		}

		[Test]
		public void Regex_DecimalLiteral_6 ()
		{
			Assert.IsTrue(IsExactMatch("-42", decimalLiteral));
		}

		[Test]
		public void Regex_DecimalLiteral_7 ()
		{
			Assert.IsTrue(IsExactMatch("3.14159", decimalLiteral));
		}

		[Test]
		public void Regex_DecimalLiteral_8 ()
		{
			Assert.IsTrue(IsExactMatch("0", decimalLiteral));
		}

		[Test]
		public void Regex_FcnName_1 ()
		{
			Assert.IsTrue(IsExactMatch("IsEligible",fcnNameRegex));
		}

		[Test]
		public void Regex_FcnName_2 ()
		{
			Assert.IsFalse(IsExactMatch("2IsEligible",fcnNameRegex));
		}

		[Test]
		public void Regex_FcnName_3 ()
		{
			Assert.IsFalse(IsExactMatch("Is Eligible",fcnNameRegex));
		}

		[Test]
		public void Regex_FcnName_4 ()
		{
			Assert.IsTrue(IsExactMatch("Is_Eligible",fcnNameRegex));
		}

		[Test]
		public void Regex_FcnName_5 ()
		{
			Assert.IsTrue(IsExactMatch("A",fcnNameRegex));
		}

		[Test]
		public void Regex_FcnSig_1 ()
		{

			Assert.IsTrue(IsExactMatch("AreRelated[A]",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_2 ()
		{
			Assert.IsTrue(IsExactMatch("AreRelated  [A]",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_3 ()
		{
			Assert.IsTrue(IsExactMatch("Are_Related[ A ]",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_4 ()
		{
			Assert.IsTrue(IsExactMatch("Are_Related[    A  ]",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_5 ()
		{
			Assert.IsTrue(IsExactMatch("AreRelated[A,B]",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_6 ()
		{
			Assert.IsTrue(IsExactMatch("f2[A]",fcnSignature));
		}

		[Test]
		public void Regex_FcnSig_7 ()
		{
			Assert.IsTrue(IsExactMatch("F[x,y,z]",fcnSignature));
		}

		[Test]
		public void Regex_FcnVar_1 ()
		{
			Assert.IsTrue(IsExactMatch("z",fcnVariable));
		}

		[Test]
		public void Regex_SetLiteral_1 ()
		{
			Assert.IsTrue(IsExactMatch("{}",setLiteral));
		}

		[Test]
		public void Regex_SetLiteral_2 ()
		{
			Assert.IsTrue(IsExactMatch("{A}",setLiteral));
		}

		[Test]
		public void Regex_SetLiteral_3 ()
		{
			Assert.IsTrue(IsExactMatch("{A,B}",setLiteral));
		}

		[Test]
		public void Regex_SetLiteral_4 ()
		{
			Assert.IsTrue(IsExactMatch("{5,3.14159}",setLiteral));
		}

		[Test]
		public void Regex_SetLiteral_5 ()
		{
			Assert.IsTrue(IsExactMatch("{0...9}",setLiteral));
		}

		[Test]
		public void Regex_SetLiteral_6 ()
		{
			Assert.IsFalse(IsExactMatch("{0,",setLiteral));
		}

		[Test]
		public void Regex_SetLiteral_7 ()
		{
			Assert.IsTrue(IsExactMatch("{A,B,{C,D}}",setLiteral));
		}

		[Test]
		public void Regex_SetLiteral_8 ()
		{
			Assert.IsTrue(IsExactMatch("{'A','B','C'}",setLiteral));
		}

		[Test]
		public void Regex_Whitespace_1 ()
		{
			Assert.IsTrue(IsExactMatch("",white));
		}

		[Test]
		public void Regex_Whitespace_2 ()
		{
			Assert.IsTrue(IsExactMatch("  ",white));
		}

		[Test]
		public void Regex_Wildcard_1 ()
		{
			Assert.IsTrue(IsExactMatch("alsdfav898U(PO#(*P!@",wildcard));
		}


		// To test match results:
		// Assert.AreEqual("(A)",Regex.Match("(A)",fcnSignature).Groups[0].Value);
	}
}
