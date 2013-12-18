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
    public class TemporalDate : H
    {
        // .Lean
        
        [Test]
        public void Tvar_Lean_1 ()
        {
            Tvar td = new Tvar();
            td.AddState(Time.DawnOf, Date(2011,01,01));
            td.AddState(Time.DawnOf.AddYears(2), Date(2011,01,01));
            Assert.AreEqual(Date(2011,1,1), td.Lean.Out);        
        }
        
        // .AsOf
        
        [Test]
        public void Tvar_AsOf_1 ()
        {
            Tvar td = new Tvar();
            td.AddState(Time.DawnOf, Date(2011,01,01));
            td.AddState(Time.DawnOf.AddYears(2), Date(2012,01,01));
            Assert.AreEqual(Date(2012,1,1), td.AsOf(Time.DawnOf.AddYears(3)).Out);        
        }
        
        [Test]
        public void Tvar_AsOf_2 ()
        {
            Tvar td = new Tvar();
            td.AddState(Time.DawnOf, Date(2011,01,01));
            td.AddState(Time.DawnOf.AddYears(2), Date(2012,01,01));
            Assert.AreEqual(Date(2011,1,1), td.AsOf(Time.DawnOf.AddYears(1)).Out);        
        }
                
        // Equals
        
        [Test]
        public void Tvar_Equals_1 ()
        {
            Tvar td1 = new Tvar(2010,5,13);

            Tvar td2 = new Tvar();
            td2.AddState(Time.DawnOf, Date(2011,01,01));
            td2.AddState(Date(2000,1,1), Date(2010,5,13));

            Tvar result = td1 == td2;
			Assert.AreEqual("{Dawn: False, 2000-01-01: True}", result.Out);        
        }
        
        [Test]
        public void Tvar_Equals_2 ()
        {
            Tvar td1 = new Tvar(2010,5,13);
            Tvar td2 = new Tvar();
            td2.AddState(Time.DawnOf, Date(2011,01,01));
            td2.AddState(Date(2000,1,1), Date(2010,5,13));
            Tvar result = td1 != td2;
			Assert.AreEqual("{Dawn: True, 2000-01-01: False}", result.Out);        
        }
        
        // IsBefore / IsAfter
        
        [Test]
        public void Tvar_IsAfter_1 ()
        {
            Tvar td1 = new Tvar(2010,1,1);
            Tvar td2 = new Tvar();
            td2.AddState(Time.DawnOf, Date(2009,1,1));
            td2.AddState(Date(2000,1,1), Date(2011,1,1));
            Tvar result = td1 > td2;
			Assert.AreEqual("{Dawn: True, 2000-01-01: False}", result.Out);        
        }
        
        [Test]
        public void Tvar_IsBefore_1 ()
        {
            Tvar td1 = new Tvar(2010,1,1);
            Tvar td2 = new Tvar();
            td2.AddState(Time.DawnOf, Date(2009,1,1));
            td2.AddState(Date(2000,1,1), Date(2011,1,1));
            Tvar result = td1 < td2;
			Assert.AreEqual("{Dawn: False, 2000-01-01: True}", result.Out);        
        }
        
        [Test]
        public void Tvar_IsAtOrAfter_1 ()
        {
            Tvar td1 = new Tvar(2008,1,1);
            Tvar td2 = new Tvar();
            td2.AddState(Time.DawnOf, Date(2009,1,1));
            td2.AddState(Date(2000,1,1), Date(2008,1,1));
            Tvar result = td1 >= td2;
			Assert.AreEqual("{Dawn: False, 2000-01-01: True}", result.Out);        
        }
        
        [Test]
        public void Tvar_IsAtOrBefore_1 ()
        {
            Tvar td1 = new Tvar(2000,1,1);
            Tvar td2 = new Tvar();
            td2.AddState(Time.DawnOf, Date(1999,1,1));
            td2.AddState(Date(2000,1,1), Date(2000,1,1));
            td2.AddState(Date(2001,1,1), Date(2008,1,1));
            Tvar result = td2 <= td1;
			Assert.AreEqual("{Dawn: True, 2001-01-01: False}", result.Out);        
        }
        
        // .AddDays
        
        [Test]
        public void Tvar_AddDays_1 ()
        {
            Tvar td = new Tvar(2000,1,1);
            Tvar result = td.AddDays(3);
            Assert.AreEqual(Date(2000,1,4), result.Out);        
        }
        
        [Test]
        public void Tvar_AddDays_2 ()
        {
            Tvar td = new Tvar(2000,1,1);
            Tvar result = td.AddDays(-3);
            Assert.AreEqual(Date(1999,12,29), result.Out);        
        }
        
        // .AddMonths
        
        [Test]
        public void Tvar_AddMonths_1 ()
        {
            Tvar td = new Tvar(2000,1,1);
            Tvar result = td.AddMonths(3);
            Assert.AreEqual(Date(2000,4,1), result.Out);        
        }
        
        [Test]
        public void Tvar_AddMonths_2 ()
        {
            Tvar td = new Tvar(2000,1,1);
            Tvar result = td.AddMonths(-3);
            Assert.AreEqual(Date(1999,10,1), result.Out);        
        }
        
        // .AddYears
        
        [Test]
        public void Tvar_AddYears_1 ()
        {
            Tvar td = new Tvar(2000,1,1);
            Tvar result = td.AddYears(3);
            Assert.AreEqual(Date(2003,1,1), result.Out);        
        }
        
        [Test]
        public void Tvar_AddYears_2 ()
        {
            Tvar td = new Tvar(2000,1,1);
            Tvar result = td.AddYears(-3);
            Assert.AreEqual(Date(1997,1,1), result.Out);        
        }

        // DayDiff
        // When the earlier date is put after the prior date, DayDiff returns a negative number.
        
        [Test]
        public void Tvar_DayDiff_2 ()
        {
            Tvar td1 = new Tvar(2010,1,1);
            Tvar td2 = new Tvar(2000,1,1);
            Tvar result = Tvar.DayDiff(td1,td2);
            Assert.AreEqual(-3653, result.Out);        
        }
    }
}   