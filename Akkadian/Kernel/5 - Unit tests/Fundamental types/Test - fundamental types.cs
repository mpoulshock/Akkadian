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

namespace Akkadian.UnitTests
{
    [TestFixture]
    public class FundamentalTypes : Tvar
    {
        // .Lean
        
        [Test]
        public void FT_Lean_1 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Time.DawnOf.AddYears(5), true);
            Tvar res = t.Lean;
            Assert.AreEqual(true, res.Out);        
        }
        
        // .AsOf
        
        [Test]
        public void FT_AsOf_1 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = t.AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void FT_AsOf_2 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void FT_AsOf_3 ()
        {
            Tvar t = new Tvar(true);
            Tvar res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void FT_AsOf_4 ()
        {
            Tvar t = new Tvar(4);
            t.AddState(Time.DawnOf.AddYears(5), 44);
            Tvar res = t.AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual(4, res.Out);        
        }
        
        [Test]
        public void FT_AsOf_5 ()
        {
            Tvar t = new Tvar(4);
            t.AddState(Time.DawnOf.AddYears(5), 44);
            Tvar res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual(44, res.Out);        
        }
        
        [Test]
        public void FT_AsOf_6 ()
        {
            Tvar t = new Tvar("ham");
            t.AddState(Time.DawnOf.AddYears(5), "sam");
            Tvar res = t.AsOf(Time.DawnOf.AddYears(12));
            Assert.AreEqual("sam", res.Out);        
        }
        
        [Test]
        public void FT_AsOf_7 ()
        {
            Tvar t = new Tvar("ham");
            t.AddState(Time.DawnOf.AddYears(5), "sam");
            Tvar res = t.AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual("ham", res.Out);        
        }
        
        [Test]
        public void FT_AsOf_8 ()
        {
            Tvar res = new Tvar(Hstate.Uncertain).AsOf(Time.DawnOf.AddYears(2));
            Assert.AreEqual("Uncertain", res.Out);       
        }

        [Test]
        public void FT_AsOf_9 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(new DateTime(2000,1,1), false);

            Tvar time = new Tvar(1999,1,1);

            Assert.AreEqual(true, t.AsOf(time).Out);        
        }

        [Test]
        public void FT_AsOf_10 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Time.DawnOf.AddYears(5), false);

            Tvar time = new Tvar(Hstate.Stub);

            Assert.AreEqual("Stub", t.AsOf(time).Out);        
        }

        [Test]
        public void FT_AsOf_11 ()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar time = new Tvar(Hstate.Unstated);
            Assert.AreEqual("Stub", t.AsOf(time).Out);        
        }

        [Test]
        public void FT_AsOf_12 ()
        {
            // Tvar varies, but base Tvar is eternal, so .AsOf should return that eternal value
            Tvar t = new Tvar(true);
            Tvar time = new Tvar(Date(2000,1,1));
            time.AddState(Date(2010,1,1),Date(2010,1,1));
            Assert.AreEqual(true, t.AsOf(time).Out);        
        }

        [Test]
        public void FT_AsOf_13 ()
        {
            // Tvar unknown, but base Tvar is eternal, so .AsOf should return that eternal value
            Tvar t = new Tvar(true);
            Tvar time = new Tvar(Hstate.Stub);
            Assert.AreEqual(true, t.AsOf(time).Out);        
        }

        [Test]
        public void FT_AsOf_14 ()
        {
            // Both Tvar and Tvar vary
            Tvar t = new Tvar(true);
            t.AddState(Date(2000,1,1),false);

            // When Tvar varies, the FirstValue is used...
            Tvar time = new Tvar(Date(1999,1,1));
            time.AddState(Date(2010,1,1),Date(2010,1,1));

            Assert.AreEqual(true, t.AsOf(time).Out);        
        }

        // ObjectAsOf

        [Test]
        public void ObjectAsOf1 ()
        {
            Thing P1 = new Thing("P1");
            Tvar tsv = new Tvar(Hstate.Stub);
            tsv.AddState(Date(2000,01,01),P1);
            tsv.AddState(Date(2001,01,01),Hstate.Uncertain);
            Assert.AreEqual(Hstate.Stub, tsv.ObjectAsOf(Date(1999,01,01)).Val);
        }

        [Test]
        public void ObjectAsOf2 ()
        {
            Thing P1 = new Thing("P1");
            Tvar tsv = new Tvar(Hstate.Stub);
            tsv.AddState(Date(2000,01,01),P1);
            tsv.AddState(Date(2001,01,01),Hstate.Uncertain);
            Assert.AreEqual(Hstate.Uncertain, tsv.ObjectAsOf(Date(2002,02,01)).Val);
        }

        [Test]
        public void ObjectAsOf3 ()
        {
            Tvar item = new Tvar(3);
            Hval h = item.ObjectAsOf(DateTime.Now);
            Assert.AreEqual(3, h.Val);        
        }

        [Test]
        public void ObjectAsOf4 ()
        {
            Tvar item = new Tvar(Hstate.Stub);
            Hval h = item.ObjectAsOf(DateTime.Now);
            Assert.AreEqual("Stub", h.ToString);        
        }

        // .IsAlways
        
        [Test]
        public void FT_IsAlways_1 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = t.IsAlwaysTrue();
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_2 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = (!t).IsAlwaysTrue();
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_3 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            Tvar res = t.IsAlwaysTrue();
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_4 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = t.IsAlwaysTrue(Time.DawnOf.AddYears(3), Time.DawnOf.AddYears(9));
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_5 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = t.IsAlwaysTrue(Time.DawnOf.AddYears(2), Time.DawnOf.AddYears(3));
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_6 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = t.IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_7 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = (!t).IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_8 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            Tvar res = t.IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_9 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            Tvar res = (!t).IsAlwaysTrue(Time.DawnOf.AddYears(7), Time.DawnOf.AddYears(9));
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_10 ()
        {
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, true);
            Tvar res = (!t).IsAlwaysTrue();
            Assert.AreEqual(false, res.Out);        
        }
        
        [Test]
        public void FT_IsAlways_11 ()
        {
            Tvar t = new Tvar(3.4) == 3.4;
            Tvar res = t.IsAlwaysTrue();
            Assert.AreEqual(true, res.Out);       
        }

        // .IsEver
        
        [Test]
        public void FT_IsEver_1 ()
        {
            Tvar t = new Tvar(true);
            t.AddState(Time.DawnOf.AddYears(5), false);
            Tvar res = t.IsEverTrue();
            Assert.AreEqual(true, res.Out);        
        }
        
        [Test]
        public void FT_IsEver_2 ()
        {
            Tvar t = new Tvar(false);
            Tvar res = t.IsEverTrue();
            Assert.AreEqual(false, res.Out);        
        }

        [Test]
        public void FT_IsEver_8 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2012,11,8), true);
            Tvar res = t.IsEverTrue();
            Assert.AreEqual(true, res.Out);            
        }
        
        [Test]
        public void FT_IsEver_9 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2012,11,8), true);
            Tvar res = t.IsEverTrue(Date(2013,1,1), Date(2014,1,1));
            Assert.AreEqual(true, res.Out);            
        }
        
        [Test]
        public void FT_IsEver_10 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2012,11,8), true);
            Tvar res = t.IsEverTrue(Date(2012,1,1), Date(2013,1,1));
            Assert.AreEqual(true, res.Out);            
        }
        
        [Test]
        public void FT_IsEver_11 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2012,11,8), true);
            Tvar res = t.IsEverTrue(Date(2011,1,1), Date(2012,1,1));
            Assert.AreEqual(false, res.Out);            
        }

        [Test]
        public void FT_IsEver_13 ()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar res = t.IsEverTrue(Time.DawnOf.AddYears(3), Time.DawnOf.AddYears(9));
            Assert.AreEqual("Unstated", res.Out);        
        }

        [Test]
        public void FT_IsEver_14 ()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar res = t.IsEverTrue();
            Assert.AreEqual("Unstated", res.Out);        
        }

        [Test]
        public void FT_IsEver_15 ()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Tvar res = t.IsEverTrue();
            Assert.AreEqual("Stub", res.Out);        
        }
        
        // .ToBool
        
        [Test]
        public void FT_ToBool_1 ()
        {
            Tvar t = new Tvar(true);
            Assert.AreEqual(true, t.ToBool);        
        }
        
        [Test]
        public void FT_ToBool_2 ()
        {
            Tvar t = new Tvar(false);
            Assert.AreEqual(false, t.ToBool);        
        }
        
        [Test]
        public void FT_ToBool_3 ()
        {
            // If value is not eternal, .ToBool should return null
            Tvar t = new Tvar(false);
            t.AddState(Time.DawnOf.AddYears(5), true);
            Assert.AreEqual(null, t.ToBool);        
        }
        
        [Test]
        public void FT_ToBool_4 ()
        {
            // If value is unknown, .ToBool should return null
            Tvar t = new Tvar(Hstate.Uncertain);
            Assert.AreEqual(null, t.ToBool);        
        }
        
        // .EverPerInterval
        
        [Test]
        public void FT_EverPerInterval_1 ()
        {
            // This will break annually b/c Year is determined by the system clock
            Tvar theYear = Time.Year(5);
            
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, false);
            t.AddState(Date(2012,11,8), true);
            
            Tvar result = t.EverPer(theYear);
			Assert.AreEqual("{Dawn: false, 2012-01-01: true}", result.Out);        
        }
        
        [Test]
        public void FT_EverPerInterval_2 ()
        {
            Tvar theYear = Time.Year(5);
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar result = t.EverPer(theYear);
            Assert.AreEqual("Unstated", result.Lean.Out);        
        }

        [Test]
        public void FT_EverPerInterval_3 ()
        {
            Tvar theYear = new Tvar(Hstate.Stub);
            Tvar t = new Tvar(Hstate.Unstated);
            Tvar result = t.EverPer(theYear);
            Assert.AreEqual("Stub", result.Lean.Out);        
        }

        
        // .AlwaysPerInterval
        
        [Test]
        public void FT_AlwaysPerInterval_1 ()
        {
            // This will break annually b/c Year is determined by the system clock
            Tvar theYear = Time.Year(5);
            
            Tvar t = new Tvar();
            t.AddState(Time.DawnOf, false);
            t.AddState(Date(2012,11,8), true);
            
            Tvar result = t.AlwaysPer(theYear);
			Assert.AreEqual("{Dawn: false, 2013-01-01: true}", result.Out);        
        }
        
        [Test]
        public void FT_AlwaysPerInterval_2 ()
        {
            Tvar theYear = Time.Year(5);
            Tvar result = new Tvar(Hstate.Stub).AlwaysPer(theYear);
            Assert.AreEqual("Stub", result.Lean.Out);        
        }
        
        // .ToInt
        
        [Test]
        public void FT_ToInt_1 () 
        {
            Tvar t = new Tvar(42);
            Assert.AreEqual(42, t.ToInt);        
        }
        
        [Test]
        public void FT_ToInt_2 ()
        {
            Tvar t = new Tvar(Hstate.Uncertain);
            Assert.AreEqual(null, t.ToInt);        
        }
        
        [Test]
        public void FT_ToInt_3 ()
        {
            Tvar t = new Tvar(42);
            t.AddState(Date(1912,5,3), 43);
            Assert.AreEqual(null, t.ToInt);        
        }
        
        // Tvar.ToDecimal
        
        [Test]
        public void FT_ToDecimal_1 ()
        {
            Tvar t = new Tvar(42.1);
            Assert.AreEqual(42.1, t.ToDecimal);        
        }
        
        [Test]
        public void FT_ToDecimal_2 ()
        {
            Tvar t = new Tvar(Hstate.Uncertain);
            Assert.AreEqual(null, t.ToDecimal);        
        }
        
        [Test]
        public void FT_ToDecimal_3 ()
        {
            Tvar t = new Tvar(42.13);
            t.AddState(Date(1912,5,3), 43.13);
            Assert.AreEqual(null, t.ToDecimal);        
        }
        
        // Tvar.ToString
        
        [Test]
        public void FT_ToString_1 ()
        {
            Tvar t = new Tvar("42");
            Assert.AreEqual("42", t.ToString());        
        }
        
        [Test]
        public void FT_ToString_2 ()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Assert.AreEqual("Stub", t.ToString());        
        }
        
        [Test]
        public void FT_ToString_3 ()
        {
            Tvar t = new Tvar("42");
            t.AddState(Date(2004,2,21), "43");
			Assert.AreEqual("{Dawn: 42, 2004-02-21: 43}", t.ToString());        
        }
        
        // Tvar.ToDateTime
        
        [Test]
        public void FT_ToDateTime_1 ()
        {
            Tvar t = new Tvar(2000,1,1);
            Assert.AreEqual(Date(2000,1,1), t.ToNullDateTime);        
        }
        
        [Test]
        public void FT_ToDateTime_2 ()
        {
            Tvar t = new Tvar(Hstate.Uncertain);
            Assert.AreEqual(null, t.ToNullDateTime);        
        }
        
        [Test]
        public void FT_ToDateTime_3 ()
        {
            Tvar t = new Tvar(Date(1000,1,1));
            t.AddState(Date(1000,1,1), Date(2000,1,1));
            Assert.AreEqual(null, t.ToNullDateTime);        
        }

        // Tvar.CountPer
        
        [Test]
        public void FT_CountPer_1 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), true);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tvar actual = t.CountPer(TheYear);
			Assert.AreEqual("{Dawn: 0, 2010-01-01: 3, 2011-01-01: 0}", actual.Out);      
        }
        
        [Test]
        public void FT_CountPer_2 ()
        {
            Tvar t = new Tvar(false);
            Tvar actual = t.CountPer(TheYear);
            Assert.AreEqual(0, actual.Out);      
        }
        
        [Test]
        public void FT_CountPer_3 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), false);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tvar actual = t.CountPer(TheYear);
			Assert.AreEqual("{Dawn: 0, 2010-01-01: 2, 2011-01-01: 0}", actual.Out);      
        }
        
        [Test]
        public void FT_CountPer_4 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2010,11,1), true);
            t.AddState(Date(2010,12,1), true);
            t.AddState(Date(2011,1,1), true);
            t.AddState(Date(2011,2,1), false);
            Tvar actual = t.CountPer(TheYear);
			Assert.AreEqual("{Dawn: 0, 2010-01-01: 2, 2011-01-01: 1, 2012-01-01: 0}", actual.Out);      
        }
        
        [Test]
        public void FT_CountPer_5 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2009,12,15), true);
            t.AddState(Date(2010,2,1), false);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tvar actual = t.CountPer(TheYear);
			Assert.AreEqual("{Dawn: 0, 2010-01-01: 1, 2011-01-01: 0}", actual.Out);      
        }

        // Tvar.RunningCountPer
        
        [Test]
        public void FT_RunningCountPer_1 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), true);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tvar actual = t.RunningCountPer(TheYear);
			Assert.AreEqual("{Dawn: 0, 2010-02-01: 1, 2010-03-01: 2, 2010-04-01: 3}", actual.Out);      
        }
        
        [Test]
        public void FT_RunningCountPer_2 ()
        {
            Tvar t = new Tvar(false);
            Tvar actual = t.RunningCountPer(TheYear);
            Assert.AreEqual(0, actual.Out);      
        }
        
        [Test]
        public void FT_RunningCountPer_3 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2010,1,1), true);
            t.AddState(Date(2010,2,1), false);
            t.AddState(Date(2010,3,1), true);
            t.AddState(Date(2010,4,1), false);
            Tvar actual = t.RunningCountPer(TheYear);
			Assert.AreEqual("{Dawn: 0, 2010-02-01: 1, 2010-04-01: 2}", actual.Out);      
        }

        // Tvar.DateFirstTrue

        [Test]
        public void DateFirst_1 ()
        {
            // Base Tvar never meets the specified condition
            Tvar t = new Tvar(false);
            Assert.AreEqual("Stub", t.DateFirstTrue.Out);      
        }
        
        [Test]
        public void DateFirst_2 ()
        {
            Tvar t = new Tvar(true);
            Assert.AreEqual(Time.DawnOf, t.DateFirstTrue.ToDateTime);   
        }
        
        [Test]
        public void DateFirst_3 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            Assert.AreEqual(Date(2000,1,1), t.DateFirstTrue.ToDateTime);      
        }

        [Test]
        public void DateFirst_4 ()
        {
            // Base Tvar is eternally unknown; that state must percolate up
            Tvar t = new Tvar(Hstate.Unstated);
            Assert.AreEqual("Unstated", t.DateFirstTrue.Out);      
        }

        [Test]
        public void DateFirst_5 ()
        {
            // Base Tvar is unknown, then the required value
            Tvar t = new Tvar(Hstate.Unstated);
            t.AddState(Date(2000,1,1), true);
            Assert.AreEqual(Date(2000,1,1), t.DateFirstTrue.ToDateTime);      
        }

        [Test]
        public void DateFirst_6 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2000,1,1),true);
            tb.AddState(new DateTime(2000,1,3),false);
            tb.AddState(new DateTime(2000,1,10),true);
            tb.AddState(new DateTime(2000,2,18),false);

            Assert.AreEqual(Date(2000,1,1), tb.DateFirstTrue.ToDateTime);      
        }

        // Tvar.DateLastTrue

        [Test]
        public void DateLast_1 ()
        {
            // Base Tvar never meets the specified condition
            Tvar t = new Tvar(false);
            Assert.AreEqual("Stub", t.DateLastTrue.Out);      
        }
        
        [Test]
        public void DateLast_2 ()
        {
            Tvar t = new Tvar(true);
            Assert.AreEqual(Time.EndOf, t.DateLastTrue.ToDateTime);   
        }
        
        [Test]
        public void DateLast_3 ()
        {
            Tvar t = new Tvar(false);
            t.AddState(Date(2000,1,1), true);
            Assert.AreEqual(Time.EndOf, t.DateLastTrue.ToDateTime);      
        }

        [Test]
        public void DateLast_4 ()
        {
            // Base Tvar is eternally unknown; that state must percolate up
            Tvar t = new Tvar(Hstate.Unstated);
            Assert.AreEqual("Unstated", t.DateLastTrue.Out);      
        }

        [Test]
        public void DateLast_5 ()
        {
            // Base Tvar is unknown, then the required value
            Tvar t = new Tvar(Hstate.Unstated);
            t.AddState(Date(2000,1,1), true);
            Assert.AreEqual(Time.EndOf, t.DateLastTrue.ToDateTime);      
        }

        [Test]
        public void DateLast_6 ()
        {
            Tvar tb = new Tvar(false);
            tb.AddState(new DateTime(2000,1,1),true);
            tb.AddState(new DateTime(2000,1,3),false);
            tb.AddState(new DateTime(2000,1,10),true);
            tb.AddState(new DateTime(2000,2,18),false);

            Assert.AreEqual(Date(2000,2,17), tb.DateLastTrue.ToDateTime);      
        }

        // Type identification
        
        [Test]
        public void TvarTypeTest1 ()
        {
            bool isTvar = new Tvar().GetType().ToString() == "Akkadian.Tvar";
            Assert.AreEqual(true, isTvar);        
        }
        
