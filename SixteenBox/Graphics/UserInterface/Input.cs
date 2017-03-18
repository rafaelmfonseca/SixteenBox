using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SixteenBox.Graphics.Styles;
using SixteenBox.Windows.Native;

namespace SixteenBox.Graphics.UserInterface
{
    public enum InputType
    {
        Text,
        Password
    }

    public class Input : Button, IWritable
    {

        protected class KeyBuffer
        {
            public Keys Key { get; set; }
            // Time in milleseconds
            public int Time { get; set; }
        }

        #region Fields

        protected int _stripInterval;
        protected bool _showStrip;
        protected int _stripTime;
        protected int _bufferInterval;
        protected Vector2 _textSize;
        protected string _space = ' '.ToString();
        protected char _passChar = '*';

        // buffer to every ket
        protected List<KeyBuffer> _buffers;

        #endregion

        #region Properties

        public InputStyle Style { get; set; }
        public bool Focused { get; protected set; }
        public Sprite StripSprite { get; set; }
        public Font Font { get; set; }
        public string Text { get; set; }

        // Redo all properties to catch font and strip too
        public override Color Color
        {
            get { return Color.White; } // Return white because the draw method calls it to draw the bg
            set { if (Font == null) return; Font.Color = value; }
        }

        public InputType Type { get; set; }

        #endregion

        public Input(InputStyle style, SpriteBatch spriteBatch) : base(style, spriteBatch)
        {
            Style = style;
            RectangleSource = style.RecSources.Get(0);
            // We have to do this to change focus status and strip
            Activated += (s, e) => { Focused = true; ResetStrip(true); };
            StripSprite = new Sprite(style.StripTexture, SpriteBatch);
            Font = new Font(style.Font, SpriteBatch);
            // We have lowercase here too
            Font.UpperCase = false;
            Color = Color.Black;
            _stripInterval = 500;
            _bufferInterval = 100;
            _buffers = new List<KeyBuffer>();
            Text = string.Empty;
            Type = InputType.Text;
        }

        public override void Update(GameTime gameTime)
        {
            if (Focused)
            {
                RectangleSource = Style.RecSources.Get(1);
            }
            else
            {
                RectangleSource = Style.RecSources.Get(0);
            }

            // Change strip status
            if (Focused)
            {
                if (_stripTime >= _stripInterval)
                {
                    ResetStrip(!_showStrip);
                }
                else
                {
                    _stripTime += gameTime.ElapsedGameTime.Milliseconds;
                }
            }

            // Change every buffer timer
            foreach(KeyBuffer b in _buffers)
            {
                b.Time += gameTime.ElapsedGameTime.Milliseconds;
            }
            
            // Handle if its password
            string passwordText = string.Empty;
            if (Type == InputType.Password)
            {
                _textSize = Font.SpriteFont.MeasureString(new string(_passChar, Text.Length));
                passwordText = new string(_passChar, Text.Length);
            }
            else
            {
                // Recalculate font texture size
                _textSize = Font.SpriteFont.MeasureString(Text);
                passwordText = Text;
            }

            // Change text to show
            Font.Text = passwordText;

            if (_textSize.X > Style.TextMaxWidth)
            {
                // Temporary variables
                int size = 0;
                int startIndex = -1;

                // Find a safety size
                do
                {
                    startIndex++;
                    size = (int)Font.SpriteFont.MeasureString(passwordText.Substring(startIndex)).X;
                }
                while (size > Style.TextMaxWidth);

                Font.Text = passwordText.Substring(startIndex);

                // Recalculate for the strip
                _textSize = Font.SpriteFont.MeasureString(Font.Text);
            }

        }

        public virtual void KeysPressed(Keys[] pressedKeys)
        {
            if (!Focused) return;
            
            if(pressedKeys.Count() > 0)
            {
                // We have to do this to always show the strip when any letter is pressed
                ResetStrip(true);

                // Catch normal keys then transform to array
                Keys[] normalKeys = pressedKeys.Intersect(Settings.AllowedKeysInput).ToArray();

                if(normalKeys.Count() > 0)
                {
                    // We catch just one key
                    Keys key = normalKeys[0];

                    KeyBuffer buffer = TakeOrCreate(key);

                    // Change caps lock status

                    // check if can write key
                    if(buffer.Time > _bufferInterval)
                    {
                        if (key == Keys.Back)
                        {
                            if(!string.IsNullOrEmpty(Font.Text))
                            {
                                Text = Text.Substring(0, Text.Length - 1);
                            }
                        }
                        else
                        {
                            bool isCapsActive = ((ushort)WinInput.GetKeyState(WinInput.Key.CapsLock)) != 0;

                            //Refactor key string
                            string keyString = key.ToString();

                            // Refactor uppercase
                            Keys[] upperKeys = pressedKeys.Intersect(Settings.UpperKeysInput).ToArray();

                            keyString = TransformKey(key, keyString, upperKeys.Length > 0);

                            // Handles shift and caps lock conflict
                            if (isCapsActive && upperKeys.Length <= 0)
                            {
                                keyString = keyString.ToUpper();
                            }
                            else if (!isCapsActive && upperKeys.Length > 0)
                            {
                                keyString = keyString.ToUpper();
                            }
                            else
                            { 
                                keyString = keyString.ToLower();
                            }

                            Text += keyString;
                        }

                        // Reset buffer
                        buffer.Time = 0;
                    }
                }
            }
        }

        /// <summary>
        /// Transform key string 
        /// </summary>
        private string TransformKey(Keys key, string keyString, bool shiftPressed)
        {
            // Check space and others
            switch (key)
            {
                case Keys.Space:
                    return this._space;
                // Adds more later
                case Keys.OemMinus:
                    return (shiftPressed) ? '_'.ToString() : '-'.ToString();
                case Keys.OemPlus:
                    return (shiftPressed) ? '+'.ToString() : '='.ToString();
            }

            // Check numbers
            keyString = (Settings.NumbersKeysInput.Any(k => k == key)) ? keyString[1].ToString() : keyString;

            return keyString;
        }

        /// <summary>
        /// Verifies if the key has buffer or not.
        /// Similar to the fade TakerOrCreate method
        /// </summary>
        /// <param name="key">The key to buffer.</param>
        /// <returns>A KeyBuffer if the key doesn't have.</returns>
        private KeyBuffer TakeOrCreate(Keys key)
        {
            int has = _buffers.FindIndex(i => i.Key == key);

            if (has >= 0)
            {
                return _buffers[has];
            }
            else
            {
                var buffer = new KeyBuffer() { Key = key, Time = _bufferInterval };
                _buffers.Add(buffer);
                return buffer;
            }
        }

        public override void ButtonPressed(MouseState state)
        {
            // We have to call base buttonpressed for the pressed propertie
            base.ButtonPressed(state);

            if(!Pressed)
            {
                Focused = false;
                // Disable strip too
                ResetStrip(false);
            }
        }

        public override void Draw(GameTime gameTime)
        {
            // We have to call base draw for the background
            base.Draw(gameTime);
            
            // Unfortunaly, we have to change strip position every draw in case input position is changed
            if (_showStrip)
            {
                StripSprite.Position = Position + Style.StripPosition + new Vector2(_textSize.X, 0);
                StripSprite.Draw(gameTime);
            }

            // And this too
            Font.Position = Position + Style.FontPosition;
            Font.Draw(gameTime);
        }

        protected virtual void ResetStrip(bool show, int time = 0)
        {
            _showStrip = show;
            _stripTime = 0;
        }
    }
}
