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
    public class StringComparison : Tvar
    {
        // EQUALS
        
        [Test]
        public void StringComparison1 ()
        {
			Tvar t = EqualTo(new Tvar("Hello, world!"), new Tvar("Hello, world!"));
            Assert.AreEqual(true , t.Out);        
        }

        [Test]
        public void StringComparison2 ()
        {
			Tvar t = EqualTo(new Tvar("Hello, world!"), new Tvar("Jello, world!"));
            Assert.AreEqual(false , t.Out);                
        }
        
        [Test]
        public void StringComparison3 ()
        {
			Tvar t = EqualTo(new Tvar("Hello, world!"), "Hello, world!");
            Assert.AreEqual(true , t.Out);                
        }
        
        [Test]
        public void StringComparison4 ()
        {
			Tvar t = EqualTo(new Tvar("Hello, world!"), "Hello, world");
            Assert.AreEqual(false , t.Out);                
        }
        
        [Test]
        public void StringComparison5 ()
        {
			Tvar t = EqualTo(new Tvar(Hstate.Unstated), "Hello, world");
            Assert.AreEqual("Unstated", t.Out);                
        }
        
        // NOT EQUAL
        
        [Test]
        public void StringComparison11 ()
        {
            Tvar t = new Tvar("Hello, world!") != new Tvar("Hello, world!");
            Assert.AreEqual(false , t.Out);        
        }

        [Test]
        public void StringComparison12 ()
        {
            Tvar t = new Tvar("Hello, world!") != new Tvar("Jello, world!");
            Assert.AreEqual(true , t.Out);                
        }
        
        [Test]
        public void StringComparison13 ()
        {
            Tvar t = new Tvar("Hello, world!") != "Hello, world!";
            Assert.AreEqual(false , t.Out);                
        }
        
        [Test]
        public void StringComparison14 ()
        {
            Tvar t = new Tvar("Hello, world!") != "Hello, world";
            Assert.AreEqual(true , t.Out);                
        }
        
    }
}
