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
using System.Linq;

namespace Akkadian
{
	public partial class Interpreter
	{
		/// <summary>
		/// The soul of a new machine.
		/// </summary>
		public static Node eval(Expr exp, Expr args)
		{
			Node result = exp.nodes[0];
			string typ = exp.nodes[0].objType;
			object ob = exp.nodes[0].obj;

			//			Console.WriteLine("eval in: " + exp.ToString() + "  " + args.ToString());

			if (typ == "var")
			{
				result = EvaluateVariableReferences(exp, args);
			}
			else if (typ == "op")
			{
				string opType = Convert.ToString(ob);
				string[] binaryOps = {"+","-","*","/","==","T+","T-","T*","T/","T&","T|","T=","T<>","T>","T>=","T<","T<=","Pow","Log2","RndUp","RndDn","RndNrUp","RndNrDn"}; 
				string[] unaryOps = {"T!","Abs","Sqrt","Log","Sin","Cos","Tan","Asin","Acos","Atan"}; 

				if (binaryOps.Contains(opType))
				{
					result = BinaryFcnEval(exp, args, opType);
				}
				else if (unaryOps.Contains(opType))
				{
					result = UnaryFcnEval(exp, args, opType);
				}
				else if (opType == "if") 
				{
					if (Convert.ToBoolean(eval(expr(exp.nodes[1]), args).obj))
					{
						result = eval(expr(exp.nodes[2]), args);
					}
					else
					{
						result = eval(expr(exp.nodes[3]), args);
					}
				}
				else if (opType == "Tmax" || opType == "Tmin")
				{
					result = MultiTnumFcnEval(exp, args, opType);
				}
				else if (opType == "Switch")
				{
					exp.nodes.RemoveAt(0);
					result = n("Tnum",Switch2<Tnum>(exp));
				}
//				else if (opType == "Exists")
//				{
//					result = EvalExists(exp, args, opType);
//				}
			}
			else if (typ == "expr")
			{
				result = eval((Expr)ob, args);
			}
			else if (typ == "fcn")
			{
				// Calls the rule (function) at the given index number
				result = eval((Expr)LoadedRules()[Convert.ToInt32(ob)], args);
			}
			else if (typ == "rec")
			{
				// Get the function reference from exp
				Expr newExp = expr(n("fcn", exp.nodes[0].obj));
				result = MixAndMatch(exp, args, newExp);
			}
			else if (typ == "ask")
			{
				// Get the info from the user / factbase
				string s = Console.ReadLine();
				//				result = n("Tnum",new Tnum(Convert.ToDecimal(s)));
				result = n("Tbool",new Tbool(Convert.ToBoolean(s)));
			}

			return result;
		}

//		private static Node EvalExists(Expr exp, Expr args, string op)
//		{
//			Node argFcnNode = n(null,null);
//			Tset theSet  = (Tset)eval(expr(exp.nodes [1]), args).obj;
//			Tset result = ApplyFcnToTset<Tset>(theSet, argFcnNode, y => CoreFilter(y));
//
//			//			Tset theSet  = (Tset)eval(expr(exp.nodes [1]), args).obj;
//			//			Func<Thing,Tbool> theFunc = (Func<Thing,Tbool>)eval(expr(exp.nodes [2]), args).obj;
//
//			return n("Tset", result);  
//		}

		public static Node eval(Node node, Expr args)
		{
			return eval(expr(node), args);
		}

