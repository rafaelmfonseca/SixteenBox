using Newtonsoft.Json;
using SixteenBox.Screens;
using SixteenBox.Transaction.Sockets.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSocketSharp;

namespace SixteenBox.Transaction.Sockets
{
    /// <summary>
    /// Handles everything
    /// </summary>
    public class SocketProvider
    {
        #region Properties
        
        /// <summary>
        /// The provider
        /// </summary>
        public IConn<WebSocket> Connection;

        #endregion

        public SocketProvider()
        {
            Connection = new WebSocketsConn();
            Connection.Provider = this;
        }

        /// <summary>
        /// Send an object with informations for the server
        /// </summary>
        public void SendObject(object obj)
        {
            string representationName = string.Format(Settings.SocketSeparators[0] + "{0}" + Settings.SocketSeparators[1], obj.GetType().Name);
            // Send to connection
            Connection.Send(string.Format("{0}{1}", representationName, JsonConvert.SerializeObject(obj)));
        }

        /// <summary>
        /// Receive an json and convert to object for the game
        /// </summary>
        public void ReceiveJson(string json)
        {
            // Get separator index
            int separatorIndex = json.IndexOf(Settings.SocketSeparators[1]);
            // Get model type
            string modelType = json.Substring(Settings.SocketSeparators[0].Length, separatorIndex - Settings.SocketSeparators[1].Length);
            // Get content to convert
            string modelContent = json.Substring(separatorIndex + Settings.SocketSeparators[1].Length);
            // Conversion from json to object
            IModel deserialized = null;
            switch (modelType)
            {
                case "ChatMessage":
                    deserialized = JsonConvert.DeserializeObject<ChatMessage>(modelContent);
                    break;
            }
            // Didn't find object? then return
            if(deserialized == null)
            {
                return;
            }
            // Send to all screens that want this
            ScreenManager.ReceiveData.Add(new ScreenManager.SocketData() { Data = deserialized });
        }

    }
}
