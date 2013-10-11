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
    public class BasicString : H
    {
        [Test]
        public void Concat_1 ()
        {
            Tstr ts1 = new Tstr("hello,");
            Tstr ts2 = new Tstr("world");
            Tstr ts3 = ts1 + ts2;
            Assert.AreEqual("hello,world", ts3.Out);            
        }
        
        [Test]
        public void Concat_2 ()
        {
            Tstr ts1 = new Tstr("hello,");
            Tstr ts2 = new Tstr("world");
            Tstr ts3 = ts1 + " " + ts2;
            Assert.AreEqual("hello, world", ts3.Out);            
        }
        
        [Test]
        public void Concat_3 ()
        {
            Tstr ts1 = new Tstr("hello,") + " world";
            Assert.AreEqual("hello, world", ts1.Out);            
        }   

        [Test]
        public void ToUSD_1 ()
        {
            Tstr ts1 = new Tnum(91.246).ToUSD;
            Assert.AreEqual("$91.25", ts1.Out);            
        } 

        [Test]
        public void ToUSD_2 ()
        {
            Tstr ts1 = new Tnum(1234567).ToUSD;
            Assert.AreEqual("$1,234,567.00", ts1.Out);            
        } 
    }
}