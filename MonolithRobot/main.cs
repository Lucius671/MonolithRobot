using System;
using System.Net;
using System.Threading;
using System.IO.Ports;

namespace MonolithRobot
{
	class MainClass
	{
        
		public static void Main (string[] args)
		{
			IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
			Console.WriteLine ("My ip(s) is(are): ");
            for (int i = 0; i < ips.Length; i++)
            {
                Console.WriteLine(i + ":" + ips[i].ToString());
            }
            new TcpClient ("192.168.1.50", 25555);
		}
	}
}