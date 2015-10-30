using System;

namespace MonolithRobot
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			new TcpClient ("192.168.1.50", 25555);
		}
	}
}
