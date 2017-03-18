using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using SixteenBox.Transaction.Sockets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Screens
{

    public abstract class GameScreen
    {

        #region Properties

        public bool IsPopUp { get; set; }
        public bool IsExiting { get; set; }
        public ScreenManager ScreenManager { get; set; }
        public SpriteBatch SpriteBatch => ScreenManager.SpriteBatch;
        public ContentManager Content => ScreenManager.Content;
        public SocketProvider Socket => Settings.Socket;

        #endregion

        #region Initialization

        public virtual void LoadContent() { }
        
        public virtual void UnloadContent() { }

        #endregion

        #region Update and Draw
        
        public virtual void Update(GameTime gameTime) { }
        
        public virtual void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(blendState: BlendState.AlphaBlend, transformMatrix: ScreenManager.Camera.GetViewMatrix(), samplerState: SamplerState.PointClamp); // PointClamp for pixeled tilesets
        }

        #endregion
    }
}