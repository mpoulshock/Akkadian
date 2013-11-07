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

using NUnit.Framework;

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class SwitchFcn : H
    {
        private static Tvar tbt = new Tvar(true);
        private static Tvar tbf = new Tvar(false);
        
        // Switch<Tvar>
        
        [Test]
        public void TvarSwitch1_lazy ()
        {
            Tvar result = Switch(()=> tbf, ()=> new Tvar(41),
                                 ()=> tbt, ()=> new Tvar(42),
                                 ()=> 43);
            
            Assert.AreEqual(42, result.Out);        
        }

        [Test]
        public void TvarSwitch2_lazy ()
        {
            Tvar result = Switch(()=> tbt, ()=> new Tvar(41),
                                 ()=> tbf, ()=> new Tvar(42),
                                 ()=> 43);
            
            Assert.AreEqual(41, result.Out);        
        }

        [Test]
        public void TvarSwitch3_lazy ()
        {
            Tvar result = Switch(()=> tbf, ()=> 41,
                                 ()=> tbt, ()=> 42,
                                 ()=> new Tvar(43));
            
            Assert.AreEqual(42, result.Out);        
        }

        [Test]
        public void TvarSwitch4_lazy ()
        {
            Tvar result = Switch(()=> false, ()=> 41,
                                 ()=> true, ()=> 42,
                                 ()=> new Tvar(43)); 
            
            Assert.AreEqual(42, result.Out);        
        }

        [Test]
        public void TvarSwitch5_lazy ()
        {
            Tvar result = Switch(()=> tbf, ()=> new Tvar(41),
                                 ()=> tbf, ()=> new Tvar(42),
                                 ()=> 43);

            Assert.AreEqual(43, result.Out);        
        }

        [Test]
        public void TvarSwitch6_lazy ()
        {
            Tvar result = Switch(()=> tbf, ()=> new Tvar(41),
                                 ()=> tbf, ()=> new Tvar(42),
                                 ()=> new Tvar(Hstate.Uncertain));  

            Assert.AreEqual("Uncertain", result.Out);        
        }

        [Test]
        public void TvarSwitch7_lazy ()
        {
            Tvar result = Switch(()=> false, ()=> 41,
                                 ()=> true, ()=> new Tvar(Hstate.Uncertain),
                                 ()=> 42);   
            
            Assert.AreEqual("Uncertain", result.Out);        
        }

        [Test]
        public void TvarSwitch8_lazy ()
        {
            Tvar result = Switch(()=> false, ()=> 41,
                                 ()=> new Tvar(Hstate.Uncertain), ()=> 101,
                                 ()=> 42);   
            
            Assert.AreEqual("Uncertain", result.Out);        
        }

        [Test]
        public void TvarSwitch9_lazy ()
        {
            Tvar result = Switch(()=> false, ()=> 41,
                                 ()=> new Tvar(Hstate.Uncertain), ()=> new Tvar(Hstate.Stub),
                                 ()=> 42);   
            
            Assert.AreEqual("Uncertain", result.Out);        
        }

        [Test]
        public void TvarSwitch10_lazy ()
        {
            Tvar x = new Tvar(10);
            x.AddState(Date(2000,1,1), 1);
            
            Tvar result = Switch(()=> x <= 1, ()=> new Tvar(1),
                                 ()=> 2);   
            
            Assert.AreEqual("{Dawn: 2; 1/1/2000: 1}", result.Out);        
        }

        [Test]
        public void TvarSwitch11_lazy ()
        {
            Tvar x = new Tvar(10);
            x.AddState(Date(2000,1,1), 1);
            
            Tvar result = Switch(()=> x >= 5, ()=> new Tvar(1),
                                 ()=> 2);   
            
            Assert.AreEqual("{Dawn: 1; 1/1/2000: 2}", result.Out);        
        }

        [Test]
        public void TvarSwitch12_lazy ()
        {
            Tvar tb = new Tvar(true);
            tb.AddState(Date(2000,1,1), false);
            
            Tvar result = Switch(()=> tb, ()=> new Tvar(41),
                                 ()=> true, ()=> new Tvar(42),
                                 ()=> new Tvar(43));   
            
            Assert.AreEqual("{Dawn: 41; 1/1/2000: 42}", result.Out);        
        }

        [Test]
        public void TvarSwitch13_lazy ()  // Tests whether irrelevant values are skipped/ignored
        {
            Tvar tb1 = new Tvar(true);
            tb1.AddState(Date(2000,1,1), false);

            Tvar tb2 = new Tvar(false);
            tb2.AddState(Date(1900,1,1),true);  // true but irrelevant b/c interval subsubmed by tb1
            tb2.AddState(Date(1912,1,1),false);

            Tvar result = Switch(()=> tb1, ()=> new Tvar(41),
                                 ()=> tb2, ()=> new Tvar(42),
                                 ()=> new Tvar(43));   
            
            Assert.AreEqual("{Dawn: 41; 1/1/2000: 43}", result.Out);        
        }

        // Switch<Tvar>

        [Test]
        public void TboolSwitch1_lazy ()
        {
            Tvar result = Switch(()=> false, ()=> true,
                                  ()=> true, ()=> true, 
                                  ()=> new Tvar(false));   
            
            Assert.AreEqual(true, result.Out);        
        }

        [Test]
        public void TboolSwitch2_lazy ()
        {
            Tvar result = Switch(()=> false, ()=> false,
                                  ()=> true, ()=> true,
                                  ()=> new Tvar(Hstate.Uncertain));   
            
            Assert.AreEqual(true, result.Out);        
        }

        [Test]
        public void TboolSwitch3_lazy () 
        {
            Tvar result = Switch(()=> false, ()=> true,
                                  ()=> new Tvar(true));   
            
            Assert.AreEqual(true, result.Out);        
        }

        // Switch<Tvar>

        [Test]
        public void TstrSwitch1_lazy ()
        {
            Tvar result = Switch(()=> false, ()=> "41",
                                  ()=> true, ()=> "42",
                                  ()=> new Tvar("43"));   
            
            Assert.AreEqual("42", result.Out);        
        }

        // MergeTvars

        [Test]
        public void MergeTvars1 ()
        {
            Hval unst = new Hval(null,Hstate.Null);

            Tvar tn1 = new Tvar(unst);
            tn1.AddState(Date(2000,1,1), 1);
            tn1.AddState(Date(2001,1,1), unst);
            tn1.AddState(Date(2002,1,1), 3);
            tn1.AddState(Date(2003,1,1), unst);

            Tvar tn2 = new Tvar(0);
            tn2.AddState(Date(2000,1,1), unst);
            tn2.AddState(Date(2001,1,1), 2);
            tn2.AddState(Date(2002,1,1), unst);
            tn2.AddState(Date(2003,1,1), unst);

            Tvar result = Util.MergeTvars(tn1,tn2);
            Assert.AreEqual("{Dawn: 0; 1/1/2000: 1; 1/1/2001: 2; 1/1/2002: 3; 1/1/2003: Null}", result.Out);        
        }

        [Test]
        public void MergeTvars2 ()  // Merge unknown Tvar w/ mixed (known and unknown) Tvar
        {
            Hval unst = new Hval(null,Hstate.Null);

            Tvar tn1 = new Tvar(unst);

            Tvar tn2 = new Tvar(0);
            tn2.AddState(Date(2000,1,1), unst);
            tn2.AddState(Date(2001,1,1), 2);
            tn2.AddState(Date(2002,1,1), unst);

            Assert.AreEqual(tn2.Out, Util.MergeTvars(tn1,tn2).Out);        
        }

        //  HasUndefinedValues

        [Test]
        public void HasUndefinedIntervals1 ()
        {
            Tvar r = new Tvar(new Hval(null,Hstate.Null));
            r.AddState(Date(2000,1,1), new Hval(null,Hstate.Unstated));
            r.AddState(Date(2001,1,1), 2);

            Assert.AreEqual(true, Util.HasUndefinedIntervals(r));        
        }

        [Test]
        public void HasUndefinedIntervals2 ()
        {
            Tvar r = new Tvar(7);
            r.AddState(Date(2000,1,1), new Hval(null,Hstate.Unstated));
            r.AddState(Date(2001,1,1), 2);

            Assert.AreEqual(false, Util.HasUndefinedIntervals(r));        
        }

        [Test]
        public void HasUndefinedIntervals3 ()  
        {
            Tvar r = new Tvar(7);
            r.AddState(Date(2000,1,1), 6);
            r.AddState(Date(2001,1,1), 2);

            Assert.AreEqual(false, Util.HasUndefinedIntervals(r));        
        }

        // Inner components of Switch

        [Test]
        public void AssignAndMerge1 ()
        {
            Tvar newCondition = new Tvar(Hstate.Uncertain);
            Tvar initialResult = new Tvar(Hstate.Null);
            Tvar newConditionIsUnknown = new Tvar(true);

            Tvar result = Util.MergeTvars(initialResult,
                                         Util.ConditionalAssignment(newConditionIsUnknown, newCondition));

            Assert.AreEqual("Uncertain", result.Out); 
        }

        [Test]
        public void AssignAndMerge2 ()
        {
            Tvar newCondition = new Tvar(Hstate.Uncertain);
            Tvar initialResult = new Tvar(Hstate.Null);
            Tvar newConditionIsUnknown = new Tvar(true);

            Tvar result = Util.MergeTvars(initialResult,
                                         Util.ConditionalAssignment(newConditionIsUnknown, newCondition));

            Assert.AreEqual(false, Util.HasUndefinedIntervals(result)); 
        }

        [Test]
        public void AssignAndMerge3 ()
        {
            Tvar newCondition = new Tvar(Hstate.Uncertain);
            Tvar newConditionIsUnknown = Util.HasUnknownState(newCondition);
            Assert.AreEqual(true, newConditionIsUnknown.Out); 
        }
    }
}