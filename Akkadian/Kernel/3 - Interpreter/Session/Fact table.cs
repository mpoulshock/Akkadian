// Copyright (c) 2013 Hammura.bi LLC
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
using System.Collections.Concurrent;

namespace Akkadian
{
	public partial class Session 
	{
		/// <summary>
		/// Data structure in which user-asserted facts are stored.
		/// </summary>
		private static ConcurrentDictionary<string,Tvar> FactTable = new ConcurrentDictionary<string,Tvar>();

		/// <summary>
		/// Adds a fact to the table.
		/// </summary>
		public void AddFact(string name, Tvar value)
		{
			FactTable.TryAdd(name, value);
		}

		/// <summary>
		/// Retrieves a fact from the fact table.
		/// </summary>
		public Tvar GetFact(string name)
		{
			return FactTable[name];
		}

		/// <summary>
		/// Indicates whether a fact has alread been asserted or inferred.
		/// </summary>
		public bool ContainsFact(string name)
		{
			return FactTable.ContainsKey(name);
		}

		/// <summary>
		/// Deletes all facts from the session.
		/// </summary>
		public void ClearFacts()
		{
			FactTable.Clear();
		}

		/// <summary>
		/// Displays all facts in the session.
		/// </summary>
		public string ShowFacts()
		{
			string result = "";
			foreach (KeyValuePair<string, Tvar> kvp in FactTable)
			{
				result += "    " + kvp.Key + ": " + kvp.Value.ToString() + "\r\n";
			}
			return result;
		}

		/// <summary>
		/// Assert a fact to the FactBase.
//		/// </summary>
//		private static void CoreAssert(string rel, object e1, object e2, object e3, Tvar val)
//		{
//			// Don't assert a fact that's already been asserted
//			if (!HasBeenAsserted(rel, e1, e2, e3))
//			{
//				// Assert the fact
//				Fact f = new Fact(rel, e1, e2, e3, val);
//				FactBase.Add(f);
//
//				// TODO: This breaks when the objects are not Things (hence the try-catch)
//				try
//				{
//					// Add Things to the ThingBase
//					AddThing((Thing)e1);
//					AddThing((Thing)e2);
//					AddThing((Thing)e3);
//
//					// Look for additional inferences that can be drawn, based on assumptions.
//					//                    Assumptions.TriggerInferences(rel, (Thing)e1, (Thing)e2, (Thing)e3, val);
//				}
//				catch
//				{
//				}
//			}
//		}

		/// <summary>
		/// Returns a symmetrical input boolean fact.  
		/// For example, A is married to B if B is married to A.
		/// </summary>
		/// <remarks>
		/// See unit tests for the truth table, which ensures that the existence
		/// of one false causes a false to be returned.
		/// Note that the Sym() functions are also designed to add facts to the
		/// Facts.Unknowns list in the proper order and with proper short-
		/// circuiting.
		/// </remarks>
//		public static Tvar Sym(object subj, string rel, object directObj)
//		{
//			if (Facts.HasBeenAsserted(rel, subj, directObj))
//			{
//				return QueryTvar<Tvar>(rel, subj, directObj);
//			}
//
//			return QueryTvar<Tvar>(rel, directObj, subj);
//		} 

		
		/// <summary>
		/// Returns true if a symmetrical fact has been assserted.
		/// </summary>
		//        public static bool HasBeenAssertedSym(object e1, string rel, object e2)
		//        {
		//            return HasBeenAsserted(rel, e1, e2) || HasBeenAsserted(rel, e2, e1);
		//        }

		/// <summary>
		/// Returns the fact's question text.
		/// </summary>
//		public string QuestionText()
//		{
//			// Embed the names of the Things into the question
//			string result = ""; //Interactive.Templates.GetQ(Relationship).questionText;
//
//			return result.Replace("{1}", Util.ArgToString(Arg1))
//				.Replace("{2}", Util.ArgToString(Arg2))
//					.Replace("{3}", Util.ArgToString(Arg3));
//		}
	}
}
