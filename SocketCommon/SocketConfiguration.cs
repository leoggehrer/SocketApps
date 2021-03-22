using System.Net;
using System.Net.Sockets;

namespace SocketCommon
{
    public partial class SocketConfiguration
    {
        public int Port { get; init; }
        public string HostName { get; init; }
        public IPHostEntry IPHostEntry { get; init; }
        public IPAddress IPAddress { get; init; }
        public IPEndPoint IPEndPoint { get; init; }

        public SocketConfiguration(string hostName, int port)
        {
            HostName = hostName;
            Port = port;

            // Get Host IP Address that is used to establish a connection  
            // In this case, we get one IP address of localhost that is IP : 127.0.0.1  
            // If a host has multiple addresses, you will get a list of addresses  
            IPHostEntry = Dns.GetHostEntry(hostName);
            IPAddress = IPHostEntry.AddressList[0];
            IPEndPoint = new IPEndPoint(IPAddress, port);
        }

        public TcpClient CreateTcpClient()
        {
            return new TcpClient(HostName, Port);
        }
        public TcpListener CreateTcpListener()
        {
            return new TcpListener(IPAddress, Port);
        }

        public Socket CreateServerSocket(SocketType socketType, ProtocolType protocolType)
        {
            var result = new Socket(IPAddress.AddressFamily, socketType, protocolType);

            result.Bind(IPEndPoint);
            return result;
        }

        public Socket CreateClientSocket(SocketType socketType, ProtocolType protocolType)
        {
            var result = new Socket(IPAddress.AddressFamily, socketType, protocolType);

            result.Connect(IPEndPoint);
            return result;
        }
    }
}
