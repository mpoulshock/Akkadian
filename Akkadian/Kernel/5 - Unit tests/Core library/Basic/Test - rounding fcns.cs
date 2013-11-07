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
    public class RoundingFcns : H
    {    
        // ROUND UP
        
        [Test]
        public void Up1 ()
        {
            Tvar res = new Tvar(121).RoundUp(10);
            Assert.AreEqual(130, res.Out);    
        }
        
        [Test]
        public void Up2 ()
        {
            Tvar res = new Tvar(120).RoundUp(10);
            Assert.AreEqual(120, res.Out);        
        }
        
        [Test]
        public void Up3 ()
        {
            Tvar res = new Tvar(7.33).RoundUp(0.25);
            Assert.AreEqual(7.50, res.Out);            
        }
        
        [Test]
        public void Up4 ()
        {
            Tvar res = new Tvar(7.5).RoundUp(0.25);
            Assert.AreEqual(7.5, res.Out);            
        }
        
        [Test]
        public void Up5 ()
        {
            Tvar res = new Tvar(1324103).RoundUp(50000);
            Assert.AreEqual(1350000, res.Out);        
        }
        
        // ROUND DOWN

        [Test]
        public void Down1 ()
        {
            Tvar res = new Tvar(121).RoundDown(10);
            Assert.AreEqual(120, res.Out);    
        }
        
        [Test]
        public void Down2 ()
        {
            Tvar res = new Tvar(7.33).RoundDown(0.25);
            Assert.AreEqual(7.25, res.Out);        
        }
        
        [Test]
        public void Down3 ()
        {
            Tvar res = new Tvar(7.5).RoundDown(0.25);
            Assert.AreEqual(7.5, res.Out);        
        }
        
        [Test]
        public void Down4 ()
        {
            Tvar res = new Tvar(1324103).RoundDown(50000);
            Assert.AreEqual(1300000, res.Out);    
        }

        [Test]
        public void Down5 ()
        {
            Tvar res = new Tvar(0).RoundDown(1);
            Assert.AreEqual(0, res.Out);    
        }

        // ROUND TO NEAREST
        
        [Test]
        public void Near1 ()
        {
            Tvar res = new Tvar(121).RoundToNearest(10);
            Assert.AreEqual(120, res.Out);    
        }
        
        [Test]
        public void Near2 ()
        {
            Tvar res = new Tvar(127).RoundToNearest(10);
            Assert.AreEqual(130, res.Out);    
        }
        
        [Test]
        public void Near3 ()
        {
            Tvar res = new Tvar(125).RoundToNearest(10);
            Assert.AreEqual(130, res.Out);        
        }
        
        [Test]
        public void Near4 ()
        {
            Tvar res = new Tvar(121).RoundToNearest(10, true);
            Assert.AreEqual(120, res.Out);
        }
        
        [Test]
        public void Near5 ()
        {
            Tvar res = new Tvar(127).RoundToNearest(10, true);
            Assert.AreEqual(130, res.Out);        
        }
        
        [Test]
        public void Near6 ()
        {
            Tvar res = new Tvar(125).RoundToNearest(10, true);
            Assert.AreEqual(120, res.Out);    
        }
        
        [Test]
        public void Near9 ()
        {
            Tvar res = new Tvar(88.34).RoundToNearest(0.10);
            Assert.AreEqual(88.30, res.Out);    
        }
        
        [Test]
        public void Near10 ()
        {
            Tvar res = new Tvar(88.34).RoundToNearest(1);
            Assert.AreEqual(88.00, res.Out);    
        }

         // Tvar.ToUSD

        [Test]
        public void ToUSD_1()
        {
            Tvar res = new Tvar(88.369).ToUSD;
            Assert.AreEqual("$88.37", res.Out);        
        }
        
        [Test]
        public void ToUSD_2()
        {
            Tvar res = new Tvar(88.3).ToUSD;
            Assert.AreEqual("$88.30", res.Out);        
        }
        
        [Test]
        public void ToUSD_3()
        {
            Tvar res = new Tvar(88).ToUSD;
            Assert.AreEqual("$88.00", res.Out);        
        }
        
        [Test]
        public void ToUSD_4()
        {
            Tvar res = new Tvar(44988).ToUSD;
            Assert.AreEqual("$44,988.00", res.Out);        
        }

        [Test]
        public void ToUSD_5()
        {
            Tvar res = new Tvar(new Hval()).ToUSD;
            Assert.AreEqual("Unstated", res.Out);        
        }

        [Test]
        public void ToUSD_6()
        {
            Tvar res = new Tvar(-44988).ToUSD;
            Assert.AreEqual("($44,988.00)", res.Out);        
        }

        [Test]
        public void ToUSD_7()
        {
            Tvar res = new Tvar(2).ToUSD;
            Assert.AreEqual("$2.00", res.Out);        
        }
    }
}