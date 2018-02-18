using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;

namespace SatProt
{
    class Server
    {
        public Action<byte[]> MessageHandler;
        public int Port;
        bool _Work;

        TcpListener listener = null;
        TcpClient client = null;

        public Server(int port)
        {
            Port = port;
        }

        public void Stop()
        {
            _Work = false;
        }

        public void Send(byte[] b)
        {
            client.GetStream().Write(b, 0, b.Length);
        }

        public void Start()
        {
            _Work = true;

            listener = new TcpListener(IPAddress.Any, Port);
            listener.Start();

            while (_Work)
            {
                client = listener.AcceptTcpClient();

                NetworkStream ns = client.GetStream();

                while (client.Connected)
                {
                    try
                    {
                        byte[] msb = new byte[4];
                        ns.Read(msb, 0, 4);
                        int length = BitConverter.ToInt32(msb, 0);
                        byte[] msg = new byte[length];
                        ns.Read(msg, 0, length);
                        MessageHandler(msg);
                    }
                    catch
                    {
                        continue;
                    }
                }
            }
        }
    }
}
