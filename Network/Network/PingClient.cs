using System;
using System.Net.NetworkInformation;

namespace Network
{
	public class PingClient
	{
		public PingClient()
		{
		}

		public void Execute(string host)
		{
            Ping ping = new Ping();

            for (int i = 0; i < 4; i++)
			{
				try
				{
					PingReply reply = ping.Send(host);
					if (reply.Status == IPStatus.Success)
					{
						Console.Write($"Ответ от: {reply.Address} ");
						Console.WriteLine($"Время: {reply.RoundtripTime} ");
					}
					else
						Console.WriteLine($"Ошибка {reply.Status}");
				}
				catch (Exception e)
				{
					if (e.InnerException?.Source == "System.Net.NameResolution")
                        Console.WriteLine($"Ошибка DNS или соединения");
					else 
						Console.WriteLine($"Ошибка {e.Message}");
                }
			}
        }
	}
}

