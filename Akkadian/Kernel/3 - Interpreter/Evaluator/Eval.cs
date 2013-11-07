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
	public partial class Session : Interpreter
	{
		/// <summary>
		/// The soul of a new machine.
		/// </summary>
		public Node eval(Expr exp, Expr args)
		{
			Node result = exp.nodes[0];
			Typ typ = exp.nodes[0].objType;
			object ob = exp.nodes[0].obj;

//			Console.WriteLine("eval in: " + exp.ToString() + "  " + args.ToString());

			if (typ == Typ.Var)
			{
				result = EvaluateVariableReferences(exp, args);
			}
			else if (typ == Typ.Op)
			{
				Op opType = (Op)ob;
				int opID = Convert.ToInt16(opType);

				if (opID < 10)
				{
					result = EvalShortCircuitFcns(exp, args, opType);
				}
				else if (opID < 100)
				{
					result = BinaryFcnEval(exp, args, opType);
				}
				else if (opID < 200)
				{
					result = UnaryFcnEval(exp, args, opType);
				}
				else if (opType == Op.Switch)
				{
					// Get arguments to the switch function
					List<Node> newArgList = new List<Node>();
					for (int i=0; i < exp.nodes.Count; i++)
					{
						if (i>0) newArgList.Add(exp.nodes[i]);
					}
					Expr newArgs = new Expr(newArgList);

					result = n(Typ.Tvar, Switch2(newArgs, args));
				}
				else if (opType == Op.Max || opType == Op.Min)
				{
					result = MultiTvarFcnEval(exp, args, opType);
				}
//				else if (opType == "Exists")
//				{
//					result = EvalExists(exp, args, opType);
//				}
			}
			else if (typ == Typ.Expr)
			{
				result = eval((Expr)ob, args);
			}
			else if (typ == Typ.Fcn)
			{
				// Call the function with the given name
				Expr ex1 = GetFunction(Convert.ToString(ob));
				result = MixAndMatch(exp, args, ex1);
			}
			else if (typ == Typ.Ask)
			{
				// Get the info from the user / factbase
				string s = Console.ReadLine();
				// result = nTvar(Convert.ToDecimal(s)));
				result = nTvar(Convert.ToBoolean(s));
			}

			return result;
		}

		public Node eval(Node node, Expr args)
		{
			return eval(expr(node), args);
		}

		public Node eval(Expr exp)
		{
			return eval(exp, expr(n(Typ.Null,null)));
		}

		/// <summary>
		/// Evaluates an expression referencing a higher order function.
		/// </summary>
		private Node EvaluateVariableReferences(Expr exp, Expr args)
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
		private Node MixAndMatch(Expr exp, Expr args, Expr newExp)
		{
			// Expr - minus the function reference - becomes the args to be evaluated
			List<Node> newArgList = new List<Node>();
			for (int i=0; i < exp.nodes.Count; i++)
			{
				if (i>0) newArgList.Add(eval(exp.nodes[i], args));
			}
			Expr newArgs = new Expr(newArgList);

			return eval(newExp, newArgs);
		}
	}
}