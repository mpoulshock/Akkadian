using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
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

			// Loop
			while (true)
			{
				try
				{
					// Read
					Console.Write("> ");
					string userInput = Console.ReadLine().TrimEnd(';');
					string result = "";

					// Clear all functions from the session
					if (userInput.ToLower() == "clear rules")
					{
						sess.ClearFunctions();
						Console.WriteLine("  All rules deleted.");
						Console.WriteLine();
						continue;
					}

					// Compile all .akk files in a specified folder
					if (userInput.ToLower().StartsWith("compile "))
					{
						sess.ClearFunctions();
						sess.LoadStandardLibrary();

						string loc = userInput.Replace("compile ","");
						if (!loc.EndsWith(@"\")) loc = loc + @"\"; 

						string[] akkFiles = Directory.GetFiles(loc, "*.akk", SearchOption.AllDirectories);
						Parallel.ForEach(akkFiles, f => 
						{
							Interpreter.ImportRuleFile(sess, f);
						} );

						Console.WriteLine("  Compilation complete.");
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
				}
				catch
				{
					Console.WriteLine("  Syntax error.");
					Console.WriteLine();
				}
			}
		}
	}
}