using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics
{
    public class Sprite : DrawableObject, IDrawable
    {
        #region Properties

        public virtual Texture2D Texture { get; set; }
        public virtual Rectangle RectangleSource { get; set; }

        #endregion

        #region IAdjustable

        // IAdjustable.BoundBox
        public override Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, RectangleSource.Width, RectangleSource.Height);
            }
        }

        #endregion

        public Sprite(Texture2D texture, SpriteBatch spriteBatch) : base(spriteBatch)
        {
            Texture = texture;
            RectangleSource = new Rectangle(0, 0, Texture.Bounds.Width, Texture.Bounds.Height);
        }

        /// <summary>
        /// Draw all things at once!
        /// </summary>
        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Draw(texture: Texture, position: Position, sourceRectangle: RectangleSource, color: Color * Alpha, rotation: Rotation, scale: Scale, effects: Effect, layerDepth: 1.0F);
        }
    }
}
