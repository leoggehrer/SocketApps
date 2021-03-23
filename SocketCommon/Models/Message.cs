using SocketCommon.Common;
using System;

namespace SocketCommon.Models
{
    [Serializable]
    public class Message
    {
        public string Command { get; set; } = SocketCommand.ClientMessage.ToString();
        public string From { get; set; }
        public string Body { get; set; }
    }
}
