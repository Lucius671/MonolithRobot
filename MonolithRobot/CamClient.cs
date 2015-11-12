using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using Emgu.CV;
using System.IO;
using System.IO.Compression;
using System.Collections;

namespace MonolithRobot
{
    public class CamClient
    {
        public const int _port = 25556;
        private static IPEndPoint remoteEP = null;
        private static Thread th_cli;
        private static UdpClient client = new UdpClient();

        private static bool _isRunning = true;

        public CamClient (IPAddress remoteIPAddress)
        {
            remoteEP = new IPEndPoint(remoteIPAddress, _port);
            StartClient ();
        }

        private static void StartClient()
        {
            th_cli = new Thread (delegate() {
                try
                {
                    ConsoleAdditives.WriteHeader("Stream started");
                    Capture cap = new Capture();
                    while(_isRunning) {
                        byte[] buf = cap.QueryGrayFrame().Bytes;
                        int buflp = buf.Length/5;

                        for(byte i=0;i<5;i++)
                        {
                            byte[] tbuf = new byte[buflp];
                            tbuf[0]=i;
                            for(int j=1;j<buflp;j++)
                            {
                                tbuf[j]=buf[i*buflp+j];
                            }
                            client.Send(tbuf,buflp,remoteEP);
                        }
                    }
                    ConsoleAdditives.WriteHeader("Stream stoped");
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }

            });
            th_cli.Start ();
        }

        public static byte[] Compress(byte[] raw) {
            using (MemoryStream memory = new MemoryStream())
            {
                using (GZipStream gzip = new GZipStream(memory, CompressionMode.Compress, true))
                {
                    gzip.Write(raw, 0, raw.Length);
                }
                return memory.ToArray();
            }
        }
    }
}

