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
    #pragma warning disable 219
    
    [TestFixture]
    public class SymmetricalFacts : H
    {
        private static Thing p1 = new Thing("P1");
        private static Thing p2 = new Thing("P2");
        
        // .Sym combos
        
        [Test]
        public void SymTT ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, true);
            Facts.Assert(p2, "IsMarriedTo", p1, true);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(true, result.Out);       
        }
        
        [Test]
        public void SymTF ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, true);
            Facts.Assert(p2, "IsMarriedTo", p1, false);                         // contradictory assertion
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(true, result.Out);    // what is desired here? (or forbid contradictions)
        }
        
        [Test]
        public void SymTU ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, true);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(true, result.Out);       
        }
        
        [Test]
        public void SymFF ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, false);
            Facts.Assert(p2, "IsMarriedTo", p1, false);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(false , result.Out);       
        }  
        
        [Test]
        public void SymFU ()
        {
            Facts.Clear();
            Facts.Assert(p1, "IsMarriedTo", p2, false);
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual(false , result.Out);       
        }
        
        [Test]
        public void SymUU ()
        {
            Facts.Clear();
            Tbool result = Facts.Sym(p1, "IsMarriedTo", p2);
            Assert.AreEqual("Unstated", result.Out);       
        }
    }
    
    #pragma warning restore 219
}
