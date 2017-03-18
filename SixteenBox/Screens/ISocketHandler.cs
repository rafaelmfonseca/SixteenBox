using Microsoft.Xna.Framework;
using SixteenBox.Transaction.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Screens
{
    public interface ISocketHandler
    {
        void OnConnecting(ConnStatus? statusBefore, GameTime gameTime);
        void OnDisconnect(ConnStatus? statusBefore, GameTime gameTime);
        void OnConnected(ConnStatus? statusBefore, GameTime gameTime);
    }
}
