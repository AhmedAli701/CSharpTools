using System;
using System.Runtime.Serialization;

namespace CSharpTools.Sockets
{
    class NoIPFoundException : Exception
    {
        public NoIPFoundException() : base() { }
        public NoIPFoundException(string message) : base(message) { }
        public NoIPFoundException(string message, Exception innerException) : base(message, innerException) { }
        public NoIPFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
