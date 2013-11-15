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
    public class BasicLogic : H
    {
        private static Tvar tbt = new Tvar(true);
        private static Tvar tbf = new Tvar(false);
        private static Tvar stub = new Tvar(Hstate.Stub);
        private static Tvar unstat = new Tvar(Hstate.Unstated); 
        private static Tvar uncert = new Tvar(Hstate.Uncertain);  
        private static Tvar tbv = VaryingTvar(); 

        private static Tvar VaryingTvar()
        {
            Tvar result = new Tvar(false);
            result.AddState(Date(2000,1,1), true);
            result.AddState(Date(2001,1,1), Hstate.Uncertain);
            result.AddState(Date(2002,1,1), Hstate.Unstated);
            return result;
        }

        // AND
        
        [Test]
        public void LogicAnd1 ()
        {
            Tvar t1 = tbt & tbf;
            Assert.AreEqual(false, t1.Out);        
        }
        
        [Test]
        public void LogicAnd2 ()
        {
            Tvar t2 = tbt & tbt;
            Assert.AreEqual(true, t2.Out);        
        }
        
        [Test]
        public void LogicAnd3 ()
        {
            Tvar t7 = tbf & tbf;
            Assert.AreEqual(false, t7.Out);        
        }
        
        [Test]
        public void LogicAnd4 ()
        {
            Tvar t1 = tbt & tbt & tbf;
            Assert.AreEqual(false, t1.Out);            
        }
        
        [Test]
        public void LogicAnd5 ()
        {
            Tvar r = tbt & uncert;
            Assert.AreEqual("Uncertain", r.Out);          
        }
        
        [Test]
        public void LogicAnd6 ()
        {
            Tvar r = tbf & uncert;
            Assert.AreEqual(false, r.Out);          
        }
        
        [Test]
        public void LogicAnd7 ()
        {
            Tvar r = uncert & uncert;
            Assert.AreEqual("Uncertain", r.Out);          
        }
        
        [Test]
        public void LogicAnd8 ()
        {
            Tvar r = unstat & uncert;
            Assert.AreEqual("Unstated", r.Out);          
        }
        
        [Test]
        public void LogicAnd9 ()
        {
            Tvar t1 = tbt & false;
            Assert.AreEqual(false, t1.Out);            
        }
        
        [Test]
        public void LogicAnd10 ()
        {
            Tvar t1 = false & tbt;
            Assert.AreEqual(false, t1.Out);            
        }
        
        [Test]
        public void Unknown_Logic_And_1 ()
        {
            Tvar t1 = tbt & tbf & unstat;
            Assert.AreEqual(false, t1.Out);        
        }
        
        [Test]
        public void Unknown_Logic_And_2 ()
        {
            Tvar t1 = tbt & unstat & tbf;
            Assert.AreEqual(false, t1.Out);        
        }
        
        [Test]
        public void Unknown_Logic_And_3 ()
        {
            Tvar t1 = tbt & unstat;
            Assert.AreEqual("Unstated", t1.Out);        
        }
        
        [Test]
        public void Unknown_Logic_And_4 ()
        {
            Tvar t1 = tbf & unstat;
            Assert.AreEqual(false, t1.Out);        
        }

        [Test]
        public void Unknown_Logic_And_5 ()
        {
            Tvar r = unstat & stub;
            Assert.AreEqual("Unstated", r.Out);        
        }

        [Test]
        public void Unknown_Logic_And_6 ()
        {
            Tvar r = tbf & stub;
            Assert.AreEqual(false, r.Out);        
        }

        [Test]
        public void Unknown_Logic_And_7 ()
        {
            Tvar r = tbt & stub;
            Assert.AreEqual("Stub", r.Out);        
        }

        [Test]
        public void Unknown_Logic_And_8 ()
        {
            Tvar r = uncert & stub;
            Assert.AreEqual("Uncertain", r.Out);        
        }

        [Test]
        public void LogicAndTime1 ()
        {
            Tvar t1 = tbv & unstat;
			Assert.AreEqual("{Dawn: false, 2000-01-01: Unstated}", t1.Out);            
        }
        
        [Test]
        public void LogicAndTime2 ()
        {
            Tvar t1 = tbv & tbt;
			Assert.AreEqual("{Dawn: false, 2000-01-01: true, 2001-01-01: Uncertain, 2002-01-01: Unstated}", t1.Out);            
        }

        [Test]
        public void LogicAndTime3 ()
        {
            Tvar t1 = tbv & tbf;
            Assert.AreEqual(false, t1.Out);            
        }
        
        [Test]
        public void LogicAndTime4 ()
        {
            Tvar t1 = tbv & uncert;
			Assert.AreEqual("{Dawn: false, 2000-01-01: Uncertain, 2002-01-01: Unstated}", t1.Out);            
        }
        
        // OR
        
        [Test]
        public void LogicOr1 ()
        {
            Tvar t1 = tbt | tbf;
            Assert.AreEqual(true, t1.Out);        
        }
        
        [Test]
        public void LogicOr2 ()
        {
            Tvar t2 = tbt | tbt;
            Assert.AreEqual(true, t2.Out);        
        }
        
        [Test]
        public void LogicOr3 ()
        {
            Tvar t7 = tbf | tbf;
            Assert.AreEqual(false, t7.Out);        
        }
        
        [Test]
        public void LogicOr4 ()
        {
            Tvar t1 = tbf | tbf | tbt;
            Assert.AreEqual(true, t1.Out);            
        }
        
        [Test]
        public void LogicOr5 ()
        {
            Tvar t1 = tbf | (tbf | tbt);
            Assert.AreEqual(true, t1.Out);            
        }
        
        [Test]
        public void Unknown_Logic_Or_1 ()
        {
            Tvar t1 = unstat | tbf | tbt;
            Assert.AreEqual(true, t1.Out);        
        }
        
        [Test]
        public void Unknown_Logic_Or_2 ()
        {
            Tvar t1 = tbt | unstat | tbf;
            Assert.AreEqual(true, t1.Out);        
        }
        
        [Test]
        public void Unknown_Logic_Or_3 ()
        {
            Tvar t1 = tbt | unstat;
            Assert.AreEqual(true, t1.Out);        
        }
        
        [Test]
        public void Unknown_Logic_Or_4 ()
        {
            Tvar t1 = tbf | unstat;
            Assert.AreEqual("Unstated", t1.Out);         
        }

        [Test]
        public void Unknown_Logic_Or_5 ()
        {
            Tvar r = unstat | stub;
            Assert.AreEqual("Unstated", r.Out);        
        }

        [Test]
        public void Unknown_Logic_Or_6 ()
        {
            Tvar r = tbf | stub;
            Assert.AreEqual("Stub", r.Out);        
        }

        [Test]
        public void Unknown_Logic_Or_7 ()
        {
            Tvar r = tbt | stub;
            Assert.AreEqual(true, r.Out);        
        }

        [Test]
        public void Unknown_Logic_Or_8 ()
        {
            Tvar r = uncert | stub;
            Assert.AreEqual("Uncertain", r.Out);        
        }
        
        [Test]
        public void LogicOrTime1 ()
        {
            Tvar t1 = tbv | unstat;
			Assert.AreEqual("{Dawn: Unstated, 2000-01-01: true, 2001-01-01: Unstated}", t1.Out);            
        }
        
        [Test]
        public void LogicOrTime2 ()
        {
            Tvar t1 = tbv | tbt;
            Assert.AreEqual(true, t1.Out);            
        }
        
        [Test]
        public void LogicOrTime3 ()
        {
            Tvar t1 = tbv | tbf;
			Assert.AreEqual("{Dawn: false, 2000-01-01: true, 2001-01-01: Uncertain, 2002-01-01: Unstated}", t1.Out);            
        }
        
        [Test]
        public void LogicOrTime4 ()
        {
            Tvar t1 = tbv | uncert;
			Assert.AreEqual("{Dawn: Uncertain, 2000-01-01: true, 2001-01-01: Uncertain, 2002-01-01: Unstated}", t1.Out);            
        }
        
        // NOT
        
        [Test]
        public void LogicNot1 ()
        {
            Tvar t1 = !tbt;
            Assert.AreEqual(false, t1.Out);    
        }
        
        [Test]
        public void LogicNot2 ()
        {
            Tvar t2 = !tbf;
            Assert.AreEqual(true, t2.Out);        
        }
        
        [Test]
        public void LogicNot3 ()
        {
            Tvar t2 = !unstat;
            Assert.AreEqual("Unstated", t2.Out);        
        }

        [Test]
        public void LogicNot4 ()
        {
            Tvar t2 = !uncert;
            Assert.AreEqual("Uncertain", t2.Out);        
        }
        
        [Test]
        public void LogicNot5 () 
        {
            Tvar t1 = !tbv;
			Assert.AreEqual("{Dawn: true, 2000-01-01: false, 2001-01-01: Uncertain, 2002-01-01: Unstated}", t1.Out);        
        }

        [Test]
        public void LogicNot5a ()
        {
			Assert.AreEqual("{Dawn: false, 2000-01-01: true, 2001-01-01: Uncertain, 2002-01-01: Unstated}", tbv.Out);        
        }

        // Basic logic - nested and/or
        
        [Test]
        public void Unknown_Logic_AndOr_1 ()
        {
            Tvar t1 = tbf | ( unstat & tbt );
            Assert.AreEqual("Unstated", t1.Out);        
        }
 
        // BOOL COUNT
        
        [Test]
        public void LogicBoolCount1 ()
        {
            Tvar result = BoolCount(tbt);
            Assert.AreEqual(1, result.Out);    
        }
        
        [Test]
        public void LogicBoolCount2 ()
        {
            Tvar result = BoolCount(tbf);
            Assert.AreEqual(0, result.Out);    
        }
        
        [Test]
        public void LogicBoolCount3 ()
        {
            Tvar result = BoolCount(tbf, tbt);
            Assert.AreEqual(1, result.Out);    
        }
        
        [Test]
        public void LogicBoolCount4 ()
        {
            Tvar result = BoolCount(tbt, tbt);
            Assert.AreEqual(2, result.Out);    
        }
        
        [Test]
        public void LogicBoolCount5 ()
        {
            Tvar result = BoolCount(tbt, tbf);
            Assert.AreEqual(1, result.Out);    
        }
        
    }
}