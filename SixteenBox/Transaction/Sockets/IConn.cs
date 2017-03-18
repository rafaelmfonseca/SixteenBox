using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SixteenBox.Transaction.Sockets
{

    /// <summary>
    /// Represents the state of the connection
    /// </summary>
    public enum ConnStatus
    {
        Disconnected,
        Connecting,
        Connected
    }

    /// <summary>
    /// Represents an object that can send and receive messages
    /// </summary>
    public interface IConn<T>
    {
        #region Properties

        /// <summary>
        /// The Connection
        /// </summary>
        T Server { get; set; }

        /// <summary>
        /// The Connection status
        /// </summary>
        ConnStatus? Status { get; set; }

        /// <summary>
        /// Socket provider
        /// </summary>
        SocketProvider Provider { get; set; }

        #endregion

        #region Methods

        /// <summary>
        /// Open the connection
        /// </summary>
        void Open();

        /// <summary>
        /// Send a message to server
        /// </summary>
        void Send(string json);

        /// <summary>
        /// When receive any messages
        /// </summary>
        void Receive(string json);

        /// <summary>
        /// Close the connection
        /// </summary>
        void Close();

        #endregion

    }
}
