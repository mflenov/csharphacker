using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace Network
{
	public class SocketClient
	{
        public void SendMessage(string command)
        {
            Socket socket = new Socket(AddressFamily.InterNetworkV6,
                 SocketType.Stream, ProtocolType.Tcp);
            EndPoint endPoint = new IPEndPoint(IPAddress.Parse("::1"), 12345);
            socket.Connect(endPoint);

            Byte[] bytesSent = Encoding.UTF8.GetBytes(command);
            socket.Send(bytesSent);

            byte[] buffer = new byte[1024];
            int readBytes;
            StringBuilder pageContent = new StringBuilder();
            if ((readBytes = socket.Receive(buffer)) > 0)
            {
                string resultStr = System.Text.Encoding.UTF8.GetString(
                    buffer, 0, readBytes);
                pageContent.Append(resultStr);
                Console.WriteLine(resultStr);
            }

            socket.Close();
        }

        // преобразование строки адреса в объект
        public IPAddress? GetAddress(string address)
        {
            IPAddress? ipAddress = null;
            try
            {
                ipAddress = IPAddress.Parse(address);
            }
            catch (Exception)
            {
                IPHostEntry heserver;
                try
                {
                    heserver = Dns.GetHostEntry(address);
                    if (heserver.AddressList.Length == 0)
                    {
                        return null;
                    }
                    ipAddress = heserver.AddressList[0];
                }
                catch
                {
                    return null;
                }
            }

            return ipAddress;
        }
    }
}

