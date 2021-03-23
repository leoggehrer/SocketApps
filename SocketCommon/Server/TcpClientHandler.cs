using System;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using SocketCommon.Common;
using SocketCommon.Extensions;

namespace SocketCommon.Server
{
    public class TcpClientHandler : Contracts.IObserver
    {
        public bool IsRunning { get; private set; }
        private TcpClient TcpClient { get; set; }
        public TcpClientHandler(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));

            MessageDistributer.Instance.AddObserver(this);
        }

        public Task StartAsync()
        {
            return Task.Factory.StartNew(() =>
            {
                var buffer = new byte[1024];

                IsRunning = true;
                while (IsRunning)
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
                                    IsRunning = false;
                                    model.Command = SocketCommand.Disconnected.ToString();
                                    model.Body = string.Empty;
                                    MessageDistributer.Instance.Distribute(model);
                                }
                                else
                                {
                                    model.Command = SocketCommand.DistributeMessage.ToString();
                                    MessageDistributer.Instance.Distribute(model);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error in {System.Reflection.MethodBase.GetCurrentMethod().Name}: {ex.Message}");
                        }
                    }
                }
                MessageDistributer.Instance.RemoveObserver(this);
                TcpClient.Close();
                TcpClient.Dispose();
                TcpClient = null;
                System.Diagnostics.Debug.WriteLine("TcpClient closed");
            });
        }

        public void Notify(Pattern.Observable sender, object eventArgs)
        {
            if (eventArgs is Models.Message msg)
            {
                var stream = TcpClient.GetStream();
                var jsonData = JsonSerializer.Serialize<Models.Message>(msg);
                var bytes = Encoding.ASCII.GetBytes(jsonData);

                stream.Write(bytes);
                stream.Flush();
            }
        }
    }
}
