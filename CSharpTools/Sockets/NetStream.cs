using System;
using System.Net;
using System.Net.Sockets;

namespace CSharpTools.Sockets
{
    public class NetStream
    {
        private Connection connection;
        private Socket clientSocket;

        public NetStream(Connection connection)
        {
            this.connection = connection;
            this.clientSocket = connection.ClientSocket;
        }

        public void Start()
        {
            // receive packet header
            ReceiveData(Header.SIZE, ReceiveHeaderCallBack, new Packet());
        }

        public void SendPacket(Header.Tag tag, String packetMsg)
        {
            // Debug
            LibGlobals.SocketLogs.PrintLog("Debug", "Sending Packet To "
                + ((IPEndPoint)clientSocket.RemoteEndPoint).Address.ToString());

            Packet packet = new Packet(tag, packetMsg);
            clientSocket.BeginSend(packet.DataToSend, 0, packet.DataToSend.Length, 0, SendCallBack, packet);
        }

        private void SendCallBack(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
                LibGlobals.SocketEvents.invokePacketSent(connection);
            }
            catch (SocketException ex)
            {
                LibGlobals.SocketLogs.PrintLog("Error - SocketException", ex.Message);
                LibGlobals.SocketEvents.invokeClientDisconnected(connection);
            }
        }

        private void ReceiveData(int dataSize, Action<IAsyncResult> callbackMethod, Packet packet)
        {
            clientSocket.BeginReceive(packet.Buffer, 0, dataSize, 0, new AsyncCallback(callbackMethod), packet);
        }

        private void ReceiveHeaderCallBack(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);

                if (count > 0)
                {
                    /*  
                        1 . get the packet object.
                        2 . set the packet header with new instance 
                            of header class using data in the buffer.
                    */
                    Packet packet = (Packet)ar.AsyncState;
                    packet.PacketHeader = new Header(packet.Buffer);

                    ReceiveData(packet.PacketHeader.PacketSize, ReadDataCallBack, packet);
                }
                else
                {
                    // receive packet header "again"
                    ReceiveData(Header.SIZE, ReceiveHeaderCallBack, new Packet());
                }
            }
            catch (SocketException ex)
            {
                LibGlobals.SocketLogs.PrintLog("Error - SocketException", ex.Message);
                LibGlobals.SocketEvents.invokeClientDisconnected(connection);
            }

        }

        private void ReadDataCallBack(IAsyncResult ar)
        {
            try
            {
                int count = clientSocket.EndReceive(ar);

                Packet packet = (Packet)ar.AsyncState;

                if (count > 0)
                {
                    LibGlobals.SocketEvents.invokePacketReceived(connection, packet);
                }

                // start listen to new packet header.
                ReceiveData(Header.SIZE, ReceiveHeaderCallBack, new Packet());
            }
            catch (SocketException ex)
            {
                LibGlobals.SocketLogs.PrintLog("Error - SocketException", ex.Message);
                LibGlobals.SocketEvents.invokeClientDisconnected(connection);
            }
        }
    }
}
