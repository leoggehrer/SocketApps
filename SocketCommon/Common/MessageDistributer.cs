using System;

namespace SocketCommon.Common
{
    public class MessageDistributer : Pattern.Observable
    {
        private static MessageDistributer instance = null;
        public static MessageDistributer Instance
        {
            get
            {
                if (instance == null)
                    instance = new MessageDistributer();

                return instance;
            }
        }
        private MessageDistributer()
        {

        }

        public void Distribute(Models.Message message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            NotifyAll(message);
        }
    }
}
