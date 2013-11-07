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
    public class BooleanComparison : H
    {
        // EQUALS
        
        [Test]
        public void BooleanComparison1 ()
        {
            Tvar t = new Tvar(true) == new Tvar(false);
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void BooleanComparison2 ()
        {
            Tvar t = new Tvar(true) == new Tvar(true);
            Assert.AreEqual(true , t.Out);            
        }
        
        [Test]
        public void BooleanComparison3 ()
        {
            Tvar t = new Tvar(false) == new Tvar(false);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison4 ()
        {
            Tvar t = new Tvar(true) == true;
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison5 ()
        {
            Tvar t = new Tvar(true) == false;
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison6 ()
        {
            Tvar t = true == new Tvar(true);
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison7 ()
        {
            Tvar t = false == new Tvar(true);
            Assert.AreEqual(false , t.Out);        
        }
        
        // NOT EQUAL
        
        [Test]
        public void BooleanComparison11 ()
        {
            Tvar t = new Tvar(true) != new Tvar(false);
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void BooleanComparison12 ()
        {
            Tvar t = new Tvar(true) != new Tvar(true);
            Assert.AreEqual(false , t.Out);            
        }
    
        [Test]
        public void BooleanComparison13 ()
        {
            Tvar t = new Tvar(false) != new Tvar(false);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison14 ()
        {
            Tvar t = new Tvar(true) != true;
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison15 ()
        {
            Tvar t = new Tvar(true) != false;
            Assert.AreEqual(true , t.Out);        
        }
        
        [Test]
        public void BooleanComparison16 ()
        {
            Tvar t = true != new Tvar(true);
            Assert.AreEqual(false , t.Out);        
        }
        
        [Test]
        public void BooleanComparison17 ()
        {
            Tvar t = false != new Tvar(true);
            Assert.AreEqual(true , t.Out);        
        }
        
    }
}
