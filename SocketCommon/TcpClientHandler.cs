using System;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocketCommon
{
    public class TcpClientHandler
    {
        private TcpClient TcpClient { get; set; }
        public TcpClientHandler(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
        }

        public Task StartAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                var buffer = new byte[1024];
                var command = default(string);

                while (command == null || command.Equals(SocketCommand.Quit.ToString()) == false)
                {
                    var readLen = 0;
                    var stringData = string.Empty;
                    var stream = TcpClient.GetStream();

                    while ((readLen = stream.Read(buffer, 0, buffer.Length)) == buffer.Length)
                    {
                        stringData += Encoding.ASCII.GetString(buffer, 0, readLen);
                    }
                    if (readLen > 0)
                    {
                        stringData += Encoding.ASCII.GetString(buffer, 0, readLen);
                        System.Diagnostics.Debug.WriteLine(stringData);
                        try
                        {
                            Models.Message message = JsonSerializer.Deserialize<Models.Message>(stringData);

                            command = message.Command;
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                        }
                    }
                }
                TcpClient.Close();
                System.Diagnostics.Debug.WriteLine("TcpClient closed");
            });
        }
    }
}
