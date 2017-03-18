using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics
{
    interface IAdjustable
    {
        // Its like Texture.Bounds, but more real
        Rectangle BoundingBox { get; }
    }
}
