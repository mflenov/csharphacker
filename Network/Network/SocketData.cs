using System;
using System.Net.Sockets;

namespace Network
{
    class SocketData
    {
        public const int BufferSize = 1024;
        public Socket? ClientConnection { get; set; }

        byte[] buffer = new byte[BufferSize];

        public byte[] Buffer
        {
            get { return buffer; }
            set { buffer = value; }
        }
    }

}

