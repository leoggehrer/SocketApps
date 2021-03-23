using SocketCommon.Common;
using SocketCommon.Models;
using SocketCommon.Contracts;
using SocketCommon.Pattern;
using System;
using System.Net;
using SocketCommon.Client;

namespace SocketClient.ConApp
{
    internal class Program
    {
        private class ConsoleWriter : IObserver
        {

            public void Notify(Observable observable, object eventArgs)
            {
                if (eventArgs is Message msg)
                {
                    System.Diagnostics.Debug.WriteLine(msg.ToString());
                }
            }
        }
        private static void Main(/*string[] args*/)
        {
            Console.WriteLine("SocketClientApp is running...");

            MessageDistributer.Instance.AddObserver(new ConsoleWriter());

            var hostName = Dns.GetHostName();
            var socketConfiguration = new SocketConfiguration(hostName, 3333);
            var tcpClient = socketConfiguration.CreateTcpClient();
            var clientListener = new TcpClientListener(tcpClient);

            clientListener.StartAsync();

            for (int i = 0; i < 10; i++)
            {
                Message message = new Message() { From = "Geri", Body = DateTime.Now.ToString() };

                clientListener.Write(message);
                System.Threading.Thread.Sleep(1000);
            }
            Message exitMsg = new Message() { Command = SocketCommand.Quit.ToString(), From = "Geri", Body = string.Empty };

            clientListener.Write(exitMsg);

            //var json = JsonSerializer.Serialize<Message>(message);
            //var bytes = Encoding.ASCII.GetBytes(json);
            //var stream = client.GetStream();

            //stream.Write(bytes, 0, bytes.Length);
            //stream.Flush();
            ////Thread.Sleep(2000);
            ////stream.Close();

            ////using var stream1 = client.GetStream();
            //stream.Write(bytes, 0, bytes.Length);
            //stream.Flush();
            ////Thread.Sleep(2000);
            ////stream1.Close();

            //message.Command = SocketCommand.Quit.ToString();
            //json = JsonSerializer.Serialize<Message>(message);
            //bytes = Encoding.ASCII.GetBytes(json);

            ////using var stream2 = client.GetStream();
            //stream.Write(bytes, 0, bytes.Length);
            //stream.Flush();
            ////Thread.Sleep(2000);
            ////stream2.Close();

            //stream.Close();
            //client.Close();
        }
    }
}
