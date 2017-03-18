using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SixteenBox.Graphics;
using SixteenBox.Graphics.Animations;
using SixteenBox.Graphics.Helpers;
using SixteenBox.Graphics.Styles;
using SixteenBox.Graphics.UserInterface;
using SixteenBox.Threading;
using SixteenBox.Transaction.Sockets;
using SixteenBox.Transaction.Sockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using static SixteenBox.Screens.ScreenManager;

namespace SixteenBox.Screens.InGame
{
    /// <summary>
    /// The lobby chat screen, without zoom to font become smaller
    /// </summary>
    public class LobbyChatScreen : GameScreen, IMouseHandler, IKeyboardHandler, ISocketHandler, ISocketDataHandler
    {
        #region Fields

        Input _inputChat;
        float zoom = 1.0F;
        Font _chatLinesFont;
        bool _enterKeyBefore, doAnimation;
        Vector2 _initialLinePos;
        Transformer<float> _lastMessage;
        Counter _counterConnecting;

        #endregion

        #region Properties

        public static List<ChatMessage> ChatLines;

        #endregion

        public LobbyChatScreen()
        {
            if (ChatLines != null)
            {
                ChatLines.Clear();
            }
        }

        public override void LoadContent()
        {
            ChatLines = new List<ChatMessage>();

            WithZoom(zoom, (b) =>
            {
                // Chat input style
                InputStyle style = new InputStyle();
                style.Texture = Primitives.Rectangle(1, 1, Color.Transparent);
                style.Font = Content.Load<SpriteFont>("fonts/goldfish_font");
                style.TextMaxWidth = Settings.GameWindowWidth - (int)(7 * b + 4 * b + 7 * b);
                style.RecSources = new RecSourceCollection();
                style.RecSources.Sources = new List<Rectangle>() {
                    new Rectangle(0, 0, style.TextMaxWidth, (int)(13 * b)),
                    new Rectangle(0, 0, style.TextMaxWidth, (int)(13 * b))
                };
                style.FontPosition = new Vector2(4, 4 * b);
                style.StripPosition = new Vector2(4, 4 * b);
                style.StripTexture = Primitives.Rectangle(3, 9, Color.White);
                // Char input
                _inputChat = new Input(style, SpriteBatch);
                _inputChat.Color = Color.White;
                // Chat font
                _chatLinesFont = new Font(style.Font, SpriteBatch);
                _chatLinesFont.UpperCase = false;
                // Open socket connection
                Settings.Socket.Connection.Open();
                // Add initial message
                ChatLines.Add(new ChatMessage() { Text = "Attempting to join lobby chat...", Color = Color.IndianRed });
                // Initial message to get bounding
                _chatLinesFont.Text = ChatLines[0].Text;
                // Counter to safety time
                _counterConnecting = new Counter() { EndTime = 2000 };
                _counterConnecting.Finished += (s, e) => ChatLines.Insert(0, new ChatMessage() { Text = "The game is trying to connect to the server has been 5 seconds...", Color = Color.IndianRed });
                Adjust(b);
            });
        }

        // Here is being draw with 1.0F of zoom already
        public void Adjust(float zoomBefore)
        {
            Positioner.Init(_inputChat)
                      .Margin(Settings.GameWindowInitialSprite, new Vector2(7 * zoomBefore, 6 * zoomBefore), Positioner.MarginFrom.BottomLeft);
            Positioner.Init(_chatLinesFont)
                      .SideBy(_inputChat, Positioner.SideFrom.Top)
                      .Margin(_chatLinesFont, new Vector2(4 * zoomBefore, 4 * zoomBefore), Positioner.MarginFrom.BottomLeft);
            // Get position to recalculate every frame
            _initialLinePos = _chatLinesFont.Position;
        }

        public override void Update(GameTime gameTime)
        {
            _inputChat.Update(gameTime);
        }

        public void OnConnecting(ConnStatus? statusBefore, GameTime gameTime)
        {
            if (statusBefore == ConnStatus.Connecting)
            {
                _counterConnecting.Update(gameTime);
            }
        }

