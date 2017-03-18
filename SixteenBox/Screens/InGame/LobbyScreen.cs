using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SixteenBox.Graphics;
using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Graphics.Helpers;

namespace SixteenBox.Screens.InGame
{
    /// <summary>
    /// The lobby screen
    /// </summary>
    public class LobbyScreen : GameScreen
    {
        #region Fields

        RecSourceCollection _lobbyChatRecs;
        LongebleSprite _lobbyChatBg;
        Texture2D _lobbyChatBgTexture;
        Font _lobbyTitle;
        Sprite _lobbyBg1, _lobbyBg2;

        #endregion

        public override void LoadContent()
        {
            // Longable regions
            _lobbyChatRecs = new RecSourceCollection();
            _lobbyChatRecs.Sources = new List<Rectangle>()
            {
                new Rectangle(0, 0, 68, 111), // Left 
                new Rectangle(68, 0, 1, 111), // Middle
                new Rectangle(116, 0, 7, 111) // Right
            };
            // Creates longable
            _lobbyChatBg = new LongebleSprite(_lobbyChatRecs, (_lobbyChatBgTexture = Content.Load<Texture2D>("ui/lobbyChatBg")), SpriteBatch);
            _lobbyChatBg.SpriteWidth = (int)Settings.GameWindowRealSize.X;
            // Font for the chat title
            _lobbyTitle = new Font(Content.Load<SpriteFont>("fonts/goldfish_font"), SpriteBatch);
            _lobbyTitle.Color = new Color(39, 55, 78);
            _lobbyTitle.Text = "Lobby";
            // Split the screen height in two minus chat height without the title
            int height = (int)Math.Ceiling((Settings.GameWindowRealSize.Y - 93) / 2);
            // Create backgrounds
            _lobbyBg1 = new Sprite(Primitives.Rectangle((int)Settings.GameWindowRealSize.X, height, new Color(210, 211, 222)), SpriteBatch);
            _lobbyBg2 = new Sprite(Primitives.Rectangle((int)Settings.GameWindowRealSize.X, height, new Color(193, 194, 209)), SpriteBatch);
            Adjust();
        }

        public void Adjust()
        {
            Positioner.Init(_lobbyChatBg).Margin(Settings.GameWindowSprite, Vector2.Zero, Positioner.MarginFrom.BottomLeft);
            Positioner.Init(_lobbyTitle).Margin(_lobbyChatBg, new Vector2(21, 7), Positioner.MarginFrom.TopLeft);
            Positioner.Init(_lobbyBg2).SideBy(_lobbyBg1, Positioner.SideFrom.Bottom);
        }


        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            _lobbyBg1.Draw(gameTime);
            _lobbyBg2.Draw(gameTime);
            _lobbyChatBg.Draw(gameTime);
            _lobbyTitle.Draw(gameTime);
            SpriteBatch.End();
        }

        public override void UnloadContent()
        {
            _lobbyChatBgTexture.Dispose();
        }
    }
}
