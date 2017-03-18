using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics
{
    public interface IWritable
    {
        void KeysPressed(Keys[] pressedKeys);
    }
}
