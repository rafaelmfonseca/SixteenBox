using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SixteenBox.Graphics
{
    /// <summary>
    /// This class only works with ONE drawed texture. More than once will be buggy.
    /// I will make another class for multi clickable textures.
    /// </summary>
    public class ClickableSprite : Sprite, IClickable
    {
        public bool Pressed { get; set; }

        public ClickableSprite(Texture2D texture, SpriteBatch spriteBatch) : base(texture, spriteBatch)
        {
            Pressed = false;
        }

        /// <summary>
        /// Called when ANY mouse button is pressed, this method don't mind which button is.
        /// </summary>
        public virtual void ButtonPressed(MouseState state)
        {
            // You have to cut this because mousestates gets the real position(not with zoom)
            Vector2 mousePosition = new Vector2(state.Position.X / Settings.GameZoom, state.Position.Y / Settings.GameZoom);

            Pressed = BoundingBox.Contains(mousePosition);
        }

        public virtual void ButtonReleased(MouseState state)
        {
            Pressed = false;
        }

    }
}
