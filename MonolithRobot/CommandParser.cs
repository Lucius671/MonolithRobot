using System;

namespace MonolithRobot
{
	public static class CommandParser
	{
		public static string SwitchAnswer(string cmd)
		{
			string[] cmdwa = cmd.Split (' ');
			switch (cmdwa [0].ToLower ()) {
			case "ping":
				return "pong";
			case "getinfo":
				if (cmdwa.Length > 1)
					return GetInfo (cmdwa);
				else
					break;
                case "arduino_serv1_left":
                    ExternalDeviceClass.send("arduino_serv1_left");
                    break;
                case "arduino_serv1_right":
                    ExternalDeviceClass.send("arduino_serv1_right");
                    break;
                case "arduino_serv2_left":
                    ExternalDeviceClass.send("arduino_serv2_left");
                    break;
                case "arduino_serv2_right":
                    ExternalDeviceClass.send("arduino_serv2_right");
                    break;
			}
			return "unknow command error";
		}

		public static string GetInfo(string[] cmdwa)
		{
			switch (cmdwa [1].ToLower ()) {
			case "os":
				return Environment.OSVersion.ToString();
			}
			return "unknow argument of getinfo function";
		}
	}
}

