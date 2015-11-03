using System;
using System.Threading;
using System.IO.Ports;

namespace MonolithRobot
{
	class ExternalDeviceClass
	{
		public static SerialPort ExternalDevice = new SerialPort ("/dev/ttyACM0", 9600, Parity.None, 8, StopBits.One);

		public static void openport()
		{
			Thread.Sleep (2000);
			ExternalDevice.Open ();
		}

		public static void closeport()
		{
			ExternalDevice.Close ();
		}

		public static void send(string message)
		{
			if (ExternalDevice.IsOpen) 
			{
				ExternalDevice.WriteLine (message);
			}
			else
			{
				Console.WriteLine ("Не могу отправить данные на управляющий микрокомпьютер, порт закрыт.");
			}
		}

		public static string read()
		{
			if (ExternalDevice.IsOpen) 
			{
				string incomingdata;
				incomingdata = ExternalDevice.ReadLine();
				return incomingdata;
			} 
			else 
			{
				Console.WriteLine ("Не могу получить данные с управляющего микрокомпьютера, порт закрыт.");
				return "fail";
			}
		}
	}
}
