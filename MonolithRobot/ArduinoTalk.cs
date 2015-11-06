using System;
using System.Threading;
using System.IO.Ports;
using System.Text;

namespace MonolithRobot
{
	public class ArduinoDevice
	{
		SerialPort arduinoBoard = new SerialPort();

        public bool IsOpen {
            get 
            {
                return arduinoBoard.IsOpen;
            }
        }

		public void OpenConnetion()
		{
			if (!arduinoBoard.IsOpen) {
                arduinoBoard.BaudRate = 9600;
				arduinoBoard.PortName = "/dev/ttyUSB0";
				arduinoBoard.Open ();
                ConsoleAdditives.WriteInfo("Arduino port open");
			} else {
				throw new InvalidOperationException ("The serial port is already open!");
			}
		}

		public void Send(string cmd)
		{
            if (arduinoBoard.IsOpen)
            {
                ConsoleAdditives.WriteInfo("Sended to A:"+cmd);
                arduinoBoard.Write(cmd + "\n");
            }else
				throw new InvalidOperationException ("The serial port is already open!");
		}

        public string ReadLn()
        {
            return arduinoBoard.ReadLine();
        }

		public void CloseConnection()
		{
			arduinoBoard.Close ();
            ConsoleAdditives.WriteHeader("Arduino port closed");
		}
	}
}
