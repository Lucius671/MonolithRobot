using System;

namespace MonolithRobot
{
	public static class ConsoleAdditives
	{
		public static void WriteHeader(string format, params object[] arg)
		{
			Console.WriteLine ("----[" + format + "]----", arg);
		}
			
		public static void WriteHeader(string str)
		{
			Console.WriteLine ("----[" + str + "]----");
		}

		public static void WriteInfo(string format, params object[] arg)
		{
			Console.WriteLine ("[" + DateTime.Now + "]:"+format,arg);
		}

		public static void WriteInfo(string str)
		{
			Console.WriteLine ("[" + DateTime.Now + "]:"+str);
		}
	}
}

