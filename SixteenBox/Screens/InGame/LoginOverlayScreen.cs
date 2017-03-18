using SixteenBox.Graphics.Animations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SixteenBox.Graphics;
using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Graphics.Helpers;
using SixteenBox.Transaction;
using SixteenBox.Threading;

namespace SixteenBox.Screens.InGame
{
    public class LoginOverlayScreen : GameScreen
    {
        #region Fields

        Sprite _overlayLogin, _overlayContent;
        Texture2D _loginConnBg, _loginConnContentBg;
        Transformer<float> _overlayShow, _overlayMove, _overlayHide;
        SpriteFont _connTextFont;
        Font _connText;
        Counter _counter, _safeTime;
        APILogin _api;
        bool _addedLobby;

        #endregion

        #region Properties

        public User User { get; set; }

        #endregion

        public override void LoadContent()
        {
            // API for login to the server
            _api = new APILogin(User);
            // Background
            _overlayLogin = new Sprite((_loginConnBg = Content.Load<Texture2D>("ui/connBg")), SpriteBatch);
            // Font background
            _overlayContent = new Sprite((_loginConnContentBg = Content.Load<Texture2D>("ui/connContentBg")), SpriteBatch);
            // Font for connection text
            _connText = new Font((_connTextFont = Content.Load<SpriteFont>("fonts/goldfish_font")), SpriteBatch);
            _connText.Color = new Color(111, 118, 130);
            _connText.UpperCase = false;
            // Text to calculate bounds
            _connText.Text = "Connecting...";
            // Counters to safe time
            _counter = new Counter() { EndTime = 1 }; // This make sure has updated at least once!
            _safeTime = new Counter() { EndTime = 1500 };
            Adjust();
        }

        public void Adjust()
        {
            Positioner.Init(_overlayContent).CenterTo(Settings.GameWindowSprite, Positioner.CenterType.Both);
            Positioner.Init(_connText).CenterTo(Settings.GameWindowSprite, Positioner.CenterType.Both);
        }

        public override void Update(GameTime gameTime)
        {
            _counter.Update(gameTime);
            if (!_counter.Done) return;

            // Fade all
            _overlayShow = _overlayShow.Transformer(0.0F, 1.0F, 300, gameTime, (t) => {
                _overlayLogin.Alpha = t.CurrentValue;
                _overlayContent.Alpha = t.CurrentValue;
                _connText.Alpha = t.CurrentValue;
            });

            // When fade is complete
            if (_overlayShow.Done)
            {
                bool online = false;

                if (User.State == LoginState.Connecting)
                {
                    // Still conecting
                    return;
                }
                else if(User.State == LoginState.Online)
                {
                    if (!_addedLobby)
                    {
                        Settings.UserOnline = User;
                        ScreenManager.SearchScreen<LoginScreen>().IsExiting = true;
                        ScreenManager.AddScreen(new LobbyScreen());
                        ScreenManager.AddScreen(new LobbyChatScreen());
                        ScreenManager.AddScreen(new LobbyBackScreen());
                    }
                    online = true;
                    _addedLobby = true;
                }

                // Safe time
                _safeTime.Update(gameTime);

                if (_safeTime.Done)
                {
                    // Hide all content
                    _overlayHide = _overlayHide.Transformer(1.0F, 0.0F, 300, gameTime, (t) =>
                    {
                        _overlayLogin.Alpha = t.CurrentValue;
                        _overlayContent.Alpha = t.CurrentValue;
                        _connText.Alpha = t.CurrentValue;
                    });

                    // Everything is hidden?
                    if (_overlayHide.Done)
                    {
                        // If use ris online
                        if (online)
                        {
                            NotifierScreen.Notifications.Enqueue(new NotifierScreen.Notification() { Text = "Logged.", Type = NotificationType.Success });
                        }
                        else
                        {
                            NotifierScreen.Notifications.Enqueue(new NotifierScreen.Notification() { Text = "Failed to make\nconnection.", Type = NotificationType.Alert });
                        }
                        IsExiting = true;
                    }
                }
            }

            _overlayMove = _overlayMove.Transformer(0F, 16F, 4000, gameTime, (t) => { });

            // We have to put after transformer fo prevent null exception
            if (_overlayMove.Done)
            {
                _overlayMove = null; // Restart the animation
                return;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (!_counter.Done) return;
            base.Draw(gameTime);
            LoginAnimation();
            _overlayContent.Draw(gameTime);
            _connText.Draw(gameTime);
            SpriteBatch.End();
        }

        public override void UnloadContent()
        {
            _loginConnBg.Dispose();
            _loginConnContentBg.Dispose();
        }

        public void LoginAnimation()
        {
            for (int x = 0; x < Settings.TilesHorizontally + 1; x++) // Plus one for left size
            {
                for (int y = 0; y < Settings.TilesVertically; y++)
                {
                    _overlayLogin.Position = new Vector2(x * Settings.TileSize - Settings.TileSize, y * Settings.TileSize);

                    if (_overlayMove != null) // The animation is running
                    {
                        _overlayLogin.Position += new Vector2(_overlayMove.CurrentValue, 0);
                    }

                    _overlayLogin.Draw(new GameTime());
                }
            }
        }
    }
}
