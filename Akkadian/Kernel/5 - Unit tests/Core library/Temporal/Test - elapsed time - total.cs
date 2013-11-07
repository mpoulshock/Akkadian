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

using System;
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public partial class ElapsedTime : H
    {   
        // TotalElapsedIntervals

        [Test]
        public void TotalElapsedIntervals1 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,1,3),false);

            Tvar r = tb.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual(2, r.Out);    
        }

        [Test]
        public void TotalElapsedIntervals2 ()
        {
            Tvar tb = new Tvar(false);
            Tvar r = tb.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual(0, r.Out);    
        }

        [Test]
        public void TotalElapsedIntervals3 () 
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1), Hstate.Unstated);
            tb.AddState(new DateTime(2015,3,1), false);

            Tvar r = tb.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void TotalElapsedIntervals4 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1), Hstate.Stub);
            tb.AddState(new DateTime(2015,3,1), false);

            Tvar r = tb.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void TotalElapsedIntervals5 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,1,3),false);
            tb.AddState(new DateTime(2015,1,10),true);
            tb.AddState(new DateTime(2015,1,18),false);

            Tvar r = tb.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual(10, r.Out);    
        }

        [Test]
        public void TotalElapsedIntervals6 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Date(2012,1,1), false);
            Tvar actual = t.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual(40908, actual.Out);
        }

        [Test]
        public void TotalElapsedIntervals7 ()
        {
            Tvar t = new Tvar(true);
            Tvar actual = t.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual(73050, actual.Out);      
        }

        [Test]
        public void TotalElapsedIntervals8 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            t.AddState(Date(2000,1,3), true);
            t.AddState(Date(2000,1,4), false);      
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(1999,1,1), Date(2000,1,6));
            Assert.AreEqual(2, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals9 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            t.AddState(Date(2000,1,3), true);
            t.AddState(Date(2000,1,4), false);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(2000,1,2), Date(2000,1,6));
            Assert.AreEqual(1, result.Out);      
        }

        [Test]
        public void TotalElapsedIntervals10 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            t.AddState(Date(2000,1,3), true);
            t.AddState(Date(2000,1,4), false);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(2010,1,2), Date(2010,1,6));
            Assert.AreEqual(0, result.Out);      
        }

        [Test]
        public void TotalElapsedIntervals11 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,2), false);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(1999,1,2), Date(1999,1,6));
            Assert.AreEqual(0, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals12 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,2,1), false);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual(5, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals13 ()
        {
            Tvar t = new Tvar(true);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual(5, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals14 ()
        {
            Tvar t = new Tvar(true);
            Tvar result = (!t).TotalElapsedIntervals(TheDay, Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual(0, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals15 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(2000,1,15), Date(2000,1,20));
            Assert.AreEqual(5, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals16 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            t.AddState(Date(2000,1,5), false);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(2000,1,2), Date(2000,1,6));
            Assert.AreEqual(3, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals17 ()
        {
            Tvar t = new Tvar(false);
            Tvar result = t.TotalElapsedIntervals(TheDay, Date(2000,1,1), Date(2010,1,1));
            Assert.AreEqual(0, result.Out);
        }

        [Test]
        public void TotalElapsedIntervals18 ()
        {
            Tvar t = new Tvar(Hstate.Uncertain);
            Tvar result = t.TotalElapsedIntervals(TheDay, Time.DawnOf, Time.EndOf);
            Assert.AreEqual("Uncertain", result.Out);
        }
    }
}