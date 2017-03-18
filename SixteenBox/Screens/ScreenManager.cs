using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SixteenBox.Transaction.Sockets;
using SixteenBox.Transaction.Sockets.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Screens
{

    public class ScreenManager : DrawableGameComponent
    {
        /// <summary>
        /// A class to represents a socket data
        /// </summary>
        public class SocketData
        {
            public IModel Data { get; set; }
            public bool IsDeleting { get; set; }
        }

        #region Fields

        List<GameScreen> _screensToUpdate;
        List<GameScreen> _screensToDraw;

        MouseState _oldMouseState;

        ConnStatus? _statusBefore;

        #endregion

        #region Properties

        public ContentManager Content { get; set; }

        public SpriteBatch SpriteBatch { get; set; }

        public List<GameScreen> Screens { get; set; }

        public Camera2D Camera { get; set; }

        public static List<SocketData> ReceiveData { get; set; }

        #endregion

        #region Initialization

        static ScreenManager()
        {
            ReceiveData = new List<SocketData>();
        }

        public ScreenManager(Game game) : base(game)
        {
            Content = new ContentManager(game.Services, "Content");
            Screens = new List<GameScreen>();
            _statusBefore = null;
        }

        /// <summary>
        /// Load all contents from screens
        /// </summary>
        protected override void LoadContent()
        {
            SpriteBatch = new SpriteBatch(GraphicsDevice);

            foreach (GameScreen screen in Screens)
            {
                screen.LoadContent();
            }
        }

        /// <summary>
        /// Unload all contents from screens
        /// </summary>
        protected override void UnloadContent()
        {
            Content.Unload();

            foreach (GameScreen screen in Screens)
            {
                screen.UnloadContent();
            }
        }


        #endregion

        #region Update and Draw

        /// <summary>
        /// Update all screens and handle handlers
        /// </summary>
        public override void Update(GameTime gameTime)
        {
            _screensToUpdate = new List<GameScreen>(Screens);

            while (_screensToUpdate.Count > 0)
            {
                GameScreen currentScreen = _screensToUpdate[_screensToUpdate.Count - 1];

                _screensToUpdate.RemoveAt(_screensToUpdate.Count - 1);

                // Verifies if we have to delete the screen 
                if (currentScreen.IsExiting == true)
                {
                    Screens.Remove(currentScreen);
                    continue;
                }

                // handles mouse input for the screen
                if (currentScreen is IMouseHandler)
                {
                    IMouseHandler handlerScreen = (IMouseHandler)currentScreen;
                    MouseState currentMouseState = Mouse.GetState();

                    if (currentMouseState.LeftButton == ButtonState.Pressed && _oldMouseState.LeftButton == ButtonState.Released)
                        handlerScreen.MouseLeftPressed(currentMouseState);

                    if (currentMouseState.LeftButton == ButtonState.Released && _oldMouseState.LeftButton == ButtonState.Pressed)
                        handlerScreen.MouseLeftReleased(currentMouseState);

                    if (currentMouseState.RightButton == ButtonState.Pressed && _oldMouseState.RightButton == ButtonState.Released)
                        handlerScreen.MouseRightPressed(currentMouseState);

                    if (currentMouseState.RightButton == ButtonState.Released && _oldMouseState.RightButton == ButtonState.Pressed)
                        handlerScreen.MouseRightReleased(currentMouseState);

                    _oldMouseState = currentMouseState;
                }

                // handles keyboard input for the screen
                if (currentScreen is IKeyboardHandler)
                {
                    IKeyboardHandler handlerScreen = (IKeyboardHandler)currentScreen;
                    KeyboardState currentKeyboardState = Keyboard.GetState();

                    Keys[] pressedKeys = currentKeyboardState.GetPressedKeys();

                    handlerScreen.KeysPressed(pressedKeys);
                }

                // handles socket connection for the screen
                if(currentScreen is ISocketHandler)
                {
                    ISocketHandler handlerScreen = (ISocketHandler)currentScreen;
                    var status = currentScreen.Socket.Connection.Status;

                    // check for no status
                    if (status != null)
                    {
                        if (status == ConnStatus.Disconnected)
                            handlerScreen.OnDisconnect(_statusBefore, gameTime);

                        if (status == ConnStatus.Connecting)
                            handlerScreen.OnConnecting(_statusBefore, gameTime);

                        if (status == ConnStatus.Connected)
                            handlerScreen.OnConnected(_statusBefore, gameTime);

                        _statusBefore = status;
                    }
                }

                // handles socket datas
                if(currentScreen is ISocketDataHandler)
                {
                    ISocketDataHandler handlerScreen = (ISocketDataHandler)currentScreen;
                    handlerScreen.Receive(ReceiveData);
                    // Select just data that can't be deleted
                    ReceiveData = new List<SocketData>(from d in ReceiveData
                                                       where d.IsDeleting == false
                                                       select d);
                }

                currentScreen.Update(gameTime);
            }
        }

        /// <summary>
        /// Draw all screens
        /// </summary>
        public override void Draw(GameTime gameTime)
        {
            _screensToDraw = new List<GameScreen>(from s in Screens
                                                  orderby !s.IsPopUp
                                                  select s);

            while (_screensToDraw.Count > 0)
            {
                GameScreen currentScreen = _screensToDraw[_screensToDraw.Count - 1];

                _screensToDraw.RemoveAt(_screensToDraw.Count - 1);

                currentScreen.Draw(gameTime);
            }
        }

        #endregion
        
        #region Public Methods

        /// <summary>
        /// Adds a screen to the game
        /// </summary>
        public void AddScreen(GameScreen screen)
        {
            if (screen != null)
            {
                screen.ScreenManager = this;
                screen.LoadContent();
                Screens.Add(screen);
            }
        }

        /// <summary>
        /// Return if a screen is being drawed
        /// </summary>
        public bool HasScreen<T>() where T: GameScreen
        {
            return Screens.OfType<T>().Count() > 0;
        }

        /// <summary>
        /// Search for a screen
        /// </summary>
        public T SearchScreen<T>() where T: GameScreen
        {
            int index = Screens.FindIndex(s => s is T);

            if(index >= 0)
            {
                return (T)Screens[index];
            }
            else
            {
                return null;
            }
        }

        #endregion
    }
}