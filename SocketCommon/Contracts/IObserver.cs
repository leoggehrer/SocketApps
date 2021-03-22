using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocketCommon.Contracts
{
    public partial interface IObserver
    {
        void Notify(object sender, EventArgs eventArgs);
    }
}
