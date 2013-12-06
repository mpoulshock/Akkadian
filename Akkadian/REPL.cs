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

//			sess.ProcessInput("FedMinWage = {1800-01-01: $0, 2008-07-24: $6.55, 2009-07-24: $7.25}");
//			Tvar r = (Tvar)sess.ProcessInput("(FedMinWage > 7) |> EverPer[TheYear]");

//			sess.ProcessInput("MeetsTest = {Dawn: False, 2014-03-15: True, 2014-05-12: False, 2014-07-03: True}");
//			Tvar r = (Tvar)sess.ProcessInput("MeetsTest |> EverPer[TheWeek] |> Regularize[TheWeek] |> CountPer[TheYear]");

			Tvar r = (Tvar)sess.ProcessInput("{2,3,5,7,9} |> SetSum2");
			Console.WriteLine(r.ToString());

//			Tvar t1 = new Tvar(3);
//			Tvar t2 = new Tvar(1);
//			t2.AddState(DateTime.Now, 5);
//
//			Tvar[] array = {t1,t2};
//
//			Tvar aTset = Tvar.MakeTset(array);
//
//			Console.WriteLine(aTset.ToString());

			// Loop
			while (true)
			{
//				try
//				{
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