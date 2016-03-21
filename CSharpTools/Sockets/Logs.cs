using System;
using System.IO;

namespace CSharpTools.Sockets
{
    public class Logs
    {

        private String logFile = "";

        /// <summary>
        /// init Logs and set log file path
        /// </summary>
        /// <param name="logPath">log file path</param>
        public Logs(string logPath)
        {
            logFile = logPath;
            LibGlobals.SocketEvents.LogReceived += SocketEvents_LogReceived;
            LibGlobals.SocketEvents.ClientDisconnected += SocketEvents_ClientDisconnected;
            LibGlobals.SocketEvents.ClientReceived += SocketEvents_ClientReceived;
            LibGlobals.SocketEvents.CommandReceived += SocketEvents_CommandReceived;
            LibGlobals.SocketEvents.MessageReceived += SocketEvents_MessageReceived;
            LibGlobals.SocketEvents.PacketReceived += SocketEvents_PacketReceived;
            LibGlobals.SocketEvents.PacketSent += SocketEvents_PacketSent;
            LibGlobals.SocketEvents.ServerConnectionError += SocketEvents_ServerConnectionError;
            LibGlobals.SocketEvents.ServerStarted += SocketEvents_ServerStarted;
            LibGlobals.SocketEvents.ServerStopped += SocketEvents_ServerStopped;
        }

        private void SocketEvents_ServerStopped(object sender, EventArgs e)
        {
            PrintLog("Server Operation", "Server Stopped!");
        }

        private void SocketEvents_ServerStarted(object sender, EventArgs e)
        {
            PrintLog("Server Operation", "Server Started!");
        }

        private void SocketEvents_ServerConnectionError(object sender, EventArgs e)
        {
            PrintLog("Server Operation", "Server Cannot Starting...");
        }

        private void SocketEvents_PacketSent(object sender, Connection e)
        {
            PrintLog("Stream Info", "Packet Sent To " + e.ToString());
        }

        private void SocketEvents_PacketReceived(Connection connection, Packet packet)
        {
            PrintLog("Stream Info", "Packet Received from " + connection.ToString());
        }

        private void SocketEvents_MessageReceived(Connection connection, string msg)
        {
            PrintLog("Stream Info", "Message Received from " + connection.ToString());
        }

        private void SocketEvents_CommandReceived(Connection connection, string msg)
        {
            PrintLog("Stream Info", "Command Received from " + connection.ToString());
        }

        private void SocketEvents_ClientReceived(object sender, Connection e)
        {
            PrintLog("Server Operation", "Client Received on " + e.ToString());
        }

        private void SocketEvents_ClientDisconnected(object sender, Connection e)
        {
            PrintLog("Server Operation", "Client " + e.ToString() + " Disconnected!");
        }

        // use LogReceived Event once after invoked and call write log. 
        private void SocketEvents_LogReceived(object sender, string e)
        {
            writeLog(((string)sender), e);
        }

        /// <summary>
        /// print your log into the log file and fire
        /// LogReceived Event.
        /// </summary>
        /// <param name="title">type of the log</param>
        /// <param name="msg">log message</param>
        public void PrintLog(String title, String msg)
        {
            LibGlobals.SocketEvents.invokeLogReceived(title, msg);
        }

        // write incoming logs to the log file.
        private void writeLog(string title, string msg)
        {
            using (StreamWriter sw = new StreamWriter(@"D:\log.txt", true))
            {
                sw.WriteLine(" Time : " + DateTime.Now + " - Debug : " + title + " : " + msg);
            }
        }
    }
}
