using SocketCommon;
using SocketCommon.Models;
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace SocketClient.ConApp
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.WriteLine("SocketClientApp is running...");

            var hostName = Dns.GetHostName();
            SocketConfiguration socketConfiguration = new SocketConfiguration(hostName, 3333);
            TcpClient client = socketConfiguration.CreateTcpClient();
            Message message = new Message() { From = "Geri", Body = "Hallo wie geht es?" };
            var json = JsonSerializer.Serialize<Message>(message);
            var bytes = Encoding.ASCII.GetBytes(json);
            var stream = client.GetStream();

            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            //Thread.Sleep(2000);
            //stream.Close();

            //using var stream1 = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            //Thread.Sleep(2000);
            //stream1.Close();

            message.Command = SocketCommand.Quit.ToString();
            json = JsonSerializer.Serialize<Message>(message);
            bytes = Encoding.ASCII.GetBytes(json);

            //using var stream2 = client.GetStream();
            stream.Write(bytes, 0, bytes.Length);
            stream.Flush();
            //Thread.Sleep(2000);
            //stream2.Close();

            stream.Close();
            client.Close();
        }
    }
}
