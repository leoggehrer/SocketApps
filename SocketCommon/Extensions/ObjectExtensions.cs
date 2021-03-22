using System;

namespace SocketCommon.Extensions
{
    public static partial class ObjectExtensions
    {
        public static void CheckArgument(this object source, string argName)
        {
            if (source == null)
                throw new ArgumentNullException(argName);
        }
        public static void CheckNotNull(this object source, string itemName)
        {
            if (source == null)
                throw new ArgumentNullException(itemName);
        }
    }
}
//MdEnd
