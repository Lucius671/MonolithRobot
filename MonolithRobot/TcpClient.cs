﻿using System;
using System.Net;
using System.Threading;
using System.Net.Sockets;
using System.Text;

namespace MonolithRobot
{
	public class TcpClient
	{
		public class StateObject {
			public Socket workSocket = null;
			public const int BufferSize = 1024;
			public byte[] buffer = new byte[BufferSize];
			public StringBuilder sb = new StringBuilder ();
		}

		private static IPEndPoint remoteEP;

		private static ManualResetEvent connectDone = new ManualResetEvent(false);
		private static ManualResetEvent receiveDone = new ManualResetEvent(false);

		private static Socket client = null;

		public TcpClient (string remoteIp, int port)
		{
			remoteEP = new IPEndPoint (IPAddress.Parse (remoteIp), port);
			StartClient ();
		}

		private static void StartClient()
		{
			Thread th = new Thread (delegate() {
				try
				{
					Console.WriteLine("Client started");
					client = new Socket (AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					client.BeginConnect (remoteEP, new AsyncCallback(connectCallback), client);
					connectDone.WaitOne ();
					while(client.Connected) {
						StateObject state = new StateObject();
						state.workSocket = client;
						client.BeginReceive (state.buffer, 0, StateObject.BufferSize, 0, 
							new AsyncCallback(receiveCallback), state);
						receiveDone.WaitOne();
					}
				}
				catch(Exception ex)
				{
					Console.WriteLine(ex.ToString());
				}
			});
			th.Start ();
		}

		private static void connectCallback(IAsyncResult ar) {
			try {
				Socket client = (Socket)ar.AsyncState;
				client.EndConnect (ar);

				Console.WriteLine ("Connected to {0}", client.RemoteEndPoint.ToString ());

				connectDone.Set ();
			} catch (Exception ex) {
				connectDone.Set ();
				Console.WriteLine (ex.ToString ());
			}
		}

		private static void receiveCallback(IAsyncResult ar) {
			try {
				StateObject state = (StateObject)ar.AsyncState;
				Socket client = state.workSocket;
				int bytesRead = client.EndReceive(ar);
				Console.WriteLine ("Received {0} bytes from {1}", bytesRead, client.RemoteEndPoint.ToString ());
				if(bytesRead > 0)
				{
					state.sb.Append(Encoding.UTF8.GetString(state.buffer, 0, bytesRead));
					if(state.sb.ToString().Contains("<!E>"))
					{
						string toSend = "";
						string cmd = Between(state.sb.ToString(),"<!S>","<!E>");
						Console.WriteLine("["+(bytesRead)+"]>"+cmd);
						toSend = SwitchAnswer(cmd);
						if(toSend.Length>0) {
							Console.WriteLine("<~"+toSend);
							byte[] bytesToSend = Encoding.UTF8.GetBytes(AddTagsToStr(toSend));
							client.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None,
								new AsyncCallback(sendCallback), state);
						}else{
							receiveDone.Set();
						}
					}
					else
					{
						client.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
							new AsyncCallback(receiveCallback), state);
					}
				}					
			}catch(Exception ex) {
				Console.WriteLine (ex.ToString ());
			}
		}

		static void sendCallback(IAsyncResult ar) {
			StateObject state = (StateObject)ar.AsyncState;
			Socket handler = state.workSocket;
			int bytesSend = handler.EndSend (ar);
			Console.WriteLine ("Sended {0} bytes to {1}", bytesSend, client.RemoteEndPoint.ToString ());
			StateObject newstate = new StateObject ();
			newstate.workSocket = handler;
			handler.BeginReceive (newstate.buffer, 0, StateObject.BufferSize, 0, 
				new AsyncCallback (receiveCallback), newstate);
		}

		private static string SwitchAnswer(string cmd) {
			string rtn = "";
			switch (cmd.ToLower ()) {
			case "ping":
				rtn = "pong";
				break;
			default:
				rtn = cmd + " accepted";
				break;
			}
			return rtn;
		}

		static string Between(string str, string str1, string str2)
		{
			int i1 = 0, i2 = 0;
			string rtn = "";
			i1 = str.IndexOf (str1, StringComparison.InvariantCultureIgnoreCase);
			if (i1 > -1) {
				i2 = str.IndexOf (str2, i1 + 1, StringComparison.CurrentCultureIgnoreCase);
				if (i2 > -1) {
					rtn = str.Substring (i1 + str1.Length, i2 - i1 - str1.Length);
				}
			}
			return rtn;
		}

		public static string AddTagsToStr(string str) 
		{
			return "<!S>" + str + "<!E>";
		}
	}
}

