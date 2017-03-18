using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SixteenBox.Graphics;
using SixteenBox.Graphics.Helpers;
using SixteenBox.Graphics.Styles;
using SixteenBox.Transaction;
using SixteenBox.Transaction.Sockets;
using System;
using System.Collections.Generic;

namespace SixteenBox
{
    public static class Settings
    {
        #region Properties

        // The graphics device shared with all files
        public static GraphicsDeviceManager Graphics { get; set; }

        // Window definitions
        public static int GameWindowWidth { get; set; }
        public static int GameWindowHeight { get; set; }
        public static float GameZoom { get; set; }
        public static Vector2 GameWindowInitialSize { get; set; }
        public static Vector2 GameWindowRealSize { get; set; }
        public static int TileSize { get; set; }
        public static int TilesHorizontally { get; set; }
        public static int TilesVertically { get; set; }

        // Input text allowed keys
        public static Keys[] AllowedKeysInput { get; set; }
        public static Keys[] UpperKeysInput { get; set; }
        public static Keys[] NumbersKeysInput { get; set; }

        // Server
        public static string ServerWebUri { get; set; }
        public static User UserOnline { get; set; }

        // Web server definitions
        public static ServerWebUri[] Paths { get; set; }

        // An sprite represeting the entire screen
        public static Sprite GameWindowInitialSprite { get; set; }
        public static Sprite GameWindowSprite { get; set; }

        // Socket Server
        public static SocketProvider Socket;
        public static string ServerSocketUri { get; set; }
        public static string[] SocketSeparators { get; set; }

        #endregion

        #region MainStyles

        public static ButtonStyle LoginButton = new ButtonStyle();
        public static InputStyle LoginInput = new InputStyle();

        #endregion

        /// <summary>
        /// Initialize all settings safety.
        /// </summary>
        static Settings()
        {
            GameWindowWidth = 1280;
            GameWindowHeight = 800;
            GameZoom = 2.0F;
            TileSize = 16;
            GameWindowWidth = (int)(Math.Ceiling(((double)GameWindowWidth / TileSize)) * TileSize);
            GameWindowHeight = (int)(Math.Ceiling(((double)GameWindowHeight / TileSize)) * TileSize);
            GameWindowRealSize = new Vector2(GameWindowWidth / GameZoom, GameWindowHeight / GameZoom);
            TilesHorizontally = (int)GameWindowRealSize.X / TileSize;
            TilesVertically = (int)GameWindowRealSize.Y / TileSize;

            // All input text allowed keys(including shift etc...)
            AllowedKeysInput = new Keys[]
            {
                Keys.A, Keys.B, Keys.C, Keys.D, Keys.E, Keys.F, Keys.G, Keys.H, Keys.I, Keys.J, Keys.K, Keys.L,
                Keys.M, Keys.N, Keys.O, Keys.P, Keys.Q, Keys.R, Keys.S, Keys.T, Keys.U, Keys.V, Keys.W, Keys.X,
                Keys.Y, Keys.Z, Keys.OemMinus, Keys.OemPlus, Keys.Back, Keys.Space, Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6,
                Keys.D7, Keys.D8, Keys.D9, Keys.D0
            };

            UpperKeysInput = new Keys[]
            {
                Keys.LeftShift, Keys.RightShift
            };

            NumbersKeysInput = new Keys[]
            {
                Keys.D1, Keys.D2, Keys.D3, Keys.D4, Keys.D5, Keys.D6, Keys.D7, Keys.D8, Keys.D9, Keys.D0
            };

            // Socket server
            Socket = new SocketProvider();

            // Servers url
            ServerWebUri = "http://localhost/sixteenbox/";
            ServerSocketUri = "ws://127.0.0.1:2222/";
            SocketSeparators = new string[] { "[[", "]]" };

            // Server paths
            Paths = new ServerWebUri[]
            {
                new ServerWebUri() { FileName = "security.php", Posts = "u={0}&p={1}" }
            };
        }

        /// <summary>
        /// We have to do this because Primitives.java needs it.
        /// </summary>
        /// <param name="graphics">The main graphics device</param>
        public static void Initialize(GraphicsDeviceManager graphics, ContentManager content)
        {
            Graphics = graphics;

            // We don't need spritebatch here because we don't draw it
            GameWindowInitialSprite = new Sprite(Primitives.Rectangle(GameWindowWidth, GameWindowHeight, Color.Transparent), null);
            GameWindowSprite = new Sprite(Primitives.Rectangle((int)GameWindowRealSize.X, (int)GameWindowRealSize.Y, Color.Transparent), null);

            // Initialize all button styles
            LoginButton.RecSources.Sources = new List<Rectangle>()
            {
                new Rectangle(1, 1, 43, 17),
                new Rectangle(1, 19, 43, 17),
                new Rectangle(1, 37, 43, 17)
            };
            LoginButton.Texture = content.Load<Texture2D>("ui/btn_login");

            // Initialize all input styles
            LoginInput.RecSources.Sources = new List<Rectangle>()
            {
                new Rectangle(1, 1, 88, 19),
                new Rectangle(1, 21, 88, 19)
            };
            LoginInput.Texture = content.Load<Texture2D>("ui/input_login");
            LoginInput.Font = content.Load<SpriteFont>("fonts/goldfish_font");
            LoginInput.FontPosition = new Vector2(5, 5);
            LoginInput.StripTexture = Primitives.Rectangle(1, 11, Color.Gray);
            LoginInput.StripPosition = new Vector2(5, 4);
            LoginInput.TextMaxWidth = 78;
        }
    }
}
