using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SocketCommon.Extensions;

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
                var connectionQuit = false;
                var buffer = new byte[1024];

                while (connectionQuit == false)
                {
                    var readLen = 0;
                    var strData = string.Empty;
                    var stream = TcpClient.GetStream();

                    while ((readLen = stream.Read(buffer, 0, buffer.Length)) == buffer.Length)
                    {
                        strData += Encoding.ASCII.GetString(buffer, 0, readLen);
                    }
                    if (readLen > 0)
                    {
                        strData += Encoding.ASCII.GetString(buffer, 0, readLen);
                        System.Diagnostics.Debug.WriteLine(strData);
                        try
                        {
                            var models = strData.GetAllTags(new string[] { "{", "}" })
                                                .Select(t => JsonSerializer.Deserialize<Models.Message>(t.FullText));

                            foreach (var model in models)
                            {
                                if (model.Command.GetValueOrDefault().Equals(SocketCommand.Quit.ToString()))
                                {
                                    connectionQuit = true;
                                }
                            }
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
