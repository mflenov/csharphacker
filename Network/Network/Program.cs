// See https://aka.ms/new-console-template for more information
using Network;

/*
var pingClient = new PingClient();
pingClient.Execute("www.flenov.info");
*/

/*
var traceClient = new TraceRoute();
traceClient.Execute("www.flenov.info");
*/

/*
var client = new WebRequestClient();
var result = client.Execute().GetAwaiter().GetResult();
Console.WriteLine(result);
*/


Console.WriteLine("Нажмите S, чтобы запустить сервер C для клиента");
if (Console.ReadLine()?.ToLower() == "s")
{
    Console.WriteLine("Сервер запущен");
    SocketServer server = new SocketServer();
    server.Start();
}
else
{
    SocketClient client = new SocketClient();
    client.SendMessage("Test");

}

Console.WriteLine("Нажмите Enter, чтобы выйти");
Console.ReadLine();
