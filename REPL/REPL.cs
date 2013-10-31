using System;
using System.Collections.Generic;
using Akkadian;


namespace REPL
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Console.Title = "Akkadian REPL";

			Session sess = new Session();

			// Loop
			while (true)
			{
//				try
//				{
					// Read
					Console.Write("> ");
					string userInput = Console.ReadLine();

					// Clear all functions from the session
					if (userInput.ToLower() == "clear rules")
					{
						sess.ClearFunctions();
						Console.WriteLine("  All rules deleted.");
						Console.WriteLine();
						continue;
					}

					// Eval
					Interpreter.ParserResponse pr = Interpreter.ParseInputLine(userInput);
					string result = "";

					if (pr.IsNewFunction)
					{
						string name = pr.FunctionName;
						
						Expr e = (Expr)Interpreter.StringToNode(pr.ParserString).obj;

						if (sess.ContainsFunction(name))
						{
							sess.UpdateFunction(name, e);
							result = "Rule updated.";
						}
						else
						{
							sess.AddFunction(name, e);
							result = "Rule added.";
						}
					}
					else
					{
						Expr exp = Interpreter.StringToExpr(pr.ParserString);
						object o = sess.eval(exp).obj;

						if (o.GetType() == typeof(Tnum)) 		{ result = Convert.ToString(((Tnum)o).Out); }
						else if (o.GetType() == typeof(Tbool)) 	{ result = Convert.ToString(((Tbool)o).Out); }
						else if (o.GetType() == typeof(Tstr)) 	{ result = Convert.ToString(((Tstr)o).Out); }
						else if (o.GetType() == typeof(Tdate)) 	{ result = Convert.ToString(((Tdate)o).Out); }
						else if (o.GetType() == typeof(Tset)) 	{ result = Convert.ToString(((Tset)o).Out); }
					}

					// Print
					Console.WriteLine("  " + result);
					Console.WriteLine();
//				}
//				catch
//				{
//					Console.WriteLine("  Syntax error.");
//					Console.WriteLine();
//				}
			}
		}
	}
}