		/// <summary>
		/// Evaluates expressions with two arguments.
		/// </summary>
		private static Node BinaryFcnEval(Expr exp, Expr args, string op)
		{
			Node n1 = eval(expr(exp.nodes [1]), args);
			Node n2 = eval(expr(exp.nodes [2]), args);

			if (op == "T&") { return n("Tbool", (Tbool)n1.obj && (Tbool)n2.obj); }
			if (op == "T|") { return n("Tbool", (Tbool)n1.obj || (Tbool)n2.obj); }
			if (op == "T!") { return n("Tbool", !(Tbool)n1.obj); }

			if (op == "T+") { return n("Tnum", (Tnum)n1.obj + (Tnum)n2.obj); }
			if (op == "T-") { return n("Tnum", (Tnum)n1.obj - (Tnum)n2.obj); }
			if (op == "T*") { return n("Tnum", (Tnum)n1.obj * (Tnum)n2.obj); }
			if (op == "T/") { return n("Tnum", (Tnum)n1.obj / (Tnum)n2.obj); }

			if (op == "T=") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj == (Tnum)n2.obj); 
				if (n1.objType == "Tstr")  return n("Tbool", (Tstr)n1.obj == (Tstr)n2.obj);
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj == (Tdate)n2.obj);
				if (n1.objType == "Tset")  return n("Tbool", (Tset)n1.obj == (Tset)n2.obj);
				if (n1.objType == "Tbool") return n("Tbool", (Tbool)n1.obj == (Tbool)n2.obj);
			}

			if (op == "T<>") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj != (Tnum)n2.obj); 
				if (n1.objType == "Tstr")  return n("Tbool", (Tstr)n1.obj != (Tstr)n2.obj);
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj != (Tdate)n2.obj);
				if (n1.objType == "Tset")  return n("Tbool", (Tset)n1.obj != (Tset)n2.obj);
				if (n1.objType == "Tbool") return n("Tbool", (Tbool)n1.obj != (Tbool)n2.obj);
			}

			if (op == "T>") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj > (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj > (Tdate)n2.obj);
			}
			if (op == "T>=") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj >= (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj >= (Tdate)n2.obj);
			}
			if (op == "T<") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj < (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj < (Tdate)n2.obj);
			}
			if (op == "T<=") 
			{ 
				if (n1.objType == "Tnum")  return n("Tbool", (Tnum)n1.obj <= (Tnum)n2.obj); 
				if (n1.objType == "Tdate") return n("Tbool", (Tdate)n1.obj <= (Tdate)n2.obj);
			}

			if (op == "RndUp") { return n("Tnum", ((Tnum)n1.obj).RoundUp((Tnum)n2.obj)); }
			if (op == "RndDn") { return n("Tnum", ((Tnum)n1.obj).RoundDown((Tnum)n2.obj)); }
			if (op == "RndNrUp") { return n("Tnum", ((Tnum)n1.obj).RoundToNearest((Tnum)n2.obj)); }
			if (op == "RndNrDn") { return n("Tnum", ((Tnum)n1.obj).RoundToNearest((Tnum)n2.obj, true)); }
			if (op == "Pow") { return n("Tnum", Tnum.Pow((Tnum)n1.obj, (Tnum)n2.obj)); }
			if (op == "Log2") { return n("Tnum", Tnum.Log((Tnum)n1.obj, (Tnum)n2.obj)); }

			if (op == "+")  { return n("dec", Convert.ToDecimal(n1.obj) + Convert.ToDecimal(n2.obj)); }
			if (op == "*")  { return n("dec", Convert.ToDecimal(n1.obj) * Convert.ToDecimal(n2.obj)); }
			if (op == "/")  { return n("dec", Convert.ToDecimal(n1.obj) / Convert.ToDecimal(n2.obj)); }
			if (op == "-")  { return n("dec", Convert.ToDecimal(n1.obj) - Convert.ToDecimal(n2.obj)); }
			if (op == "==") { return n("bool", Convert.ToBoolean(Convert.ToDecimal(n1.obj) == Convert.ToDecimal(n2.obj))); }

			return n(null,null);
		}

		/// <summary>
		/// Evaluates expressions with one argument.
		/// </summary>
		private static Node UnaryFcnEval(Expr exp, Expr args, string op)
		{
			Node n1 = eval(expr(exp.nodes [1]), args);

			if (op == "T!")    { return n("Tbool", !(Tbool)n1.obj); }
			if (op == "Abs")   { return n("Tnum", Tnum.Abs((Tnum)n1.obj)); }
			if (op == "Sqrt")  { return n("Tnum", Tnum.Sqrt((Tnum)n1.obj)); }
			if (op == "Log")   { return n("Tnum", Tnum.Log((Tnum)n1.obj)); }
			if (op == "Sin")   { return n("Tnum", Tnum.Sin((Tnum)n1.obj)); }
			if (op == "Cos")   { return n("Tnum", Tnum.Cos((Tnum)n1.obj)); }
			if (op == "Tan")   { return n("Tnum", Tnum.Tan((Tnum)n1.obj)); }
			if (op == "Asin")  { return n("Tnum", Tnum.ArcSin((Tnum)n1.obj)); }
			if (op == "Acos")  { return n("Tnum", Tnum.ArcCos((Tnum)n1.obj)); }
			if (op == "Atan")  { return n("Tnum", Tnum.ArcTan((Tnum)n1.obj)); }

			return n(null,null);
		}

		/// <summary>
		/// Evaluates expressions with three or more arguments.
		/// </summary>
		private static Node MultiTnumFcnEval(Expr exp, Expr args, string op)
		{
			Tnum[] list = new Tnum[exp.nodes.Count-1];
			for (int i=1; i<exp.nodes.Count; i++)
			{
				list[i-1] = (Tnum)eval(expr(exp.nodes[i]), args).obj;
			}

			if (op == "Tmax") { return n("Tbool", Tnum.Max(list)); }
			if (op == "Tmin") { return n("Tbool", Tnum.Min(list)); }

			return n(null,null);
		}

		/// <summary>
		/// Evaluates an expression referencing a higher order function.
		/// </summary>
		private static Node EvaluateVariableReferences(Expr exp, Expr args)
		{
			object ob = exp.nodes[0].obj;

			// Shortcut for ordinary variable references - faster
			if (exp.nodes.Count == 1)
			{
				// Gets the variable value from vals, at the specificed index
				return eval(args.nodes[Convert.ToInt16(ob)], args);
			}

			// New expr is the function reference in args
			Expr newExp = expr(args.nodes[Convert.ToInt16(ob)]);

			return MixAndMatch(exp, args, newExp);
		}

		/// <summary>
		/// Cross-breeds part of the expression and part of the arguments to form a new
		/// expression-argument pair to be evaluated.
		/// </summary>
		private static Node MixAndMatch(Expr exp, Expr args, Expr newExp)
		{
			// Expr - minus the function reference - becomes the args to be evaluated
			exp.nodes.RemoveAt(0);
			Expr newArgs = exp;

			// Evaluate any variable references or nested expressions
			for (int i=0; i < newArgs.nodes.Count; i++)
			{
				newArgs.nodes[i] = eval(newArgs.nodes[i], args);
			}

			return eval(newExp, newArgs);
		}

		/// <summary>
		/// Simple way to instantiate a new Node.
		/// </summary>
		public static Node n(string typ, object o)
		{
			return new Node(typ,o);
		}

		/// <summary>
		/// Simple way to instantiate a new expression.
		/// </summary>
		public static Expr expr(params Node[] nodes)
		{
			return new Expr(new List<Node>(nodes));
		}

	}
}

