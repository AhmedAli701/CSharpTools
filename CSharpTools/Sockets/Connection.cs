using System;
using System.Net;
using System.Net.Sockets;

namespace CSharpTools.Sockets
{
    public class Connection
    {
        private Socket clientSocket;
        private NetStream netStream;
        private string name = "";

        public Connection(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            IPEndPoint ipep = (IPEndPoint)clientSocket.RemoteEndPoint;
            name = ipep.Address.ToString() + ":" + ipep.Port.ToString();
            this.netStream = new NetStream(this);
            this.netStream.Start();
        }

        /// <summary>
        /// Send Message to this Connection.
        /// </summary>
        /// <param name="message">Message in String Format</param>
        public void SendMessage(String message)
        {
            netStream.SendPacket(Header.Tag.MSG, message);
        }

        public void SendCommand(String cmd)
        {
            netStream.SendPacket(Header.Tag.CMD, cmd);
        }

        public Socket ClientSocket
        {
            get { return clientSocket; }
        }

        public void Close()
        {
            try
            {
                clientSocket.Close();
            }
            catch { }
        }

        public override string ToString()
        {
            return name;
        }
    }
}
