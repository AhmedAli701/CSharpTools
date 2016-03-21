using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace CSharpTools.Sockets
{
    class Server
    {
        private ServerType serverType;
        private Socket listener;

        /// <summary>
        /// init new instance of the Server.
        /// </summary>
        /// <param name="serverType">decide how 
        /// the server will handle the amount of Client</param>
        public Server(ServerType serverType)
        {
            this.serverType = serverType;
        }

        /// <summary>
        /// Start Listen to incoming connections,
        /// on default IP Address and Port
        /// </summary>
        public void Start()
        {
            List<IPAddress> ips = new List<IPAddress>(getServerIP());

            if (ips.Capacity > 0)
            {
                IPEndPoint localEndPoint = new IPEndPoint(ips.First(), 9988);
                StartListen(localEndPoint);
            }
            else throw new NoIPFoundException();
        }

        /// <summary>
        /// Start Listen to incoming connections,
        /// on given IP Address and Port
        /// </summary>
        /// <param name="ip">Server IP Address</param>
        /// <param name="port">Port Number to listen on</param>
        public void Start(string ip, int port)
        {
            IPEndPoint localEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            StartListen(localEndPoint);
        }


        private void StartListen(IPEndPoint ipep)
        {
            try
            {
                // Create a TCP/IP socket.
                listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Bind the socket to the local endpoint and listen for incoming connections.
                listener.Bind(ipep);
                listener.Listen(100);

                // Start an asynchronous socket to listen for connections.
                listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);

                // print log >>>
                LibGlobals.SocketLogs.PrintLog(
                    "Server Starting", "Server Start To Listen on " + ipep.Address.ToString() + ":" + ipep.Port.ToString()
                );
                // invoke server started event.
                LibGlobals.SocketEvents.invokeServerStarted();
            }
            catch (SocketException ex)
            {
                // invoke ServerConnectionError Event.
                LibGlobals.SocketEvents.invokeServerConnectionError();
            }
        }

        private void AcceptCallBack(IAsyncResult ar)
        {
            try
            {
                // Get the socket that handles the client request.
                Socket listener = (Socket)ar.AsyncState;
                Socket handler = listener.EndAccept(ar);

                // print log >>>
                LibGlobals.SocketLogs.PrintLog("Client Received", "on IP:"
                    + ((IPEndPoint)handler.RemoteEndPoint).Address.ToString()
                );

                Connection connection = new Connection(handler);           //create connection instance.
                LibGlobals.SocketEvents.invokeClientReceived(connection);  //invoke client received event.

                if (serverType == ServerType.MULTI_CLIENTS)
                {
                    // Debug
                    LibGlobals.SocketLogs.PrintLog("Server Operation", "Server support multi-clients and listen for more...");

                    // listen for more connections.
                    listener.BeginAccept(new AsyncCallback(AcceptCallBack), listener);
                }
                else
                {
                    // Debug
                    LibGlobals.SocketLogs.PrintLog("Server Operation", "server either dose not support multi-clients or turned off...");
                    // invoke server started event.
                    LibGlobals.SocketEvents.invokeServerStopped();
                }
            }
            catch (SocketException ex)
            {
                //print log >>>
                LibGlobals.SocketLogs.PrintLog("Socket Error - Server Closed", ex.Message);
                //invoke server error event
                LibGlobals.SocketEvents.invokeServerConnectionError();
            }
        }

        /// <summary>
        /// this returns a List of all possible IP Address
        /// on the Server Machine
        /// </summary>
        /// <returns>List of IPAddress</returns>
        public List<IPAddress> getServerIP()
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry(Dns.GetHostName());
            List<IPAddress> v4Address = new List<IPAddress>();

            foreach (IPAddress ip in ipHostInfo.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                    v4Address.Add(ip);
            }

            return v4Address;
        }

        /// <summary>
        /// stop receiveing more clients and Disconnect 
        /// from all clients connections.
        /// </summary>
        public void Close()
        {
            try
            {
                listener.Close();
            }
            catch { }

            LibGlobals.ClientCR.Disconnect();
        }

        /// <summary>
        /// stop receiveing more clients.
        /// </summary>
        public void StopListen()
        {
            listener.Close();
        }

        public enum ServerType { MULTI_CLIENTS, SINGLE_CLIENT }
    }
}
