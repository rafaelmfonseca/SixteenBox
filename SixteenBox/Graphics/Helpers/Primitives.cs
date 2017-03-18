using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics.Helpers
{
    public static class Primitives
    {
        /// <summary>
        /// Draw a simple rectangle.
        /// </summary>
        /// <param name="width">Rectangle width</param>
        /// <param name="height">Rectangle height</param>
        /// <param name="bgColor">Background Color</param>
        /// <returns></returns>
        public static Texture2D Rectangle(int width, int height, Color bgColor)
        {
            Texture2D rect = new Texture2D(Settings.Graphics.GraphicsDevice, width, height);
            Color[] data = new Color[width * height];
            for (int i = 0; i < data.Length; ++i)
            {
                data[i] = bgColor;
            }
            rect.SetData(data);
            return rect;
        }
    }
}
