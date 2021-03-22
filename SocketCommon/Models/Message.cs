using System;

namespace SocketCommon.Models
{
    [Serializable]
    public class Message
    {
        public string Command { get; set; }
        public string From { get; set; }
        public string Body { get; set; }
    }
}
