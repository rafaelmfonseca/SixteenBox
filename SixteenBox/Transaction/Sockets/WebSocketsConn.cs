using System;
using WebSocketSharp;
using System.Threading;

namespace SixteenBox.Transaction.Sockets
{
    /// <summary>
    /// The main server connection, maybe changing to Proxy connection?
    /// </summary>
    public class WebSocketsConn : IConn<WebSocket>
    {

        #region Properties

        /// <summary>
        /// The Connection
        /// </summary>
        public WebSocket Server { get; set; }

        /// <summary>
        /// The Connection status
        /// </summary>
        public ConnStatus? Status { get; set; }

        /// <summary>
        /// Socket provider
        /// </summary>
        public SocketProvider Provider { get; set; }

        #endregion

        public WebSocketsConn()
        {
            Status = null;
        }

        /// <summary>
        /// Open the connection
        /// </summary>
        public void Open()
        {
            // Send user id and password
            Server = new WebSocket(Settings.ServerSocketUri + Settings.UserOnline.Id + "/" + Settings.UserOnline.TempPassword);
            // Is connecting
            Status = ConnStatus.Connecting;
            // Handle all type os connection
            Server.OnOpen += (s, e) => Status = ConnStatus.Connected;
            // This two means the user is disconnected
            Server.OnClose += (s, e) => Status = ConnStatus.Disconnected;
            Server.OnError += (s, e) => Status = ConnStatus.Disconnected;
            Server.OnMessage += (s, e) => Receive(e.Data);
            // Conenct
            Thread thread = new Thread(Connect);
            thread.Start();
        }

        /// <summary>
        /// Connects to the server
        /// </summary>
        private void Connect()
        {
            try
            {
                Server.ConnectAsync();
            }
            catch (Exception)
            {
                // Does nothing or notify
            }
        }

        /// <summary>
        /// Send a message to server
        /// </summary>
        public void Send(string json)
        {
            Server.Send(json);
        }

        /// <summary>
        /// When receive any messages
        /// </summary>
        public void Receive(string json)
        {
            Provider.ReceiveJson(json);
        }

        /// <summary>
        /// Close the connection
        /// </summary>
        public void Close()
        {
            Server.Close();
        }
    }
}
