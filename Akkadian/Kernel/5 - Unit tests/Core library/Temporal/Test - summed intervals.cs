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
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class SummedIntervals : H
    {   
        // RUNNING SUMMED INTERVALS

        [Test]
        public void RunningSummedIntervals_1()
        {
            Tvar t = new Tvar(0);
            t.AddState(Date(2012, 1, 1), 1000);
            t.AddState(Date(2012, 3, 1), 0);
            Tvar r = t.RunningSummedIntervals(TheMonth);           
            Assert.AreEqual("{Dawn: 0; 2/1/2012: 1000; 3/1/2012: 2000}", r.Out);    
        }

        [Test]
        public void RunningSummedIntervals_2()
        {
            Tvar t = new Tvar(0);
            Tvar r = t.RunningSummedIntervals(TheMonth);           
            Assert.AreEqual(0, r.Out);    
        }
        
        [Test]
        public void RunningSummedIntervals_3()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar r = t.RunningSummedIntervals(TheMonth);           
            Assert.AreEqual("Unstated", r.Out);    
        }
        
        [Test]
        public void RunningSummedIntervals_4()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar r = t.RunningSummedIntervals(TheMonth);           
            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void RunningSummedIntervals_5()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar r = (new Tvar(10)).RunningSummedIntervals(t);           
            Assert.AreEqual("Stub", r.Out);    
        }

        // SLIDING SUMMED INTERVALS

        [Test]
        public void SlidingSummedIntervals_1()
        {
            Tvar t = new Tvar(1000);
            Tvar r = t.SlidingSummedIntervals(TheMonth, 2);           
            Assert.AreEqual(2000, r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_2()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar r = t.SlidingSummedIntervals(TheMonth, 2);            
            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_3()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar r = t.SlidingSummedIntervals(TheMonth, 2);            
            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_4()
        {
            Tvar t = new Tvar(1000);
            t.AddState(Date(2013, 1, 1), 2000);
            Tvar r = t.SlidingSummedIntervals(TheYear, 2);           
            Assert.AreEqual("{Dawn: 2000; 1/1/2014: 3000; 1/1/2015: 4000}", r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_5()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar r = (new Tvar(6)).SlidingSummedIntervals(TheMonth, t);            
            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_6()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar r = (new Tvar(6)).SlidingSummedIntervals(t, 2);            
            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void SlidingSummedIntervals_7()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            t.AddState(Date(2013, 1, 1), 2000);
            Tvar r = t.SlidingSummedIntervals(TheYear, 2);           
            Assert.AreEqual("Unstated", r.Out);    
        }

        // TOTAL SUMMED INTERVALS

        [Test]
        public void TotalSummedIntervals_1()
        {
            Tvar t = new Tvar(1000);
            Tvar r = t.TotalSummedIntervals(TheMonth, Date(2015,1,1), Date(2016,1,1));           
            Assert.AreEqual(12000, r.Out);    
        }

        [Test]
        public void TotalSummedIntervals_2()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar r = t.TotalSummedIntervals(TheMonth, Date(2015,1,1), Date(2015,12,31));           
            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void TotalSummedIntervals_3()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar r = (new Tvar(44)).TotalSummedIntervals(TheMonth, t, Date(2015,12,31));           
            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void TotalSummedIntervals_4()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar r = (new Tvar(44)).TotalSummedIntervals(TheMonth, Date(2015,12,31), t);           
            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void TotalSummedIntervals_5()
        {
            Tvar t = new Tvar(1000);
            t.AddState(Date(2015, 6, 1), 2000);
            Tvar r = t.TotalSummedIntervals(TheMonth, Date(2015,1,1), Date(2016,1,1));           
            Assert.AreEqual(19000, r.Out);    
        }
    }
}