        public void OnConnected(ConnStatus? statusBefore, GameTime gameTime)
        {
            // Was connecting but went connected
            if (statusBefore == ConnStatus.Connecting)
            {
                ChatLines.Insert(0, new ChatMessage() { Text = "Joined!", Color = Color.ForestGreen });
            }
            // The user is connected
            if (statusBefore == ConnStatus.Connected)
            {

            }
        }

        public void OnDisconnect(ConnStatus? statusBefore, GameTime gameTime)
        {
            // Was disconnected but went connecting
            if (statusBefore == ConnStatus.Connecting)
            {
                ChatLines.Insert(0, new ChatMessage() { Text = "Failed to join lobby chat...", Color = Color.IndianRed });
            }

            // Was disconnected but went connected
            if (statusBefore == ConnStatus.Connected)
            {
                ChatLines.Insert(0, new ChatMessage() { Text = "You have been disconnected from server...", Color = Color.IndianRed });
            }
        }

        public void Receive(IEnumerable<SocketData> received)
        {
            // Go to each data
            foreach (var socketData in received)
            {
                ChatMessage message;
                // Check data is message
                if((message = socketData.Data as ChatMessage) != null)
                {
                    ChatLines.Insert(0, message);
                    socketData.IsDeleting = true;
                }
            }
        }

        public void MouseLeftPressed(MouseState state)
        {
            WithZoom(zoom, (b) =>
            {
                _inputChat.ButtonPressed(state);
            });
        }

        public void MouseLeftReleased(MouseState state)
        {
            WithZoom(zoom, (b) =>
            {
                _inputChat.ButtonReleased(state);
            });
        }

        public void MouseRightPressed(MouseState state)
        {

        }

        public void MouseRightReleased(MouseState state)
        {

        }

        public void KeysPressed(Keys[] pressedKeys)
        {
            _inputChat.KeysPressed(pressedKeys);
            // Check any key is enter
            bool enterPressed = pressedKeys.Any(k => k == Keys.Enter);
            // Check for empty input and enter key pressed
            if (enterPressed && !_enterKeyBefore && !string.IsNullOrWhiteSpace(_inputChat.Text))
            {
                ChatMessage message = new ChatMessage() { Text = _inputChat.Text };
                // Send to the server
                Socket.SendObject(message);
                // Reset to the next message
                _inputChat.Text = string.Empty;
                doAnimation = true;
            }
            _enterKeyBefore = enterPressed;
        }

        public override void Draw(GameTime gameTime)
        {
            WithZoom(zoom, (b) =>
            {
                base.Draw(gameTime);
                _inputChat.Draw(gameTime);
                // Stop drawing
                if(ChatLines.Count <= 0)
                {
                    SpriteBatch.End();
                    return;
                }
                int count = 0;
                _chatLinesFont.Position = _initialLinePos;

                // Go to every line
                foreach (ChatMessage line in ChatLines)
                {
                    _chatLinesFont.Text = line.Text;
                    _chatLinesFont.Color = line.Color;

                    if (count > 0) // Passed the first
                    {
                        _chatLinesFont.Alpha = 1.0F;
                        _chatLinesFont.Position = Positioner.GetSideBy(_chatLinesFont.BoundingBox, _chatLinesFont.BoundingBox, Positioner.SideFrom.Top);
                    }
                    else if (count == 0) // The last, the most recent message
                    {
                        // Checks do animation again
                        if(doAnimation)
                        {
                            _lastMessage = _lastMessage.Transformer(0.0F, 1.0F, 250, gameTime, (t) => _chatLinesFont.Alpha = t.CurrentValue);

                            // If last message animation is done, reset and restart
                            if (_lastMessage.Done)
                            {
                                _lastMessage = null;
                                doAnimation = false;
                            }
                        }
                    }
                    _chatLinesFont.Draw(gameTime);
                    count++;
                }
                SpriteBatch.End();
            });
        }

        public override void UnloadContent()
        {
             _inputChat.Style.Texture.Dispose();
        }

        // Fix ButtonPressed and others
        public void WithZoom(float zoom, Action<float> act)
        {
            float zoomBefore = Settings.GameZoom;
            Settings.GameZoom = zoom;
            ScreenManager.Camera.Zoom = Settings.GameZoom;
            act(zoomBefore);
            Settings.GameZoom = zoomBefore;
            ScreenManager.Camera.Zoom = Settings.GameZoom;
        }
    }
}
