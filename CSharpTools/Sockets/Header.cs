using System;
using System.Text;

namespace CSharpTools.Sockets
{
    public class Header
    {
        public const int SIZE = 8;
        private byte[] encodedMsg = new byte[8];
        private string msg = String.Empty;
        private const char SEPARATOR = '|';

        // next packet information.
        private Tag packetTag;
        private int packetSize = 0;

        /// <summary>
        /// Header Decoder Constructor
        /// </summary>
        /// <param name="headerMsg">msg array</param>
        /// <exception cref="ArgumentException">incorrect header message format!</exception>
        /// <exception cref="NullReferenceException">encoded message array either empty or undefined</exception>
        public Header(byte[] headerMsg)
        {
            if (headerMsg != null && headerMsg.Length >= 8)
            {
                for (int i = 0; i < SIZE; i++)
                {
                    encodedMsg[i] = headerMsg[i];
                }

            }
            else
                throw new NullReferenceException("encoded message array either empty or undefined");

            //convert incoming byte array insto readable string.
            msg = Encoding.ASCII.GetString(encodedMsg);

            LibGlobals.SocketLogs.PrintLog("Decoding this msg", msg);

            //encode the incoming header message
            if (!decodeMsg())
            {
                throw new ArgumentException("incorrect header message format!");
            }
        }

        /// <summary>
        /// Header Encoder Constructor
        /// initialize new instance of the header class with 
        /// tag type and size of the data to send  
        /// </summary>
        /// <param name="tag">data tage</param>
        /// <param name="size">length of the data</param>
        /// <example>ORN|0000</example>
        public Header(Tag tag, int size)
        {
            // ex : MSG|0015
            msg = tag.ToString() + SEPARATOR + size.ToString("0000");
        }

        private bool decodeMsg()
        {
            // validate Message size 
            if (msg.Length != SIZE)
            {
                LibGlobals.SocketLogs.PrintLog("error validating Header",
                    "Decoded Header not in correct size expect : " + SIZE + " found : " + msg.Length);
                return false;
            }


            // split the message into two parts 
            // first is the Tag second is the Next 
            // Packet Size
            string[] msgParts = msg.Split(SEPARATOR);

            // validate that the message has two parts.
            if (msgParts.Length != 2)
                return false;

            // get the next packet size 
            if (!int.TryParse(msgParts[1], out packetSize))
            {
                LibGlobals.SocketLogs.PrintLog("error validating Header",
                    "cannot parse packet size");
                return false;
            }

            try
            {
                // get the Tag 
                packetTag = (Tag)Enum.Parse(typeof(Tag), msgParts[0]);
            }
            catch (Exception ex) { return false; }

            return true;
        }

        /// <summary>
        /// get the packet Tag from decoded 
        /// header message.
        /// </summary>
        public Tag PacketTag
        {
            get { return packetTag; }
        }

        /// <summary>
        /// get the packet size from decoded 
        /// header message
        /// </summary>
        public int PacketSize
        {
            get { return packetSize; }
        }

        /// <summary>
        /// get encoded header message
        /// in string format
        /// </summary>
        public string HeaderMessage
        {
            get { return msg; }
        }

        public enum Tag { MSG, CMD, NON }
    }
}
