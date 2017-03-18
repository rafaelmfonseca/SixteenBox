using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Graphics.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics.Styles
{
    public class ButtonStyle
    {
        #region Properties

        public RecSourceCollection RecSources { get; set; } // This must be three: Normal, Hover and Desactivated
        public Texture2D Texture { get; set; }

        #endregion

        public ButtonStyle()
        {
            RecSources = new RecSourceCollection();
        }
    }
}
