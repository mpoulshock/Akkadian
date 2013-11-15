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

using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class NumericComparison : Tvar
    {
        // EQUALS
        
        [Test]
        public void Test1 ()
        {
            Tvar t = new Tvar(2.0) == new Tvar(2);
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void Test2 ()
        {
            Tvar t = new Tvar(2.00000000000001) == new Tvar(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test3 ()
        {
            Tvar t = new Tvar(1.99999999999999) == new Tvar(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test4 ()
        {
            Tvar t = new Tvar(-2) == new Tvar(2);        
            Assert.AreEqual(false , t.Out);        
        }
        
        // TODO: Convert string currency values to Tvars
        
//        [Test]
//        public void Test5 ()
//        {
//            Tvar t = new Tvar("$3.94") == new Tvar(3.94);        
//            Assert.AreEqual(true , t.Out);        
//        }
        
        [Test]
        public void Test6 ()
        {
            Tvar t = new Tvar(3.94) == new Tvar(3.940);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test7 ()
        {
            Tvar t = new Tvar(-3.94) == new Tvar(-3.940);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test8 ()
        {
            Tvar t = new Tvar(-3.94) == -3.940;        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test9 ()
        {
            Tvar t = new Tvar(0.1) == new Tvar(0.10);        
            Assert.AreEqual(true , t.Out);        
        }

        
        // NOT EQUAL
        
        [Test]
        public void Test11 ()
        {
            Tvar t = new Tvar(2.0) != new Tvar(2);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test12 ()
        {
            Tvar t = new Tvar(2.00000000000001) != new Tvar(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void Test13 ()
        {
            Tvar t = new Tvar(1.99999999999999) != new Tvar(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test14 ()
        {
            Tvar t = new Tvar(-2) != new Tvar(2);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test16 ()
        {
            Tvar t = new Tvar(3.94) != new Tvar(3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test17 ()
        {
            Tvar t = new Tvar(-3.94) != new Tvar(-3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test20 ()
        {
            Tvar t = new Tvar(-3.94) != -3.940;        
            Assert.AreEqual(false , t.Out);        
        }
        
        // GREATER THAN
        
        [Test]
        public void Test21 ()
        {
            Tvar t = new Tvar(2.0) > new Tvar(2);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test22 ()
        {
            Tvar t = new Tvar(2.00000000000001) > new Tvar(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void Test23 ()
        {
            Tvar t = new Tvar(1.99999999999999) > new Tvar(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test24 ()
        {
            Tvar t = new Tvar(-2) > new Tvar(2);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test26 ()
        {
            Tvar t = new Tvar(3.94) > new Tvar(3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test27 ()
        {
            Tvar t = new Tvar(-3.94) > new Tvar(-3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test30 ()
        {
            Tvar t = new Tvar(-3.94) > -3.940;        
            Assert.AreEqual(false , t.Out);        
        }

        
        [Test]
        public void Test30_a ()
        {
            Tvar t = new Tvar(0 > 365);      
            Assert.AreEqual(false , t.Out);        
        }

        // LESS THAN
        
        [Test]
        public void Test31 ()
        {
            Tvar t = new Tvar(2.0) < new Tvar(2);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test32 ()
        {
            Tvar t = new Tvar(2.00000000000001) < new Tvar(2);            // Max precision
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void Test33 ()
        {
            Tvar t = new Tvar(1.99999999999999) < new Tvar(2);            // Max precision
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test34 ()
        {
            Tvar t = new Tvar(-2) < new Tvar(2);        
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test36 ()
        {
            Tvar t = new Tvar(3.94) < new Tvar(3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test37 ()
        {
            Tvar t = new Tvar(-3.94) < new Tvar(-3.940);        
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test40 ()
        {
            Tvar t = new Tvar(-3.94) < -3.940;        
            Assert.AreEqual(false , t.Out);        
        }
        
        // >=
        
        [Test]
        public void Test41 ()
        {
            Tvar t = new Tvar(2.0) >= new Tvar(2);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test42 ()
        {
            Tvar t = new Tvar(44) >= new Tvar(-3);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test43 ()
        {
            Tvar t = new Tvar(-44) >= new Tvar(-446);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test44 ()
        {
            Tvar t = new Tvar(-44) >= new Tvar(-4);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test45 ()
        {
            Tvar t = new Tvar(-3.94) >= -3.940;        
            Assert.AreEqual(true , t.Out);        
        }
        
        // <=
        
        [Test]
        public void Test51 ()
        {
            Tvar t = new Tvar(2.0) <= new Tvar(2);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test52 ()
        {
            Tvar t = new Tvar(44) <= new Tvar(-3);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test53 ()
        {
            Tvar t = new Tvar(-44) <= new Tvar(-446);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void Test54 ()
        {
            Tvar t = new Tvar(-44) <= new Tvar(-4);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void Test55 ()
        {
            Tvar t = new Tvar(-3.94) <= -3.940;        
            Assert.AreEqual(true , t.Out);        
        }
        
        // Temporal
        
        [Test]
        public void TemporalComparison1 ()
        {
            Tvar x = new Tvar(10);
            x.AddState(Date(2000,1,1), 1);
			Assert.AreEqual("{Dawn: false, 2000-01-01: true}", (x <= 1).Out );    
        }
    }
}