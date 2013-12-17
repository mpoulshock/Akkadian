// Copyright (c) 2011-2013 Hammura.bi LLC
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
    public class Set : Tvar
    {        
        // Legal entities to be used as members of the sets
        public static Thing P1 = new Thing("P1");
        public static Thing P2 = new Thing("P2");
        public static Thing P3 = new Thing("P3");
        
        
        // Construct a Tvar from another Tvar
        
        [Test]
        public void Constructor1 ()
        {
            Tvar s1 = new Tvar();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P1);
            Tvar s2 = new Tvar(s1);
			Assert.AreEqual("{P1}", s2.LeanTset.Out); 
        }
        
        // .Lean
        
        [Test]
        public void Test0_1 ()
        {
            Tvar s1 = new Tvar();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P1);
			Assert.AreEqual("{P1}", s1.LeanTset.Out);
        }
        
        // .AsOf
        
        [Test]
        public void Test_AsOf_1 ()
        {
            Tvar s1 = new Tvar();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P2);
			Assert.AreEqual("{P2}", s1.AsOf(Time.DawnOf.AddYears(2)).Out);        // Lean not working
        }
        
        [Test]
        public void Test_AsOf_2 ()
        {
            Tvar s1 = new Tvar();
            s1.AddState(Time.DawnOf, P1, P2);
            s1.AddState(Time.DawnOf.AddYears(3), P2);
			Assert.AreEqual("{P1,P2}", s1.AsOf(Time.DawnOf.AddYears(2)).Out);        // Lean not working
        }
        
        
        // .Out
    
        [Test]
        public void Test1_1 ()
        {
			Assert.AreEqual("{P1,P2}", MakeTset(P1,P2).Out);        
        }
        
        [Test]
        public void Test1_2 ()
        {
            Tvar s1 = new Tvar();
            s1.SetEternally();
			Assert.AreEqual("{}", s1.Out);        
        }
        
        // .Count
        
        [Test]
        public void Count1 ()
        {
			Assert.AreEqual(2, MakeTset(P1,P2).Count.Out);        
        }
        
        [Test]
        public void Count2 ()
        {
            // This is how you assert an eternally empty set
			Tvar s1 = MakeTset(); 
            Assert.AreEqual(0, s1.Count.Out);        
        }
        
        [Test]
        public void Count3 ()
        {
            Tvar s1 = new Tvar();
            s1.SetEternally();
            Assert.AreEqual(0, s1.Count.Out);        
        }

        [Test]
        public void Count4 ()
        {
            Tvar s1 = new Tvar(Hstate.Stub);
            Assert.AreEqual("Stub", s1.Count.Out);        
        }

        // .IsSubsetOf
        
        [Test]
        public void Test6 ()
        {
			Tvar s1 = MakeTset(P1,P2);
			Tvar s2 = MakeTset(P1,P3);    
            Assert.AreEqual(false, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test7 ()
        {
            Tvar s1 = MakeTset(P1,P2);    
            Tvar s2 = MakeTset(P1);
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test8 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Tvar s2 = MakeTset(P1,P2,P3);
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test9 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);    
			Tvar s2 = MakeTset(P1,P2);
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test10 ()
        {
			Tvar s1 = MakeTset(P1,P2);        
			Tvar s2 = MakeTset(P1,P2,P3);
            Assert.AreEqual(false, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test11 ()
        {
			Tvar s1 = MakeTset(P1,P2);        
            Tvar s2 = new Tvar();
            s2.SetEternally();
            Assert.AreEqual(true, s2.IsSubsetOf(s1).Out);        
        }
        
        [Test]
        public void Test12 ()
        {
			Tvar s1 = new Tvar();    
            s1.SetEternally();
			Tvar s2 = MakeTset(P1,P2);
            Assert.AreEqual(false, s2.IsSubsetOf(s1).Out);        
        }
        
        // Union
        
        [Test]
        public void Union1 ()
        {
			Tvar s1 = MakeTset(P1,P2);
			Tvar s2 = MakeTset(P2,P3);
            Tvar res = Union(s1, s2);
			Assert.AreEqual("{P1,P2,P3}", res.Out);        
        }
        
        [Test]
        public void Union2 ()
        {
			Tvar s1 = MakeTset(P1);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Union(s1, s2);
			Assert.AreEqual("{P1,P2,P3}", res.Out);        
        }
        
        [Test]
        public void Union3 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Union(s1, s2);
			Assert.AreEqual("{P2,P3}", res.Out);        
        }

        [Test]
        public void Union4 ()
        {
			Tvar s1 = new Tvar(Hstate.Stub);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Union(s1, s2);
			Assert.AreEqual("Stub", res.Out);        
        }

        // Intersection
        
        [Test]
        public void Intersection1 ()
        {
			Tvar s1 = MakeTset(P1,P2);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
			Assert.AreEqual("{P2}", res.Out);        
        }
        
        [Test]
        public void Intersection2 ()
        {
            Tvar s1 = MakeTset(P1);
            Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
			Assert.AreEqual("{}", res.Out);        
        }
        
        [Test]
        public void Intersection3 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
			Assert.AreEqual("{}", res.Out);        
        }
        
        [Test]
        public void Intersection4 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
			Assert.AreEqual("{P2,P3}", res.Out);        
        }

        [Test]
        public void Intersection5 ()
        {
			Tvar s1 = new Tvar(Hstate.Stub);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
			Assert.AreEqual("Stub", res.Out);        
        }

        // Relative complement
        
        [Test]
        public void Complement1 ()
        {
            Tvar s1 = MakeTset(P1,P2);
            Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
			Assert.AreEqual("{P1}", res.Out);        
        }
        
        [Test]
        public void Complement2 ()
        {
            Tvar s1 = MakeTset(P1);
            Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
			Assert.AreEqual("{P1}", res.Out);        
        }
        
        [Test]
        public void Complement3 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
			Assert.AreEqual("{}", res.Out);        
        }
        
        [Test]
        public void Complement4 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
			Assert.AreEqual("{P1}", res.Out);        
        }
        
        [Test]
        public void Complement5 ()
        {
			Tvar s1 = new Tvar();
			s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s2,s1);
			Assert.AreEqual("{P2,P3}", res.Out);        
        }

        [Test]
        public void Complement6 ()
        {
			Tvar s1 = MakeTset(Hstate.Unstated);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
            Assert.AreEqual("Unstated", res.Out);        
        }

        // Equality
        
        [Test]
        public void Test60 ()
        {
			Tvar s1 = MakeTset(P1,P2);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = AreEquivalentSets(s1,s2);
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test61 ()
        {
            Tvar s1 = MakeTset(P1);
            Tvar s2 = MakeTset(P1);
			Tvar res = AreEquivalentSets(s1,s2);
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void Test62 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = AreEquivalentSets(s1,s2);	
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test63 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Tvar s2 = MakeTset(P2,P3,P3);
			Tvar res = AreEquivalentSets(s1,s2);
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test64 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = AreEquivalentSets(s1,s2);
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void Test65 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Tvar s2 = MakeTset(P1,P2,P3);
			Tvar res = AreEquivalentSets(s1,s2);
            Assert.AreEqual(true, res.Out);        
        }
        
        // Reverse

        [Test]
        public void Reverse1 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Assert.AreEqual("{P3,P2,P1}", s1.Reverse.Out);        
        }

        [Test]
        public void Reverse2 ()
        {
            Tvar s1 = new Tvar(Hstate.Unstated);
            Assert.AreEqual("Unstated", s1.Reverse.Out);        
        }

		// Sum

		[Test]
		public void Sum1 ()
		{
			Tvar s1 = MakeTset(1,3,5,7,9);
			Assert.AreEqual(25, s1.SumItems.Out);        
		}

		// First

		[Test]
		public void First1 ()
		{
			Tvar s1 = MakeTset(1,3,5,7,9);
			Assert.AreEqual(1, s1.First.Out);        
		}

		[Test]
		public void First2 ()
		{
			Tvar s1 = MakeTset(3,5,7,9);
			Assert.AreEqual(3, s1.First.Out);        
		}

		[Test]
		public void First3 ()
		{
			Tvar s1 = MakeTset(7);
			Assert.AreEqual(7, s1.First.Out);        
		}

		[Test]
		public void First4 ()
		{
			Tvar s1 = new Tvar(Hstate.Unstated);
			Assert.AreEqual("Unstated", s1.First.Out);       
		}

		[Test]
		public void First5 ()
		{
			Tvar s1 = MakeTset("3","5","7","9");
			Assert.AreEqual("3", s1.First.Out);        
		}

		[Test]
		public void First6 ()
		{
			Tvar s1 = MakeTset(true, false, false);
			Assert.AreEqual(true, s1.First.Out);        
		}

		[Test]
		public void First7 ()
		{
			Tvar s1 = new Tvar();
			s1.AddState(Time.DawnOf, new Hval(new List<object>(){3}));  // TODO: ??????????????????
			s1.AddState(new DateTime(2010,12,12), new Hval(new List<object>(){7}));
			Assert.AreEqual("{Dawn: 3, 2010-12-12: 7}", s1.First.Out);
		}

		// Rest

		[Test]
		public void Rest1 ()
		{
			Tvar s1 = MakeTset(1,3,5,7,9);
			Assert.AreEqual("{3,5,7,9}", s1.Rest.Out);        
		}

		[Test]
		public void Rest2 ()
		{
			Tvar s1 = MakeTset(5,7,9);
			Assert.AreEqual("{7,9}", s1.Rest.Out);        
		}

		[Test]
		public void Rest3 ()
		{
			Tvar s1 = MakeTset(9);
			Assert.AreEqual("{}", s1.Rest.Out);        
		}

		[Test]
		public void Rest4 ()
		{
			Tvar s1 = new Tvar(Hstate.Unstated);
			Assert.AreEqual("Unstated", s1.Rest.Out);      
		}
    }
}