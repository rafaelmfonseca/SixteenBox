using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SixteenBox.Graphics.Styles;

namespace SixteenBox.Graphics.UserInterface
{
    public enum UIState
    {
        Enabled,
        Disabled
    }

    public delegate void ActivatedEventHandler(object sender, EventArgs e);

    public class Button : ClickableSprite, IUpdatable
    {

        #region Properties

        private ButtonStyle Style { get; set; }
        public UIState State { get; set; }
        public event ActivatedEventHandler Activated;

        #endregion

        public Button(ButtonStyle style, SpriteBatch spriteBatch) : base(style.Texture, spriteBatch)
        {
            Style = style;
            State = UIState.Enabled;
            RectangleSource = style.RecSources.Get(0);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (State == UIState.Disabled)
            {
                RectangleSource = Style.RecSources.Get(2);
                // If its disabled, we can't change texture
                return;
            }

            if (Pressed)
            {
                RectangleSource = Style.RecSources.Get(1);
            }
            else
            {
                RectangleSource = Style.RecSources.Get(0);
            }
        }

        /// <summary>
        /// We have to do this to take the previous pressed status and the actual
        /// </summary>
        public override void ButtonPressed(MouseState state)
        {
            if (State == UIState.Disabled)
            {
                return;
            }
            // Stores the last pressed
            bool tempPressed = Pressed;
            // Then checks for pressed again
            base.ButtonPressed(state);
            if(!tempPressed && Pressed)
            {
                OnActivated();
            }
        }

        public void OnActivated()
        {
            Activated?.Invoke(this, EventArgs.Empty);
        }
    }
}
