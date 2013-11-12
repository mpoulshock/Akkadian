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
            Assert.AreEqual("P1", s2.LeanTset.Out); 
        }
        
        // .Lean
        
        [Test]
        public void Test0_1 ()
        {
            Tvar s1 = new Tvar();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P1);
            Assert.AreEqual("P1", s1.LeanTset.Out);
        }
        
        // .AsOf
        
        [Test]
        public void Test_AsOf_1 ()
        {
            Tvar s1 = new Tvar();
            s1.AddState(Time.DawnOf, P1);
            s1.AddState(Time.DawnOf.AddYears(1), P2);
            Assert.AreEqual("P2", s1.AsOf(Time.DawnOf.AddYears(2)).Out);        // Lean not working
        }
        
        [Test]
        public void Test_AsOf_2 ()
        {
            Tvar s1 = new Tvar();
            s1.AddState(Time.DawnOf, P1, P2);
            s1.AddState(Time.DawnOf.AddYears(3), P2);
            Assert.AreEqual("P1,P2", s1.AsOf(Time.DawnOf.AddYears(2)).Out);        // Lean not working
        }
        
        
        // .Out
    
        [Test]
        public void Test1_1 ()
        {
			Assert.AreEqual("P1,P2", MakeTset(P1,P2).Out);        
        }
        
        [Test]
        public void Test1_2 ()
        {
            Tvar s1 = new Tvar();
            s1.SetEternally();
            Assert.AreEqual("", s1.Out);        
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
            Tvar s1 = new Tvar();
            s1.SetEternally();
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

        // .IsEmpty
        
        [Test]
        public void IsEmpty1 ()
        {
			Assert.AreEqual(false, MakeTset(P1,P2).IsEmpty.Out);        
        }
        
        [Test]
        public void IsEmpty2 ()
        {
            Tvar s1 = new Tvar();
            s1.SetEternally();
            Assert.AreEqual(true, s1.IsEmpty.Out);        
        }

        [Test]
        public void IsEmpty3 ()
        {
            Tvar s1 = new Tvar(Hstate.Uncertain);
            Assert.AreEqual("Uncertain", s1.IsEmpty.Out);        
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
        
        // .Contains
        
        [Test]
        public void Test20 ()
        {
			Assert.AreEqual(true, MakeTset(P1,P2).Contains(P1).Out);        
        }
        
        [Test]
        public void Test21 ()
        {
			Assert.AreEqual(false, MakeTset(P1,P2).Contains(P3).Out);        
        }
        
        [Test]
        public void Test22 ()
        {
            Tvar s1 = new Tvar();
            s1.SetEternally();
            Assert.AreEqual(false, s1.Contains(P3).Out);        
        }
        
        // Union
        
        [Test]
        public void Union1 ()
        {
			Tvar s1 = MakeTset(P1,P2);
			Tvar s2 = MakeTset(P2,P3);
            Tvar res = Union(s1, s2);
            Assert.AreEqual("P1,P2,P3", res.Out);        
        }
        
        [Test]
        public void Union2 ()
        {
			Tvar s1 = MakeTset(P1);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Union(s1, s2);
            Assert.AreEqual("P1,P2,P3", res.Out);        
        }
        
        [Test]
        public void Union3 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Union(s1, s2);
            Assert.AreEqual("P2,P3", res.Out);        
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
            Assert.AreEqual("P2", res.Out);        
        }
        
        [Test]
        public void Intersection2 ()
        {
            Tvar s1 = MakeTset(P1);
            Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
            Assert.AreEqual("", res.Out);        
        }
        
        [Test]
        public void Intersection3 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
            Assert.AreEqual("", res.Out);        
        }
        
        [Test]
        public void Intersection4 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = Intersection(s1, s2);
            Assert.AreEqual("P2,P3", res.Out);        
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
            Assert.AreEqual("P1", res.Out);        
        }
        
        [Test]
        public void Complement2 ()
        {
            Tvar s1 = MakeTset(P1);
            Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
            Assert.AreEqual("P1", res.Out);        
        }
        
        [Test]
        public void Complement3 ()
        {
			Tvar s1 = new Tvar();
            s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
            Assert.AreEqual("", res.Out);        
        }
        
        [Test]
        public void Complement4 ()
        {
			Tvar s1 = MakeTset(P1,P2,P3);
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s1,s2);
            Assert.AreEqual("P1", res.Out);        
        }
        
        [Test]
        public void Complement5 ()
        {
			Tvar s1 = new Tvar();
			s1.SetEternally();
			Tvar s2 = MakeTset(P2,P3);
			Tvar res = RelativeComplement(s2,s1);
			Assert.AreEqual("P2,P3", res.Out);        
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
            Assert.AreEqual("P3,P2,P1", s1.Reverse.Out);        
        }

        [Test]
        public void Reverse2 ()
        {
            Tvar s1 = new Tvar(Hstate.Unstated);
            Assert.AreEqual("Unstated", s1.Reverse.Out);        
        }

        // Tvar.Out

        [Test]
        public void TestOutput1 ()
        {
            string val = "ham; beans";
            string[] items = val.Split(new char[] {';'});
			List<object> list = new List<object>();

            foreach (string i in items)
            {
                list.Add(i.Trim());
            }

			Tvar result = MakeTset(list);
            Assert.AreEqual("ham,beans", result.Out);        
        }
    }
}