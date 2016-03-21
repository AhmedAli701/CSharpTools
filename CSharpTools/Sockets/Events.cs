using System;

namespace CSharpTools.Sockets
{
    internal delegate void PacketReceivedEventHandler(Connection connection, Packet packet);
    public delegate void MessageReceivedEventHandler(Connection connection, String msg);

    public class Events
    {
        /// <summary>
        /// event means that the server couldn't start.
        /// </summary>
        public event EventHandler ServerConnectionError;
        
        /// <summary>
        /// event to track any new Log. 
        /// </summary>
        public event EventHandler<string> LogReceived;

        public event EventHandler ServerStarted;
        public event EventHandler ServerStopped;
        public event EventHandler<Connection> ClientReceived;
        public event EventHandler<Connection> ClientDisconnected;
        internal event PacketReceivedEventHandler PacketReceived;
        internal event EventHandler<Connection> PacketSent;
        public event MessageReceivedEventHandler MessageReceived;
        public event MessageReceivedEventHandler CommandReceived;

        /// <summary>
        /// fire this event.
        /// </summary>
        /// <param name="title">log type</param>
        /// <param name="msg">log message</param>
        public void invokeLogReceived(string title, string msg)
        {
            LogReceived(title, msg);
        }

        public void invokeServerConnectionError()
        {
            ServerConnectionError(null, EventArgs.Empty);
        }

        public void invokeServerStarted()
        {
            ServerStarted(null, EventArgs.Empty);
        }

        public void invokeServerStopped()
        {
            ServerStopped(null, EventArgs.Empty);
        }

        public void invokeClientReceived(Connection connection)
        {
            ClientReceived(null, connection);
        }

        internal void invokePacketReceived(Connection connection, Packet packet)
        {
            PacketReceived(connection, packet);
        }

        public void invokeClientDisconnected(Connection connection)
        {
            ClientDisconnected(null, connection);
        }

        public void invokePacketSent(Connection connection)
        {
            PacketSent(null, connection);
        }

        public void invokeMessageReceived(Connection connection, String msg)
        {
            MessageReceived(connection, msg);
        }

        public void invokeCommandReceived(Connection connection, String cmd)
        {
            CommandReceived(connection, cmd);
        }
    }
}
