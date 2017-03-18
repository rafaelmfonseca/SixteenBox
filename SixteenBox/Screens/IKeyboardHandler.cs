using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Screens
{
    public interface IKeyboardHandler
    {
        void KeysPressed(Keys[] pressedKeys);
    }
}
