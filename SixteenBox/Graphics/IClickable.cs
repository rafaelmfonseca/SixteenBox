using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics
{
    interface IClickable
    {
        bool Pressed { get; set; }
        void ButtonPressed(MouseState state);
        void ButtonReleased(MouseState state);
    }
}
