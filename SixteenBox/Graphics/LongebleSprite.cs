using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Graphics.Helpers;

namespace SixteenBox.Graphics
{
    public class LongebleSprite : Sprite
    {
        #region Fields
        
        protected Sprite _sprite;

        #endregion

        #region Properties

        public int SpriteWidth { get; set; }
        public RecSourceCollection RecSources { get; set; }

        #endregion

        #region IAdjustable

        // IAdjustable.BoundBox
        public override Rectangle BoundingBox
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, SpriteWidth, RecSources.Get(0).Height);
            }
        }

        #endregion

        public LongebleSprite(RecSourceCollection recs, Texture2D texture, SpriteBatch spriteBatch) : base(texture, spriteBatch)
        {
            RecSources = recs;
            _sprite = new Sprite(texture, SpriteBatch);
        }

        public override void Draw(GameTime gameTime)
        {
            if (SpriteWidth < 0) throw new Exception("Set a size for the longeble before drawing.");

            // Left part
            _sprite.Position = Position;
            _sprite.RectangleSource = RecSources.Get(0);
            _sprite.Draw(gameTime);
            // Middle part
            int countMiddle = 0;
            Vector2 middlePos = new Vector2(Position.X + _sprite.RectangleSource.Width, Position.Y);
            do
            {
                _sprite.RectangleSource = RecSources.Get(1);
                _sprite.Position = middlePos;
                _sprite.Draw(gameTime);
                middlePos += new Vector2(_sprite.RectangleSource.Width, 0);
                countMiddle++;
            } while (RecSources.Get(0).Width + (countMiddle * _sprite.RectangleSource.Width) + RecSources.Get(2).Width < SpriteWidth);
            // Right part
            _sprite.RectangleSource = RecSources.Get(2);
            _sprite.Position = new Vector2(Position.X + (SpriteWidth - _sprite.RectangleSource.Width), Position.Y);
            _sprite.Draw(gameTime);
        }
    }
}
