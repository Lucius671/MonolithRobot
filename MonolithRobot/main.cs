using System;
using System.Net;
using System.Threading;
using System.IO.Ports;
using System.IO;

namespace MonolithRobot
{
	class MainClass
	{

        public static string ip = "192.168.1.50";

		public static void Main (string[] args)
		{
			IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
			Console.WriteLine ("My ip(s) is(are): ");
            for (int i = 0; i < ips.Length; i++)
            {
                Console.WriteLine(i + ":" + ips[i].ToString());
            }
            try {
                ReadSettings();
                new TcpClient (ip, 25555);
            }catch{

            }
		}

        public static void ReadSettings() {
            string[] strs = File.ReadAllLines("settings.cfg");
            for (int i=0; i<strs.Length; i++)
            {
                if (strs[i].Split('=')[0] == "ip")
                {
                    ip = strs[i].Split('=')[1];
                    Console.WriteLine("Now ip is:" + ip);
                }
            }
        }
	}
}