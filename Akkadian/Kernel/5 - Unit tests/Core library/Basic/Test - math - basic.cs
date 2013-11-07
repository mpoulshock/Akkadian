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
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class Math : H
    {
        // ADDITION
        
        [Test]
        public void Test1 ()
        {
            Tvar res = new Tvar(8) + new Tvar(9);
            Assert.AreEqual(17, res.Out);        
        }

        [Test]
        public void Test2 ()
        {
            Tvar res = new Tvar(8) + new Tvar(-9);
            Assert.AreEqual(-1, res.Out);        
        }
        
        [Test]
        public void Test3 ()
        {
            Tvar res = new Tvar(8) + -9;
            Assert.AreEqual(-1, res.Out);        
        }
        
        [Test]
        public void Test4 ()
        {
            Tvar res = new Tvar(8.001) + -9;
            Assert.AreEqual(-0.999, res.Out);        
        }
        
        [Test]
        public void Test5 ()
        {
            Tvar res = new Tvar(8.001) + -9 + 3;
            Assert.AreEqual(2.001, res.Out);        
        }
        
        [Test]
        public void Test6 ()
        {
            Tvar res = new Tvar(8) + new Tvar(9) + new Tvar(10);
            Assert.AreEqual(27, res.Out);        
        }
        
        [Test]
        public void Test7 ()
        {
            Tvar res = new Tvar(8) + new Tvar(0.10) + new Tvar(10);
            Assert.AreEqual(18.1, res.Out);        
        }
        
        // SUBTRACTION
        
        [Test]
        public void Test11 ()
        {
            Tvar res = new Tvar(8) - new Tvar(9);
            Assert.AreEqual(-1, res.Out);        
        }

        [Test]
        public void Test12 ()
        {
            Tvar res = new Tvar(8) - new Tvar(-9);
            Assert.AreEqual(17, res.Out);        
        }
        
        [Test]
        public void Test13 ()
        {
            Tvar res = new Tvar(8) - -9;
            Assert.AreEqual(17, res.Out);        
        }
        
        [Test]
        public void Test14 ()
        {
            Tvar res = new Tvar(8.001) - -9;
            Assert.AreEqual(17.001, res.Out);        
        }
        
        [Test]
        public void Test15 ()
        {
            Tvar res = new Tvar(8) - -9 - 3;
            Assert.AreEqual(14, res.Out);        
        }
        
        [Test]
        public void Test16 ()
        {
            Tvar res = new Tvar(8) - new Tvar(9) - new Tvar(10);
            Assert.AreEqual(-11, res.Out);        
        }
        
        // MULTIPLICATION
        
        [Test]
        public void Test21 ()
        {
            Tvar res = new Tvar(8) * new Tvar(9);
            Assert.AreEqual(72, res.Out);        
        }

        [Test]
        public void Test22 ()
        {
            Tvar res = new Tvar(8) * new Tvar(-9);
            Assert.AreEqual(-72, res.Out);        
        }
        
        [Test]
        public void Test23 ()
        {
            Tvar res = new Tvar(8) * -9;
            Assert.AreEqual(-72, res.Out);        
        }
        
        [Test]
        public void Test24 ()
        {
            Tvar res = new Tvar(8.001) * 9;
            Assert.AreEqual(72.009, res.Out);        
        }
        
        [Test]
        public void Test25 ()
        {
            Tvar res = new Tvar(8) * -9 * 3;
            Assert.AreEqual(-216, res.Out);        
        }
        
        [Test]
        public void Test26 ()
        {
            Tvar res = new Tvar(8) * new Tvar(9) * new Tvar(10);
            Assert.AreEqual(720, res.Out);        
        }
        
        [Test]
        public void Test27 ()
        {
            Tvar res = new Tvar(8) * new Tvar(0.10) * new Tvar(10);
            Assert.AreEqual(8.0, res.Out);        
        }
        
        // DIVISION
        
        [Test]
        public void Test31 ()
        {
            Tvar res = new Tvar(12) / new Tvar(3);
            Assert.AreEqual(4, res.Out);        
        }

        [Test]
        public void Test32 ()
        {
            Tvar res = new Tvar(12) / new Tvar(-3);
            Assert.AreEqual(-4, res.Out);        
        }
        
        [Test]
        public void Test33 ()
        {
            Tvar res = new Tvar(12) / -2;
            Assert.AreEqual(-6, res.Out);        
        }
        
        [Test]
        public void Test34 ()
        {
            Tvar res = new Tvar(8.001) / 9;
            Assert.AreEqual(0.889, res.Out);        
        }
        
        [Test]
        public void Test35_Div_by_zero_issue ()
        {
            Tvar res = new Tvar(8) / 0;
            Assert.AreEqual("Uncertain", res.Out);        
        }
        
        [Test]
        public void Test36 ()
        {
            Tvar res = 8 / new Tvar(2);
            Assert.AreEqual(4, res.Out);        
        }
        
        [Test]
        public void Test39 ()
        {
            Tvar res = -0.10 / new Tvar(2);
            Assert.AreEqual(-0.05, res.Out);        
        }

        [Test]
        public void Test39a ()
        {
            Tvar res = new Tvar(0) / 7;
            Assert.AreEqual(0, res.Out);        
        }

        [Test]
        public void Test39b ()
        {
            Tvar t = new Tvar(0);
            t.AddState(new DateTime(2000,1,1), 7);

            Tvar res = t / 7;
            Assert.AreEqual("{Dawn: 0; 1/1/2000: 1}", res.Out);        
        }

        // MODULO
        
        [Test]
        public void Test40 ()
        {
            Tvar res = new Tvar(12) % new Tvar(2);
            Assert.AreEqual(0, res.Out);        
        }
        
        [Test]
        public void Test41 ()
        {
            Tvar res = new Tvar(13) % new Tvar(2);
            Assert.AreEqual(1, res.Out);        
        }
        
        [Test]
        public void Test45 ()
        {
            Tvar res = new Tvar(12.01) % 2;
            Assert.AreEqual(0.01, res.Out);        
        }
        
        // MAXIMUM
        
        [Test]
        public void Test50 ()
        {
            Tvar res = Max(new Tvar(12), new Tvar(2), new Tvar(99));
            Assert.AreEqual(99, res.Out);        
        }
        
        [Test]
        public void Test51 ()
        {
            Tvar res = Max(new Tvar(12), new Tvar(2), new Tvar(-99));
            Assert.AreEqual(12, res.Out);        
        }
        
        [Test]
        public void Test52 ()
        {
            Tvar res = Max(new Tvar(12), new Tvar(0), new Tvar(-99));
            Assert.AreEqual(12, res.Out);        
        }
        
        [Test]
        public void Test53 ()
        {
            Tvar res = Max(12, 0, -99);
            Assert.AreEqual(12, res.Out);        
        }
        
        [Test]
        public void Test60 ()
        {
            Tvar res = Max(new Tvar(12), new Tvar(0.01), new Tvar(-99));
            Assert.AreEqual(12, res.Out);        
        }
        
        // MINIMUM
        
        [Test]
        public void Test70 ()
        {
            Tvar res = Min(new Tvar(12), new Tvar(2), new Tvar(99));
            Assert.AreEqual(2, res.Out);        
        }
        
        [Test]
        public void Test71 ()
        {
            Tvar res = Min(new Tvar(12), new Tvar(2), new Tvar(-99));
            Assert.AreEqual(-99, res.Out);        
        }
        
        [Test]
        public void Test72 ()
        {
            Tvar res = Min(new Tvar(12), new Tvar(0), new Tvar(-99));
            Assert.AreEqual(-99, res.Out);        
        }
        
        [Test]
        public void Test73 ()
        {
            Tvar res = Min(12, 0, -99);
            Assert.AreEqual(-99, res.Out);        
        }
        
        [Test]
        public void Test80 ()
        {
            Tvar res = Min(new Tvar(12), new Tvar(0.01), new Tvar(-99));
            Assert.AreEqual(-99, res.Out);        
        }
        
        // ABSOLUTE VALUE
        
        [Test]
        public void Test81 ()
        {
            Assert.AreEqual(88, Abs(new Tvar(88)).Out);        
        }
        
        [Test]
        public void Test82 ()
        {
            Assert.AreEqual(88, Abs(new Tvar(-88)).Out);        
        }

        // Temporal
        
        [Test]
        public void TemporalMath1 ()
        {
            Tvar x = new Tvar(10);
            x.AddState(new DateTime(2000,1,1), 1);
            Assert.AreEqual("{Dawn: 11; 1/1/2000: 2}", (x+1).Out );    
        }
        
        [Test]
        public void TemporalMath2 ()
        {
            Tvar x = new Tvar(10);
            x.AddState(new DateTime(2000,1,1), 1);
            Assert.AreEqual("{Dawn: 9; 1/1/2000: 0}", (x-1).Out );    
        }

        // Pow(a,b)

        [Test]
        public void Pow_1 ()
        {
            Assert.AreEqual(27, Pow(3,3).Out);        
        }

        [Test]
        public void Pow_2 ()
        {
            Assert.AreEqual(46.765, Pow(3,3.5).RoundToNearest(0.001).Out);        
        }

        // Square root

        [Test]
        public void Sqrt_1 ()
        {
            Assert.AreEqual(5, Sqrt(25).Out);        
        }

        // Logarithms

        [Test]
        public void Log_1 ()
        {
            Assert.AreEqual(3.219, Log(25).RoundToNearest(0.001).Out);        
        }

        [Test]
        public void Log_2 ()
        {
            Assert.AreEqual(1.398, Log(10,25).RoundToNearest(0.001).Out);        
        }

        // Constants

        [Test]
        public void Pi_1 ()
        {
            Assert.AreEqual(3.1415927, ConstPi.RoundToNearest(0.0000001).Out);        
        }

        [Test]
        public void E_1 ()
        {
            Assert.AreEqual(2.718, ConstE.RoundToNearest(0.001).Out);        
        }
    }
}
