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
			sess.AskQuestions = true;

//			sess.ProcessInput("FedMinWage = {1800-01-01: $0, 2008-07-24: $6.55, 2009-07-24: $7.25}");
//			Tvar r = (Tvar)sess.ProcessInput("(FedMinWage > 7) |> EverPer[TheYear]");

//			sess.ProcessInput("MeetsTest = {Dawn: False, 2014-03-15: True, 2014-05-12: False, 2014-07-03: True}");
//			Tvar r = (Tvar)sess.ProcessInput("MeetsTest |> EverPer[TheWeek] |> Regularize[TheWeek] |> CountPer[TheYear]");
//			Console.WriteLine(r.ToString());

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
						
						Expr e = new Expr(new List<Node>(){pr.ThatWhichHasBeenParsed});

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
						Expr exp = new Expr(new List<Node>(){pr.ThatWhichHasBeenParsed});
						object o = sess.eval(exp).obj;
						if (o.GetType() == typeof(Tvar)) 	{ result = ((Tvar)o).ToString(); }
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