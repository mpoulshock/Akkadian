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
	public class EvalMisc : Interpreter
	{
		[Test]
		public void Misc_1 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Misc_2 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 + (4 * 2)");
			Assert.AreEqual(11, r.Out);                
		}

		[Test]
		public void Misc_3_Parens ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 + (4 * ( 2 + 1 ))");
			Assert.AreEqual(15, r.Out);                
		}

		[Test]
		public void Misc_4 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("(4+6) * ( 2 + 1 )");
			Assert.AreEqual(30, r.Out);                
		}

		[Test]
		public void Misc_5 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("1 + 4 / 2");
			Assert.AreEqual(3, r.Out);                
		}

		[Test]
		public void Misc_6 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & ( false | true )");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Misc_7 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & 5 > 99");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Misc_8 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("true & 5 + 31 > 99");
			Assert.AreEqual(false, r.Out);                
		}

		[Test]
		public void Misc_9 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 * Abs[-9]");
			Assert.AreEqual(27, r.Out);                
		}

		[Test]
		public void Misc_10 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("RoundUp[0.01,Sin[Abs[9]] * Cos[33]]");
			Assert.AreEqual(0.01, r.Out);                
		}

		[Test]
		public void Misc_11 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[9 + -11]");
			Assert.AreEqual(2, r.Out);                
		}

		[Test]
		public void Misc_12 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("3 * (Abs[-32] / 2)");
			Assert.AreEqual(48, r.Out);                
		}

		[Test]
		public void Misc_13 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Abs[(4+6) * ( 2 + 1 )]");
			Assert.AreEqual(30, r.Out);                
		}

		[Test]
		public void Misc_14 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("RoundUp[10,Abs[2]]");
			Assert.AreEqual(10, r.Out);                
		}

		[Test]
		public void Misc_15 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = Abs[x]");
			Tvar r = (Tvar)sess.ProcessInput("F[-3]");
			Assert.AreEqual(3, r.Out);                
		}

		[Test]
		public void Misc_16 ()
		{
			Session sess = new Session();
			sess.ProcessInput("IsEligible[p] = Age[p] > 65 | Income[p] < $17,000");
			Tvar r = (Tvar)sess.ProcessInput("IsEligible[\"jim\"]");
			Assert.AreEqual("Unstated", r.Out);                
		}

		[Test]
		public void Misc_17 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y] = Abs[x] + Cos[y]");
			Tvar r = (Tvar)sess.ProcessInput("F[5,1] |> RoundUp[0.001]");
			Assert.AreEqual(5.541, r.Out);                
		}

		[Test]
		public void Misc_18 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Factorial[n] = n == 1 -> 1, n * Factorial[n-1]");
			Tvar r = (Tvar)sess.ProcessInput("Factorial[4]");
			Assert.AreEqual(24, r.Out);                
		}

		[Test]
		public void Misc_19_MultByNegativeNum ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("4 * -12");
			Assert.AreEqual(-48, r.Out);                
		}

		[Test]
		public void Misc_20_MultByNegativeNum ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("-4 * 12");
			Assert.AreEqual(-48, r.Out);                
		}

		//		[Test]
		//		public void Misc_21 ()
		//		{
		//			Session sess = new Session();
		//			sess.ProcessInput("IsEligible[p] = !Over65[p]");
		//			sess.ProcessInput("Over65['jon'] = false");
		//			Tvar r = (Tvar)sess.ProcessInput("IsEligible['jon']");
		//			Assert.AreEqual(true, r.Out);                
		//		}

		[Test]
		public void Misc_22 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("!(true & false) | true");
			Assert.AreEqual(true, r.Out);                
		}

		[Test]
		public void Misc_23 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x] = x");
			Tvar r = (Tvar)sess.ProcessInput("F[4]");
			Assert.AreEqual(4, r.Out);                
		}

		[Test]
		public void Misc_24 ()
		{
			Session sess = new Session();
			sess.ProcessInput("Pi = 4");
			Tvar r = (Tvar)sess.ProcessInput("Pi");
			Assert.AreEqual(4, r.Out);                
		}

		[Test]
		public void Misc_25 ()
		{
			Session sess = new Session();
			sess.ProcessInput("F[x,y,z] = x * y + z");
			Tvar r = (Tvar)sess.ProcessInput("F[3,4,5]");
			Assert.AreEqual(17, r.Out);                
		}

		[Test]
		public void Misc_27_EmptySet ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{  }");
			Assert.AreEqual("{}", r.Out);                
		}

		[Test]
		public void Misc_28 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7,9} |> SetSum");
			Assert.AreEqual(26, r.Out);                
		}

		[Test]
		public void Misc_29 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7,9} |> SetMax");
			Assert.AreEqual(9, r.Out);                
		}

		[Test]
		public void Misc_30 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7,9} |> SetMin");
			Assert.AreEqual(2, r.Out);                
		}

		[Test]
		public void Misc_31 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("WeekDiff[2014-01-01,2020-12-15]");
			Assert.AreEqual("362.857", r.ToString());                
		}

		[Test]
		public void Misc_32 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{} |> Count");
			Assert.AreEqual(0, r.Out);                
		}

		[Test]
		public void Misc_33 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{}");
			Assert.AreEqual(1, r.IntervalValues.Count);                
		}

		[Test]
		public void Misc_34 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("\"homer\"");
			Assert.AreEqual("homer", r.Out);                
		}

		[Test]
		public void Misc_35 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("YearDiff[2014-01-01,2020-12-15]");
			Assert.AreEqual(6.956, r.Out);                
		}

		[Test]
		public void Misc_36 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("YearDiff[2014-01-01,2020-12-15]");
			Assert.AreEqual("6.956", r.ToString());                
		}

		[Test]
		public void Misc_37_Tset_list ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("kids = {\"Mary\",\"Jill\",\"Laura\"}");
			Assert.AreEqual("{Mary,Jill,Laura}", r.ToString());                
		}

		[Test]
		public void Misc_38 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{Dawn: Stub, 2000-01-01: true, 2010-12-31: false} & {Dawn: false, 1985-12-16: true, 2018-06-14: false}");
			Assert.AreEqual("{Dawn: false, 1985-12-16: Stub, 2000-01-01: true, 2010-12-31: false}", r.ToString());                
		}

		[Test]
		public void Misc_39_unnecessary_parens_in_pipeline ()
		{
			// EverPer[TheYear] is getting parse as #1# but not added to subExpr list ???
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {1800-01-01: $0, 2008-07-24: $6.55, 2009-07-24: $7.25}");
			Tvar r = (Tvar)sess.ProcessInput("(FedMinWage > 7) |> EverPer[TheYear]");
			Assert.AreEqual("{Dawn: false, 2009-01-01: true}", r.ToString());                
		}

		[Test]
		public void Misc_40 ()
		{
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {1800-01-01: $0, 2008-07-24: $6.55, 2009-07-24: $7.25}");
			Tvar r = (Tvar)sess.ProcessInput("FedMinWage > 7");
			Assert.AreEqual("{Dawn: false, 2009-07-24: true}", r.ToString());                
		}

		[Test]
		public void Misc_41 ()
		{
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {1800-01-01: $0, 2008-07-24: $6.55, 2009-07-24: $7.25}");
			Tvar r = (Tvar)sess.ProcessInput("{Dawn: false, 2009-07-24: true} |> EverPer[TheYear]");
			Assert.AreEqual("{Dawn: false, 2009-01-01: true}", r.ToString());                
		}

		[Test]
		public void Misc_42 ()
		{
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {1800-01-01: $0, 2008-07-24: $6.55, 2009-07-24: $7.25}");
			Tvar r = (Tvar)sess.ProcessInput("EverPer[TheYear, FedMinWage > 7]");
			Assert.AreEqual("{Dawn: false, 2009-01-01: true}", r.ToString());                
		}

		[Test]
		public void Misc_43 ()
		{
			Session sess = new Session();
			sess.ProcessInput("FedMinWage = {1800-01-01: $0, 2008-07-24: $6.55, 2009-07-24: $7.25}");
			Tvar r = (Tvar)sess.ProcessInput("EverPer[TheYear, (FedMinWage > 7)]");
			Assert.AreEqual("{Dawn: false, 2009-01-01: true}", r.ToString());                
		}

		[Test]
		public void Misc_44 ()
		{
			Session sess = new Session();
			sess.ProcessInput("MeetsTest = {Dawn: False, 2014-03-15: True, 2014-05-12: False, 2014-07-03: True}");
			Tvar r = (Tvar)sess.ProcessInput("MeetsTest |> EverPer[TheWeek]");
			Assert.AreEqual("{Dawn: false, 2014-03-15: true, 2014-05-17: false, 2014-06-28: true}", r.ToString());                
		}

		[Test]
		public void Misc_45_Regularize ()
		{
			Session sess = new Session();
			sess.ProcessInput("MeetsTest = {Dawn: False, 2014-03-15: True, 2014-05-12: False, 2014-07-03: True}");
			Tvar r = (Tvar)sess.ProcessInput("MeetsTest |> EverPer[TheWeek] |> Regularize[TheWeek] |> CountPer[TheYear]");
			Assert.AreEqual("{ }", r.ToString());                
		}

		[Test]
		public void Misc_46_Regularize ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{Dawn: false, 2009-01-01: true, 2015-12-31: false} |> Regularize[TheWeek] |> CountPer[TheYear]");
			Assert.AreEqual("{Dawn: 0, 2009-01-01: 51, 2010-01-01: 52, 2012-01-01: 51, 2016-01-01: 0}", r.ToString());                
		}

		[Test]
		public void Misc_47_Contains_nums ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{2,3,4} |> Contains[2]");
			Assert.AreEqual("True", r.ToString());                
		}

		[Test]
		public void Misc_48 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("4-3-2 == (4-3)-2");
			Assert.AreEqual("True", r.ToString());                
		}

		[Test]
		public void Misc_49_YearDiff ()
		{
			Session sess = new Session();      // 6.956							    	-6.954
			Tvar r = (Tvar)sess.ProcessInput("YearDiff[2014-01-01,2020-12-15] == 0 - YearDiff[2020-12-15,2014-01-01]");
			Assert.AreEqual("True", r.ToString());                
		}

		[Test]
		public void Misc_50_Diff ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("TheYear |> Diff[2]");  // Compares current value to value at time lag 2
			Assert.AreEqual("2", r.ToString());                
		}

		[Test]
		public void Misc_51 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Seq[0,9]");
			Assert.AreEqual("{0,1,2,3,4,5,6,7,8,9}", r.ToString());                
		}

		[Test]
		public void Misc_52 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Seq[Stub,9]");
			Assert.AreEqual("Stub", r.ToString());                
		}

		[Test]
		public void Misc_53 ()
		{
			Session sess = new Session();
			sess.ProcessInput("var1 = {Dawn: 4, 2015-01-01: 9}");
			sess.ProcessInput("var2 = {777,var1}");
			Tvar r = (Tvar)sess.ProcessInput("var2 |> SetSum2");
			Assert.AreEqual("{Dawn: 781, 2015-01-01: 786}", r.ToString());                
		}

		[Test]
		public void Misc_54 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{1,2,3,4,5} |> SetSum2");
			Assert.AreEqual(15, r.Out);          
		}

		[Test]
		public void Misc_55 ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("Seq[1,5] |> SetSum2");
			Assert.AreEqual(15, r.Out);                
		}
		
		[Test]
		public void Misc_56 ()
		{
			Session sess = new Session();
			sess.ProcessInput("f = {Dawn: 1, 2014-01-01: 5}");
			sess.ProcessInput("s = {3,f}");
			Tvar r = (Tvar)sess.ProcessInput("s |> SetSum2");
			Assert.AreEqual("{Dawn: 4, 2014-01-01: 8}", r.Out);                
		}

		[Test]
		public void Misc_57_Parser_error ()
		{
			Session sess = new Session();
			Tvar r = (Tvar)sess.ProcessInput("{3,{Dawn: 1, 2014-01-01: 5}} |> SetSum2");  // Parser error
			Assert.AreEqual("{Dawn: 4, 2014-01-01: 8}", r.Out);                
		}

		[Test]
		public void Misc_58_tvar_literal_as_arg ()
		{
			Session sess = new Session();
			sess.ProcessInput("Sq[x] = x*x");
			Tvar r = (Tvar)sess.ProcessInput("Sq[{Dawn: 4, 2014-02-02: 5}]");
			Assert.AreEqual("{Dawn: 16, 2014-02-02: 25}", r.Out);                
		}
	}
}