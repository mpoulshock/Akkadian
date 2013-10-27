using System;
using Akkadian;

namespace REPL
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Expr exp = Interpreter.StringToExpr("Expr:{Op:Abs,Expr:{Op:Minus,Expr:{Op:Mult,Tnum:11,Tnum:6},Tnum:100}}");
			Tnum r = (Tnum)Interpreter.eval(exp).obj;
			int i = Convert.ToInt16(r.Out);

			Console.WriteLine (i);
		}
	}
}
