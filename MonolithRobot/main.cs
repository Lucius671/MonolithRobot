using System;
using System.Net;

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
            ExternalDeviceClass.openport();
            new TcpClient ("185.48.112.23", 25555);
		}
	}
}