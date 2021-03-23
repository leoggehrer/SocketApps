using SocketCommon.Common;
using SocketCommon.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace SocketCommon.Client
{
    public class TcpClientListener
    {
        public bool IsRunning { get; private set; }
        private TcpClient TcpClient { get; set; }
        public TcpClientListener(TcpClient tcpClient)
        {
            TcpClient = tcpClient ?? throw new ArgumentNullException(nameof(tcpClient));
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
                                if (model.Command.GetValueOrDefault().Equals(SocketCommand.Disconnected.ToString()))
                                {
                                    IsRunning = false;
                                }
                                else
                                {
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
                TcpClient.Close();
                TcpClient.Dispose();
                TcpClient = null;
                System.Diagnostics.Debug.WriteLine("TcpClient closed");
            });
        }
        public void Write(Models.Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            if (TcpClient != null)
            {
                var json = JsonSerializer.Serialize<Models.Message>(message);
                var bytes = Encoding.ASCII.GetBytes(json);
                var stream = TcpClient.GetStream();

                stream.Write(bytes);
                stream.Flush();
            }
        }
    }
}
