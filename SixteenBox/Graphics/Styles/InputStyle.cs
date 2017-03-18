using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Graphics.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics.Styles
{
    public class InputStyle : ButtonStyle
    {
        #region Properties

        public SpriteFont Font { get; set; }
        public Vector2 FontPosition { get; set; }
        public Texture2D StripTexture { get; set; }
        public Vector2 StripPosition { get; set; }
        public int TextMaxWidth { get; set; }

        #endregion
    }
}
