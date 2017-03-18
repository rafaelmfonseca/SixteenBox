using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Screens
{
    public interface IMouseHandler
    {
        void MouseLeftPressed(MouseState state);
        void MouseLeftReleased(MouseState state);
        void MouseRightPressed(MouseState state);
        void MouseRightReleased(MouseState state);
    }
}
