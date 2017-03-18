using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using SixteenBox.Graphics;
using SixteenBox.Graphics.Animations;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SixteenBox.Graphics.UserInterface;
using SixteenBox.Graphics.Helpers;
using SixteenBox.Transaction;

namespace SixteenBox.Screens.InGame
{
    public class LoginScreen : GameScreen, IMouseHandler, IKeyboardHandler
    {
        #region Fields

        Sprite _loginBg, _loginContentBg;
        Texture2D _loginBgTexture, _loginContentBgTexture;
        Button _btnLogin;
        Input _inputName, _inputPassword;

        #endregion

        public override void LoadContent()
        {
            // Background
            _loginBg = new Sprite((_loginBgTexture = Content.Load<Texture2D>("ui/loginBg")), SpriteBatch);
            // Login box
            _loginContentBg = new Sprite((_loginContentBgTexture = Content.Load<Texture2D>("ui/loginContentBg")), SpriteBatch);
            // Button for login
            _btnLogin = new Button(Settings.LoginButton, SpriteBatch);
            _btnLogin.Activated += btnLogin_Activated;
            // Input for name
            _inputName = new Input(Settings.LoginInput, SpriteBatch);
            // Input for password
            _inputPassword = new Input(Settings.LoginInput, SpriteBatch);
            _inputPassword.Type = InputType.Password;
            Adjust();
        }

        private void btnLogin_Activated(object sender, EventArgs e)
        {
            // When button is clicked, show overlay screen
            ScreenManager.AddScreen(new LoginOverlayScreen() { User = new User(_inputName.Text, _inputPassword.Text), IsPopUp = true });
        }

        private void Adjust()
        {
            Positioner.Init(_loginContentBg)
                      .Margin(Settings.GameWindowSprite, new Vector2(0, 10), Positioner.MarginFrom.TopLeft)
                      .CenterTo(Settings.GameWindowSprite, Positioner.CenterType.Horizontally);
            Positioner.Init(_btnLogin)
                      .Margin(_loginContentBg, new Vector2(10, 10), Positioner.MarginFrom.BottomRight);
            Positioner.Init(_inputName)
                      .Margin(_loginContentBg, new Vector2(10, 10), Positioner.MarginFrom.TopLeft);
            Positioner.Init(_inputPassword)
                      .Margin(_inputName, new Vector2(0, _inputName.BoundingBox.Height + 10), Positioner.MarginFrom.TopLeft); 
        }

        public override void Update(GameTime gameTime)
        {
            _btnLogin.Update(gameTime);
            _inputName.Update(gameTime);
            _inputPassword.Update(gameTime);

            // Check for empty inputs for button status
            if (string.IsNullOrWhiteSpace(_inputName.Text) || string.IsNullOrWhiteSpace(_inputPassword.Text))
            {
                _btnLogin.State = UIState.Disabled;
            }
            else
            {
                _btnLogin.State = UIState.Enabled;
            }
        }

        public void MouseLeftPressed(MouseState state)
        {
            // Cancel all button behaviors if its trying to login
            if (ScreenManager.HasScreen<LoginOverlayScreen>()) return;

            _btnLogin.ButtonPressed(state);
            _inputName.ButtonPressed(state);
            _inputPassword.ButtonPressed(state);
        }

        public void MouseLeftReleased(MouseState state)
        {
            _btnLogin.ButtonReleased(state);
            _inputName.ButtonReleased(state);
            _inputPassword.ButtonReleased(state);
        }

        public void MouseRightPressed(MouseState state)
        {
            
        }

        public void MouseRightReleased(MouseState state)
        {
            
        }

        public void KeysPressed(Keys[] pressedKeys)
        {
            _inputName.KeysPressed(pressedKeys);
            _inputPassword.KeysPressed(pressedKeys);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime); // This already calls Begin() with right parameters
            FullBg(_loginBg);
            _loginContentBg.Draw(gameTime);
            _btnLogin.Draw(gameTime);
            _inputName.Draw(gameTime);
            _inputPassword.Draw(gameTime);
            SpriteBatch.End();
        }

        public override void UnloadContent()
        {
            _loginBgTexture.Dispose();
            _loginContentBgTexture.Dispose();
            // You can't dispose button texture because it is on the settings class
        }

        public void FullBg(Sprite drawable)
        {
            for (int x = 0; x < Settings.TilesHorizontally; x++)
            {
                for(int y = 0; y < Settings.TilesVertically; y++)
                {
                    drawable.Position = new Vector2(x * Settings.TileSize, y * Settings.TileSize);
                    drawable.Draw(new GameTime());
                }
            }
        }
    }
}
