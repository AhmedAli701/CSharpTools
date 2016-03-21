using System.Text;

namespace CSharpTools.Sockets
{
    class Packet
    {
        private Header header;
        private byte[] data;      // packet data in bytes 
        public const int BUFFER_SIZE = 1024;
        private byte[] buffer = new byte[BUFFER_SIZE];


        /// <summary>
        /// Packet Decoder
        /// initialize new instance of the packet class
        /// </summary>
        public Packet() { }

        /// <summary>
        /// Packet Encoder.
        /// </summary>
        /// <param name="tag"></param>
        /// <param name="dataToSend"></param>
        public Packet(Header.Tag tag, string dataToSend)
        {
            int size = Encoding.ASCII.GetByteCount(dataToSend);         // get data size.
            header = new Header(tag, size);                             // create packet header.
            string packetMsg = header.HeaderMessage + dataToSend;      // combine both header and data into one string.
            data = Encoding.ASCII.GetBytes(packetMsg);                  // convert string to byte array to send them later.

            //Debug
            LibGlobals.SocketLogs.PrintLog("Packet To Send", packetMsg);
            LibGlobals.SocketLogs.PrintLog("Packet Size is : ", data.Length.ToString());
        }

        /// <summary>
        /// get data in string format by encoding 
        /// current data in the buffer.
        /// </summary>
        public string PacketData
        {
            get
            {
                return Encoding.ASCII.GetString(buffer, 0, header.PacketSize);
            }
        }

        /// <summary>
        /// get data buffer which store all incoming 
        /// and outgoing data.
        /// </summary>
        public byte[] Buffer
        {
            get { return buffer; }
        }

        /// <summary>
        /// get and set Packet Header
        /// which is the header of the 
        /// packet contains Packet Tag and Size.
        /// </summary>
        public Header PacketHeader
        {
            get { return header; }
            set { header = value; }
        }

        public byte[] DataToSend
        {
            get { return data; }
        }

    }
}
