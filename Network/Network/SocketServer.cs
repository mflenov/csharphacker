using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network
{
	public class SocketServer
	{
        Socket? server = null;

		public SocketServer()
		{
		}

		public void Start()
		{
            server = new Socket(AddressFamily.InterNetworkV6,
                     SocketType.Stream, ProtocolType.Tcp);
            EndPoint endPoint = new IPEndPoint(IPAddress.IPv6Any, 12345);

            try
            {
                server.Bind(endPoint);
                server.Listen(100);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Невозможно запустить сервер " + exc.Message);
                return;
            }

            server.BeginAccept(new AsyncCallback(AcceptCallback), server);
        }

        void AcceptCallback(IAsyncResult result)
        {
            Socket? serverSocket = (Socket?)result.AsyncState;

            SocketData data = new SocketData();
            data.ClientConnection = serverSocket?.EndAccept(result);

            data.ClientConnection?.BeginReceive(data.Buffer, 0,
                     1024, SocketFlags.None,
                     new AsyncCallback(ReadCallback), data);
        }

        void ReadCallback(IAsyncResult result)
        {
            SocketData? data = (SocketData?)result.AsyncState;
            int bytes = data?.ClientConnection?.EndReceive(result) ?? 0;

            if (bytes > 0 && data?.Buffer != null)
            {
                string s = Encoding.UTF8.GetString(data?.Buffer!, 0, bytes);
                Console.WriteLine(s);
                data?.ClientConnection?.Send(
                      Encoding.UTF8.GetBytes("Получено: " +
                      s.Length + " символов"));
            }
        }
    }
}

