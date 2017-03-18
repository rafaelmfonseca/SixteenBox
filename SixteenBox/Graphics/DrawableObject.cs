using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Graphics
{
    public abstract class DrawableObject : IAdjustable
    {
        public virtual SpriteBatch SpriteBatch { get; set; }
        public virtual Vector2 Position { get; set; }
        public virtual float Rotation { get; set; }
        public virtual Vector2 Scale { get; set; }
        public virtual Color Color { get; set; }
        public virtual float Alpha { get; set; }
        public virtual SpriteEffects Effect { get; set; }

        // IAdjustable.BoundBox
        public virtual Rectangle BoundingBox { get; }

        public DrawableObject(SpriteBatch spriteBatch)
        {
            SpriteBatch = spriteBatch;
            Position = Vector2.Zero;
            Rotation = 0.0F;
            Scale = new Vector2(1.0F, 1.0F);
            Color = Color.White;
            Alpha = 1.0F;
            Effect = SpriteEffects.None;
            BoundingBox = Rectangle.Empty;
        }
    }
}
