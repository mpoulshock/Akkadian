// Copyright (c) 2012-2013 Hammura.bi LLC
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

using Akkadian;
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class Hvals 
    {
        // Some Hvals
        private static Hval Huncertain = new Hval(null,Hstate.Uncertain);
        private static Hval Hunstated = new Hval();
        private static Hval HboolKnown = new Hval(true);
        private static Hval HboolKnown2 = new Hval(true);
        private static Hval HnumKnown = new Hval(42);
        private static Hval HnumKnown2 = new Hval(43);
        private static Hval Hstub = new Hval(null,Hstate.Stub);

        
        // Loading and unloading Hvals
        
        [Test]
        public void Val_1 ()
        {
            Assert.AreEqual(Hstate.Uncertain, Huncertain.Val);        
        }
        
        [Test]
		public void Val_2 ()
        {
            Assert.AreEqual(Hstate.Unstated, Hunstated.Val);        
        }
        
        [Test]
		public void Val_3 ()
        {
            Assert.AreEqual(true, HboolKnown.Val);        
        }
        
        [Test]
		public void Val_4 ()
        {
            Assert.AreEqual(42, HnumKnown.Val);        
        }
        
        [Test]
		public void Val_5 ()
        {
            Assert.AreEqual(Hstate.Stub, Hstub.Val);        
        }
        
        [Test]
		public void Val_6 ()
        {
            Hval HnumKnown2 = new Hval(42);
            Assert.AreEqual(HnumKnown.Val, HnumKnown2.Val);        
        }

        [Test]
		public void Val_7 ()
        {
            bool r = HboolKnown.IsTrue;
            Assert.AreEqual(true, r);        
        }

        [Test]
		public void Val_8 ()
        {
            Hval HboolF = new Hval(false);
            bool r = HboolF.IsTrue;
            Assert.AreEqual(false, r);        
        }

        [Test]
		public void Val_9 ()
        {
            bool r = Huncertain.IsTrue;
            Assert.AreEqual(false, r);        
        }

        [Test]
		public void Val_10 ()
        {
            Hval h = new Hval(null,Hstate.Uncertain);
            Assert.AreEqual(false, h.IsKnown);        
        }

        [Test]
		public void Val_11 ()
        {
            Hval h = new Hval(44);
            Assert.AreEqual(true, h.IsKnown);        
        }

        [Test]
		public void Val_12 ()
        {
            Hval h = new Hval(44);
            Assert.AreEqual("44", h.ToString);        
        }

        [Test]
		public void Val_13 ()
        {
            Hval h = new Hval(null,Hstate.Uncertain);
            Assert.AreEqual("Uncertain", h.ToString);        
        }

        // Using Hvals in Tvars
//
//        private static Tvar tb1 = new Tvar(true);
//        private static Tvar tb2 = new Tvar(true);  
//        private static Tvar tn0 = new Tvar(42); 
//        private static Tvar tn1 = new Tvar(42);
//        private static Tvar tn2 = new Tvar(43); 
//        private static Tvar tbu = new Tvar(Hstate.Unstated);
//        private static Tvar tbn = new Tvar(Hstate.Uncertain);
//
//        [Test]
//        public void TestTvar1 () //
//        {
//            Assert.AreEqual(42, tn1.AsOf(Time.DawnOf).Out);        
//        }
//
//        [Test]
//        public void TestTvar3 ()
//        {
//            Assert.AreEqual(tb1.Out,tb2.Out);        
//        }
//        
//        [Test]
//        public void TestTvar4 ()
//        {
//            Assert.AreEqual(true, tb1.Out);        
//        }
//        
//        [Test]
//        public void TestTvar5 ()
//        {
//            Tvar eq = tb1 == tb2;
//            Assert.AreEqual(true, eq.Out);        
//        }
//        
//        [Test]
//        public void TestTvar6 () 
//        {
//            Tvar eq = tn1 == tn2;
//            Assert.AreEqual(false, eq.Out);        
//        }
//
//        [Test]
//        public void TestTvar6a() 
//        {
//            Tvar eq = tn0 == tn1;
//            Assert.AreEqual(true, eq.Out);        
//        }
//
//        [Test]
//        public void TestTvar6b() 
//        {
//            Tvar eq = tn1 != tn2;
//            Assert.AreEqual(true, eq.Out);        
//        }
//
//        [Test]
//        public void TestTvar8 ()
//        {
//            Tvar t2 = tbu;
//            Assert.AreEqual("Unstated", t2.Out);        
//        }
//
//        [Test]
//        public void TestTvar9 ()
//        {
//            Tvar t2 = tbn;
//            Assert.AreEqual("Uncertain", t2.Out);        
//        }
//
//        [Test]
//        public void TestTvar10 ()
//        {
//            Tvar t2 = new Tvar(true);
//            Assert.AreEqual(true, t2.Out);        
//        }

        // Hval equality

        [Test]
        public void Equality_1 ()
        {
            Hval h1 = new Hval(22);
            Hval h2 = new Hval(22);
            Assert.AreEqual(true, h1.IsEqualTo(h2));        
        }

        [Test]
		public void Equality_2 ()
        {
            Hval h1 = new Hval(22);
            Hval h2 = new Hval(23);
            Assert.AreEqual(false, h1.IsEqualTo(h2));        
        }

        [Test]
		public void Equality_3 ()
        {
            Hval h1 = new Hval(true);
            Hval h2 = new Hval(false);
            Assert.AreEqual(false, h1.IsEqualTo(h2));        
        }

        [Test]
		public void Equality_4 ()
        {
            Hval h1 = new Hval(true);
            Hval h2 = new Hval(true);
            Assert.AreEqual(true, h1.IsEqualTo(h2));        
        }
    }
}