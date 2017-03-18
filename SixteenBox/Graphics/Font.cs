using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SixteenBox.Graphics
{
    public class Font : DrawableObject, IDrawable
    {
        #region Properties

        public virtual SpriteFont SpriteFont { get; set; }
        public virtual string Text { get; set; }
        public bool UpperCase { get; set; }

        #endregion

        #region IAdjustable

        // IAdjustable.BoundBox
        public override Rectangle BoundingBox
        {
            get
            {
                Vector2 size = SpriteFont.MeasureString(Text);
                return new Rectangle((int)Position.X, (int)Position.Y, (int)size.X, (int)size.Y);
            }
        }

        #endregion

        public Font(SpriteFont font, SpriteBatch spriteBatch) : base(spriteBatch)
        {
            SpriteFont = font;
            Text = String.Empty;
            UpperCase = true;
        }

        // Draw all at once too!
        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.DrawString(SpriteFont, UpperCase ? Text.ToUpper() : Text, Position, Color * Alpha, Rotation, Vector2.Zero, Scale, Effect, 1.0F);
        }

    }
}
