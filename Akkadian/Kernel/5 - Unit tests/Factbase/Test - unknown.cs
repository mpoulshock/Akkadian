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
using System;
using System.Collections.Generic;

namespace Akkadian.UnitTests
{
    [TestFixture]
	public class Hstates : Tvar
    {
        private static string eternallyUnstated = "Unstated";
        private static Tvar tbt = new Tvar(true);
        private static Tvar tbf = new Tvar(false);
        private static Tvar tbu = new Tvar(Hstate.Unstated);
        private static Tvar theSet = new Tvar(Hstate.Unstated);
        private static Tvar n = new Tvar(Hstate.Unstated);

        private static Tvar tnv()
        {
            Tvar result = new Tvar(Hstate.Stub);
            result.AddState(Date(2001,1,1), Hstate.Uncertain);
            result.AddState(Date(2002,1,1), Hstate.Unstated);
            return result;
        }

        // Math - addition
        
        [Test]
        public void Unknown_Add_1 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = n + n2;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Add_2 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = tnv() + n2;
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Math - subtraction

        [Test]
        public void Unknown_Subtract_1 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = n - n2;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Subtract_2 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = tnv() - n2;
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Math - multiplication
        
        [Test]
        public void Unknown_Mult_1 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = n * n2;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }
        
        [Test]
        public void Unknown_Mult_2 ()
        {
            Tvar n2 = new Tvar(0);
            Tvar result = n * n2;
            Assert.AreEqual(0, result.Out);        
        }
        
        [Test]
        public void Unknown_Mult_3 ()
        {
            Tvar result = n * 0;
            Assert.AreEqual(0, result.Out);        
        }

        [Test]
        public void Unknown_Mult_4 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = tnv() * n2;
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Math - div

        [Test]
        public void Unknown_Div_2 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = tnv() / n2;
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Math - modulo

        [Test]
        public void Modulo_1 ()
        {
            Tvar result = 3 % 2;
            Assert.AreEqual(1, result.Out);        
        }

        [Test]
        public void Unknown_Modulo_1 ()
        {
            Tvar result = n % 3;
            Assert.AreEqual("Unstated", result.Out);        
        }

        [Test]
        public void Unknown_Modulo_2 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = tnv() % n2;
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Math - abs
        
