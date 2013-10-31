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

			Interpreter.InitializeOperatorRegistry();
			FcnTable.ClearFunctionTable();
			int fcnCount = FcnTable.Count();

			// Loop
			while (true)
			{
//				try
//				{
					// Read
					Console.Write("> ");
					string userInput = Console.ReadLine();

					// Eval
					string fcn = Interpreter.ParseFcn(userInput); 
					string result = "";

					if (FcnTable.Count() != fcnCount)
					{
						result = "Rule added.";
						fcnCount = FcnTable.Count();
					}
					else 

					{
						Expr exp = Interpreter.StringToExpr(fcn);
						object o = Interpreter.eval(exp).obj;

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