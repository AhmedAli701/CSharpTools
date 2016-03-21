using System.Collections.Generic;

namespace CSharpTools.Sockets
{
    public class ClientControl
    {
        private List<Connection> connectionsList;

        /// <summary>
        /// init new instance of Client Control.
        /// </summary>
        public ClientControl()
        {
            connectionsList = new List<Connection>();
            LibGlobals.SocketEvents.ClientReceived += SocketEvents_ClientReceived;
            LibGlobals.SocketEvents.ClientDisconnected += SocketEvents_ClientDisconnected;
            LibGlobals.SocketEvents.PacketReceived += SocketEvents_PacketReceived;
        }

        private void SocketEvents_ClientDisconnected(object sender, Connection e)
        {
            connectionsList.Remove(e);
        }

        private void SocketEvents_ClientReceived(object sender, Connection e)
        {
            connectionsList.Add(e);
        }

        private void SocketEvents_PacketReceived(Connection connection, Packet packet)
        {
            // invoke Packet Received.
            switch (packet.PacketHeader.PacketTag)
            {
                case Header.Tag.MSG:
                    LibGlobals.SocketEvents.invokeMessageReceived(connection, packet.PacketData);
                    break;
                case Header.Tag.CMD:
                    LibGlobals.SocketEvents.invokeCommandReceived(connection, packet.PacketData);
                    break;
            }
        }

        /// <summary>
        /// remove Client From the Clients List.
        /// </summary>
        /// <param name="connection">Client Connection</param>
        public void removeConnection(Connection connection)
        {
            connectionsList.Remove(connection);
        }

        public void Disconnect()
        {
            foreach(Connection connection in connectionsList)
            {
                /*  
                    this will close the connection
                    and invoke event which remove 
                    the connection from the Client list.
                */
                connection.Close(); 
            }
        }
    }
}
