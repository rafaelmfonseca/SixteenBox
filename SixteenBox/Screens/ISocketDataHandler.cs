using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SixteenBox.Screens.ScreenManager;

namespace SixteenBox.Screens
{
    public interface ISocketDataHandler
    {
        void Receive(IEnumerable<SocketData> received);
    }
}
