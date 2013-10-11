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
using System.Collections.Generic;
using NUnit.Framework;

namespace Akkadian.UnitTests
{
    #pragma warning disable 219
    
    /*
     * These test cases verify that unknown facts are being added to the
     * Facts.Unknowns list in the desired order.  The desired order is one
     * that mimics a backwards-chaining expert system.  Specifically, facts
     * are added:
     *  
     *      1. In left-to-right order as they are written in an expression, and
     *  
     *      2. Such that facts on irrelevant branches are omitted.
     * 
     * These criteria will enable the unknown facts list to suggest the next
     * question that should be asked in a user-facing interview.  Questions 
     * will occur in a sensible order and with unnecessary questions left out.
     * 
     * Currently, criteria (1) is satisfied but (2) is not (because arguments
     * to a function are evaluated before the funtion itself).
     */
     
    [TestFixture]
    public class UnknownFacts : H
    {
        // Some people
        private static Thing p1 = new Thing("p1");
        private static Thing p2 = new Thing("p2");
        
        // Some functions that ask for input facts
        private static Tbool A()
        {
            return Facts.QueryTvar<Tbool>("A", p1, p2);
        }
        
        private static Tbool B()
        {
            return Facts.QueryTvar<Tbool>("B", p1, p2);
        }
        
        private static Tbool C()
        {
            return Facts.QueryTvar<Tbool>("C", p1, p2);
        }
        
        private static Tbool D()
        {
            return Facts.QueryTvar<Tbool>("D", p1, p2);
        }
        
        private static Tnum X()
        {
            return Facts.QueryTvar<Tnum>("X", p1, p2);
        } 
        
        private static Tnum Y()
        {
            return Facts.QueryTvar<Tnum>("Y", p1, p2);
        } 

        /// <summary>
        /// Returns a string showing all relationships in Facts.Unknowns.
        /// Used to test the order in which factls are added to that list.
        /// </summary>
        private static string ShowUnknownTest()
        {
            string result = "";
            
            foreach (Facts.Fact f in Facts.Unknowns)
            {
                result += f.Relationship + " ";
            }
            
            return result.Trim();
        }

        // Facts.HasBeenAsserted
        
        [Test]
        public void HasBeenAsserted1 ()
        {
            Facts.Clear();
            Facts.Assert(p2, "FamilyRelationship", p1, "Biological child");
            bool result = Facts.HasBeenAsserted("FamilyRelationship", p2, p1);
            Assert.AreEqual(true, result);         
        }
        
        [Test]
        public void HasBeenAsserted2 ()
        {
            Facts.Clear();
            Facts.Assert(p2, "FamilyRelationship", p1, "Biological child");
            bool result = Facts.HasBeenAsserted("FamilyRelationship", p1, p2);
            Assert.AreEqual(false, result);         
        }
        
        // And()
        
        [Test]
        public void FactOrder1a ()
        {
            // Tell system to list unknown facts when a rule is invoked
            Facts.GetUnknowns = true;
            
            // Clear the lists of known and unknown facts
            Facts.Clear();
            Facts.Unknowns.Clear();
            
            // Invoke a rule
            Tbool theRule = A() & B() & C();
            
            // Check the order in which the unknown facts are added to the list
            Assert.AreEqual("A B C", ShowUnknownTest());           
        }

        [Test]
        public void FactOrder1b ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "A", p2, true);
            Tbool theRule = A() & B() & C();
            Assert.AreEqual("B C", ShowUnknownTest());           
        }
        
        [Test]
        public void FactOrder1c ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "B", p2, true);
            Tbool theRule = A() & B() & C();
            Assert.AreEqual("A C", ShowUnknownTest());           
        }
        
        [Test]
        public void FactOrder1d ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "C", p2, true);
            Tbool theRule = A() & B() & C();
            Assert.AreEqual("A B", ShowUnknownTest());           
        }
        
        [Test]
        public void FactOrder1e ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "A", p2, false);
            Tbool theRule = A() && B() && C();
            Assert.AreEqual("", ShowUnknownTest());         
        }

        // IfThen()
        
        [Test]
        public void FactOrder2a ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Tbool theRule = IfThen(A(), B());
            Assert.AreEqual("A B", ShowUnknownTest());         
        }

        // Not()
        
        [Test]
        public void FactOrder3a ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Tbool theRule = A() & !B();
            Assert.AreEqual("A B", ShowUnknownTest());         
        }
        
        [Test]
        public void FactOrder3b ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "B", p2, false);
            Tbool theRule = A() & !B();
            Assert.AreEqual("A", ShowUnknownTest());         
        }
        
        // Nested And() and Or()
        
        [Test]
        public void FactOrder4a ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Tbool theRule = A() & (B() | C());
            Assert.AreEqual("A B C", ShowUnknownTest());      
        }
        
        [Test]
        public void FactOrder4b ()
        {
            Facts.Reset();
            Facts.GetUnknowns = true;
            Tbool theRule = (A() & B()) | C();
            Assert.AreEqual("A B C", ShowUnknownTest());    
        }
        
        // Switch()

        [Test]
        public void FactOrder5a_lazy ()
        {
            Hval h = new Hval(null, Hstate.Null);
            Tnum u = new Tnum(h);

            Facts.Reset();
            Facts.GetUnknowns = true;
            Tnum theRule = Switch<Tnum>(()=> A(), ()=> X(),
                                  ()=> B(), ()=> Y(),
                                  ()=> u);
            Assert.AreEqual("A", ShowUnknownTest()); 
        }

        [Test]
        public void FactOrder5b_lazy ()
        {
            Hval h = new Hval(null, Hstate.Null);
            Tnum u = new Tnum(h);

            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "A", p2, true);
            Tnum theRule = Switch<Tnum>(()=> A(), ()=> X(),
                                  ()=> B(), ()=> Y(),
                                  ()=> u);
            Assert.AreEqual("X", ShowUnknownTest()); 
        }

        [Test]
        public void FactOrder5c_lazy ()
        {
            Hval h = new Hval(null, Hstate.Null);
            Tnum u = new Tnum(h);

            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "A", p2, false);
            Tnum theRule = Switch<Tnum>(()=> A(), ()=> X(),
                                  ()=> B(), ()=> Y(),
                                  ()=> u);
            Assert.AreEqual("B", ShowUnknownTest());
        }

        [Test]
        public void FactOrder5d_lazy ()
        {
            Hval h = new Hval(null, Hstate.Null);
            Tnum u = new Tnum(h);

            Facts.Reset();
            Facts.GetUnknowns = true;
            Facts.Assert(p1, "A", p2, false);
            Facts.Assert(p1, "B", p2, true);
            Tnum theRule = Switch<Tnum>(()=> A(), ()=> X(),
                                  ()=> B(), ()=> Y(),
                                  ()=> u);
            Assert.AreEqual("Y", ShowUnknownTest());  
            Facts.GetUnknowns = true;
        }
    }
    
    #pragma warning restore 219
}