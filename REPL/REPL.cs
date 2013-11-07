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
					string result = "";

					// Clear all functions from the session
					if (userInput.ToLower() == "clear rules")
					{
						sess.ClearFunctions();
						Console.WriteLine("  All rules deleted.");
						Console.WriteLine();
						continue;
					}

					// Import rules from a text file
					if (userInput.ToLower().StartsWith("import "))
					{
						string loc = userInput.Replace("import ","");
						result = Interpreter.ImportRuleFile(sess, "C:\\Users\\mpoulshock\\Documents\\MP\\" + loc);  // Test.txt
						Console.WriteLine("  " + result);
						Console.WriteLine();
						continue;
					}

					// Eval
					Interpreter.ParserResponse pr = Interpreter.ParseInputLine(userInput);
					

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

						if (o.GetType() == typeof(Tvar)) 		{ result = Convert.ToString(((Tvar)o).Out); }
						else if (o.GetType() == typeof(Tvar)) 	{ result = Convert.ToString(((Tvar)o).Out); }
						else if (o.GetType() == typeof(Tvar)) 	{ result = Convert.ToString(((Tvar)o).Out); }
						else if (o.GetType() == typeof(Tvar)) 	{ result = Convert.ToString(((Tvar)o).Out); }
						else if (o.GetType() == typeof(Tvar)) 	{ result = Convert.ToString(((Tvar)o).Out); }
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