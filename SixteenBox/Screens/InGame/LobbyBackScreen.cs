using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Graphics;
using SixteenBox.Graphics.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Screens.InGame
{
    /// <summary>
    /// The lobby chat screen back
    /// </summary>
    public class LobbyBackScreen : GameScreen
    {
        #region Fields

        RecSourceCollection _lobbyChatRecs;
        LongebleSprite _lobbyChatBg;
        Texture2D _lobbyChatBgTexture;

        #endregion

        public override void LoadContent()
        {
            // Longeble chat background 
            _lobbyChatRecs = new RecSourceCollection();
            _lobbyChatRecs.Sources = new List<Rectangle>()
            {
                new Rectangle(125, 25, 4, 80), // Left 
                new Rectangle(129, 25, 1, 80), // Middle
                new Rectangle(129, 25, 1, 80) // Right
            };
            // Create longeble
            _lobbyChatBg = new LongebleSprite(_lobbyChatRecs, (_lobbyChatBgTexture = Content.Load<Texture2D>("ui/lobbyChatBg")), SpriteBatch);
            // Longeble size, game width minus chat paddings
            _lobbyChatBg.SpriteWidth = (int)Settings.GameWindowRealSize.X - 14;
            Adjust();
        }

        public void Adjust()
        {
            Positioner.Init(_lobbyChatBg).Margin(Settings.GameWindowSprite, new Vector2(7, 6), Positioner.MarginFrom.BottomLeft);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _lobbyChatBg.Draw(gameTime);
            SpriteBatch.End();
        }

        public override void UnloadContent()
        {
            _lobbyChatBgTexture.Dispose();
        }
    }

}
