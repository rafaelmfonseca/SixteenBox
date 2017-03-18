using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Graphics;
using SixteenBox.Graphics.Animations;
using SixteenBox.Graphics.Helpers;
using SixteenBox.Threading;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Screens.InGame
{
    public enum NotificationType
    {
        Success,
        Alert
    }

    /// <summary>
    /// Notify Screen
    /// </summary>
    public class NotifierScreen : GameScreen
    {
        /// <summary>
        /// Represents a notification
        /// </summary>
        public class Notification
        {
            public NotificationType Type { get; set; }
            public string Text { get; set; }
        }

        #region Fields

        private Sprite _notSprite;
        private Font _notFont;
        private Counter _counter;
        Transformer<float> _notificationShow, _notificationHide;
        RecSourceCollection _notificationsRecs;

        #endregion

        #region Properties

        // You can add notifications here
        public static Queue<Notification> Notifications;

        #endregion

        static NotifierScreen()
        {
            // Initialize
            Notifications = new Queue<Notification>();
        }

        public override void LoadContent()
        {
            // Notification rectangles
            _notificationsRecs = new RecSourceCollection();
            _notificationsRecs.Sources = new List<Rectangle>()
            {
                new Rectangle(1, 1, 111, 32),
                new Rectangle(1, 34, 111, 32)
            };
            _notSprite = new Sprite(Content.Load<Texture2D>("ui/popups"), SpriteBatch);
            _notSprite.RectangleSource = _notificationsRecs.Get(0); // We do this just for Adjust()
            _notFont = new Font(Content.Load<SpriteFont>("fonts/goldfish_font"), SpriteBatch);
            _notFont.Color = Color.White;
            _notFont.UpperCase = false;
            _counter = new Counter() { EndTime = 3500 };
            Adjust();
        }

        private void Adjust()
        {
            Positioner.Init(_notSprite)
                      .Margin(Settings.GameWindowSprite, new Vector2(10, 10), Positioner.MarginFrom.BottomRight);
            Positioner.Init(_notFont)
                      .Margin(_notSprite, new Vector2(28, 6), Positioner.MarginFrom.TopLeft);
        }

        public override void Update(GameTime gameTime)
        {
            // Check size
            if (Notifications.Count <= 0) return;

            _counter.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            // Check size
            if (Notifications.Count <= 0) return;

            base.Draw(gameTime);

            // Take the first without removing, Dequeue() method removes it
            Notification first = Notifications.Peek();

            _notSprite.RectangleSource = (first.Type == NotificationType.Success) ? _notificationsRecs.Get(0) : _notificationsRecs.Get(1);
            _notificationShow = _notificationShow.Transformer(0.0F, 1.0F, 250, gameTime, (t) => _notSprite.Alpha = t.CurrentValue);
            _notSprite.Draw(gameTime);
            _notFont.Text = first.Text;
            _notFont.Draw(gameTime);

            // Delete Notification
            if (_counter.Done)
            {
                // Hides notification
                _notificationHide = _notificationHide.Transformer(1.0F, 0.0F, 250, gameTime, (t) => _notSprite.Alpha = t.CurrentValue);

                // Checks if its hidden
                if (_notificationHide.Done)
                {
                    _counter.Reset();
                    Notifications.Dequeue();

                    // Resets transforms
                    _notificationShow = null;
                    _notificationHide = null;
                }
            }

            SpriteBatch.End();
        }

    }
}
