using System;
using System.Net.NetworkInformation;

namespace Network
{
	public class TraceRoute
	{
		public TraceRoute()
		{
		}

        public void Execute(string host)
        {
            Ping ping = new Ping();
            PingOptions options = new PingOptions();
            options.Ttl = 1;

            PingReply reply;
            do
            {
                try
                {
                    reply = ping.Send(host, 1000, new byte[] { 110, 110 }, options);
                    if (reply.Status == IPStatus.TtlExpired)
                    {
                        Console.Write($"Ответ {options.Ttl} от: {reply.Address} ");
                        Console.WriteLine($"Время: {reply.RoundtripTime} ");
                    }
                    options.Ttl = options.Ttl + 1;
                }
                catch (Exception)
                {
                    Console.WriteLine($"Ошибка DNS или соединения");
                    return;
                }
            } while (reply.Status != IPStatus.Success);
            Console.WriteLine("Прибыли");
        }
    }
}

