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
			string filePath = "";

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
						result = "All rules deleted.";
					}

					// Compile all .akk files in a specified folder
					else if (userInput.StartsWith("compile "))
					{
						filePath = userInput.Replace("compile ","");
						if (!filePath.EndsWith(@"\")) filePath = filePath + @"\"; 

						CompileAkkFiles(sess, filePath);
						result = "Compilation complete.";
					}

					// Recompile the loaded project
					else if (userInput.ToLower() == "r")
					{
						CompileAkkFiles(sess, filePath);
						result = "Recompilation complete.";
					}

					// Retract all facts
					else if (userInput.ToLower() == "retract all")
					{
						sess.ClearFacts();
						result = "All facts retracted.";
					}

					// Display session stats
					else if (userInput.ToLower() == "stats")
					{
						result = "Session contains " + Convert.ToString(sess.CountFunctions()) + " functions.";
					}

					else
					{
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

		/// <summary>
		/// Compiles all .akk files in a given directory.
		/// </summary>
		public static void CompileAkkFiles(Session sess, string path)
		{
			sess.ClearFunctions();
			sess.LoadStandardLibrary();

			string[] akkFiles = Directory.GetFiles(path, "*.akk", SearchOption.AllDirectories);
			Parallel.ForEach(akkFiles, f => 
			{
				Interpreter.ImportRuleFile(sess, f);
			} );
		}
	}
}