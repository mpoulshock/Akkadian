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
			Typ typ = exp.nodes[0].objType;
			object ob = exp.nodes[0].obj;

//			Console.WriteLine("eval in: " + exp.ToString() + "  " + args.ToString());

			if (typ == Typ.Var) 		{ return EvaluateVariableReferences(exp, args); }
			else if (typ == Typ.Expr) 	{ return eval((Expr)ob, args); }

			else if (typ == Typ.Op)
			{
				Op opType = (Op)ob;
				int opID = Convert.ToInt16(opType);

				if (opID < 100) 			{ return BinaryFcnEval(exp, args, opType); }
				else if (opID < 200) 		{ return UnaryFcnEval(exp, args, opType); }
				else if (opID < 250) 		{ return ThreeArgFcnEval(exp, args, opType); }
				else if (opID < 300) 		{ return FourArgFcnEval(exp, args, opType); }
				else if (opID > 400) 		{ return ConstantEval(opType); }
				else if (opType == Op.Pipe) { return EvaluatePipelinedExpression(exp, args); }

				else if (opType == Op.Switch)
				{
					// Get arguments to the switch function
					List<Node> newArgList = new List<Node>();
					for (int i=0; i < exp.nodes.Count; i++)
					{
						if (i>0) newArgList.Add(exp.nodes[i]);
					}
					Expr newArgs = new Expr(newArgList);

					return n(Typ.Tvar, Switch2(newArgs, args));
				}
				else if (opType == Op.Max || opType == Op.Min || opType == Op.BoolCount)
				{
					return MultiTvarFcnEval(exp, args, opType);
				}
			}
			else if (typ == Typ.Fcn)
			{
				string fcnName = Convert.ToString(ob);
				if (ContainsFunction(fcnName))
				{
					// Call the function with the given name
					Expr ex1 = GetFunction(fcnName);
					return MixAndMatch(exp, args, ex1);
				}
				else
				{
					// Make a list of the arguments
					string argString = "";
					for (int i=0; i < exp.nodes.Count; i++)
					{
						if (i>0) 
						{
							Tvar argTv = (Tvar)eval(exp.nodes[i], args).obj;
							argString += argTv.ToString() + ",";
						}
					}

					// Pose the leaf node as a question
//					Console.Write("  -" + fcnName + "[" + argString.TrimEnd(',') + "]? ");
//					string s = Console.ReadLine();
//					result = nTvar(s);

					return nTvar(new Tvar(Hstate.Unstated));
				}
			}

			return exp.nodes[0];
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

		/// <summary>
		/// Evaluates an expression with a pipeline operator (|>)
		/// </summary>
		private Node EvaluatePipelinedExpression(Expr exp, Expr args)
		{
			// Get rid of pipe node, put new first node as last node, then eval 
			// For example, -9.1 |> Abs becomes Abs[-9.1]
			// Expr:{Op:Pipe,Tvar:-9.1,Op:Abs} becomes Expr:{Op:Abs,Tvar:-9.1}

			List<Node> dePipedNodes = new List<Node>();

			// E.g. Expr:{Op:Pipe,Tvar:-9.1,Op:Abs}
			if (exp.nodes[2].objType == Typ.Op || exp.nodes[2].objType == Typ.Fcn)
			{
				dePipedNodes.Add(exp.nodes[2]);
			}
			// E.g. Expr:{Op:Pipe,Tvar:33,Expr:{Fcn:F,Tvar:2}}
			else
			{
				Expr deepExpr = (Expr)exp.nodes[2].obj;
				List<Node> exprList = (List<Node>)deepExpr.nodes;
				for (int i=0; i < exprList.Count; i++)
				{
					dePipedNodes.Add(exprList[i]);
				}
			}
			dePipedNodes.Add(exp.nodes[1]);

			return eval(new Expr(dePipedNodes), args);
		}
	}
}