        [Test]
        public void Unknown_Abs_1 ()
        {
            Tvar result = Abs(n);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Abs_2 ()
        {
            Tvar result = Abs(tnv());
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Math - round
        
        [Test]
        public void Unknown_Round_1 ()
        {
            Tvar result = n.RoundUp(2);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Round_2 ()
        {
            Tvar result = tnv().RoundToNearest(10);
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        [Test]
        public void Unknown_Round_3 ()
        {
            Tvar result = tnv().RoundUp(10);
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }
        [Test]
        public void Unknown_Round_4 ()
        {
            Tvar result = tnv().RoundDown(10);
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }


        // Math - min
        
        [Test]
        public void Unknown_Min_1 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = Min(n, n2);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Min_2 ()
        {
            Tvar result = Min(tnv(),3);
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Math - max
        
        [Test]
        public void Unknown_Max_1 ()
        {
            Tvar n2 = new Tvar(4);
            Tvar result = Max(n, n2);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Max_2 ()
        {
            Tvar result = Max(tnv(),3);
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", result.Out);        
        }

        // Tvar.Lean
        
        [Test]
        public void Unknown_Lean_1 ()
        {
            Tvar result = tbu.Lean;
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        [Test]
        public void Unknown_Lean_2 ()
        {
            Tvar t = new Tvar(Hstate.Stub);
            t.AddState(DateTime.Now, Hstate.Stub);
            Assert.AreEqual("Stub", t.Lean.Out);        
        }

        // Tvar.AsOf
        
        [Test]
        public void Unknown_AsOf_1 ()
        {
            Tvar result = tbu.AsOf(DateTime.Now);
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }
        
        // Tvar.IsAlways / IsEver
        
        [Test]
        public void Unknown_IsAlways_1 ()
        {
            Tvar result = tbu.IsAlwaysTrue();
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }
        
        [Test]
        public void Unknown_IsEver_1 ()
        {
            Tvar result = tbu.IsEverTrue();
            Assert.AreEqual(eternallyUnstated, result.Out);        
        }

        // String concatenation

        [Test]
        public void Unknown_Concat_1 ()
        {
            Tvar ts2 = new Tvar(Hstate.Unstated);
            Tvar ts3 = " x" + ts2;
            Assert.AreEqual(eternallyUnstated, ts3.Out);            
        }

        // Set.AsOf
        
        [Test]
        public void Unknown_SetAsOf_1 ()
        {
            Assert.AreEqual(eternallyUnstated, theSet.AsOf(Time.DawnOf).Out);    
        }
        
        // Set.IsSubsetOf
        
        [Test]
        public void Unknown_Subset_1 ()
        {
            Thing P1 = new Thing("P1");
            Thing P2 = new Thing("P2");
			Tvar s1 = MakeTset(P1,P2);    
            Assert.AreEqual(eternallyUnstated, theSet.IsSubsetOf(s1).Out);        
        }
        
        // Set.Contains
        
        [Test]
        public void Unknown_SetContains_1 ()
        {
            Thing P1 = new Thing("P1");
            Assert.AreEqual(eternallyUnstated, theSet.Contains(P1).Out);        
        }
        
        // Set equality
        
        [Test]
        public void Unknown_SetEquality_1 ()
        {
            Thing P1 = new Thing("P1");
            Thing P2 = new Thing("P2");
			Tvar s1 = MakeTset(P1,P2);
            Tvar res = s1 == theSet;
            Assert.AreEqual(eternallyUnstated, res.Out);        
        }
        
        [Test]
        public void Unknown_SetEquality_2 ()
        {
            Thing P1 = new Thing("P1");
            Thing P2 = new Thing("P2");
			Tvar s1 = MakeTset(P1,P2);
            Tvar res = s1 != theSet;
            Assert.AreEqual(eternallyUnstated, res.Out);        
        }
        
        // IsAtOrBefore
        
        [Test]
        public void Unknown_IsAtOrBefore_1 ()
        {
            Tvar td1 = new Tvar(2000,1,1);
            Tvar td2 = new Tvar(Hstate.Unstated);
            Tvar result = td2 <= td1;
            Assert.AreEqual(eternallyUnstated, result.Out);    
        }

        // Numeric comparison
        
        [Test]
        public void Unknown_NumericComparison1 ()
        {
            Tvar r = tnv() > 7;
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison2 ()
        {
            Tvar r = tnv() == 7;
			Assert.AreEqual("{Dawn: Stub, 2001-01-01: Uncertain, 2002-01-01: Unstated}", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison3 ()
        {
            Tvar r = new Tvar(Hstate.Uncertain) == new Tvar(Hstate.Unstated);
            Assert.AreEqual("Uncertain", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison4 ()
        {
            Tvar r = new Tvar(Hstate.Uncertain) == new Tvar(Hstate.Uncertain);
            Assert.AreEqual("Uncertain", r.Out);    
        }

        [Test]
        public void Unknown_NumericComparison5 ()
        {
            Tvar r = new Tvar(Hstate.Stub) == new Tvar(Hstate.Uncertain);
            Assert.AreEqual("Stub", r.Out);    
        }

        // Unknown entity instances (that are function arguments)

        [Test]
        public void Unknown_EntInst1 ()
        {
            Thing p = new Thing("");
            Assert.AreEqual(Hstate.Unstated, EntityArgIsUnknown(p));    
        }

        [Test]
        public void Unknown_EntInst2 ()
        {
            Thing p = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p));    
        }

        [Test]
        public void Unknown_EntInst3 ()
        {
            Thing p1 = new Thing("");
            Thing p2 = new Thing("Jane");
            Assert.AreEqual(Hstate.Unstated, EntityArgIsUnknown(p1,p2));    
        }

        [Test]
        public void Unknown_EntInst4 ()
        {
            Thing p1 = new Thing("Jim");
            Thing p2 = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p1,p2));    
        }

        [Test]
        public void Unknown_EntInst5 ()
        {
            Thing p1 = new Thing("");
            Thing p2 = new Thing("");
            Assert.AreEqual(Hstate.Unstated, EntityArgIsUnknown(p1,p2));    
        }

        [Test]
        public void Unknown_EntInst6 ()
        {
            Thing p = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p,"someString"));    
        }

        [Test]
        public void Unknown_EntInst7 ()
        {
            Thing p = new Thing("Jane");
            Assert.AreEqual(Hstate.Known, EntityArgIsUnknown(p,new Tvar("")));    
        }
    }
}