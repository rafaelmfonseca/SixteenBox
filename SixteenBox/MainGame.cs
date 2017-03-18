using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SixteenBox.Screens;
using SixteenBox.Screens.InGame;

namespace SixteenBox
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class MainGame : Game
    {
        GraphicsDeviceManager graphics;
        ScreenManager screenManager;

        public MainGame()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = Settings.GameWindowWidth;
            graphics.PreferredBackBufferHeight = Settings.GameWindowHeight;
            graphics.ApplyChanges();

            this.Window.Title = "16Box";
            this.IsMouseVisible = true;

            Content.RootDirectory = "Content";
            screenManager = new ScreenManager(this);
            // you can't create camera here because graphicsdevice its initialized on the Run() method, look at Program.cs
            Components.Add(screenManager);
        }

        protected override void Initialize()
        {
            base.Initialize();
            Settings.Initialize(graphics, Content);
            screenManager.Camera = new Camera2D(graphics.GraphicsDevice.Viewport);
            screenManager.Camera.Zoom = Settings.GameZoom;
            screenManager.Camera.Origin = Vector2.Zero; // this fixes camera starting at middle of screen
            screenManager.AddScreen(new LoginScreen());
            screenManager.AddScreen(new NotifierScreen() { IsPopUp = true });
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightGray);
            base.Draw(gameTime);
        }
    }
}
