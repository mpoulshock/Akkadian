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

				if (shortCircuits.Contains(opType))
				{
					result = EvalShortCircuitFcns(exp, args, opType);
				}
				else if (binaryOps.Contains(opType))
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
					result = n("Tnum",Switch2<Tnum>(exp, args));
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


		public static Node nTbool(Tbool t)
		{
			return new Node("Tbool",t);
		}

		public static Node nTnum(Tnum t)
		{
			return new Node("Tnum",t);
		}

		public static Node nTstr(Tstr t)
		{
			return new Node("Tstr",t);
		}

		public static Node nTset(Tset t)
		{
			return new Node("Tset",t);
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