//        [Test]
//        public void TvarTypeTest1 ()
//        {
//            Tvar item = new Tvar(3);
//            bool istype = Util.IsType<Tvar>(item);
//            Assert.AreEqual(true, istype);        
//        }
//        
//        [Test]
//        public void TvarTypeTest2 ()
//        {
//            Tvar item = new Tvar(3);
//            bool istype = Util.IsType<Tvar>(item);
//            Assert.AreEqual(false, istype);        
//        }

        // Value re-assignment

        [Test]
        public void ValueReassignment1 ()
        {
            Tvar t = new Tvar(3);
            t = new Tvar(4);
            Assert.AreEqual(4, t.Out);        
        }

        // .IsUnstated

        [Test]
        public void IsUnstated1 ()
        {
            Hval unst = new Hval(null,Hstate.Unstated);

            Tvar tb1 = new Tvar(false);
            tb1.AddState(Date(2000,1,1), unst);
            tb1.AddState(Date(2001,1,1), true);

			Assert.AreEqual("{Dawn: false, 2000-01-01: true, 2001-01-01: false}", tb1.IsUnstated.Out);        
        }

        // .Shift

        [Test]
        public void FT_Shift_1 ()
        {
            Tvar t = new Tvar(0);
            t.AddState(Date(2010,1,1), 100);
            t.AddState(Date(2011,1,1), 200);
            Tvar actual = t.Shift(-1, TheYear);
			Assert.AreEqual("{Dawn: 0, 2011-01-01: 100, 2012-01-01: 200}", actual.Out);      
        }

        [Test]
        public void FT_Shift_2 ()
        {
            Tvar t = new Tvar(0);
            t.AddState(Date(2010,1,1), 100);
            t.AddState(Date(2011,1,1), 200);
            Tvar actual = t.Shift(0, TheYear);
			Assert.AreEqual("{Dawn: 0, 2010-01-01: 100, 2011-01-01: 200}", actual.Out);      
        }

        [Test]
        public void FT_Shift_3 ()
        {
            Tvar t = new Tvar(0);
            t.AddState(Date(2010,1,1), 100);
            t.AddState(Date(2011,1,1), 200);
            Tvar actual = t.Shift(2, TheYear);
			Assert.AreEqual("{Dawn: 0, 2008-01-01: 100, 2009-01-01: 200}", actual.Out);      
        }

        [Test]
        public void FT_Shift_uncertain1 ()
        {
            Tvar t = new Tvar(Hstate.Stub);
            Assert.AreEqual("Stub", t.Shift(2, TheYear).Out);      
        }

        [Test]
        public void FT_Shift_uncertain2 ()
        {
            Tvar t = new Tvar(Hstate.Uncertain);
            Assert.AreEqual("Uncertain", t.Shift(-2, TheYear).Out);      
        }

        [Test]
        public void FT_Shift_uncertain3 ()
        {
            Tvar t = new Tvar(Hstate.Unstated);
            Assert.AreEqual("Unstated", t.Shift(0, TheYear).Out);      
        }

		[Test]
		public void FT_Shift_uncertain4 ()
		{
			Tvar t1 = new Tvar(6);
			Tvar t2 = new Tvar(Hstate.Unstated);
			Assert.AreEqual("Unstated", t1.Shift(t2, TheYear).Out);      
		}

        // DateIsInPeriod

        [Test]
        public void DateIsInPeriod1 ()
        {
            Tvar d = new Tvar(2015, 1, 2);
            Assert.AreEqual(false, d.IsInPeriod(TheYear).AsOf(Date(2014, 12, 31)).Out);      
        }

        [Test]
        public void DateIsInPeriod2 ()
        {
            Tvar d = new Tvar(2015, 1, 2);
            Assert.AreEqual(true, d.IsInPeriod(TheYear).AsOf(Date(2015, 12, 31)).Out);      
        }

        [Test]
        public void DateIsInPeriod3 ()
        {
            Tvar d = new Tvar(2015, 1, 2);
            Assert.AreEqual(false, d.IsInPeriod(TheYear).AsOf(Date(2016, 12, 31)).Out);      
        }

        // Tvar.Quarter

        [Test]
        public void Test_GetQtr1()
        {
            Assert.AreEqual(1, (new Tvar(2010,3,2)).Quarter.Out);
        }

        [Test]
        public void Test_GetQtr2()
        {
            Assert.AreEqual(4, (new Tvar(2010,12,2)).Quarter.Out);
        }

        [Test]
        public void Test_GetQtr3()
        {
            Assert.AreEqual("Unstated", (new Tvar(Hstate.Unstated)).Quarter.Out);
        }

		// Converting Tvars to more specific T-types

//		[Test]
//		public void Test_CastTvar_1()
//		{
//			Tvar tv = new Tvar(4);
//			Tvar cast = (Tvar)tv;
//			Assert.AreEqual(4, cast.Out);
//		}
    }    
}