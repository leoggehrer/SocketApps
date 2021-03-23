using SocketCommon.Common;
using SocketCommon.Server;
using System;
using System.Net;
using System.Threading.Tasks;

namespace SocketServer.ConApp
{
    internal class Program
    {
        private static Task Main(/*string[] args*/)
        {
            Console.WriteLine("SocketServerApp is running...");

            var hostName = Dns.GetHostName();
            var socketConfiguration = new SocketConfiguration(hostName, 3333);
            var tcpListener = socketConfiguration.CreateTcpListener();

            tcpListener.Start();
            while (true)
            {
                var handler = tcpListener.AcceptTcpClient();
                var clientHandler = new TcpClientHandler(handler);

                clientHandler.StartAsync();
            }
        }
    }
}
