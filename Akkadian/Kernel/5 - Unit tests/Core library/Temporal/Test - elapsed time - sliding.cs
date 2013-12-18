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
        // SlidingElapsedIntervals

        [Test]
        public void SlidingElapsedIntervals1 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,1,3),false);

			Tvar r = tb.SlidingElapsedIntervals(TheDate, 2);

			Assert.AreEqual("{Dawn: 0, 2015-01-02: 1, 2015-01-03: 2, 2015-01-04: 1, 2015-01-05: 0}", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals2 ()
        {
            Tvar tb = new Tvar(false);
			Tvar r = tb.SlidingElapsedIntervals(TheDate, 2);

            Assert.AreEqual(0, r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals3 () 
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1), Hstate.Unstated);
            tb.AddState(new DateTime(2015,3,1), false);

			Tvar r = tb.SlidingElapsedIntervals(TheDate, 2);

            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals4 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1), Hstate.Stub);
            tb.AddState(new DateTime(2015,3,1), false);

			Tvar r = tb.SlidingElapsedIntervals(TheDate, 2);

            Assert.AreEqual("Stub", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals5 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2015,1,1),true);
            tb.AddState(new DateTime(2015,1,3),false);
            tb.AddState(new DateTime(2015,1,10),true);
            tb.AddState(new DateTime(2015,2,18),false);

			Tvar r = tb.SlidingElapsedIntervals(TheDate, 2);
			string tline = "{Dawn: 0, 2015-01-02: 1, 2015-01-03: 2, 2015-01-04: 1, 2015-01-05: 0, " +
			               "2015-01-11: 1, 2015-01-12: 2, 2015-02-19: 1, 2015-02-20: 0}"; 

            Assert.AreEqual(tline, r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals6 ()
        {
            Tvar tb = new Tvar(true);
			Tvar r = tb.SlidingElapsedIntervals(TheDate, 2);
            Assert.AreEqual(2, r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals7 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Date(2012,1,1), false);
            Tvar actual = t.SlidingElapsedIntervals(TheYear, 2);
			Assert.AreEqual("{Dawn: 0, 1901-01-01: 1, 1902-01-01: 2, 2013-01-01: 1, 2014-01-01: 0}", actual.Out);
        }

        [Test]
        public void SlidingElapsedIntervals8 ()
        {
            Tvar t = new Tvar(true);
            Tvar actual = t.SlidingElapsedIntervals(TheYear, 2);
            Assert.AreEqual(2, actual.Out);      
        }

        [Test]
        public void SlidingElapsedIntervals9 () 
        {
            Tvar tb = new Tvar(Hstate.Unstated);
			Tvar r = tb.SlidingElapsedIntervals(TheDate, 2);
            Assert.AreEqual("Unstated", r.Out);    
        }

        [Test]
        public void SlidingElapsedIntervals10 () 
        {
            // Window size is unstated
            Tvar tb = new Tvar(true);
			Tvar r = tb.SlidingElapsedIntervals(TheDate, new Tvar(Hstate.Unstated));
            Assert.AreEqual("Unstated", r.Out);    
        }
    }
}
