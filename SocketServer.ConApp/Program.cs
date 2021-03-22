using SocketCommon;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace SocketServer.ConApp
{
    class Program
    {
        static Task Main(string[] args)
        {
            Console.WriteLine("SocketServerApp is running...");

            var hostName = Dns.GetHostName();
            var socketConfiguration = new SocketConfiguration(hostName, 3333);
            var tcpListener = socketConfiguration.CreateTcpListener();

            tcpListener.Start();
            while (true)
            {
                var handler = tcpListener.AcceptTcpClient();
                var socketHandler = new TcpClientHandler(handler);

                socketHandler.StartAsync();
            }
        }
    }
}
