using System;
using Akkadian;

namespace REPL
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			while (true)
			{
				Console.Write("> ");
				string userInput = Console.ReadLine();

				string result = "";
				object o = Interpreter.ParseEvalUserString(userInput);

				if (o.GetType() == typeof(Tnum)) 
				{
					result = Convert.ToString(((Tnum)o).Out); 
				}
				else if (o.GetType() == typeof(Tbool)) 
				{
					result = Convert.ToString(((Tbool)o).Out); 
				}
				else if (o.GetType() == typeof(Tstr)) 
				{
					result = Convert.ToString(((Tstr)o).Out); 
				}
				else if (o.GetType() == typeof(Tdate)) 
				{
					result = Convert.ToString(((Tdate)o).Out); 
				}
				else if (o.GetType() == typeof(Tset)) 
				{
					result = Convert.ToString(((Tset)o).Out); 
				}

				Console.WriteLine("  " + result);
				Console.WriteLine();
			}
		}
	}
}
