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

namespace Akkadian
{
	public class Session
	{
		/// <summary>
		/// Instantiates a new Session object.
		/// </summary>
		public Session()
		{
			Interpreter.InitializeOperatorRegistry();
			ClearFunctions();
		}

		/// <summary>
		/// Data structure in which user-defined functions are stored.
		/// </summary>
		private Dictionary<string,Expr> FunctionTable = new Dictionary<string,Expr>();

		/// <summary>
		/// Adds a function to the function table.
		/// </summary>
		public void AddFunction(string name, Expr fcnExpr)
		{
			FunctionTable.Add(name, fcnExpr);
		}

		/// <summary>
		/// Gets the expression (Expr) of a function from the function table.
		/// </summary>
		public Expr GetFunction(string name)
		{
			return FunctionTable[name];
		}

		/// <summary>
		/// Deletes all functions from the session.
		/// </summary>
		public void ClearFunctions()
		{
			FunctionTable.Clear();
		}

		/// <summary>
		/// Returns a count of the number of functions in the session.
		/// </summary>
		public int CountFunctions()
		{
			return FunctionTable.Count;
		}

		/// <summary>
		/// Displays all functions in the session.
		/// </summary>
		public string ShowFunctions()
		{
			string result = "";
			foreach (KeyValuePair<string, Expr> kvp in FunctionTable)
			{
				result += "    " + kvp.Key + ": " + kvp.Value.ToString() + "\r\n";
			}
			return result;
		}

		/// <summary>
		/// Processes a user-input string in light of the session data.
		/// </summary>
		public object ProcessInput(string s)
		{
			Interpreter.ParserResponse pr = Interpreter.ParseInputLine(s);

			if (pr.IsNewFunction)
			{
				AddFunction(pr.FunctionName, pr.FunctionExpression);
				return true;
			}
			else
			{
				Expr exp = Interpreter.StringToExpr(pr.ParserString);
				return Interpreter.eval(exp).obj;
			}
		}
	}
}